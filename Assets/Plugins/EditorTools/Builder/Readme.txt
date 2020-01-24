###-
# Name = Builder
# Version = 1.1
# Author = Edward Atencio
###~

# HOW TO OPEN

In the menu bar: [Tools] -> [Buider]
or press F5

# HOW TO USE

At the end of "Builder.cs" are the methods called to do the building, they set up the destination path and build settings, and then call the Build() method.

Build() returns the BuildReport so that you can do other stuff depending on the build result. The Production (x86) configuration generates a "Version.json" (containing the info modifiable from the builder window) in the build path if the build is successful.

To add a new build option, make a method with all the stuff you want to do in "Builder.cs" and add a button in the "BuilderWindow.cs" script.

The Button() function needs the text that it's going to display, the function that's going to call when pressed, and optionally you can set the height, width, and color of the button.

All the builds are created in the "Builds" folder inside your project folder.

# SOURCE

https://github.com/edatencio/Unity-Editor-Tools