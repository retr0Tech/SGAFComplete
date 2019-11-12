using System;
using System.Collections.Generic;
using System.Threading;
using SGAFComplete.ViewModels;
using Xamarin.Forms;

namespace SGAFComplete.Views
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = new SettingsViewModel();
        }
    }
}
