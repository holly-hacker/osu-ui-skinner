# osu!ui skinner

osu!ui skinner is basically a more complex version of my 
[osu!BackgroundChanger](//github.com/HoLLy-HaCKeR/osu-BackgroundChanger) project.
It allows you to extract your osu!ui.dll file into folders containing all the resources 
in it, and can compile them back into a working osu!ui.dll after you made the changes you 
desire. It is incompatible with other resource DLL's (such as osu!gameplay) to prevent
users from getting themselves banned.

## F.A.Q.

### How do I use it?
1. Drag an `osu!ui.dll` on the program and wait for it to do its magic. A new folder called
`extracted` will be created next to the dll files containing 2 other folders: `Original`
and `Changes`. 
2. Copy anything you wish to change from the `Original` to the `Changes` folder and edit 
whatever your heart desires. Make sure you keep the same path used in the `Original` folder. 
**Do not edit anything in the `Original` folder itself!**  This folder should contain the 
original files for the program to use as reference.
3. Drag the `extracted` folder (which contains `Original` and `Changes` and drag it on top 
of the program. It will create a new file called `osu!ui-rebuilt.dll` which is now modded with
your custom resources.

### Help, nothing changed!
Make sure that you replaced the osu!ui.dll in your osu! folder with the one you created. Also
check that you actually replaced/edited a file and that they have the same file format. For
example: changing a .png file with a .jpeg may not work.

If nothing helped, your file may have been overwritten by the osu! updater. If so, check `Help, 
osu! overwrite my DLL!`

### Help, osu! overwrites my DLL!
Place your osu!ui.dll in your osu! folder again and delete `osu!.cfg` (not the one containing 
your windows username!). This will make osu! forget about the old osu!ui.dll.

### Help, I cannot see any osu!ui.dll files in my osu! folder!
Make sure you can view hidden files. [Google will tell you how.](//lmgtfy.com/?q=Windows+show+hidden+files)

### But how does it work?
I'm glad you ask! osu!ui skinner uses a library called [dnlib](//github.com/0xd4d/dnlib/)
to read the osu!ui file you provide it, reads all resources contained in it and uses some logic
to try and determine what file type it originally was. Then it divides it up into categories such
as Shaders, Images, Audio, etc. and writes them to disk with their original file extension. When 
you give it an "extracted" folder, it will read through all the categories in the "Original"
folder, check if a modified version exists in the "Changes" folder and write them to a new list 
of resources. It then generates a new DLL file, recreates the code from an original osu!ui.dll from 
scratch and embeds the resources.
