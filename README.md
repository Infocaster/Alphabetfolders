# Alphabetfolders 
This package creates AlphabetFolders for the specified doctype for Umbraco 8

# Behavior
- When you create a document with doctype "itemDocType", this package will automatically create alphabet folders for it
- When you edit the name, the document is automatically moved to the correct alphabet folder
- Automatically cleans up empty alphabet folders
- Orders the alphabet folders and items inside the alphabet folders by with every action

## Configuration
Add these keys/values to your appSettings section in the web.config:

Key: "alphabetfolders:ItemDocType" - the doctype alias to create alphabetfolders for (e.g. "newsItem") - comma separated values are allowed for multiple doctype aliases 

Key: "alphabetfolders:FolderDocType" - the doctype to use for creating the letter folders (e.g "AlphabetFolder")

Key: "alphabetfolders:OrderByDescending"  - boolean indicating sort order for date folders (default: false)


## Changelog

Version 3.0.0
- Updated to use umbraco v8