namespace WhiteLabel.Core
{
	public class AnimatedIsVisible : AnimatedBoolean
	{
		protected override void SetPropertyValue(bool value)
		{
			base.Target.IsVisible = value;
		}
	}
}
