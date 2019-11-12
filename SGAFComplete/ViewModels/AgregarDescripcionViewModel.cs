using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Rg.Plugins.Popup.Services;
using SGAFComplete.Views;
using Xamarin.Forms;

namespace SGAFComplete.ViewModels
{
    public class AgregarDescripcionViewModel : INotifyPropertyChanged
    {
        public static bool pedirSerial { get; set; }
        public static string claseSeleccionada { get; set; }

        public ICommand BtnAgregar { get; set; }
        public ICommand BtnCerrar { get; set; }
        public static List<string> data { get; set; }
        public static ObservableCollection<string> decrpData { get; set; }
        public ObservableCollection<string> Descripcion { get; set; }
        public bool enable { get; set; }
        string sel;
        public string Selectoption
        {
            get
            {
                return sel;
            }
            set
            {
                sel = value;
                if (sel != null) { 
                    enable = true;
                    claseSeleccionada = sel;
                }
            }
        }
        public bool switchPedirSerial { get; set; }
        public bool switchPedirERP { get; set; }

        public AgregarDescripcionViewModel()
        {
            pedirSerial = switchPedirSerial;
            Descripcion = new ObservableCollection<string>();
            foreach(var i in MainCapturaDeActivosViewModel.ListaDeClases)
            {
                Descripcion.Add(i.Descripcion);
            }
            data = Descripcion.ToList();
            decrpData = Descripcion;
            BtnAgregar = new Command(async ()=>
            {
                await PopupNavigation.Instance.PushAsync(new AgregarPage());
            });
            BtnCerrar = new Command(async () =>
            {
                await App.Current.MainPage.Navigation.PopModalAsync();
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
