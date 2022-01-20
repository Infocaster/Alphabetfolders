# Alphabetfolders
his package creates AlphabetFolders for the specified doctype for Umbraco 9. For umbraco 8 use v3, for older versions please use v2

# Behavior
- When you create a document with doctype "itemDocType", this package will automatically create alphabet folders for it
- When you edit the name, the document is automatically moved to the correct alphabet folder
- Automatically cleans up empty alphabet folders
- Orders the alphabet folders and items inside the alphabet folders by with every action

## Configuration
Add these keys/values to your appsettings.json in a new section:
 
<pre>
"AlphabetFolders": {
    "AllowedParentIds": [],             // List of parent ids to check. Folders wil only be created if parent matches any of the provided ids
    "OrderByDescending": true,          // boolean indicating sort order for date folders (default: false)
    "FolderDocType": "dateFolder",      // the doctype to use for creating the year/month/day folders (e.g "DateFolder")
    "ItemDocTypes": [ "contentPage" ]   // the doctype alias to create datefolders for (e.g. "newsItem") - multiple doctype aliases are allowed
}
</pre>