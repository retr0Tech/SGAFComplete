using System;
using System.Collections.Generic;
using SGAFComplete.ViewModels;
using Xamarin.Forms;

namespace SGAFComplete.Views
{
    public partial class DondeEstoyPage : ContentPage
    {
        public DondeEstoyPage()
        {
            InitializeComponent();
            BindingContext = new DondeEstoyViewModel();
        }
    }
}
