using System;

namespace WhiteLabel.Core
{
	public class TabTappedEventArgs : EventArgs
	{
		public object Tab
		{
			get;
		}

		public TabTappedEventArgs(object tab)
		{
			Tab = tab;
		}
	}
}
