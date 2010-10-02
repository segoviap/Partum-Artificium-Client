#region Copyright Partum Artificium 2010
/************************************************************************
*   Copyright Partum Artificium 2010
*   All rights reserved. Reproduction or transmission in whole or in part
*   in any form or by any means is prohibited without prior written 
*   consent of copyright owner.
*************************************************************************/
#endregion

using System;

namespace PartumArtificium.Framework
{
	/// <summary> </summary>
	public abstract class DataObject
	{
		private bool _isDirty = false;

		#region Events
		/// <summary> Event raised when DataClass is marked IsDirty </summary>
		protected event EventHandler DataChanged;
		#endregion

		#region Properties
		/// <summary> Data on object changed </summary>
		/// <remarks> Be sure when first populating a data object to reset this property correctly</remarks>
		public bool IsDirty
		{
			get { return _isDirty; }
			set 
			{ 
				_isDirty = value;

				if (_isDirty && DataChanged != null)
				{
					DataChanged(this, new EventArgs());
				}
			}
		}
		#endregion
	}
}
