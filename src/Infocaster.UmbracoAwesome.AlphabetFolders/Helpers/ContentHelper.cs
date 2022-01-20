using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace AlphabetFolder9.Helpers
{
    static class ContentHelper
    {
        /// <summary>
        /// Deletes the alphabetic folder if it does not have any children
        /// </summary>
        public static void DeleteAlphabeticFolder(string folderDocType, IContent folder, IContentService contentService)
        {
            if (folder.ContentType.Alias == folderDocType && !contentService.HasChildren(folder.Id))
            {
                contentService.MoveToRecycleBin(folder);
            }
        }
    }
}
