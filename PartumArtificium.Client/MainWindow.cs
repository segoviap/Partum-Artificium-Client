#region Copyright Partum Artificium 2010
/************************************************************************
*   Copyright Partum Artificium 2010
*   All rights reserved. Reproduction or transmission in whole or in part
*   in any form or by any means is prohibited without prior written 
*   consent of copyright owner.
*************************************************************************/
#endregion

using System;
using Mogre;
using MOIS;

namespace PartumArtificium.Client
{
    /// <summary>
    /// Main window for rendering 3d world
    /// </summary>
	/// <remarks>
	/// This is for testing and getting the basics up and running.  Once things are figured out
	/// this will be moved and rewritten in proper design.
	/// </remarks>
	public class MainWindow
	{
		private Root _root;
        private RenderWindow _renderWindow;
        private Keyboard _keyboard;
        private Mouse _mouse;
        private MOIS.InputManager _inputManager;
        private bool _shutingDown = false;

        /// <summary> Initialize 3D window</summary>
		public void InitializeWindow()
		{
            CreateRoot();

			DefineResources();

			CreateRenderSystem();

			CreateRenderWindow();

			CreateInputSystem();

			InitializeResources();

			CreateScene();

			CreateFrameListeners();

		    EnterRenderLoop();
		}

		/// <summary> Create the root for ogre</summary>
		protected void CreateRoot()
		{
			_root = new Root();
		}

		/// <summary> Define resources needed by application.  Should be located in config file.</summary>
		/// <remarks> 
		/// The resource file must contain all possible resources that the application may use.  These
		/// resources will not be initialized for use till they are actually needed.  You must initialize
		/// them before Ogre can use them.
		/// </remarks>
		protected void DefineResources()
		{
			ConfigFile configFile = new ConfigFile();
			configFile.Load(@"resources.cfg", "\t:=", true);

			var section = configFile.GetSectionIterator();
			while (section.MoveNext())
			{
				foreach (var line in section.Current)
				{
					ResourceGroupManager.Singleton.AddResourceLocation(
						line.Value, line.Key, section.CurrentKey);
				}
			}
		}

		/// <summary> </summary>
		protected void CreateRenderSystem()
		{
			RenderSystem renderSystem = _root.GetRenderSystemByName("Direct3D9 Rendering Subsystem");
			renderSystem.SetConfigOption("Full Screen", "No");
			renderSystem.SetConfigOption("Video Mode", "800 x 600 @ 32-bit colour");
			_root.RenderSystem = renderSystem;
		}

		/// <summary> </summary>
		protected void CreateRenderWindow()
		{
			_renderWindow = _root.Initialise(true, "Main Ogre Window");
		}

		/// <summary> </summary>
		protected void InitializeResources()
		{
			TextureManager.Singleton.DefaultNumMipmaps = 5;
			ResourceGroupManager.Singleton.InitialiseAllResourceGroups();
		}

		/// <summary> </summary>
		protected void CreateScene()
		{
			SceneManager sceneMgr = _root.CreateSceneManager(SceneType.ST_GENERIC);
			Camera camera = sceneMgr.CreateCamera("Camera");
			camera.Position = new Mogre.Vector3(0, 0, 150);
			camera.LookAt(Mogre.Vector3.ZERO);
			_renderWindow.AddViewport(camera);

			//Could add something to the scene.  To do this need the ogrehead.mesh resource.
		}

		/// <summary> </summary>
		protected void CreateFrameListeners()
		{
            //IMPORTANT: this is very important till more logic is added to exist the program.  If you remove this
            //and do not program a way to exit the ogre window it will not allow you to exit.
			_root.FrameRenderingQueued += new FrameListener.FrameRenderingQueuedHandler(OnFrameRenderingQueued);
		}

		/// <summary> Handles main logic of the application</summary>
		/// <param name="evt"></param>
		/// <returns></returns>
		protected bool OnFrameRenderingQueued(FrameEvent e)
		{
			//For testing just going to exit after 5 seconds
			return true;
		}

		/// <summary> Main render loop for Ogre</summary>
		protected void EnterRenderLoop()
		{
			while (_root.RenderOneFrame())
			{
				// Can do other opertions not dependant on frame listeners.
                CaptureInputDevice();

				//Can work on some final clean up here
				if (_shutingDown)
				{
					//Remove all listeners
					_renderWindow.RemoveAllListeners();

					//Clean up the user input devices
					ShutDownInput();

					return;
				}
			}
		}

		/// <summary> </summary>
		protected void CreateInputSystem()
		{
			ParamList parameterList = new ParamList();
			IntPtr windowPtr;

			//Get the rendered windows handle and set it the the parameter list which will be used
			//to listen for input from the keyboard and the mouse.
            _renderWindow.GetCustomAttribute("WINDOW", out windowPtr);
			parameterList.Insert("WINDOW", windowPtr.ToString());

			_inputManager = MOIS.InputManager.CreateInputSystem(parameterList);

			//Create the two inputs we will be reading from.  In our case it will be the keyboard and the mouse.
			_keyboard = (MOIS.Keyboard)_inputManager.CreateInputObject(MOIS.Type.OISKeyboard, true);
			_mouse = (MOIS.Mouse)_inputManager.CreateInputObject(MOIS.Type.OISMouse, true);

			_keyboard.KeyPressed += new KeyListener.KeyPressedHandler(_keyboard_KeyPressed);
			_keyboard.KeyReleased += new KeyListener.KeyReleasedHandler(_keyboard_KeyReleased);
		}

		protected bool _keyboard_KeyReleased(KeyEvent arg)
		{
			return true;
		}

		protected bool _keyboard_KeyPressed(KeyEvent e)
		{
			if (e.key == KeyCode.KC_ESCAPE)
			{
				_shutingDown = true;
			}

			return true;
		}

		protected void CaptureInputDevice()
		{
			_keyboard.Capture();
			_mouse.Capture();
		}

		protected void ShutDownInput()
		{
			if (_inputManager != null)
			{
				_inputManager.DestroyInputObject(_mouse);
				_inputManager.DestroyInputObject(_keyboard);

				MOIS.InputManager.DestroyInputSystem(_inputManager);
				_inputManager = null;
			}
		}
	}
}
