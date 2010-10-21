#include "PartumArtificiumMain.h"
#include <OgreConfigFile.h>
#include <OgreCamera.h>
#include <OgreViewport.h>
#include <OgreSceneManager.h>
#include <OgreRenderWindow.h>
#include <OgreEntity.h>
#include <OgreWindowEventUtilities.h>

//Constructor
PartumArtificiumMain::PartumArtificiumMain(void)
: mRoot(0), mPluginsCfg(Ogre::StringUtil::BLANK), mResourcesCfg(Ogre::StringUtil::BLANK)
{
}

//Deconstructor
PartumArtificiumMain::~PartumArtificiumMain(void)
{
	delete mRoot;
}

//Run the main ogre application
bool PartumArtificiumMain::Go(void)
{
	mResourcesCfg = "resources_d.cfg";
	mPluginsCfg = "plugins_d.cfg";

	DefineRoot();

	DefineResources();

	InitializeRenderSystem();

	CreateRenderWindow();

	// Set default mipmap level (NB some APIs ignore this)
	Ogre::TextureManager::getSingleton().setDefaultNumMipmaps(5);
	// initialise all resource groups
	Ogre::ResourceGroupManager::getSingleton().initialiseAllResourceGroups();

	// Create the SceneManager, in this case a generic one
	mSceneMgr = mRoot->createSceneManager("DefaultSceneManager");

	// Create the camera
	mCamera = mSceneMgr->createCamera("PlayerCam");
	 
	// Position it at 500 in Z direction
	mCamera->setPosition(Ogre::Vector3(0,0,80));
	// Look back along -Z
	mCamera->lookAt(Ogre::Vector3(0,0,-300));
	mCamera->setNearClipDistance(5);

	// Create one viewport, entire window
	Ogre::Viewport* vp = mWindow->addViewport(mCamera);
	vp->setBackgroundColour(Ogre::ColourValue(0,0,0));
	 
	// Alter the camera aspect ratio to match the viewport
	mCamera->setAspectRatio(
		Ogre::Real(vp->getActualWidth()) / Ogre::Real(vp->getActualHeight()));


	Ogre::Entity* ogreHead = mSceneMgr->createEntity("Head", "ogrehead.mesh");
 
	Ogre::SceneNode* headNode = mSceneMgr->getRootSceneNode()->createChildSceneNode();
		headNode->attachObject(ogreHead);

	// Set ambient light
	mSceneMgr->setAmbientLight(Ogre::ColourValue(0.5, 0.5, 0.5));

	// Create a light
	Ogre::Light* l = mSceneMgr->createLight("MainLight");
		l->setPosition(20,80,50);

	return RunRenderLoop();
}

void PartumArtificiumMain::DefineRoot()
{
	mRoot = new Ogre::Root(mPluginsCfg);
}

//Load all resources for ogre to use.  Ogre will not use these resources until they
//have been initialized
void PartumArtificiumMain::DefineResources()
{
	//Setup resources
	Ogre::ConfigFile config;
	config.load(mResourcesCfg);

	//Go through all the sections and setting in the file
	Ogre::ConfigFile::SectionIterator section = config.getSectionIterator();

	Ogre::String secName, typeName, archName;
	while (section.hasMoreElements())
	{
		secName = section.peekNextKey();
		Ogre::ConfigFile::SettingsMultiMap *settings = section.getNext();
		Ogre::ConfigFile::SettingsMultiMap::iterator i;
		for (i = settings->begin(); i != settings->end(); ++i)
		{
			typeName = i->first;
			archName = i->second;
			Ogre::ResourceGroupManager::getSingleton().addResourceLocation(
				archName, typeName, secName);
		}
	}
}

void PartumArtificiumMain::InitializeRenderSystem()
{
	Ogre::RenderSystem *rs = mRoot->getRenderSystemByName("Direct3D9 Rendering Subsystem");
	mRoot->setRenderSystem(rs);
	rs->setConfigOption("Full Screen", "No");
	rs->setConfigOption("Video Mode", "800 x 600 @ 32-bit colour");

	//Using Root::getAvailableRenderers and RenderSystem::getConfigOptions we can setup
	//our own config dialog for our program
}

void PartumArtificiumMain::CreateRenderWindow()
{
	mWindow = mRoot->initialise(true, "BasicTutorial6 Render Window");
}

bool PartumArtificiumMain::RunRenderLoop()
{
	while(true)
	{
		// Pump window messages for nice behaviour
		Ogre::WindowEventUtilities::messagePump();
	 
		if(mWindow->isClosed())
		{
			return false;
		}
	 
		// Render a frame
		if(!mRoot->renderOneFrame()) return false;
	}
}