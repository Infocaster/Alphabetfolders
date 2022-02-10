
     /\   | |     | |         | |        | | |  ____|  | |   | |              
    /  \  | |_ __ | |__   __ _| |__   ___| |_| |__ ___ | | __| | ___ _ __ ___ 
   / /\ \ | | '_ \| '_ \ / _` | '_ \ / _ \ __|  __/ _ \| |/ _` |/ _ \ '__/ __|
  / ____ \| | |_) | | | | (_| | |_) |  __/ |_| | | (_) | | (_| |  __/ |  \__ \
 /_/    \_\_| .__/|_| |_|\__,_|_.__/ \___|\__|_|  \___/|_|\__,_|\___|_|  |___/
            | |                                                               
            |_|                                                               

===============================================================================

Thank you for downloading our package!

To get started, add the following configuration to your appsettings.json file:

"AlphabetFolders": {
    "AllowedParentIds": [],   
    "OrderByDescending": true,          
    "FolderDocType": "alphabetFolder",      
    "ItemDocTypes": [ "contentPage" ]   
}

AllowedParentIds | The ids for specific documents for which the folders need to be made. Makes it possible to only create new folders for specific documents in case there are multiple documents of the same type - comma seperated values are allowed for multiple documents. (OPTIONAL)
OrderByDescending | Boolean indicating sort order for alphabet folders.
FolderDocType | The doctype to use for creating the letter folders. (e.g "alphabetFolder")
ItemDocType | The doctype alias to create alphabetfolders for. (e.g. "contentPage") - comma separated values are allowed for multiple doctype aliases 


===============================================================================

For more information, check out the links below:
Github - https://github.com/Infocaster/Alphabetfolders
Our Umbraco - https://our.umbraco.com/packages/developer-tools/alphabetfolders/