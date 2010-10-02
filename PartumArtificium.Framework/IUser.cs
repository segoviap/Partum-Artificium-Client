#region Copyright Partum Artificium 2010
/************************************************************************
*   Copyright Partum Artificium 2010
*   All rights reserved. Reproduction or transmission in whole or in part
*   in any form or by any means is prohibited without prior written 
*   consent of copyright owner.
*************************************************************************/
#endregion

namespace PartumArtificium.Framework
{
	/// <summary> </summary>
	public interface IUser
	{
		int UserId { get; set; }
		string Username { get; set; }
		string Password { get; set; }
	}
}
