# Using the uploader


1. Set [your nuget API key](http://blog.davidebbo.com/2011/03/saving-your-api-key-with-nugetexe.html). 
2. Copy the latest version of the AForge framework into this folder. 'nuget' and 'Source' should be sibling folders.
3. Run nuget\AForgeUploader\bin\debug\AForgeUploader.exe (Make yourself a shortcut in the root)
4. Enter the version number of the library you are uploading. 
5. Press 'y', then 'c', 'u', or 'cu' for each nuget package to create and/or upload it.

## Adding a new nuget package

1. Copy and paste AForge.Math.nuspec and AForge.Math.symbols.nuspec. Rename them to AForge.NewNamespace.nusepc and AForge.NewNamespace.symbols.nuspec respectively.
2. Open both in Notepad++, and change..
3. The ID in both files 
4. The dependencies in both files
5. The Title and Description in the .nuspec file
6. In the .symbols.nuspec, specify the .dll, .pdb, and the source files for the assembly.
7. In the .nuspec file, specify only the .dll and .xml for the assembly.


