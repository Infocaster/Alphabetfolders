using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Events;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Core.Services.Implement;


namespace InfoCaster.Umbraco.AlphabetFolders.Composers
{
    public class AlphabetfoldersComposer : IUserComposer
    {
        public void Compose(Composition composition)
        {
            composition.Components().Append<AlphabetfoldersComponent>();
        }
    }

    public class AlphabetfoldersComponent : IComponent
    {
        static string[] _itemDocTypes;
        static string _folderDocType;
        static readonly string _allowedCharacters = "abcdefghijklmnopqrstuvwxyz0123456789".ToUpper();
        static readonly object _syncer = new object();

        private readonly ILogger _logger;
        private readonly IContentService _contentService;
        private readonly IContentTypeService _contentTypeService;

        public AlphabetfoldersComponent(ILogger logger, IContentService contentService, IContentTypeService contentTypeService)
        {
            _logger = logger;
            _contentService = contentService;
            _contentTypeService = contentTypeService;

            // Todo: Implement parentDoctype logic

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["alphabetfolders:ItemDocType"]) && !string.IsNullOrEmpty(ConfigurationManager.AppSettings["alphabetfolders:FolderDocType"]))
            {
                _itemDocTypes = ConfigurationManager.AppSettings["alphabetfolders:ItemDocType"].Split(',');
                _folderDocType = ConfigurationManager.AppSettings["alphabetfolders:FolderDocType"];
            }
        }

        public void Initialize()
        {
            if (_itemDocTypes == null || !_itemDocTypes.Any() || string.IsNullOrEmpty(_folderDocType)) return;

            ContentService.Saved += ContentService_Saved;
            ContentService.Trashing += ContentService_Trashing;
            ContentService.Trashed += ContentService_Trashed;
        }

        private void ContentService_Trashed(IContentService sender, MoveEventArgs<IContent> e)
        {
            foreach (var item in e.MoveInfoCollection)
            {
                if (!_itemDocTypes.Contains(item.Entity.ContentType.Alias)) continue;
                if (!HttpContext.Current.Items.Contains("parentId")) continue;

                try
                {
                    int parentId = -1;
                    if (int.TryParse(HttpContext.Current.Items["parentId"].ToString(), out parentId) && parentId > 0)
                    {
                        HttpContext.Current.Items.Remove("parentId");
                        IContent parent = _contentService.GetById(parentId);
                        DeleteAlphabeticFolder(parent);

                        // Todo: Refresh tree in umbraco?
                    }
                }
                catch (Exception ex)
                {
                    // Todo: Show error message?
                    _logger.Error<AlphabetfoldersComponent>(ex, "DateFolders ContentService_Trashed exception");
                }
            }
        }

        private void ContentService_Saved(IContentService sender, ContentSavedEventArgs e)
        {
            foreach (IContent content in e.SavedEntities)
            {
                CreateAlphabeticFolder(content);
            }
        }

        private void ContentService_Trashing(IContentService sender, MoveEventArgs<IContent> e)
        {
            foreach (var item in e.MoveInfoCollection)
            {
                if (!_itemDocTypes.Contains(item.Entity.ContentType.Alias)) continue;

                try
                {
                    var parent = _contentService.GetById(item.Entity.ParentId);
                    if (parent == null || !parent.ContentType.Alias.Equals(_folderDocType)) continue;

                    HttpContext.Current.Items.Add("parentId", parent.Id);

                }
                catch (Exception ex)
                {
                    // Todo: Show error message?
                    _logger.Error<AlphabetfoldersComponent>(ex);
                }
            }
        }

        public void Terminate()
        {
        }

        /// <summary>
        /// Creates an alphabetic folder for the first letter of the Documents name
        /// </summary>
        void CreateAlphabeticFolder(IContent content)
        {
            if (_itemDocTypes.Contains(content.ContentType.Alias))
            {
                try
                {
                    if (!string.IsNullOrEmpty(content.Name))
                    {
                        IContent parentFolder = _contentService.GetById(content.ParentId);
                        IContent alphabeticFolder = null;
                        if (parentFolder.ContentType.Alias == _folderDocType)
                        {
                            alphabeticFolder = parentFolder;
                            parentFolder = _contentService.GetById(parentFolder.ParentId);
                        }

                        string firstLetter;
                        int charIndex = 1;
                        do
                        {
                            firstLetter = content.Name.Substring(charIndex - 1, 1).ToUpper();
                            charIndex++;
                        }
                        while (!_allowedCharacters.Contains(firstLetter) && content.Name.Length >= charIndex);

                        if ((alphabeticFolder != null && alphabeticFolder.Name != firstLetter) || alphabeticFolder == null)
                        {
                            if (_itemDocTypes.Contains(content.ContentType.Alias))
                            {
                                IContent oldParent = alphabeticFolder;

                                long totalChildren;
                                int childCount = _contentService.CountChildren(parentFolder.Id);

                                var parentChildren = _contentService.GetPagedChildren(parentFolder.Id, 0, childCount, out totalChildren);

                                if (!parentChildren.Any(x => x.Name.ToUpper() == firstLetter))
                                {
                                    alphabeticFolder = _contentService.CreateContent(firstLetter, parentFolder.GetUdi(), _folderDocType);
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
                                    DeleteAlphabeticFolder(oldParent);
                                }

                                OrderChildrenByName(alphabeticFolder);
                                OrderChildrenByName(parentFolder);

                                // Todo: Refresh tree in umbraco?
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error<AlphabetfoldersComponent>(ex, "CreateAlphabeticFolder exception");
                    // Todo: Show error to user?
                }
            }
        }

        /// <summary>
        /// Orders the children from a document by their name
        /// </summary>
        void OrderChildrenByName(IContent parent, bool orderByDesc = false)
        {
            lock (_syncer)
            {
                try
                {
                    // Todo: find alternative for getPagedChildren, IContent.Children() doesn't exist at the moment
                    long totalChildren;
                    int childCount = _contentService.CountChildren(parent.Id);
                    var childItems = _contentService.GetPagedChildren(parent.Id, 0, childCount, out totalChildren);

                    var orderedItems = orderByDesc ? childItems.OrderByDescending(x => x.Name) : childItems.OrderBy(x => x.Name);

                    _contentService.Sort(orderedItems);
                }
                catch (Exception ex)
                {
                    _logger.Error<AlphabetfoldersComponent>(ex, "AlphabetFolders SortChildrenByName exception");
                   // Todo: show error to umbraco
                }
            }
        }

        /// <summary>
        /// Deletes the alphabetic folder if it does not have any children
        /// </summary>
        void DeleteAlphabeticFolder(IContent folder)
        {
            if (folder.ContentType.Alias == _folderDocType && !_contentService.HasChildren(folder.Id))
            {
                _contentService.Delete(folder);
            }
        }
    }
}
