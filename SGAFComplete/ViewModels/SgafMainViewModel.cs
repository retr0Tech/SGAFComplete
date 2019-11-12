using System;
using System.ComponentModel;
using System.Windows.Input;
using Android.Content;
using SGAFComplete.Views;
using Xamarin.Forms;

namespace SGAFComplete.ViewModels
{
    public class SgafMainViewModel : INotifyPropertyChanged
    {
        public ICommand Clicked { get; set; }
        public ICommand ClickConfig { get; set; }
        public SgafMainViewModel()
        {
            Clicked = new Command(async () => {
                await App.Current.MainPage.Navigation.PushAsync(new MainCapturaDeActivosPage());
            });
            ClickConfig = new Command(async () => {
                await App.Current.MainPage.Navigation.PushModalAsync(new SettingsPage());
            });
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
