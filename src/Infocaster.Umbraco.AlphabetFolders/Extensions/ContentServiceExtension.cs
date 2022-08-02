using System.Collections.Generic;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace Infocaster.Umbraco.AlphabetFolders.Extensions
{
    static class ContentServiceExtension
    {
        public static IEnumerable<IContent> GetAllChildren(this IContent item, IContentService contentService)
        {
            int childCount = contentService.CountChildren(item.Id);
            if (childCount > 0)
            {
                return contentService.GetPagedChildren(item.Id, 0, childCount, out long totalChildren);
            }
            else
            {
                return new List<IContent>();
            }
        }
    }
}
