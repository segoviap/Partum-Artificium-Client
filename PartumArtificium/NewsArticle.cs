#region Copyright Partum Artificium 2010
/************************************************************************
*   Copyright Partum Artificium 2010
*   All rights reserved. Reproduction or transmission in whole or in part
*   in any form or by any means is prohibited without prior written 
*   consent of copyright owner.
*************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using PartumArtificium.Framework;

namespace PartumArtificium
{
	/// <summary> Single news article</summary>
	public class NewsArticle
	{
		#region Private Varibles
		private string _title = DefaultConstants.DefaultString;
		private string _description = DefaultConstants.DefaultString;
		private string _summary = DefaultConstants.DefaultString;
		private string _link = DefaultConstants.DefaultString;
		private int _guid = DefaultConstants.DefaultInt;
		private DateTime _publicationDate = DefaultConstants.DefaultDateTime;
		#endregion

		#region Properites
		/// <summary> </summary>
		public string Title
		{
			get { return _title; }
			set { _title = value; }
		}
		/// <summary> </summary>
		public string Description 
		{
			set { _description = value; }
			get { return _description; }
		}
		///  </summary>
		public string Summary 
		{
			set { _summary = value; }
			get { return _summary; }
		}
		/// <summary> </summary>
		public string Link
		{
			set { _link = value; }
			get { return _link; }
		}
		/// <summary> </summary>
		public int Guid
		{
			set { _guid = value; }
			get { return _guid; }
		}
		#endregion
	}

	/// <summary> </summary>
	public class NewsArticleCollection : Dictionary<int, NewsArticle>
	{
		
	}
}
