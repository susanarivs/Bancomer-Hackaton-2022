using System;

namespace WhiteLabel.Core
{
	public class IsCheckedChangedEventArgs : EventArgs
	{
		public bool IsChecked
		{
			get;
			set;
		}
	}
}
