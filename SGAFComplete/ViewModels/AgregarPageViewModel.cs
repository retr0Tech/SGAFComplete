using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using SGAFComplete.Models;
using SQLite;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SGAFComplete.ViewModels
{
    public class AgregarPageViewModel : INotifyPropertyChanged
    {
        public string nombreDescripcion {get;set;}
        public string codigoDescripcion { get; set; }

        public ICommand enterKeyPressed { get; set; }

        public AgregarPageViewModel()
        {
            enterKeyPressed = new Command(async () =>
            {
                var db = new SQLiteConnection(Preferences.Get("DB_PATH", ""));
                DescripcionesActivos descrip = new DescripcionesActivos();
                try
                {
                    try
                    {
                        descrip.DescripcionActivo = Convert.ToInt32(codigoDescripcion);
                    }
                    catch
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "El codigo solo puede contener numeros", "ok");
                        return;
                    }
                    descrip.Descripcion = nombreDescripcion;
                    descrip.PedirSerie = AgregarDescripcionViewModel.pedirSerial ? 1 : 0;
                    descrip.ClasesActivos = MainCapturaDeActivosViewModel.ListaDeClases.Where(x => x.Descripcion == AgregarDescripcionViewModel.claseSeleccionada).First().ClaseActivo;
                    descrip.Fecha_Ingreso = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    descrip.Fecha_Modificado = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    db.Update(descrip);
                    MainCapturaDeActivosViewModel.ListaDescripcionesActivos.Where(x => x.DescripcionActivo == Convert.ToInt32(codigoDescripcion)).First().Descripcion = descrip.Descripcion;
                }
                catch(Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Este no es un codigo de Descripciones", "ok");
                }

            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
