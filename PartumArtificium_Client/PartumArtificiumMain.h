/*
* Copyright
*/

#include "OgreRoot.h"

class PartumArtificiumMain
{
private:
	Ogre::Root* mRoot;
	Ogre::String mPluginsCfg;
	Ogre::String mResourcesCfg;
	Ogre::RenderWindow* mWindow;
	Ogre::SceneManager* mSceneMgr;
	Ogre::Camera* mCamera;

	void DefineRoot();
	void DefineResources();
	void InitializeRenderSystem();
	void CreateRenderWindow();
	bool RunRenderLoop();
public:
	PartumArtificiumMain(void);
	virtual ~PartumArtificiumMain(void);
	
	bool Go(void);
};
