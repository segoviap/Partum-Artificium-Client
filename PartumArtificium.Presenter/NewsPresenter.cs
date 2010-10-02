using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PartumArtificium.View;

namespace PartumArtificium.Presenter
{
	/// <summary> </summary>
	public class NewsPresenter
	{
		private INewsView _view;

		/// <summary> </summary>
		/// <param name="view"></param>
		public NewsPresenter(INewsView view)
		{
			_view = view;
		}
	}
}
