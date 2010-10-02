using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace PartumArtificium.Network
{
	/// <summary> </summary>
	public class EnvironmentService
	{
		private static EnvironmentDetails _environmentDetails;

		/// <summary> </summary>
		public EnvironmentService() //TODO: add service information parameter
		{
			Load();
		}

		public static EnvironmentDetails EnvironmentDetails
		{
			get { return _environmentDetails; }
		}

		private void Load()
		{
			//TODO: load list of settings

			//TODO: store list into a string dictionary

			//Set our environement
			_environmentDetails = new EnvironmentDetails(new StringDictionary());
		}
	}
}
