using Infocaster.Umbraco.AlphabetFolders.Extensions;
using Infocaster.Umbraco.AlphabetFolders.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace Infocaster.Umbraco.AlphabetFolders.Composers
{
    public class AlphabetFoldersComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.Services.Configure<AlphabetConfigBase>(builder.Config.GetSection("AlphabetFolders"));
            builder.AddNotificationHandler<ContentMovedToRecycleBinNotification, CustomContentMovedToRecycleBinNotificationHandler>();
            builder.AddNotificationHandler<ContentMovingToRecycleBinNotification, CustomContentMovingToRecycleBinNotification>();
            builder.AddNotificationHandler<ContentSavedNotification, CustomContentSavedNotification>();
        }
    }

    public class CustomContentMovingToRecycleBinNotification : INotificationHandler<ContentMovingToRecycleBinNotification>
    {
        private readonly ILogger<ContentMovingToRecycleBinNotification> _logger;
        private readonly IContentService _contentService;
        private AlphabetConfigBase _options;

        public CustomContentMovingToRecycleBinNotification(IOptions<AlphabetConfigBase> options, ILogger<ContentMovingToRecycleBinNotification> logger, IContentService contentService)
        {
            _options = options.Value;
            _logger = logger;
            _contentService = contentService;
        }

        public void Handle(ContentMovingToRecycleBinNotification notification)
        {
            //save context of parent (if parent is alphabetfolder) to delete it in Deleted event
            foreach (var item in notification.MoveInfoCollection)
            {
                if (!_options.ItemDocTypes.Contains(item.Entity.ContentType.Alias)) continue;
                try
                {
                    var parent = _contentService.GetById(item.Entity.ParentId);
                    if (parent == null || !parent.ContentType.Alias.Equals(_options.FolderDocType)) continue;

                    notification.State.Add(item.Entity.GetUdi().ToString(), parent.Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
        }
    }

    public class CustomContentMovedToRecycleBinNotificationHandler : INotificationHandler<ContentMovedToRecycleBinNotification>
    {
        private readonly AlphabetConfigBase _options;
        private readonly ILogger<CustomContentMovedToRecycleBinNotificationHandler> _logger;
        private readonly IContentService _contentService;

        public CustomContentMovedToRecycleBinNotificationHandler(IOptions<AlphabetConfigBase> options, ILogger<CustomContentMovedToRecycleBinNotificationHandler> logger, IContentService contentService)
        {
            _options = options.Value;
            _logger = logger;
            _contentService = contentService;
        }

        public void Handle(ContentMovedToRecycleBinNotification notification)
        {
            foreach (var item in notification.MoveInfoCollection)
            {
                string itemKey = item.Entity.GetUdi().ToString();

                if (!_options.ItemDocTypes.Contains(item.Entity.ContentType.Alias)) continue;
                if (!notification.State.ContainsKey(itemKey)) continue;

                try
                {
                    int parentId = -1;
                    if (int.TryParse(notification.State[itemKey].ToString(), out parentId) && parentId > 0)
                    {
                        notification.State.Remove(itemKey);
                        IContent parent = _contentService.GetById(parentId);

                        ContentHelper.DeleteAlphabeticFolder(_options.FolderDocType, parent, _contentService);
                        
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
        }
    }

    public class CustomContentSavedNotification : INotificationHandler<ContentSavedNotification>
    {
        private readonly ILogger<ContentSavedNotification> _logger;
        private readonly IContentService _contentService;
        private readonly AlphabetConfigBase _options;

        public CustomContentSavedNotification(ILogger<ContentSavedNotification> logger, IContentService contentService, IOptions<AlphabetConfigBase> options)
        {
            _logger = logger;
            _contentService = contentService;
            _options = options.Value;

            if (_options.ItemDocTypes == null || string.IsNullOrEmpty(_options.FolderDocType))
            {
                _logger.LogError("One or more of the values required are null or empty.");
            }
        }
        public void Handle(ContentSavedNotification notification)
        {
            foreach (IContent content in notification.SavedEntities)
            {
                //Check if id is in allowed ids, if not; don't create folders
                if (_options.AllowedParentIds.Count > 0 && !_options.AllowedParentIds.Contains(content.ParentId)) continue;

                if (_options.ItemDocTypes.Contains(content.ContentType.Alias) && !string.IsNullOrEmpty(content.Name))
                {
                    CreateAlphabetFolderAsync(content);
                }
            }
        }

        public void CreateAlphabetFolderAsync(IContent content)
        {
            try
            {
                IContent parentFolder = _contentService.GetById(content.ParentId);
                IContent alphabeticFolder = null;

                //if parent is a AlphabetFolder, go one parent further up
                if (parentFolder.ContentType.Alias == _options.FolderDocType)
                {
                    alphabeticFolder = parentFolder;
                    parentFolder = _contentService.GetById(parentFolder.ParentId);
                }

                //get the firstLetter to use as folder (filter for valid letters)
                string firstLetter = new String(content.Name.TakeWhile(Char.IsLetterOrDigit).ToArray()).Substring(0, 1).ToUpper();

                if (alphabeticFolder == null || alphabeticFolder.Name != firstLetter)
                {
                    //get all children of the parent
                    var oldParent = alphabeticFolder;
                    var parentChildren = parentFolder.GetAllChildren(_contentService);

                    if (!parentChildren.Any(x => x.Name.ToUpper() == firstLetter))
                    {
                        alphabeticFolder = _contentService.CreateContent(firstLetter, parentFolder.GetUdi(), _options.FolderDocType);
                        _contentService.SaveAndPublish(alphabeticFolder);
                    }
                    else
                    {
                        var firstItem = parentChildren.Where(x => x.Name.ToUpper() == firstLetter).First();
                        alphabeticFolder = _contentService.GetById(firstItem.Id);
                    }

                    _contentService.Move(content, alphabeticFolder.Id);

                    if (oldParent != null)
                    {
                        ContentHelper.DeleteAlphabeticFolder(_options.FolderDocType, oldParent, _contentService);
                    }

                    OrderChildrenByName(alphabeticFolder);
                    OrderChildrenByName(parentFolder);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        /// <summary>
        /// Orders the children from a document by their name
        /// </summary>
        void OrderChildrenByName(IContent parent)
        {
            try
            {
                var childItems = parent.GetAllChildren(_contentService);
                var orderedItems = _options.OrderByDescending ? childItems.OrderByDescending(x => x.Name) : childItems.OrderBy(x => x.Name);

                _contentService.Sort(orderedItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
