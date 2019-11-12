using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Com.Cipherlab.Rfid;
using Rg.Plugins.Popup.Services;
using SGAFComplete.Droid;
using SGAFComplete.Models;
using SGAFComplete.Views;
using SQLite;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SGAFComplete.ViewModels
{
    public class MainCapturaDeActivosViewModel : INotifyPropertyChanged
    {
        #region Class Properties

        /*
         * Empresa 0
         * Edificio 1
         * Piso 2
         * Centro de Costo 3
         * Oficina 4
         * Description 5
         * Marca/Modelo 6
         * Empleado 7
         */

        int ListaActual = 0;
        private long selectedIndex;
        public static List<ActivosFijos> ListaDeActivos; //Activos agregados
        public List<Companias> ListaDeEmpresas; //Se declara la Lista de Empresas
        public List<Localidades> ListaDeEdificios; //Se declara la Lista de Edificios
        public List<Areas> ListaPisos; //Se declara la Lista de pisos
        public List<Departamentos> ListaCentroCostos; //Se declaran los centros de costos
        public List<Oficinas> ListaDeOficinas; //Se declara la Lista de las oficinas
        public static List<DescripcionesActivos> ListaDescripcionesActivos; //Se declara la Lista de descripciones
        public static List<Marcas_Modelos> ListaMarcaModelos; //Se declara la Lista de Marcas y modelos
        public List<Empleados> ListaDeEmpleados; //Se declara la Lista de Empleados
        public static List<ClasesActivos> ListaDeClases; //Se declara la Lista de clases de activos
        public string CurrentTab { get; set; }
        public ICommand Salir { get; set; } //Salir
        public ICommand config { get; set; } //Settings

        #endregion

        #region Static Properties

        public static string _Empresa; //Companias
        public static string _Edificio; //Localidades
        public static string _Piso; //Areas
        public static string _CentroCosto; //Departamentos 
        public static string _Oficina; //Oficinas
        public static string _Descripcion; //DescripcionesActivos
        public static string _MarcaModelo; //Marcas_Modelos
        public static string _Empleado; //Empleados
        public static List<string> data { get; set; }
        public static ObservableCollection<string> decrpData { get; set; }
        public static string tagsPrefijo { get; set; }

        #endregion

        #region Propiedades Parametros

        public ICommand BtnUbicacion { get; set; }
        public ICommand BtnAgregar { get; set; }
        public bool Habilitar { get; set; }
        public string Busqueda { get; set; }
        public ObservableCollection<string> ListaDescripcion { get; set; }
        public ObservableCollection<string> Descripcion { get; set; }
        string descrip;
        int listadedescripciones;
        public ICommand AnteriorBtn { get; set; }
        public bool HabilitarAnterior { get; set; }
        public string ListaDe
        {
            get
            {
                return descrip;
            }
            set
            {
                descrip = value;
                if (descrip != null)
                {
                    if (descrip == "Empresa")
                    {
                        listadedescripciones = 0;
                    }
                    else if (descrip == "Edificio")
                    {
                        listadedescripciones = 1;
                    }
                    else if (descrip == "Piso")
                    {
                        listadedescripciones = 2;
                    }
                    else if (descrip == "Centro de Costo")
                    {
                        listadedescripciones = 3;
                    }
                    else if (descrip == "Oficina")
                    {
                        listadedescripciones = 4;
                    }
                    else if (descrip == "Descripción")
                    {
                        listadedescripciones = 5;
                    }
                    else if (descrip == "Marca/Modelo")
                    {
                        listadedescripciones = 6;
                    }
                    else
                    {
                        listadedescripciones = 7;
                    }
                    Habilitar = false;
                    switch (listadedescripciones)
                    {
                        case 0:
                            Descripcion.Clear();
                            data.Clear();
                            HabilitarAnterior = false;
                            foreach (var i in ListaDeEmpresas)
                            {
                                data.Add(i.Descripcion);
                            }
                            foreach (var element in data)
                                Descripcion.Add(element);
                            decrpData = Descripcion;
                            ListaActual = 0;
                            ListaDe = ListaDescripcion[ListaActual];
                            break;

                        case 1:
                            Descripcion.Clear();
                            data.Clear();
                            HabilitarAnterior = true;
                            foreach (var i in ListaDeEdificios.Where(x => x.Parent == (ListaDeEmpresas.Where(p => p.Descripcion == _Empresa)).First().Compania))
                            {
                                data.Add(i.Descripcion);
                            }
                            foreach (var element in data)
                                Descripcion.Add(element);
                            decrpData = Descripcion;
                            ListaActual = 1;
                            ListaDe = ListaDescripcion[ListaActual];
                            break;
                        case 2:
                            Descripcion.Clear();
                            data.Clear();
                            foreach (var i in ListaPisos.Where(x => x.Parent == (ListaDeEdificios.Where(p => p.Descripcion == _Edificio)).First().Localidad))
                            {
                                data.Add(i.Descripcion);
                            }
                            foreach (var i in data)
                                Descripcion.Add(i);
                            data = Descripcion.ToList();
                            decrpData = Descripcion;
                            ListaActual = 2;
                            ListaDe = ListaDescripcion[ListaActual];
                            break;
                        case 3:
                            Descripcion.Clear();
                            data.Clear();
                            foreach (var i in ListaCentroCostos.Where(x => x.Parent == (ListaDeEmpresas.Where(p => p.Descripcion == _Empresa)).First().Compania))
                            {
                                data.Add(i.Descripcion);
                            }
                            foreach (var i in data)
                                Descripcion.Add(i);
                            data = Descripcion.ToList();
                            decrpData = Descripcion;
                            ListaActual = 3;
                            ListaDe = ListaDescripcion[ListaActual];
                            break;
                        case 4:
                            Descripcion.Clear();
                            data.Clear();
                           
                            foreach (var i in ListaDeOficinas.Where(x => x.Parent == (ListaPisos.Where(p => p.Descripcion == _Piso)).First().Area))
                            {
                                data.Add(i.Descripcion);
                            }
                            foreach (var i in data)
                                Descripcion.Add(i);
                            data = Descripcion.ToList();
                            decrpData = Descripcion;
                            ListaActual = 4;
                            ListaDe = ListaDescripcion[ListaActual];
                            break;
                        case 5:
                            Descripcion.Clear();
                            data.Clear();
                            Habilitar = true;
                            foreach (var i in ListaDescripcionesActivos)
                            {
                                data.Add(i.Descripcion);
                            }
                            foreach (var i in data)
                                Descripcion.Add(i);
                            data = Descripcion.ToList();
                            decrpData = Descripcion;
                            ListaActual = 5;
                            ListaDe = ListaDescripcion[ListaActual];
                            break;
                        case 6:
                            Descripcion.Clear();
                            data.Clear();
                            Habilitar = true;
                            foreach (var i in ListaMarcaModelos.Where(x => x.DescripcionActivo == (ListaDescripcionesActivos.Where(p => p.Descripcion == _Descripcion)).First().DescripcionActivo))
                            {
                                data.Add(i.Descripcion);
                            }
                            foreach (var i in data)
                                Descripcion.Add(i);
                            data = Descripcion.ToList();
                            decrpData = Descripcion;
                            ListaActual = 6;
                            ListaDe = ListaDescripcion[ListaActual];
                            break;
                        case 7:
                            Descripcion.Clear();
                            data.Clear();

                            foreach (var i in ListaDeEmpleados)
                            {
                                data.Add(i.Descripcion);
                            }
                            foreach (var i in data)
                                Descripcion.Add(i);
                            data = Descripcion.ToList();
                            decrpData = Descripcion;
                            ListaActual = 7;
                            ListaDe = ListaDescripcion[ListaActual];
                            break;
                    }
                }
            }
        }
        public long SelectedIndex
        {
            get
            {
                return selectedIndex;
            }
            set
            {
                selectedIndex = value;
                NotifyPropertyChanged("SelectedIndex");
            }
        }
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
                Habilitar = false;
                switch (ListaActual)
                {
                    case 0:
                        if (sel != null)
                        {
                            _Empresa = sel;
                            //Descripcion.Clear();
                            //data.Clear();
                            //foreach (var i in ListaDeEdificios.Where(x => x.Parent == (ListaDeEmpresas.Where(p => p.Descripcion == _Empresa)).First().Compania))
                            //{ 
                            //    data.Add(i.Descripcion);
                            //}
                            //foreach (var element in data)
                            //    Descripcion.Add(element);
                            //decrpData = Descripcion;
                            ListaActual++;
                            ListaDe = ListaDescripcion[ListaActual];
                        }
                        break;

                    case 1:
                        if (sel != null)
                        {
                            _Edificio = sel;
                            //Descripcion.Clear();
                            //data.Clear();
                            //foreach (var i in ListaPisos.Where(x => x.Parent == (ListaDeEdificios.Where(p => p.Descripcion == _Edificio)).First().Localidad))
                            //{
                            //    data.Add(i.Descripcion);
                            //}
                            //foreach (var element in data)
                            //    Descripcion.Add(element);
                            //decrpData = Descripcion;
                            ListaActual++;
                            ListaDe = ListaDescripcion[ListaActual];
                        }
                        break;
                    case 2:
                        if (sel != null)
                        {
                            _Piso = sel;
                            //Descripcion.Clear();
                            //data.Clear();
                            //foreach (var i in ListaCentroCostos.Where(x => x.Parent == ListaCentroCostos.Where(p => ListaCentroCostos.Any(l => p.Descripcion == l.Descripcion)).First().Parent))
                            //{
                            //    data.Add(i.Descripcion);
                            //}
                            //foreach (var i in data)
                            //    Descripcion.Add(i);
                            //data = Descripcion.ToList();
                            //decrpData = Descripcion;
                            ListaActual++;
                            ListaDe = ListaDescripcion[ListaActual];
                        }
                        break;
                    case 3:
                        if (sel != null)
                        {
                            _CentroCosto = sel;
                            LinkCentroCosto = sel;
                            //Descripcion.Clear();
                            //data.Clear();
                            //foreach (var i in ListaDeOficinas.Where(x => x.Parent == ListaDeOficinas.Where(p => ListaDeOficinas.Any(l => p.Descripcion == l.Descripcion)).First().Parent))
                            //{
                            //    data.Add(i.Descripcion);
                            //}
                            //foreach (var i in data)
                            //    Descripcion.Add(i);
                            //data = Descripcion.ToList();
                            //decrpData = Descripcion;
                            ListaActual++;
                            ListaDe = ListaDescripcion[ListaActual];
                        }
                        break;
                    case 4:
                        if (sel != null)
                        {
                            _Oficina = sel;
                            LinkOficina = sel;
                            //Descripcion.Clear();
                            //data.Clear();
                            Habilitar = true;
                            //foreach (var i in ListaDescripcionesActivos)
                            //{
                            //    data.Add(i.Descripcion);
                            //}
                            //foreach (var i in data)
                            //    Descripcion.Add(i);
                            //data = Descripcion.ToList();
                            //decrpData = Descripcion;
                            ListaActual++;
                            ListaDe = ListaDescripcion[ListaActual];
                        }
                        break;
                    case 5:
                        if (sel != null)
                        {
                            _Descripcion = sel;
                            LinkDescripcion = sel;
                            //Descripcion.Clear();
                            //data.Clear();
                            Habilitar = true;
                            //foreach (var i in ListaMarcaModelos.Where(x => x.DescripcionActivo == ListaMarcaModelos.Where(p => ListaMarcaModelos.Any(l => p.Descripcion == l.Descripcion)).First().DescripcionActivo))
                            //{
                            //    data.Add(i.Descripcion);
                            //}
                            //foreach (var i in data)
                            //    Descripcion.Add(i);
                            //data = Descripcion.ToList();
                            //decrpData = Descripcion;
                            ListaActual++;
                            ListaDe = ListaDescripcion[ListaActual];
                        }
                        break;
                    case 6:
                        if (sel != null)
                        {
                            _MarcaModelo = sel;
                            LinkMarcaModelo = sel;
                            //Descripcion.Clear();
                            //data.Clear();
                            //foreach (var i in ListaDeEmpleados)
                            //{
                            //    data.Add(i.Descripcion);
                            //}
                            //foreach (var i in data)
                            //    Descripcion.Add(i);
                            //data = Descripcion.ToList();
                            //decrpData = Descripcion;
                            ListaActual++;
                            ListaDe = ListaDescripcion[ListaActual];
                        }
                        break;
                    case 7:
                        if (sel != null)
                        {
                            _Empleado = sel;
                            MessagingCenter.Send<MainCapturaDeActivosViewModel, string>(this, "Code", "Change1");
                        }
                        break;
                }


            }
        }
        public ICommand enter { get; set; }

        #endregion

        #region Propiedades Entrada

        public string LinkCentroCosto { get; set; }
        public ICommand tappedCentroCosto { get; set; }
        public string LinkOficina { get; set; }
        public ICommand tappedOficina { get; set; }
        public string Tag { get; set; }
        public string LinkDescripcion { get; set; }
        public ICommand tappedDescripcion { get; set; }
        public string LinkMarcaModelo { get; set;  }
        public ICommand tappedMarcaModelo { get; set; }
        public string RFIDTag { get; set; }
        public ICommand enterPressed { get; set; }

        #endregion

        #region Propiedades Consulta

        public string RFIDTagParaAsignar { get; set; }
        public string Empresa { get; set; }
        public string Piso { get; set; }
        public string CentroCosto { get; set; }
        public string Oficina { get; set; }
        public string DescripcionActivo { get; set; }
        public string Serial { get; set; }
        public string Empleado { get; set; }
        public ICommand enterPressedConsulta { get; set; }

        #endregion

        #region Propiedades Anterior
        public ICommand PrimerActivo { get; set; }
        public ICommand ActivoAnterior { get; set; }
        public ICommand ActivoSiguiente { get; set; }
        public ICommand UltimoActivo { get; set; }
        public string OficinaAnterior { get; set; }
        public string DescripcionAnterior { get; set; }
        public string TAG { get; set; }
        public bool FirstAsset { get; set; }
        public bool PreviousAsset { get; set; }
        public bool NextAsset { get; set; }
        public bool LastAsset { get; set; }
        #endregion

        public MainCapturaDeActivosViewModel()
        {
            #region Llenado de Listas

            //Llenado de La lista de Empresas
            ListaDeEmpresas = new List<Companias>();
            ListaDeActivos = new List<ActivosFijos>();
            Descripcion = new ObservableCollection<string>();
            var db = new SQLiteConnection(Preferences.Get("DB_PATH", ""));
            var querry = "SELECT * FROM Companias";
            try
            {
                //Ejecutamos el query.
                var registro = db.Query<Companias>(querry);

                if (registro.First().Descripcion == null)
                {
                    //No hacer nada por ahora
                }
                else
                {
                    for (int i = 0; i < registro.Count; i++)
                    {
                        ListaDeEmpresas.Add(registro[i]);
                    }
                }

            }
            catch
            {

            }
            if(ListaDeEmpresas.Count > 0)
                tagsPrefijo = ListaDeEmpresas.First().PrefijoRFID;
            //Terminamos el Llenado de La lista de Empresas

            //Llenado de la Lista de Edificios
            ListaDeEdificios = new List<Localidades>();
            var querryEdificios = "SELECT * FROM Localidades";
            try
            {
                //Ejecutamos el query.
                var registro = db.Query<Localidades>(querryEdificios);

                if (registro.First().Descripcion == null)
                {
                    //No hacer nada por ahora
                }
                else
                {
                    for (int i = 0; i < registro.Count; i++)
                    {
                        ListaDeEdificios.Add(registro[i]);
                    }
                }

            }
            catch
            {

            }
            //Terminamos el Llenado de La lista de Edificios

            //Llenado de la Lista de pisos
            ListaPisos = new List<Areas>();
            var querryPisos = "SELECT * FROM Areas";
            try
            {
                //Ejecutamos el query.
                var registro = db.Query<Areas>(querryPisos);

                if (registro.First().Descripcion == null)
                {
                    //No hacer nada por ahora
                }
                else
                {
                    for (int i = 0; i < registro.Count; i++)
                    {
                        ListaPisos.Add(registro[i]);
                    }
                }

            }
            catch
            {

            }
            //Terminamos el Llenado de La lista de pisos

            //Llenado de la Lista de Centro de costos
            ListaCentroCostos = new List<Departamentos>();
            var querryCentroCosto = "SELECT * FROM Departamentos";
            try
            {
                //Ejecutamos el query.
                var registro = db.Query<Departamentos>(querryCentroCosto);

                if (registro.First().Descripcion == null)
                {
                    //No hacer nada por ahora
                }
                else
                {
                    for (int i = 0; i < registro.Count; i++)
                    {
                        ListaCentroCostos.Add(registro[i]);
                    }
                }
            }
            catch
            {

            }
            //Terminamos el Llenado de La lista de Centro de costos

            //Llenado de la Lista de Oficinas
            ListaDeOficinas = new List<Oficinas>();
            var querryOficinas = "SELECT * FROM Oficinas";
            try
            {
                //Ejecutamos el query.
                var registro = db.Query<Oficinas>(querryOficinas);

                if (registro.First().Descripcion == null)
                {
                    //No hacer nada por ahora
                }
                else
                {
                    for (int i = 0; i < registro.Count; i++)
                    {
                        ListaDeOficinas.Add(registro[i]);
                    }
                }
            }
            catch
            {

            }
            //Terminamos el Llenado de la lista de Oficinas

            //Llenado de la Lista de Descripciones
            ListaDescripcionesActivos = new List<DescripcionesActivos>();
            var querryDescripciones = "SELECT * FROM DescripcionesActivos";
            try
            {
                //Ejecutamos el query.
                var registro = db.Query<DescripcionesActivos>(querryDescripciones);

                if (registro.First().Descripcion == null)
                {
                    //No hacer nada por ahora
                }
                else
                {
                    for (int i = 0; i < registro.Count; i++)
                    {
                        ListaDescripcionesActivos.Add(registro[i]);
                    }
                }
            }
            catch
            {

            }
            //Terminamos el Llenado de la lista de descripciones

            //Llenado de la Lista de MarcaModelos
            ListaMarcaModelos = new List<Marcas_Modelos>();
            var querryMarca = "SELECT * FROM Marcas_Modelos";
            try
            {
                //Ejecutamos el query.
                var registro = db.Query<Marcas_Modelos>(querryMarca);

                if (registro.First().Descripcion == null)
                {
                    //No hacer nada por ahora
                }
                else
                {
                    for (int i = 0; i < registro.Count; i++)
                    {
                        ListaMarcaModelos.Add(registro[i]);
                    }
                }
            }
            catch
            {

            }
            //Terminamos el Llenado de la Lista de MarcaModelos

            //Llenado de la lista de EMpleados
            ListaDeEmpleados = new List<Empleados>();
            var querryEmpleado = "SELECT * FROM Empleados";
            try
            {
                //Ejecutamos el query.
                var registro = db.Query<Empleados>(querryEmpleado);

                if (registro.First().Descripcion == null)
                {
                    //No hacer nada por ahora
                }
                else
                {
                    for (int i = 0; i < registro.Count; i++)
                    {
                        ListaDeEmpleados.Add(registro[i]);
                    }
                }
            }
            catch
            {

            }
            //Terminamos el llenado de la lista de empleados

            //Llenado de Lista de clases
            ListaDeClases = new List<ClasesActivos>();
            var querryClases = "SELECT * FROM ClasesActivos";
            try
            {
                //Ejecutamos el query.
                var registro = db.Query<ClasesActivos>(querryClases);

                if (registro.First().Descripcion == null)
                {
                    //No hacer nada por ahora
                }
                else
                {
                    for (int i = 0; i < registro.Count; i++)
                    {
                        ListaDeClases.Add(registro[i]);
                    }
                }
            }
            catch
            {

            }
            //Terminamos el llenado de la lista de clases

            #endregion

            #region Metodos Parametros

            ListaDescripcion = new ObservableCollection<string>();


            //Insertar Descripciones 
            ListaDescripcion.Add("Empresa");
            ListaDescripcion.Add("Edificio");
            ListaDescripcion.Add("Piso");
            ListaDescripcion.Add("Centro de Costo");
            ListaDescripcion.Add("Oficina");
            ListaDescripcion.Add("Descripción");
            ListaDescripcion.Add("Marca/Modelo");
            ListaDescripcion.Add("Empleado");

            //Insertar ubicaciones
            foreach (var element in ListaDeEmpresas)
            {
                Descripcion.Add(element.Descripcion);
            }
            Habilitar = false;
            //Referencias
            data = Descripcion.ToList();
            decrpData = Descripcion;

            BtnUbicacion = new Command(async () =>
            {
                await App.Current.MainPage.Navigation.PushAsync(new DondeEstoyPage());
            });
            BtnAgregar = new Command(async ()=>
            {
                if (ListaActual == 5)
                {
                    await App.Current.MainPage.Navigation.PushModalAsync(new AgregarDescripcionPage());
                }
                else
                {
                    await PopupNavigation.Instance.PushAsync(new AgregarMarcaModeloPage());
                }
            });

            AnteriorBtn = new Command(async ()=>
            {
                ListaActual--;
                ListaDe = ListaDescripcion[ListaActual];
            });

            #endregion

            #region Metodos Entrada

            #region Metodos

            MainActivity.mRfidManager.SetScanMode(ScanMode.Single);
            LinkCentroCosto = string.IsNullOrEmpty(CentroCosto) ? "Seleccionar Centro de Costos" : _CentroCosto;
            tappedCentroCosto = new Command(async () =>
            {

                MessagingCenter.Send<MainCapturaDeActivosViewModel, string>(this, "Code", "Change");
                Descripcion.Clear();
                data.Clear();
                foreach (var i in ListaCentroCostos.Where(x => x.Parent == ListaCentroCostos.Where(p => ListaCentroCostos.Any(l => p.Descripcion == l.Descripcion)).First().Parent))
                {
                    data.Add(i.Descripcion);
                }
                foreach (var i in data)
                    Descripcion.Add(i);
                data = Descripcion.ToList();
                decrpData = Descripcion;
                ListaActual = 3;
                ListaDe = ListaDescripcion[ListaActual];
            });
            LinkOficina = string.IsNullOrEmpty(Oficina) ? "Seleccionar Oficina" : _Oficina;
            tappedOficina = new Command(async () =>
            {
                MessagingCenter.Send<MainCapturaDeActivosViewModel, string>(this, "Code", "Change");
                Descripcion.Clear();
                data.Clear();
                foreach (var i in ListaDeOficinas.Where(x => x.Parent == ListaDeOficinas.Where(p => ListaDeOficinas.Any(l => p.Descripcion == l.Descripcion)).First().Parent))
                {
                    data.Add(i.Descripcion);
                }
                foreach (var i in data)
                    Descripcion.Add(i);
                data = Descripcion.ToList();
                decrpData = Descripcion;
                ListaActual = 4;
                ListaDe = ListaDescripcion[ListaActual];
            });

            LinkDescripcion = string.IsNullOrEmpty(Oficina) ? "Seleccionar Descripcion" : _Descripcion;
            tappedDescripcion = new Command(async () =>
            {
                MessagingCenter.Send<MainCapturaDeActivosViewModel, string>(this, "Code", "Change");
                Descripcion.Clear();
                data.Clear();
                foreach (var i in ListaDescripcionesActivos)
                {
                    data.Add(i.Descripcion);
                }
                foreach (var i in data)
                    Descripcion.Add(i);
                data = Descripcion.ToList();
                decrpData = Descripcion;
                ListaActual = 5;
                ListaDe = ListaDescripcion[ListaActual];
            });

            LinkMarcaModelo = string.IsNullOrEmpty(Oficina) ? "Seleccionar Marca | Modelo" : _MarcaModelo;
            tappedMarcaModelo = new Command(async () =>
            {
                MessagingCenter.Send<MainCapturaDeActivosViewModel, string>(this, "Code", "Change");
                Descripcion.Clear();
                data.Clear();
                foreach (var i in ListaMarcaModelos.Where(x => x.DescripcionActivo == ListaMarcaModelos.Where(p => ListaMarcaModelos.Any(l => p.Descripcion == l.Descripcion)).First().DescripcionActivo))
                {
                    data.Add(i.Descripcion);
                }
                foreach (var i in data)
                    Descripcion.Add(i);
                data = Descripcion.ToList();
                decrpData = Descripcion;
                ListaActual = 6;
                ListaDe = ListaDescripcion[ListaActual];
            });

            enterPressed = new Command(async () =>
            {
                if (!RFIDTag.StartsWith(tagsPrefijo) && RFIDTag.Length < 24)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Este no es un tag RFID", "ok");
                    return;
                }
                if (!string.IsNullOrEmpty(_Empresa) &&
                !string.IsNullOrEmpty(_Edificio) &&
                !string.IsNullOrEmpty(_Piso) &&
                !string.IsNullOrEmpty(_CentroCosto) &&
                !string.IsNullOrEmpty(_Oficina) &&
                !string.IsNullOrEmpty(_Descripcion) &&
                !string.IsNullOrEmpty(_MarcaModelo) &&
                !string.IsNullOrEmpty(_Empleado))
                {
                    try
                    {
                        Tag = RFIDTag.Substring(RFIDTag.Length - 2);
                        ActivosFijos activo = new ActivosFijos();
                        int test = ListaDeEmpresas.Where(x => x.Descripcion == _Empresa).First().Compania;
                        activo.Compania = ListaDeEmpresas.Where(x => x.Descripcion == _Empresa).First().Compania;
                        activo.Localidad = ListaDeEdificios.Where(x => x.Descripcion == _Edificio).First().Localidad;
                        activo.Area = ListaPisos.Where(x => x.Descripcion == _Piso).First().Area;
                        activo.Departamento = ListaCentroCostos.Where(x => x.Descripcion == _CentroCosto).First().Departamento;
                        activo.Oficina = ListaDeOficinas.Where(x => x.Descripcion == _Oficina).First().Oficina;
                        activo.DescripcionActivo = ListaDescripcionesActivos.Where(x => x.Descripcion == _Descripcion).First().DescripcionActivo;
                        activo.ClaseActivo = ListaDescripcionesActivos.Where(x => x.Descripcion == _Descripcion).First().ClasesActivos;
                        activo.MarcaModelo = ListaMarcaModelos.Where(x => x.Descripcion == _MarcaModelo).First().Marca;
                        activo.Empleado = ListaDeEmpleados.Where(x => x.Descripcion == _Empleado).First().Empleado;
                        activo.TagRFID = RFIDTag;
                        activo.Descripcion = $"{_Descripcion} {_MarcaModelo}";
                        activo.Fecha_Ingreso = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        activo.Activo = 1;
                        activo.Movible = "N";
                        activo.Asignado = "N";
                        activo.CapturadoVia = "HH";
                        activo.TagCodigo = RFIDTag.Substring(RFIDTag.Length - 5);
                        activo.Origen = 1;
                        activo.Actualizador = 1;

                        //modificar luego
                        ListaDeActivos.Add(activo);
                        db.Insert(activo);
                    }
                    catch (Exception ex)
                    {
                        App.Current.MainPage.DisplayAlert("Error", "Debe seleccionar todos los campos del activo!", "ok");
                    }
                }
            });

            #endregion

            #region MessaggingCenter
            MessagingCenter.Subscribe<MyBroadcastReceiver, string>(this, "Code", ((sender, barcode) =>
            {
                //var db = new SQLiteConnection(Preferences.Get("DB_PATH", ""));
                if (CurrentTab == "Entrada")
                {
                    if (!barcode.StartsWith(tagsPrefijo) && barcode.Length > 24)
                    {
                        App.Current.MainPage.DisplayAlert("Error", "Este no es un tag RFID", "ok");
                        return;
                    }
                    if (!string.IsNullOrEmpty(_Empresa) &&
                    !string.IsNullOrEmpty(_Edificio) &&
                    !string.IsNullOrEmpty(_Piso) &&
                    !string.IsNullOrEmpty(_CentroCosto) &&
                    !string.IsNullOrEmpty(_Oficina) &&
                    !string.IsNullOrEmpty(_Descripcion) &&
                    !string.IsNullOrEmpty(_MarcaModelo) &&
                    !string.IsNullOrEmpty(_Empleado))
                    {
                        try { 
                            RFIDTag = barcode;
                            Tag = barcode.Substring(barcode.Length - 2);
                            ActivosFijos activo = new ActivosFijos();
                            int test = ListaDeEmpresas.Where(x => x.Descripcion == _Empresa).First().Compania;
                            activo.Compania = ListaDeEmpresas.Where(x => x.Descripcion == _Empresa).First().Compania;
                            activo.Localidad = ListaDeEdificios.Where(x => x.Descripcion == _Edificio).First().Localidad;
                            activo.Area = ListaPisos.Where(x => x.Descripcion == _Piso).First().Area;
                            activo.Departamento = ListaCentroCostos.Where(x => x.Descripcion == _CentroCosto).First().Departamento;
                            activo.Oficina = ListaDeOficinas.Where(x => x.Descripcion == _Oficina).First().Oficina;
                            activo.DescripcionActivo = ListaDescripcionesActivos.Where(x => x.Descripcion == _Descripcion).First().DescripcionActivo;
                            activo.ClaseActivo = ListaDescripcionesActivos.Where(x => x.Descripcion == _Descripcion).First().ClasesActivos;
                            activo.MarcaModelo = ListaMarcaModelos.Where(x => x.Descripcion == _MarcaModelo).First().Marca;
                            activo.Empleado = ListaDeEmpleados.Where(x => x.Descripcion == _Empleado).First().Empleado;
                            activo.TagRFID = barcode;
                            activo.Descripcion = $"{_Descripcion} {_MarcaModelo}";
                            activo.Fecha_Ingreso = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                            activo.Activo = 1;
                            activo.Movible = "N";
                            activo.Asignado = "N";
                            activo.CapturadoVia = "HH";
                            activo.TagCodigo = barcode.Substring(barcode.Length - 5);
                            activo.Origen = 1;
                            activo.Actualizador = 1;

                            //modificar luego
                            ListaDeActivos.Add(activo);
                            db.Insert(activo);
                        }
                        catch(Exception ex)
                        {
                            App.Current.MainPage.DisplayAlert("Error", "Debe seleccionar todos los campos del activo!", "ok");
                        }
                    }
                }
                else if(CurrentTab == "Consulta")
                {
                    if (!barcode.StartsWith(tagsPrefijo) && barcode.Length > 24)
                    {
                        App.Current.MainPage.DisplayAlert("Error", "Este no es un tag RFID", "ok");
                        return;
                    }
                    RFIDTagParaAsignar = barcode;
                    var newQuerry = $"SELECT * FROM ActivosFijos WHERE TagRFID = '{RFIDTagParaAsignar}'";
                    try
                    {
                        //Ejecutamos el query.
                        var registro = db.Query<ActivosFijos>(newQuerry);

                        if (registro.First().Descripcion == null)
                        {
                            //No hacer nada por ahora
                        }
                        else
                        {
                            ActivosFijos asset = new ActivosFijos();
                            asset = registro.First();
                            Empresa = ListaDeEmpresas.Where(x => x.Compania == asset.Compania).First().Descripcion;
                            Piso = ListaPisos.Where(x => x.Area == asset.Area).First().Descripcion;
                            CentroCosto = ListaCentroCostos.Where(x => x.Departamento == asset.Departamento).First().Descripcion;
                            Oficina = ListaDeOficinas.Where(x => x.Oficina == asset.Oficina).First().Descripcion;
                            DescripcionActivo = ListaDescripcionesActivos.Where(x => x.DescripcionActivo == asset.DescripcionActivo).First().Descripcion;
                            Serial = asset.NumeroSerie;
                            Empleado = ListaDeEmpleados.Where(x => x.Empleado == asset.Empleado).First().Descripcion;
                        }
                    }
                    catch
                    {
                        Empresa = string.Empty;
                        Piso = string.Empty;
                        CentroCosto = string.Empty;
                        Oficina = string.Empty;
                        DescripcionActivo = string.Empty;
                        Serial = string.Empty;
                        Empleado = string.Empty;
                        App.Current.MainPage.DisplayAlert("Error", "Este activo no existe", "ok");
                    }
                }
            }));
            MessagingCenter.Subscribe<MainCapturaDeActivosPage, string>(this, "Code", ((sender, tab) =>
            {
                CurrentTab = tab;
            }));
            #endregion

            #endregion

            #region Metodos Consuta

            enterPressedConsulta = new Command(async () =>
            {
                if (!RFIDTagParaAsignar.StartsWith(tagsPrefijo))
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Este no es un tag RFID", "ok");
                    return;
                }
                var newQuerry = $"SELECT * FROM ActivosFijos WHERE TagRFID = '{RFIDTagParaAsignar}'";
                try
                {
                    //Ejecutamos el query.
                    var registro = db.Query<ActivosFijos>(newQuerry);

                    if (registro.First().Descripcion == null)
                    {
                        //No hacer nada por ahora
                    }
                    else
                    {
                        ActivosFijos asset = new ActivosFijos();
                        asset = registro.First();
                        Empresa = ListaDeEmpresas.Where(x => x.Compania == asset.Compania).First().Descripcion;
                        Piso = ListaPisos.Where(x => x.Area == asset.Area).First().Descripcion;
                        CentroCosto = ListaCentroCostos.Where(x => x.Departamento == asset.Departamento).First().Descripcion;
                        Oficina = ListaDeOficinas.Where(x => x.Oficina == asset.Oficina).First().Descripcion;
                        DescripcionActivo = asset.Descripcion;
                        Serial = asset.NumeroSerie;
                        Empleado = ListaDeEmpleados.Where(x => x.Empleado == asset.Empleado).First().Descripcion;
                    }
                }
                catch
                {
                    Empresa = string.Empty;
                    Piso = string.Empty;
                    CentroCosto = string.Empty;
                    Oficina = string.Empty;
                    DescripcionActivo = string.Empty;
                    Serial = string.Empty;
                    Empleado = string.Empty;
                    await App.Current.MainPage.DisplayAlert("Error", "Este activo no existe", "ok");
                }
            });

            #endregion

            #region Metodos Anterior

            FirstAsset = ListaDeActivos.Count <= 0 ? false : true;
            PreviousAsset = ListaDeActivos.Count <= 0 ? false : true;
            NextAsset = ListaDeActivos.Count <= 0 ? false : true;
            LastAsset = ListaDeActivos.Count <= 0 ? false : true;
            int last = ListaDeActivos.Count;
            int current = last;
            PrimerActivo = new Command(async () =>
            {
                try
                { 
                    OficinaAnterior = ListaDeOficinas.Where(x => x.Oficina == ListaDeActivos.First().Oficina).First().Descripcion;
                    DescripcionAnterior = ListaDeActivos.First().Descripcion;
                    TAG = ListaDeActivos.First().TagRFID.Substring(ListaDeActivos.First().TagRFID.Length - 3);
                    current = 0;
                    FirstAsset = false;
                    PreviousAsset = false;
                }
                catch(Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Error", ex.Message, "ok");
                }
            });

            ActivoAnterior = new Command(async () =>
            {
                try
                {
                    if(current > 0)
                        --current;
                    OficinaAnterior = ListaDeOficinas.Where(x => x.Oficina == ListaDeOficinas.First().Oficina).ToList()[current].Descripcion;
                    DescripcionAnterior = ListaDeActivos.First().Descripcion;
                    TAG = ListaDeActivos.First().TagRFID.Substring(ListaDeActivos.First().TagRFID.Length - 3);
                    if (current == 0)
                    {
                        FirstAsset = false;
                        PreviousAsset = false;
                    }
                }
                catch(Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Error", ex.Message, "ok");
                }
            });
            ActivoSiguiente = new Command(async () =>
            {
                try
                {
                    if(current < ListaDeActivos.Count)
                        ++current;
                    OficinaAnterior = ListaDeOficinas.Where(x => x.Oficina == ListaDeOficinas.First().Oficina).ToList()[current].Descripcion;
                    DescripcionAnterior = ListaDeActivos.First().Descripcion;
                    TAG = ListaDeActivos.First().TagRFID.Substring(ListaDeActivos.First().TagRFID.Length - 3);
                    if (current == last)
                    {
                        LastAsset = false;
                        NextAsset = false;
                    }
                }
                catch(Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Error", ex.Message, "ok");
                }
            });

            UltimoActivo = new Command(async () =>
            {
                try
                { 
                    OficinaAnterior = ListaDeOficinas.Where(x => x.Oficina == ListaDeOficinas.First().Oficina).Last().Descripcion;
                    DescripcionAnterior = ListaDeActivos.First().Descripcion;
                    TAG = ListaDeActivos.First().TagRFID.Substring(ListaDeActivos.First().TagRFID.Length - 3);
                    current = last;
                    LastAsset = false;
                    NextAsset = false;
                }
                catch(Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Error", ex.Message, "ok");
                }
            });

            #endregion

            #region Metodos de la vista

            Salir = new Command(async () =>
            {
                await App.Current.MainPage.Navigation.PopToRootAsync();
            });

            config = new Command(async () =>
            {
                await App.Current.MainPage.Navigation.PushModalAsync(new SettingsPage());
            });

            #endregion
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}
