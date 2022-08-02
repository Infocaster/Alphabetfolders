<h3 align="center">
<img height="100" src="https://raw.githubusercontent.com/Infocaster/.github/main/assets/infocaster_nuget_pink.png">
</h3>

<h1 align="center">
Alphabetfolders


[![Downloads](https://img.shields.io/nuget/dt/Infocaster.Umbraco.AlphabetFolders?color=ffc800)](https://www.nuget.org/packages/Infocaster.Umbraco.AlphabetFolders/)
[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Infocaster.Umbraco.AlphabetFolders?color=ff0069)](https://www.nuget.org/packages/Infocaster.Umbraco.AlphabetFolders/)
![GitHub](https://img.shields.io/github/license/Infocaster/AlphabetFolders?color=ffc800)

</h1>

Alphabetsfolder is <b>the</b> package to make sure that all of your content is ordered alphabetically.
This package automatically adds all of your new nodes to a folder that seperates the content alphabetically.
You'll never need to search for your content again! Especially awesome when you have a lot of content.

## Requirements
The Alphabetfolders package is made for Umbraco v8, V9 and V10.
Version 3+ is made for Umbraco V8, version V9.x is made for Umbraco 9 and V10.x is made for Umbraco 10.

## Getting Started
This package is available via NuGet. Visit [the AlphabetFolders package on NuGet](https://www.nuget.org/packages/Infocaster.Umbraco.AlphabetFolders/) for instructions on how to install this package in your website.
Once installed, build your project and you're ready to make your visitors happy.

## Configuration
Upon installation, the AlphabetFolders package will need several variables in your Appsettings.Json file:

```json
"AlphabetFolders": {
    "AllowedParentIds": [],            
    "OrderByDescending": true,         
    "FolderDocType": "alphabetFolder",      
    "ItemDocTypes": [ "contentPage" ]  
}
```

- **AllowedParentIds** | The ids for specific documents for which the folders need to be made. Makes it possible to only create new folders for specific documents in case there are multiple documents of the same type - comma seperated values are allowed for multiple documents. (OPTIONAL)
- **OrderByDescending** | Boolean indicating sort order for alphabet folders.
- **FolderDocType** | The doctype to use for creating the letter folders. (e.g "alphabetFolder")
- **ItemDocType** | The doctype alias to create alphabetfolders for. (e.g. "contentPage") - comma separated values are allowed for multiple doctype aliases 


-----

## Changelog
Version 10.0.0
- Updated to use Umbraco v10

Version 9.0.0
- Updated to use Umbraco v9

Version 3.0.1
- Added optional key alphabetfolders:AllowedParentIds to specify specific documents for which the AlphabetFolders need to be made.

Version 3.0.0
- Updated to use Umbraco v8

<a href="https://infocaster.net">
<img align="right" height="200" src="https://raw.githubusercontent.com/Infocaster/.github/main/assets/Infocaster_Corner.png">
</a>