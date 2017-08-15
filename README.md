# FileTime v1.0 - by genBTC

[](http://www.mediafire.com/convkey/b1e0/t73bcv46dnw37adfg.jpg)

[](http://www.mediafire.com/convkey/ddf4/iclzu8ozyxd5eddfg.jpg)

## FileTime, v1.0 - Can modify folder and file timestamps in bulk / recursively.

### Feature: Bulk timestamp modification based on rules.

### Background:
When you copy files with windows, it changes the dates of files to the time you copied it. But the "Modification" date is still intact. The folder dates however are completely changed, and with this program, you can bulk fix them based on the rule you choose. (usually using the intact file modification time to fix the 3 wrong dates on the parent folder. For file moves, the situation is a bit better, only the date accessed gets messed up (IMO the least useful of the three). For folder moves, oddly, the folder creation date is the only one that remains intact.

Mode 1: Standard
Mode 2: Set Folder timestamps based on the files inside.

Created completely in C#, and .NET 2.0 using Visual Studio 2010 (WinForms).
Source is available upon request.

Authored by genBTC. Donate @ 1genbtcPLjAEk6RnfC66chYniFKfP7vAS
Date: August 24, 2014 (my 30th birthday)
Modified: August 15, 2017


### How to Use:
Mode 1: Standard Mode
 Take note of the "Recurse" checkbox, and "Perform on files" checkbox.
 Starts at the root folder, works its way down(deeper).
Mode 2: Recursively Set Folders based on Files/Dirs Inside
 Starts at the root folder, inspects whats inside, decides what to use as the time source (either file or subdir), oldest/newest/random file, and which attribute (or any/random) based on what radiobuttons are used. Then sets the date of the root folder on what is in the root folder. Then it recursively digs deeper and repeats the same process for any/all subdirectorys. Does not change dates of any files.

After clicking the "Update" button, in either mode, a second "Confirm" window will open up.
This confirm window is an essential part of the program, where you can double check what exactly will be done, remove from, add to, change date/times, and open Windows Explorer.
The confirm window has a choice of "Update if date is newer/older/always" - a filter mask, that applies once you click the final "Confirm" button.

### Behavior/Quirks:
-From Currently Selected cannot be selected if nothing on the right side is selected.
-To unselect something (since Ctrl click doesnt work) on the right side, there is a small blank empty space next to the scrollbar that you can click. (Outside of the first column).
-On the confirm window, there is an issue where selecting items will cause the checkboxes to change. I built in a protection circuit that will prevent this (to make it seem normal) but you will find that if you try to change the checkboxes within 800ms of selecting something, it won't let you. The other side of this means that if you manage to hold the mouse button down for more than 800ms between when you select something and when you let go, you will bypass the protection and the checkbox will change. I have not found a better solution for this, as it is inherent in the ListView control for WinForms.

### A Side Note on Read Only Folders:
-Read only folders are actually not what you think they are. Windows uses the folder read-only flag for internal purposes. The checkbox in the Windows Explorer Properties window of a user folder serves only one purpose, it is there to allow a user to change the read-only status of All the sub-files inside the folder at once. It _is_ possible to change the folder read-only flag to "not read-only" (with attrib -r) but absolutely nothing is gained by doing this. This confusing checkbox is not to be confused with PERMISSIONS. For further reading  It is not recommended to change the read-only flag on folders such as C:\Windows\Fonts, because it literally is what makes the special custom view be displayed.  This program should still modify Timestamps of read-only FOLDERS but not read-only files (unless it is desired/set).

