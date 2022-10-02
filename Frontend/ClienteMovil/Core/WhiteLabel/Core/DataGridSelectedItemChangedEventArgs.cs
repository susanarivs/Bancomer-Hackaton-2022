using System;

namespace WhiteLabel.Core
{
	public class DataGridSelectedItemChangedEventArgs : EventArgs
	{
		public object SelectedItem
		{
			get;
		}

		public DataGridSelectedItemChangedEventArgs(object selectedItem)
		{
			SelectedItem = selectedItem;
		}
	}
}
