using Xamarin.Forms;

namespace WhiteLabel.Core
{
	internal class MirrorContentPage : MirrorViewBase<ContentPage>
	{
		protected override void Mirror(ContentPage target, LayoutDirection direction, bool childrenOnly)
		{
			if (target.Content == null)
			{
				WaitForLoad(target, direction, childrenOnly);
				return;
			}
			RtlInternal.SetCurrentLayoutDirection(target, direction);
			MirrorChild(target.Content, direction);
		}
	}
}
