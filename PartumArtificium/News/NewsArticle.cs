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

namespace PartumArtificium.News
{
	/// <summary> </summary>
	public class NewsArticle : ComparableDataObject
	{
        private int _uniqueId;
        private DateTime _newsDate = DefaultConstants.DefaultDateTime;
		private string _title;
		private string _text;
		private string _summary;

        public NewsArticle()
        {
            //Default compare value will be the data the article was submited
            SortValue = _newsDate;
        }

        /// <summary>Get/Set Unique ID for News Article</summary>
        public int UniqueId
        {
            get { return _uniqueId; }
            set { _uniqueId = value; }
        }

        /// <summary> Get/Set Title</summary>
		public string Title
		{
			get { return _title; }
			set 
            { 
                _title = value;
                IsDirty = true;
            }
		}
        /// <summary> Get/Set Text</summary>
		public string Text 
		{
			set 
            { 
                _text = value;
                IsDirty = true;
            }
		}
        /// <summary> Get/Set Summary</summary>
		public string Summary 
		{
			set 
            {
                _summary = value;
                IsDirty = true;
            }
		}
	}

	/// <summary> </summary>
	public class NewsArticleCollection : Dictionary<int, NewsArticle>
	{
		
	}
}
