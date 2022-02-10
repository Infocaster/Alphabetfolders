<h3 align="center">
<img height="100" src="docs/assets/infocaster_nuget_pink.svg">
</h3>

<h1 align="center">
Alphabetfolders
</h1>

[![Downloads](https://img.shields.io/nuget/dt/Infocaster.Umbraco.AlphabetFolders?color=ffc800)](https://www.nuget.org/packages/Infocaster.Umbraco.AlphabetFolders/)
[![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/Infocaster.Umbraco.AlphabetFolders?color=ff0069)](https://www.nuget.org/packages/Infocaster.Umbraco.AlphabetFolders/)
![GitHub](https://img.shields.io/github/license/Infocaster/DateFolders?color=ffc800)

Alphabetsfolder is <b>the</b> package to make sure that all of your content is ordered alphabetically.
This package automatically adds all of your new nodes to a folder that seperates the content alphabetically.
You'll never need to search for your content again! Especially awesome when you have a lot of content.

## Requirements
The Alphabetfolders package is made for Umbraco v8 and V9.
Version 3+ is made for Umbraco V8 and version 9 and above are made for Umbraco V9.

## Getting Started
This package is available via NuGet. Visit [the AlphabetFolders package on NuGet](https://www.nuget.org/packages/Infocaster.Umbraco.AlphabetFolders/) for instructions on how to install this package in your website.
Once installed, build your project and you're ready to make your visitors happy.

## Configuration
Upon installation, the AlphabetFolders package need several configurations in your Web.config file under the AppSettings section:

```xml
<add key="alpabetfolders:ItemDocType" value="newsItem" />
<add key="alpabetfolders:FolderDocType" value="AlphabetFolder" />
<add key="alpabetfolders:OrderByDescending" value="false" />
<add key="alpabetfolders:AllowedParentIds" value="1234, 5678" />
```

- **ItemDocType** | The doctype alias to create alphabetfolders for. (e.g. "newsItem") - comma separated values are allowed for multiple doctype aliases 
- **FolderDocType** | The doctype to use for creating the letter folders. (e.g "AlphabetFolder")
- **OrderByDescending** | Boolean indicating sort order for date folders. (default: false)
- **AllowedParentIds** | The ids for specific documents for which the folders need to be made. Makes it possible to only create new folders for specific documents in case there are multiple documents of the same type - comma seperated values are allowed for multiple documents. (OPTIONAL)


-----

## Credits ##
The Support Module is made by Infocaster, a Dutch company dedicated to improving your Umbraco experience.

## Changelog
Version 3.0.1
- Added optional key alphabetfolders:AllowedParentIds to specify specific documents for which the AlphabetFolders need to be made.

Version 3.0.0
- Updated to use umbraco v8

<a href="https://infocaster.net">
<img align="right" height="200" src="docs/assets/Infocaster_Corner.png">
</a>