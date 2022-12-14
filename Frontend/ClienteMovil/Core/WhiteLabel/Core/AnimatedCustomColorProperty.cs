using Xamarin.Forms;

namespace WhiteLabel.Core
{
	public class AnimatedCustomColorProperty : AnimatedColor
	{
		public static readonly BindableProperty TargetPropertyProperty = BindableProperty.Create("TargetProperty", typeof(BindableProperty), typeof(AnimatedCustomDoubleProperty), null, BindingMode.OneWay, null, AnimatedBaseBehavior.OnChanged);

		public BindableProperty TargetProperty
		{
			get
			{
				return (BindableProperty)GetValue(TargetPropertyProperty);
			}
			set
			{
				SetValue(TargetPropertyProperty, value);
			}
		}

		protected override void SetPropertyValue(Color value)
		{
			base.Target.SetValue(TargetProperty, value);
		}
	}
}
