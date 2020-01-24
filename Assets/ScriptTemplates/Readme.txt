###-
# Name = ScriptTemplates
# Version = 1.0
# Author = Edward Atencio
###~

# INSTALLATION

When you import this tool or create a new template in your project, you have to restart Unity. Also, the ScriptTemplates folder has to be in your Assets folder; if you place it anywhere else, the editor will ignore all your templates. 

"81-C# Script-NewBehaviourScript.cs.txt" is the default script template used by Unity.

# HOW TO USE

In "ScriptKeywordProcessor.cs" there is a string array called keywords, change it's values to your heart's content. These are the default values:

string[,] keywords = new string[,]
{
    { "#AUTHOR#", "Edward Atencio" },
    { "#CREATION_DATE#", System.DateTime.Now.ToString("dd/MM/yy") },
    { "#COMPANY_NAME#", ParseName(PlayerSettings.companyName) },
    { "#PRODUCT_NAME#", ParseName(PlayerSettings.productName) }
};

# SOURCE

https://github.com/edatencio/Unity-Editor-Tools