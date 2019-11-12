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
    public class AgregarMarcaModeloViewModel : INotifyPropertyChanged
    {
        public string nombreMarcaModelo { get; set; }
        public string codigoMarcaModelo { get; set; }
        public ICommand enterKeyPressed { get; set; }

        public AgregarMarcaModeloViewModel()
        {
            var db = new SQLiteConnection(Preferences.Get("DB_PATH", ""));
            enterKeyPressed = new Command(async() =>
            {
                Marcas_Modelos brand = new Marcas_Modelos();
                try
                {
                    try
                    { 
                        brand.Marca = Convert.ToInt32(codigoMarcaModelo);
                    }
                    catch
                    {
                        await App.Current.MainPage.DisplayAlert("Error", "El codigo solo puede contener numeros", "ok");
                        return;
                    }
                    brand.Descripcion = nombreMarcaModelo;
                    brand.DescripcionActivo = MainCapturaDeActivosViewModel.ListaDescripcionesActivos.Where(x => x.Descripcion == MainCapturaDeActivosViewModel._Descripcion).First().DescripcionActivo;
                    brand.Fecha_Ingreso = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    brand.Fecha_Modificado = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    brand.Actualizador = 1;
                    db.Update(brand);
                    MainCapturaDeActivosViewModel.ListaMarcaModelos.Where(x => x.Marca == Convert.ToInt32(codigoMarcaModelo)).First().Descripcion = brand.Descripcion;
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Este no es un codigo de Marcas Modelos", "ok");
                }

            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
