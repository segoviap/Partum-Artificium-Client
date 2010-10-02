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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PartumArtificium.View;

namespace PartumArtificium.Client
{
	public partial class NewsScreen : Form, INewsView
	{
		/// <summary> </summary>
		public NewsScreen()
		{
			InitializeComponent();
		}

		#region Event Methods

		/// <summary> </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonEnter_Click(object sender, EventArgs e)
		{
            //Needs to be put in proper design
            this.Close();

            MainOgreStartup startup = new MainOgreStartup();
            startup.InitializeOgre();
		}

		#endregion

		#region INewsView Members

		public string Version
		{
			set { throw new NotImplementedException(); }
		}

		public string Title
		{
			set { this.Title = value; }
		}

		#endregion
	}
}
