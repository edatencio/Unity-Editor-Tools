# Unity Editor Tools
This repository is a collection of tools I regularly use while developing projects; some are made by myself, other just found them online and modified them a little (the sources are in the respective script). Hope you'll find them useful!.

## Script templates with custom keywords

When you create a new script, Unity uses a template to generate code automatically; you can customize that code or create custom templates by placing files in ```Assets\ScriptTemplates\```. For a template naming guide see [here](https://twitter.com/UnityBerserkers/status/1105556466725998599).


These templates have keywords that are replaced by Unity during creation, there is [not much information](https://forum.unity.com/threads/c-script-template-how-to-make-custom-changes.273191/#post-1806414) about which keywords you can use, but with this tool, you can create your own!.

* **Files:** The ```ScriptKeywordProcessor.cs``` script is the one in charge of replacing your custom keywords, ```81-C# Script-NewBehaviourScript.cs.txt``` is the default script template.

![](https://github.com/edatencio/Unity-Editor-Tools/blob/master/Screenshots/ScriptTemplate_files.png)

* **Installation:** When you import this tool or create a new template in your project, you have to restart Unity. Also, the ```ScriptTemplates``` folder has to be in your ```Assets``` folder; if you place it anywhere else, the editor will ignore all your templates.

* **How to use:** In ```ScriptKeywordProcessor.cs``` there is a string array called ```keywords```, change it's values to your heart's content. These are the default values:
```
string[,] keywords = new string[,]
{
    { "#AUTHOR#", "Edward Atencio" },
    { "#CREATION_DATE#", System.DateTime.Now.ToString("dd/MM/yy") },
    { "#COMPANY_NAME#", ParseName(PlayerSettings.companyName) },
    { "#PRODUCT_NAME#", ParseName(PlayerSettings.productName) }
};
```
Note: The ```ParseName()``` method removes any non-letter character to avoid naming conflicts.

* **Example:**

![](https://github.com/edatencio/Unity-Editor-Tools/blob/master/Screenshots/ScriptTemplate_Template.png)
![](https://github.com/edatencio/Unity-Editor-Tools/blob/master/Screenshots/ScriptTemplate_NewScript.png)

## Builder
With this tool, you can edit some player settings and setup different build configurations.

The thing I like the most about this tool is that **it kills any instance of a previous build** and deletes everything in the build folder before creating the new one.

![](https://github.com/edatencio/Unity-Editor-Tools/blob/master/Screenshots/Builder/Builder.png)

* **How to use:**

At the end of ```Builder.cs``` are the methods called to do the building, they set up the destination path and build settings, and then call the ```Build()``` method.

```Build()``` returns the ```BuildReport``` so that you can do other stuff depending on the build result. The ```Production (x86)``` configuration generates a ```Version.json``` (containing the info modifiable from the builder window) in the build path if the build is successful.

![](https://github.com/edatencio/Unity-Editor-Tools/blob/master/Screenshots/Builder/production_method.png)

To add a new build option, make a method with all the stuff you want to do in ```Builder.cs``` and add a button in the ```BuilderWindow.cs``` script.

The ```Button()``` function needs the text that it's going to display, the function that's going to call when pressed, and optionally you can set the height, width, and color of the button.

![](https://github.com/edatencio/Unity-Editor-Tools/blob/master/Screenshots/Builder/build_buttons.png)

All the builds are created in the ```Builds``` folder inside your project folder.

![](https://github.com/edatencio/Unity-Editor-Tools/blob/master/Screenshots/Builds_folder.png)

## Package Exporter

With this tool, you can select a folder and generate a ```.unitypackage``` file with an added readme file; the readme contains the author name, package name, version, and any other additional info editable from the window.

![](https://github.com/edatencio/Unity-Editor-Tools/blob/master/Screenshots/Package_Exporter.png)

* **How to use:** 

This one is pretty straight forward since it's not meant to be modified; simply set up all the info and hit the ```Export Package``` button, the package will be exported in the ```Builds``` folder inside your project folder.

Note that **this tool only packs the stuff inside the selected folder**.

![](https://github.com/edatencio/Unity-Editor-Tools/blob/master/Screenshots/builds_folder.png)
