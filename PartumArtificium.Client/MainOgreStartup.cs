#region Copyright Partum Artificium 2010
/************************************************************************
*   Copyright Partum Artificium 2010
*   All rights reserved. Reproduction or transmission in whole or in part
*   in any form or by any means is prohibited without prior written 
*   consent of copyright owner.
*************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace PartumArtificium.Client
{
	/// <remarks>
	/// This is for testing and getting the basics up and running.  Once things are figured out
	/// this will be moved and rewritten in proper design.
	/// </remarks>
	public class MainOgreStartup
	{
		protected Root _root;
		protected RenderWindow _renderWindow;
		protected float _timer = 10;

		public void InitializeOgre()
		{
            CreateRoot();

			DefineResources();

			CreateRenderSystem();

			CreateRenderWindow();

			InitializeResources();

			CreateScene();

			CreateFrameListeners();

		    EnterRenderLoop();
		}

		/// <summary> </summary>
		protected void CreateRoot()
		{
			_root = new Root();
		}

		/// <summary> </summary>
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
			renderSystem.SetConfigOption("Full Screen", "Yes");
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
			camera.Position = new Vector3(0, 0, 150);
			camera.LookAt(Vector3.ZERO);
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
		protected bool OnFrameRenderingQueued(FrameEvent evt)
		{
			//For testing just going to exit after 5 seconds
			_timer -= evt.timeSinceLastFrame;
			return (_timer > 0);
		}

		/// <summary> Main render loop for Ogre</summary>
		protected void EnterRenderLoop()
		{
			while (_root.RenderOneFrame())
			{
				// Can do other opertions not dependant on frame listeners.
			}
		}
	}
}
