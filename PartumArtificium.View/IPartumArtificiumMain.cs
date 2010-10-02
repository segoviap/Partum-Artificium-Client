using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PartumArtificium.View
{
	/// <summary> Base interface for all client views</summary>
	public interface IPartumArtificiumMain
	{
		/// <summary> 
		/// Exit the game.  Used to clean up everything when the user decides to quit 
		/// the game.  Can save settings, current location, etc...
		/// </summary>
		void ExitClient();
	}
}
