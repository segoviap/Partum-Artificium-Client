using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PartumArtificium.View
{
	/// <summary> </summary>
	public interface ILoginView : IPartumArtificiumMain
	{
		string Username { get; set; }
		string Password { get; set; }
	}
}
