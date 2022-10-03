using System;

namespace WhiteLabel.Core
{
	public class TabSelectedEventArgs : EventArgs
	{
		public object Selected
		{
			get;
		}

		public TabSelectedEventArgs(object selected)
		{
			Selected = selected;
		}
	}
}
