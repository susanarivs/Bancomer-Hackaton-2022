using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WhiteLabel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactDetailPage : ContentPage
    {
        public ContactDetailPage()
            : this(-1)
        {
        }

        public ContactDetailPage(int id)
        {
            InitializeComponent();

            BindingContext = new ContactDetailViewModel(id);
        }

        private async void OnDelete(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("¿Desea eliminar este registro?", "Cancelar", null, "Si");
            switch (action)
            {
                case "Si":
                    var model = (ContactDetailViewModel)BindingContext;
                    if (await model.DeleteContact())
                    {
                        await Navigation.PopAsync();
                    }
                    break;
            }
        }
    }
}
