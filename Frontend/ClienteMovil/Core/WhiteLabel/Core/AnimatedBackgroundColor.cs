using Xamarin.Forms;

namespace WhiteLabel.Core
{
	public class AnimatedBackgroundColor : AnimatedColor
	{
		protected override void SetPropertyValue(Color value)
		{
			base.Target.BackgroundColor = value;
		}
	}
}
