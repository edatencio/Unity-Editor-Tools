# Unity Editor Tools
This repository is a collection of tools I regularly use while developing projects; some are made by myself, other just found them online and modified them a little (the sources are in the respective script). Hope you'll find them useful!.

## Script templates with custom keywords

When you create a new script, Unity uses a template to generate code automatically; you can customize that code or create custom templates by placing files in ```Assets\ScriptTemplates\```. For a template naming guide see [here](https://twitter.com/UnityBerserkers/status/1105556466725998599).


These templates have keywords that are replaced by Unity during creation, there is [not much information](https://forum.unity.com/threads/c-script-template-how-to-make-custom-changes.273191/#post-1806414) about which keywords you can use, but with this tool, you can create your own!.

* **Files:** The ```ScriptKeywordProcessor.cs``` script is the one in charge of replacing your custom keywords, ```81-C# Script-NewBehaviourScript.cs.txt``` is the default script template.
```
|-- Assets/
    |-- ScriptTemplates/
        |-- Editor/
            |-- ScriptKeywordProcessor.cs
        |-- 81-C# Script-NewBehaviourScript.cs.txt
```

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
