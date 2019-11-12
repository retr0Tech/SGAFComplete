using System;
using System.Collections.Generic;
using SGAFComplete.ViewModels;
using Xamarin.Forms;

namespace SGAFComplete.Views
{
    public partial class AgregarDescripcionPage : ContentPage
    {
        public AgregarDescripcionPage()
        {
            InitializeComponent();
            BindingContext = new AgregarDescripcionViewModel();
        }
    }
}
