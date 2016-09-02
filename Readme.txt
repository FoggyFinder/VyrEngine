The VyrEngine is a Multi-Project Solution rendering realtime graphics based on OpenGL and DotNet.
The Engine is structured as follows:
Engine:
	1. VyrCore - Core Library containing all necessary interfaces to run the vyrEngine
	2. VyrRenderer - References OpenTk, Core Library for the renderer, #ifdef implementation of specific functions
		2.1 VyrRenderer.Android/IOS - Links files of VyrRenderer
Editor:
	1. VyrVM - Core Library for all GUI-Interfaces regarding the Editor (ViewModels/MVVM Design) [generally windows class library, but implemented with #ifdef]
		1.1 VyrVM.Android - Link to VyrVM Files in order to create the android VyrVM version
		1.2 VyrVM.IOS - Link to VyrVM Files in order to create the ios VyrVM version
	2. VyrEditor.Windows - Windows-Specific implementation of all the Views. 