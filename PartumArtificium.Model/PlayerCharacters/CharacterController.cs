using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace PartumArtificium.Model.PlayerCharacters
{
	/// <summary> </summary>
	/// <remarks> Testing only.  will be moved around correctly.</remarks>
	public abstract class CharacterController
	{
		private SceneManager _sceneManager;

		public CharacterController(Camera camera)
		{
			_sceneManager = camera.SceneManager;

			SetupBody();
		}

		public SceneManager SceneManager
		{
			get { return _sceneManager; }
		}

		protected abstract void SetupBody();
	}
}
