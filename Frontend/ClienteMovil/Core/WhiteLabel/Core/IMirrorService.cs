using Xamarin.Forms;

namespace WhiteLabel.Core
{
	public interface IMirrorService
	{
		void Mirror(VisualElement target, LayoutDirection direction);
	}
}
