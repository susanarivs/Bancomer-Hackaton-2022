using System.Globalization;

namespace WhiteLabel.Core
{
	public interface ICultureService
	{
		CultureInfo CurrentCulture
		{
			get;
		}

		event CultureChangedEventHandler CurrentCultureChanged;

		void SimulateCultureChange(CultureInfo ci);
	}
}
