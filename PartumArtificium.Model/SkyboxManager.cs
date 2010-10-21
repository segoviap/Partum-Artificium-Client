using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace PartumArtificium.Models
{
	public class SkyboxManager
	{
		private SceneManager _scene;

		public SkyboxManager(SceneManager scene)
		{
			_scene = scene;
		}

		public void SetCloudyNoonSky()
		{
			_scene.SetSkyBox(true, "Examples/CloudyNoonSkyBox");
		}
	}
}
