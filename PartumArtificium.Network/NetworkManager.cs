using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PartumArtificium.Network
{
	/// <summary> Controls all network communication on the client </summary>
	public class NetworkManager
	{
		private EnvironmentService _environmentService = null;

		/// <summary> </summary>
		public NetworkManager() //TODO: load correct parameters
		{
			_environmentService = new EnvironmentService();
		}
	}
}
