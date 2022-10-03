using System;

namespace WhiteLabel.Core
{
	public interface IDeviceOrientation
	{
		DeviceOrientation Orientation
		{
			get;
		}

		bool IsPortrait
		{
			get;
		}

		bool IsLandscape
		{
			get;
		}

		event EventHandler OrientationChanged;
	}
}
