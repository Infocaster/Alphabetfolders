namespace Infocaster.Umbraco.AlphabetFolders.Composers
{
    public class AlphabetConfigBase
    {
        public string[] ItemDocTypes { get; set; }
        public int[] AllowedParentIds { get; set; }
        public string FolderDocType { get; set; }
        public bool OrderByDescending { get; set; }
    }
}