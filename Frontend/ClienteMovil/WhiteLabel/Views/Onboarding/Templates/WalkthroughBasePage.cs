using System.Collections.Specialized;
using System.Threading.Tasks;
using WhiteLabel.Views.Identity;
using Xamarin.Forms;

namespace WhiteLabel.Views.Onboarding.Templates
{
    public class WalkthroughBasePage : CarouselPage
    {
        public async Task MoveNext()
        {
            var index = Children.IndexOf(CurrentPage);

            if (index < Children.Count - 1)
            {
                CurrentPage = Children[index + 1];
            }
            else
            {
                await Close();
            }
        }

        public async Task Close()
        {
            //if (Navigation.ModalStack.Count > 0)
            //{
            //    await Navigation.PopModalAsync();
            //}
            await Navigation.PushAsync(new OtpVerificationPage());
            Navigation.RemovePage(this);
        }

        protected override void OnPagesChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnPagesChanged(e);

            if (e.NewItems?.Count > 0)
            {
                for (var i = 0; i < e.NewItems.Count; i++)
                {
                    (e.NewItems[i] as WalkthroughBaseStepItemTemplate)?.Initialize();
                }
            }
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();

            for (var i = 0; i < Children.Count; i++)
            {
                var item = Children[i];

                if (item == CurrentPage)
                {
                    (item as WalkthroughBaseStepItemTemplate)?.ItemAppear();
                }
                else
                {
                    (item as WalkthroughBaseStepItemTemplate)?.ItemDisappear();
                }
            }
        }
    }
}
