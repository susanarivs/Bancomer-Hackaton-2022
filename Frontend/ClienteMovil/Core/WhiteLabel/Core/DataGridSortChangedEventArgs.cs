using System;

namespace WhiteLabel.Core
{
	public class DataGridSortChangedEventArgs : EventArgs
	{
		public DataGridSortType SortType
		{
			get;
		}

		public DataGridSortChangedEventArgs(DataGridSortType sortType)
		{
			SortType = sortType;
		}
	}
}
