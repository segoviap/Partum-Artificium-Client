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
	/// <summary> Comparable Data Object.  Used for sorting collections. </summary>
	public abstract class ComparableDataObject : DataObject, IComparable
	{
		private IComparable _sortValue = DefaultConstants.DefaultString;

		public IComparable SortValue
		{
			get { return _sortValue; }
			set { _sortValue = value; }
		}

		#region IComparable Members

		public int CompareTo(object obj)
		{
			if (obj is ComparableDataObject)
			{
				return SortValue.CompareTo(((ComparableDataObject)obj).SortValue);
			}

			return SortValue.ToString().CompareTo(obj.ToString());
		}

		#endregion
	}
}
