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
	/// <summary> Base interface for objects that will load xml files from the client</summary>
	public interface IXmlLoader
	{
		/// <summary> Load xml file to object</summary>
		/// <param name="fileName">path for file location</param>
		void LoadFile(string fileName);
	}
}
