using System;
using System.Collections.Generic;
using Android.Content;
using SGAFComplete.ViewModels;
using Xamarin.Forms;

namespace SGAFComplete.Views
{
    public partial class SgafMainPage : ContentPage
    {
        public SgafMainPage(Context context)
        {
            InitializeComponent();
            BindingContext = new SgafMainViewModel();
        }
    }
}
