using System.Collections.Generic;

namespace Infocaster.Umbraco.AlphabetFolders.Composers
{
    public class AlphabetConfigBase
    {
        public List<string> ItemDocTypes { get; set; } = new List<string>();
        public List<int> AllowedParentIds { get; set; } = new List<int>();
        public string FolderDocType { get; set; }
        public bool OrderByDescending { get; set; } = true;
    }
}