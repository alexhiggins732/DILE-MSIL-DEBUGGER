# DILE
DILE (Dotnet IL Editor) is a decompiler and an IL debugger.

It allows managed application to be debugged without having the source code and it can help to understand what is going on in the background as even the .NET Framework assemblies can be debugged. Works both on x86 (32-bit) and x64 (64-bit) operating systems.

# Disassembling features:
- displaying IL code
- generics support
- quick search by different categories (classes, methods, fields, tokens, etc.)
- supports both 32 and 64-bit assemblies

# Debugger features:
- both v1.1 and v2.0 applications can be debugged
- allows debugging dynamic assemblies
- attaching to process/detaching; starting new process to debug with arguments
- stepping in/out/over on the IL level
- setting instruction pointer; run to cursor
- debuggee can be stopped on specific events (e.g.: CreateThread, LoadAssembly)
- MDA (Managed Debug Assistant) notifications
- adding/removing/activating/deactivating breakpoints
- "Object Viewer" to check the current value of an object's properties and fields or evaluate expressions, call methods
- text displayer form to quickly view strings with or without escape characters as simple text or html
- call stack; local variables/arguments; debug messages; debugging events; modules; threads; watch panel
- filter on which exceptions to stop the debuggee
- decimal or hexadecimal number display

Recently updated to Visual Studio 2017, .Net Framework 4.7.2 from stale source forge project.

# Requirements:
- The solution requires C\C++ support (Available as Visual Studio component). The CPP project is currently linked to  NETFXSDK\4.7.2 and the Windows 10 SDK Kit.
- Alternately, you already have other versions installed, can manually link the CPP project to their mscoree.lib and dbghelp.lib files.

This is the v0.2.26 release of DILE which contains both disassembling and debugging related enhancements and/or bug fixes. The known bugs are described below, but should you find anything else, please let me know using the project's homepage.

Debugging has been tested with v1.1, v2.0, v3.0, v3.5 and v4.0 applications but theoretically it should work with v1.0, v4.7 and Mono programs as well.

Note: every grid and listview has an associated context menu that includes some basic and specific (depending on the context) commands.

# Text editor:
Items which are opened will be shown here. Right now, this is functioning as a text viewer as it's read-only.
By right-clicking on it, a context menu is shown with 2 menu items: "Locate in Project Explorer". This will jump in the Project Explorer to the currently shown item's node. The second option is the "Set IP to this instruction" but this appears only during debugging when the active frame is displayed.
Used colors:
- Red - Active breakpoint.
-Orange - Inactive breakpoint.
-Light Blue - Not exact caller/current instruction pointer.
-Light Green - Exact caller instruction pointer.
-Yellow - Exact current instruction pointer.

Panels:
1. Project explorer
This panel contains the loaded assemblies and their content in a tree structure. 
By double-clicking on a leaf node, its definition will be opened.
By right clicking on a node, a context sensitive menu appears. The menu items are the following:
	- Project node:
		- Add assembly...: browse for an assembly and added to the project.
		- Reload all assemblies: all the assemblies in the project will be loaded again.
		- Properties..: display the project's properties.
	- Assembly node:
		- Set as startup assembly: this assembly will be started when debugging starts. The selected assembly's name appears with red color.
		- Reload assembly: the assembly will be reloaded from its place.	
		- Remove assembly: remove the current assembly from the project.
	- Referenced assembly node:
		- Open reference in project: the current assembly will be opened and added to the project. Before using this feature, it's a good idea to open the definition (by double-clicking on it) to check whether DILE has really found the right assembly.
			
2. Quick Search
The idea is based on the VS File Finder Add-In [5].
By typing in the textbox it's possible to search in the project's items. After each typed character, the items which name contain the typed string are shown below. By double clicking on an item, it will be opened or a "Locate in Project Explorer" menu option is also available from the context menu that will display the selected item in the Project Explorer.
The searching process can be aborted by pressing Escape.
The following items can be searched:
  - Assembly
  - AssemblyReference
  - EventDefinition
  - ModuleScope
  - ModuleReference
  - File
  - ManifestResource
  - TypeDefinition
  - MethodDefinition
  - Property
  - FieldDefinition
  - TokenValues: in this case the token number should be typed as a hexadecimal number (without 0x) and the items that can be searched are those which are also displayed in the Project Explorer (so for example signatures can't be searched)
  
In the settings dialog ("..." next to the textbox) the options can be changed per-project. These settings are stored in the .dileproj file.

3. Information Panel
Every informational message given by DILE is displayed here.

4. Debug Output Panel
During debugging the debugger is notified about different kind of events like when a thread is started, an exception occures in the debuggee, a class loaded etc. These events are shown in this panel. When an event is selected in the left list box, some additional information is displayed in the tree next to it.

5. Call Stack Panel
During debugging if the debuggee is suspended then the current call stack is displayed here. By double clicking on a row that place will be activated which means that the corresponding code, local variables and arguments are shown.
If the call stack contains a method which is not recognized by DILE then a warning is displayed instead of it. If you right click on an "Unknown method..." row you can choose from the context menu the option "Add the referenced module to the project" and the assembly will be added to the DILE project and all the information (local variables, arguments, callstack etc.) will be updated using the new assembly.
Using the context menu it is also possible to copy the whole callstack to the clipboard or display it in the Text Displayer window.

6. Breakpoints Panel
The breakpoints which were set in the code are collected here.
By double-clicking on a row DILE will navigate to the breakpoints location.
A breakpoint can be activated/deactivated by clicking on the checkbox in the first column.

7. Log Message Panel
Debug information printed by the debuggee are displayed here.
A "---------------Debuggee started--------------------" message shows the start of each debugging session.

8. Local Variables Panel
When the debuggee is suspended, the current method's local variables are displayed here. If a variable is an object then by double-clicking on it, the Object Viewer will be opened on where the object's fields/properties can be checked.

9. Arguments Panel
When the debuggee is suspended, the current method's arguments are displayed here. If an argument is an object then by double-clicking on it the Object Viewer will be opened where the object's fields/properties can be checked.

10. Auto Object Panel
When the debuggee is suspended because an exception is thrown then the exception which is being thrown will be displayed here. By double-clicking on it the Object Viewer will be opened where the exception's fields/properties can be checked.

11. Threads Panel
The debuggee's threads are displayed in this panel. By double-clicking on a row, the corresponding thread will be the "active" and its call-stack will be displayed.

12. Modules Panel
Modules which are loaded by the debuggee are shown in this panel. By double-clicking on a row, the corresponding module - after a confirmation - will be loaded by DILE and added to the current project.

13. Watch Panel
Different expressions can be added to this panel. All of these expressions will be evaluated every time the debuggee is paused (this also means that they will be refreshed after each step). By double-clicking on a row the expression will be evaluated and displayed using the Object Viewer.

-Object Viewer

This window helps to inspect an object during debugging. On the left side a tree structure can be seen which represents the object's hierarchy. The object's fields and properties which are also objects are displayed as a node. By expanding such a node its object fields and properties are displayed as child nodes. When a node is selected then the given object's fields, properties and ToString() method will be evaluated and the results will be displayed on the right side. When an evaluated result is an object then by double-clicking on it, its values will be displayed similarly as if the tree node would have been expanded and the value selected.

Different expressions can be entered in the Expression field in the top of the window that will be evaluated during debugging. Local variables, arguments and auto objects can be referred by their name (V_0, V_1, A_0, A_1, {expression} etc.). This feature is similar to the Visual Studio's Quick Watch thus the expressions have to be given similarly as well. There are some special rules however:
	- the assembly that contains the definition of a used type must be added to the DILE project
  - types must be written with their fully qualified names (System.Convert.ToString() for example and not Convert.ToString())
  - some basic types are automatically mapped, thus their short name is enough:
	 	- bool -> System.Boolean
	 	- char -> System.Char
	 	- sbyte -> System.SByte
	 	- byte -> System.Byte
	 	- short -> System.Int16
	 	- ushort -> System.UInt16
	 	- int -> System.Int32
	 	- uint -> System.UInt32
	 	- long -> System.Int64
	 	- ulong -> System.UInt64
	 	- float -> System.Single
	 	- double -> System.Double
	 	- decimal -> System.Decimal
	 	- object -> System.Object
	 	- string -> System.String
	- it is possible to create new objects using the "new" keyword (e.g.: (new object()).ToString(), new string[] {"a", "b"})
	- overloaded operators are used during the evaluation
	- implicit and explicit operators are used during the evaluation
	- it is possible to call methods that have params arguments
	- true, false and null keywords are recognized
	- generics are not supported yet

Here are a few expressions that I used for testing:
- 5 * -6
- 1 + 2 * 3 - 10 / 5 * 5
- (1 + 2 * 3 - 10 / 5 * 5).ToString()
- (-5).ToString()
- new object() + "a"
- "abc".Length.ToString()
- System.Type.GetType("System.String").GUID.ToByteArray()
- TestApplication.DebugTest.CreateOperatorTest4("op1") | true
- TestApplication.DebugTest.ParamsTest2()
- TestApplication.DebugTest.ParamsTest2(5, 6)
- System.String.Format("{0}{1}{2}{3}{4}", "a", "b", "c", "d", "e")
- new object[] {4, "a", 5}
- ((System.Exception){exception}).Message
- TestApplication.GenericClass<int, System.DateTime>.StaticMethod<string>("test")
- - new TestApplication.TestClass<int, string>[] {new TestApplication.TestClass<int, string>(1, "one")}
- TestApplication.GenericClass<int, string>.NestedGenericClass<System.Type>.StaticMixedMethod<System.DateTime>(System.DateTime.Now, 5, null)
and so on...

# Attach to process
In this dialog currently running managed processes are displayed. The .NET Framework version used by the debuggee is automatically detected. The list of managed processes can be refreshed with the Refresh button.

# Default shortcuts:
- Ctrl + N - Create a new project.
- Ctrl + O - Open an existing project.
- Ctrl + S - Save the current project.
- Ctrl + Shift + S - Save the current project to a new file.
- Ctrl + T - Settings.
- Ctrl + Tab - Select current window.
- Ctrl + Shift + W - Close all windows.
- Ctrl + P - Attach to a running process.
- Ctrl + W - Display the Object Viewer window.
- F5 - Run the selected startup project.
- F9 - Toggle breakpoint at the current line.
- F10 - Step over.
- F11 - Step into.
- F12 - Step out.
- Ctrl + F10 - Run to cursor.
  
# Disassemble related known bugs:
  - if a permission set is not stored in the assembly as an xml string then it will not be shown correctly (most likely, strange unicode characters will be displayed).
  - Build version must match architecture of debugged program (x86 or x64)
  -  Right now, if the program encounters something that is not recognized then NotImplementedException will be thrown which will be displayed in the "Information Panel". If this happens, please let me know, send me the assembly as an attached file if it's possible; or a description of how can the error be reproduced.
  
# Debugging related known bugs:
	- The stepping is confusing, more instructions are stepped at once. A good explanation of this behavior can be found in Mike Stall's blog [6].
	- In some cases, exceptions which are thrown during method evaluations are forwarded to the debuggee and thus this will affect the debuggee.

Requirements:
  - Windows
  - .NET Framework 4.7.2. The program was compiled and tested with the RTM version which is available from MSDN [2] for free.
  - in some cases, the Visual C++ 10 SP1 runtime files might be missing; these can be downloaded and installed from the Microsoft website (x86: and x64: )
  
# Source code:
The source code looks awful. There are no comments and the exception handling is also quite simple. But the original code is available in an SVN repository on SourceForge [1] for the curious ones. Github repo will be updated in conjuction with a seperate project
  
# Installation:
  xcopy 
  
# Usage:
dile [/p "Project name"] [/a "assembly path"] [/l "project name.dileproj"]

	/p	Optional. When DILE is loaded, a new project will be created with the given name.
	/a	Optional, can be repeated. When DILE is loaded, a new project will be created and the given assemblies will be added to it.
	/l	Optional. DILE will load the given dileproj file. If this parameter is given then /p and /a will be ignored.

If a parameter is followed by a name/path which contains spaces then it should be written between quotes.

# Examples:
Create a new project with the name Test project:
dile /p "Test project"

Create a new project called Test project and add the TestAssembly.exe to it:
dile /p "Test project" /a TestAssembly.exe

Create a new project and add the TestAssembly.exe and another My test.dll from a different directory:
dile /a TestAssembly.exe /a "c:\assemblies\My test.dll"

Load an existing project:
dile /l TestProject.dileproj

Feedback:
  You can contact me via Github (https://github.com/alexhiggins732/DILE/)
  
The program uses Weifen Luo's DockPanel Suite [4].
  
Links:

  [1] Original Project homepage: http://dile.sourceforge.net/
  
  [2] .NET Framework 4.0: http://www.microsoft.com/en-us/download/details.aspx?id=17851
  
  [3] My blog: http://pzsolt.blogspot.com
  
  [4] DockPanel Suite: http://sourceforge.net/projects/dockpanelsuite/
  
  [5] Visual Studio File Finder Add-In: http://www.codeproject.com/dotnet/VS_File_Finder.asp
  
  [6] Mike Stall's .NET Debugging Blog - Debugging IL: http://blogs.msdn.com/jmstall/archive/2004/10/03/237137.aspx
  



Copyright (C) 2013 Petrény Zsolt

Copyright (C) 2019 Alexander Higgins

This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
