# pdn-tfm-filetype
A Paint.NET file type plugin that adds support for the Pterodon TFM texture format used in the Flying Heroes game.

# Features
## Supported pixel formats
### Loading
* R8G8B8 (256 color palette)
* R5G6B5
* R4G4B4A4
* R5G5B5A1

### Saving
* R5G6B5
* R4G4B4A4
* R5G5B5A1

## Format limitations
The resolution of the TFM texture must be powers of two and both dimensions must be equal.

# Installation
1. Put TFMFileType.dll in the Paint.NET FileTypes folder depending on your installed Paint.NET version.
2. Restart Paint.NET

| Paint.NET version | Folder location                         |
|-------------------|-----------------------------------------|
| Classic           | C:\Program Files\Paint.NET\FileTypes    |
| Microsoft Store   | Documents\paint.net App Files\FileTypes |