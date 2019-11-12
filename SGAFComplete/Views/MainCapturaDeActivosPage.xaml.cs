using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SGAFComplete.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SGAFComplete.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainCapturaDeActivosPage : TabbedPage
    {
        public MainCapturaDeActivosPage()
        {
            InitializeComponent();
            BindingContext = new MainCapturaDeActivosViewModel();
            myPicker.SelectedIndex = 0;

        }
        protected override void OnAppearing()
        {
            MessagingCenter.Subscribe<MainCapturaDeActivosViewModel, string>(this, "Code", ((sender, barcode) =>
            {
                if(barcode == "Change")
                    this.CurrentPage = this.Children[0];
                else if(barcode == "Change1")
                    this.CurrentPage = this.Children[1];
            }));
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<MainCapturaDeActivosViewModel, string>(this, "Code");
            base.OnDisappearing();
        }
        private void TabChanged(object sender, EventArgs e)
        {
            var tabbedPage = (TabbedPage)sender;
            Title = tabbedPage.CurrentPage.Title;
            MessagingCenter.Send<MainCapturaDeActivosPage, string>(this, "Code", this.CurrentPage.Title);
        }
    }
}
