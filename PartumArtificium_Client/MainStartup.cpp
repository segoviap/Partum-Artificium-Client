#include "windows.h"
#include "OgreException.h"
#include "PartumArtificiumMain.h"

INT WINAPI WinMain( HINSTANCE hInst, HINSTANCE, LPSTR strCmdLine, INT )
{
	PartumArtificiumMain application;

	try
	{
		application.Go();
	}
	catch(Ogre::Exception& e)
	{
		MessageBox( NULL, e.getFullDescription().c_str(), "An exception has occured!", MB_OK | MB_ICONERROR | MB_TASKMODAL);
	}

	return 0;
}