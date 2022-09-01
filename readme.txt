           _       _           _          _   ______    _     _               
     /\   | |     | |         | |        | | |  ____|  | |   | |              
    /  \  | |_ __ | |__   __ _| |__   ___| |_| |__ ___ | | __| | ___ _ __ ___ 
   / /\ \ | | '_ \| '_ \ / _` | '_ \ / _ \ __|  __/ _ \| |/ _` |/ _ \ '__/ __|
  / ____ \| | |_) | | | | (_| | |_) |  __/ |_| | | (_) | | (_| |  __/ |  \__ \
 /_/    \_\_| .__/|_| |_|\__,_|_.__/ \___|\__|_|  \___/|_|\__,_|\___|_|  |___/
            | |                                                               
            |_|                                                               

===============================================================================

Thanks for installing the AlphabetFolders package!

To get started, follow the installation guide in the ReadMe and add the correct keys to the web.config file.

<add key="alpabetfolders:ItemDocType" value="newsItem" />
<add key="alpabetfolders:FolderDocType" value="AlphabetFolder" />
<add key="alpabetfolders:OrderByDescending" value="false" />
<add key="alpabetfolders:AllowedParentIds" value="1234, 5678" />

ItemDocType | The doctype alias to create alphabetfolders for. (e.g. "newsItem") - comma separated values are allowed for multiple doctype aliases 
FolderDocType | The doctype to use for creating the letter folders. (e.g "AlphabetFolder")
OrderByDescending | Boolean indicating sort order for date folders. (default: false)
AllowedParentIds | The ids for specific documents for which the folders need to be made. Makes it possible to only create new folders for specific documents in case there are multiple documents of the same type - comma seperated values are allowed for multiple documents. (OPTIONAL)


For more information, check out the links below:
Github - https://github.com/Infocaster/Alphabetfolders
Our Umbraco - https://our.umbraco.com/packages/developer-tools/alphabetfolders/

Enjoy this package!