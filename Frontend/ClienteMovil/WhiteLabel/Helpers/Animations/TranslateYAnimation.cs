using System;
using System.Threading.Tasks;
using Xamanimation;
using WhiteLabel.Core;

namespace WhiteLabel
{
    public class TranslateYAnimation : TranslateToAnimation, ITriggerAction
    {
        public Task Execute()
        {
            return BeginAnimation();
        }

        protected override Task BeginAnimation()
        {
            TranslateX = Target.TranslationX;

            return base.BeginAnimation();
        }
    }
}
