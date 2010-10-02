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
using System.Linq;
using System.Text;

namespace PartumArtificium.Constants
{
	/// <summary> Constants for nodes and attributes related to the RSS Feed</summary>
	public class RSSConstants
	{
		public const string RSSNode = "rss";
		public const string RSSVersionAttribute = "version";

		/// <summary> Constants for RSS news channel</summary>
		public class NewsChannel
		{
			public const string ChannelNode = "NewsChannel";
			public const string TitleNode = "Title";
			public const string DescriptionNode = "DescriptionNode";
			public const string LinkNode = "Link";
			public const string BuildDateNode = "BuildDate";
			public const string PublishedDateNode = "PublishedDate";
			public const string ItemNode = "Item";

			/// <summary> Constants for each RSS news item</summary>
			public class Item
			{
				public const string TitleNode = "Title";
				public const string DescriptionNode = "Description";
				public const string LinkNode = "Link";
				public const string GuidNode = "Guid";
				public const string PublicationDateNode = "PublicationDate";
				public const string SummaryNode = "SummaryNode";
				public const string IsHtmlSummaryNode = "IsHtmlSummary";
			}
		}
	}
}
