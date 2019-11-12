using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using SGAFComplete.ViewModels;

namespace SGAFComplete.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AgregarPage 
    {
        public AgregarPage()
        {
            InitializeComponent();
            BindingContext = new AgregarPageViewModel();
        }
    }
}
