using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Input;
using SGAFComplete.Models;
using SQLite;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SGAFComplete.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        #region Activosfijos a sincronizar

        List<ActivosFijos> ActivosAInsertar;
        List<ActivosFijos> ActivosAModificar;
        List<ActivosFijos> ActivosABorrar;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Class properties

        public string Timeout { get; set; }
        public string ServerIp { get; set; }
        public string DatabaseName { get; set; }
        public string Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public ICommand ExitBtn { get; set; }
        public ICommand Save_Ping { get; set; }
        public Color bckColor { get; set; }
        public bool visible { get; set; }
        public string cloud;
        public string cloudName;
        public string cloudUser;
        public string cloudPassword;
        public string LastSync { get; set; }
        public ICommand btnSync { get; set; }

        #endregion

        public SettingsViewModel()
        {
            var db = new SQLiteConnection(Preferences.Get("DB_PATH", ""));
            var querry = "SELECT * FROM ServidorDescrip";
            var querrySync = "SELECT * FROM LastSync";
            try
            {
                //Ejecutamos el query.
                var registro = db.Query<ServidorDescrip>(querry);
                var registro2 = db.Query<LastSync>(querrySync);

                if (registro.First().ServerIp == null)
                {
                    //No hacer nada por ahora
                }
                else
                {
                    cloud = registro.First().ServerIp;
                    cloudUser = registro.First().User;
                    cloudName = registro.First().Database;
                    cloudPassword = registro.First().Password;
                    LastSync = registro2.First().lastSync;
                    Timeout = registro.First().Timeout.ToString();
                    ServerIp = registro.First().ServerIp;
                    DatabaseName = registro.First().Database;
                    Port = registro.First().Port.ToString();
                    User = registro.First().User;
                    Password = registro.First().Password;
                }
            }
            catch (Exception ex)
            {

            }

            ExitBtn = new Command(async () => {
                await App.Current.MainPage.Navigation.PopModalAsync();
            });

            Save_Ping = new Command(async () =>
            {
                visible = false;
                if (Ping())
                {
                    ServidorDescrip server = new ServidorDescrip();
                    server.id = 1;
                    server.Timeout = Convert.ToInt32(Timeout);
                    server.ServerIp = ServerIp;
                    server.Database = DatabaseName;
                    server.Port = Convert.ToInt32(Port);
                    server.User = User;
                    server.Password = Password;
                    db.Update(server);
                    cloud = ServerIp;
                    cloudUser = User;
                    cloudPassword = Password;
                    bckColor = Color.FromHex("10AB0A");
                }
                else
                {
                    bckColor = Color.FromHex("AB0A0A");
                    await App.Current.MainPage.DisplayAlert("Error", "No se encuentra la conexion con el servidor", "ok");
                }
            });
            btnSync = new Command(async ()=>
            {
                if (Ping())
                {
                    var response = await App.Current.MainPage.DisplayActionSheet("Tipos de Sync", "cancel", "", "Intelligent Sync", "Brute Sync");
                    if (response == "Intelligent Sync")
                    {
                        intelligentSync();
                    }
                    else if (response == "Brute Sync")
                    {
                        var response2 = await App.Current.MainPage.DisplayAlert("Confirmación", "Se perderan todos datos auditados localmente", "Continuar", "Cancelar");
                        if(response2)
                            bruteSync();
                    }
                    
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Ocurrio un error con la comunicación con el servidor", "ok");
                }
            });
        }

        public bool Ping()
        {
            Ping ping = null;
            bool pingable = false;
            try
            {
                IPAddress ipPrinter = IPAddress.Parse(ServerIp);
                ping = new Ping();
                PingReply reply = ping.Send(ipPrinter, int.Parse(Port));
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
            }
            finally
            {
                if (ping != null)
                {
                    ping.Dispose();
                }
            }

            return pingable;

        }

        public void sync()
        {
            
            var dbSqlite = new SQLiteConnection(Preferences.Get("DB_PATH", ""));
            //try
            //{
                SqlConnection db = new SqlConnection($"Data Source={cloud};Initial Catalog={cloudName};Persist Security Info=True;User ID={cloudUser};Password={cloudPassword}");
                if (Ping())
                {
                    #region Activos Sync
                    //se trae la informacion de los Activos 
                    string querys = "SELECT * " +
                    "FROM [dbo].[ActivosFijos]" +
                     "ORDER BY Actualizador";
                    DataTable dataTable = new DataTable();
                    using (SqlCommand cmd = new SqlCommand(querys, db))
                    {
                        var da = new SqlDataAdapter(cmd);
                        da.Fill(dataTable);
                        //dataTable = dts.Tables[0];
                    }

                    SQLiteConnection con;
                    SQLiteCommand cmdl;
                    /*creating or openning database*/
                    
                    //se borran los productos modificados en la base de datos local

                    var querry = "SELECT * " +
                    "FROM ActivosFijos " +
                    "WHERE Actualizador > 0 OR Actualizador IS NULL " +
                    "ORDERBY Actualizador";

                    dbSqlite.BeginTransaction();
                    //var registro = dbSqlite.Query<ActivosFijos>(querry);

                    //frm.ShowDialog();
                    foreach (var row in dataTable.Select())
                    {
                        object[] props = row.ItemArray;

                        ActivosFijos activos = new ActivosFijos
                        {
                            ActivoFijo = Convert.ToInt32(props[0].ToString()),
                            Compania = Convert.ToInt32(props[1].ToString()),
                            Localidad = Convert.ToInt32(props[2].ToString()),
                            Area = Convert.ToInt32(props[3].ToString()),
                            Departamento = Convert.ToInt32(props[4].ToString()),
                            Oficina = Convert.ToInt32(props[5].ToString()),
                            DescripcionActivo = Convert.ToInt32(props[6].ToString()),
                            ClaseActivo = Convert.ToInt32(props[7].ToString()),
                            MarcaModelo = Convert.ToInt32(props[8].ToString()),
                            Empleado = Convert.ToInt32(props[9].ToString()),
                            TagRFID = props[10].ToString(),
                            Descripcion = props[11].ToString(),
                            CodigoERP = props[12].ToString(),
                            DescripcionERP = props[13].ToString(),
                            Imagen = string.IsNullOrEmpty(props[14].ToString()) ? 0 : Convert.ToInt32(props[14].ToString()),
                            FotoID = props[15].ToString(),
                            NumeroSerie = props[16].ToString(),
                            Fecha_Ingreso = props[17].ToString(),
                            Usuario_Ingreso = props[18].ToString(),
                            Fecha_Modificado = props[19].ToString(),
                            Usuario_Modificado = props[20].ToString(),
                            Activo = Convert.ToInt32(props[21].ToString()),
                            Fecha_Ultima_Auditoria = props[22].ToString(),
                            Movible = props[23].ToString(),
                            Asignado = props[24].ToString(),
                            CapturadoVia = props[25].ToString(),
                            TagCodigo = props[26].ToString(),
                            ComentarioInactivo = props[27].ToString(),
                            ComentarioActivo = props[28].ToString(),
                            Comentarios = props[29].ToString(),
                            Origen = Convert.ToInt32(props[30].ToString()),
                            RecepcionID = string.IsNullOrEmpty(props[31].ToString()) ? 0 : Convert.ToInt32(props[31].ToString()),
                            Fecha_Fin_Garantia = props[32].ToString()
                    };
                        activos.Descripcion = activos.Descripcion.Replace("'", "''");
                        
                        if (activos.Imagen == 0)
                            activos.Imagen = null;
                        if (activos.RecepcionID == 0)
                            activos.RecepcionID = null;
                        dbSqlite.Insert(activos);
                        
                        
                        //se actualizan los datos
                    }

                    #endregion

                    #region Descripciones Sync

                    string queryDescrip = "SELECT * " +
                    "FROM [dbo].[DescripcionesActivos]" +
                     "ORDER BY Actualizador";
                    DataTable dataTableDescrip = new DataTable();
                    using (SqlCommand cmd = new SqlCommand(queryDescrip, db))
                    {
                        var da = new SqlDataAdapter(cmd);
                        da.Fill(dataTableDescrip);
                        //dataTable = dts.Tables[0];
                    }
                    //se borran los productos modificados en la base de datos local

                    var querryDescrip = "SELECT * " +
                    "FROM DescripcionesActivos " +
                    "WHERE Actualizador > 0 OR Actualizador IS NULL " +
                    "ORDERBY Actualizador";

                    //dbSqlite.BeginTransaction();
                    //var registroDescrip = dbSqlite.Query<DescripcionesActivos>(querryDescrip);

                    //frm.ShowDialog();
                    foreach (var row in dataTableDescrip.Select())
                    {
                        object[] props = row.ItemArray;
                        DescripcionesActivos activos = new DescripcionesActivos
                        {
                            DescripcionActivo = Convert.ToInt32(props[0].ToString()),
                            Descripcion = props[1].ToString(),
                            ClasesActivos = Convert.ToInt32(props[2].ToString()),
                            Fecha_Ingreso = props[3].ToString(),
                            Usuario_Ingreso = props[4].ToString(),
                            Fecha_Modificado = props[5].ToString(),
                            Usuario_Modificado = props[6].ToString(),
                            PedirSerie = Convert.ToInt32(props[7].ToString()),
                            PedirEmpleado = Convert.ToInt32(props[8].ToString()),
                            PedirIDFoto = Convert.ToInt32(props[9].ToString()),
                            CodigoERP = props[10].ToString(),
                            PedirCodigoERP = Convert.ToInt32(props[11].ToString())
                        };
                        activos.Descripcion = activos.Descripcion.Replace("'", "''");
                        dbSqlite.Insert(activos);

                        //se actualizan los datos
                    }

                    #endregion

                    #region Marcas | Modelos Sync

                    string queryMarcaModelo = "SELECT * " +
                    "FROM [dbo].[Marcas_Modelos]" +
                     "ORDER BY Actualizador";
                    DataTable dataTableMarcaModelo = new DataTable();
                    using (SqlCommand cmd = new SqlCommand(queryMarcaModelo, db))
                    {
                        var da = new SqlDataAdapter(cmd);
                        da.Fill(dataTableMarcaModelo);
                        //dataTable = dts.Tables[0];
                    }
                    //se borran los productos modificados en la base de datos local

                    var querryMarcaModelo = "SELECT * " +
                    "FROM Marcas_Modelos " +
                    "WHERE Actualizador > 0 OR Actualizador IS NULL " +
                    "ORDERBY Actualizador";

                    //dbSqlite.BeginTransaction();
                    //var registroDescrip = dbSqlite.Query<Marcas_Modelos>(querryDescrip);

                    //frm.ShowDialog();
                    foreach (var row in dataTableMarcaModelo.Select())
                    {
                        object[] props = row.ItemArray;
                        Marcas_Modelos activos = new Marcas_Modelos
                        {
                            Marca = Convert.ToInt32(props[0].ToString()),
                            Descripcion = props[1].ToString(),
                            DescripcionActivo = Convert.ToInt32(props[2].ToString()),
                            Fecha_Ingreso = props[3].ToString(),
                            Usuario_Ingreso = props[4].ToString(),
                            Fecha_Modificado = props[5].ToString(),
                            Usuario_Modificado = props[6].ToString(),
                            CodigoERP = props[7].ToString(),
                            CodigoFoto = props[8].ToString(),
                            CodigoFotoEtiquetado = props[9].ToString(),
                            Descripcion_Etiquetado = props[10].ToString()
                        };
                        activos.Descripcion = activos.Descripcion.Replace("'", "''");
                        dbSqlite.Insert(activos);

                        //se actualizan los datos
                    }

                    #endregion

                    #region Companias Sync

                    string queryCompanias = "SELECT * " +
                    "FROM [dbo].[Companias]" +
                     "ORDER BY Actualizador";
                    DataTable dataTableCompanias = new DataTable();
                    using (SqlCommand cmd = new SqlCommand(queryCompanias, db))
                    {
                        var da = new SqlDataAdapter(cmd);
                        da.Fill(dataTableCompanias);
                        //dataTable = dts.Tables[0];
                    }
                    var querryCompanias = "SELECT * " +
                    "FROM Companias " +
                    "WHERE Actualizador > 0 OR Actualizador IS NULL " +
                    "ORDERBY Actualizador";

                    //dbSqlite.BeginTransaction();
                    //var registro = dbSqlite.Query<ActivosFijos>(querry);

                    //frm.ShowDialog();
                    foreach (var row in dataTableCompanias.Select())
                    {
                        object[] props = row.ItemArray;
                        Companias activos = new Companias
                        {
                            Compania = Convert.ToInt32(props[0].ToString()),
                            CodigoERP = props[1].ToString(),
                            Descripcion = props[2].ToString(),
                            Descripcion_Abreviada = props[3].ToString(),
                            Activo = Convert.ToInt32(props[4].ToString()),
                            Localidad = Convert.ToInt32(props[5].ToString()),
                            Fecha_Ingreso = props[6].ToString(),
                            Usuario_Ingreso = props[7].ToString(),
                            Fecha_Modificado = props[8].ToString(),
                            Usuario_Modificado = props[9].ToString(),
                            Direccion1 = props[10].ToString(),
                            Direccion2 = props[11].ToString(),
                            Telefono_Central = props[12].ToString(),
                            RNC = props[13].ToString(),
                            Logo = props[14].ToString(),
                            PrefijoRFID = props[15].ToString(),
                        };
                        activos.Descripcion = activos.Descripcion.Replace("'", "''");
                        dbSqlite.Insert(activos);

                        //se actualizan los datos
                    }

                    #endregion

                    #region Localidades Sync

                    string queryLocalidades = "SELECT * " +
                    "FROM [dbo].[Localidades]" +
                     "ORDER BY Actualizador";
                    DataTable dataTableLocalidades = new DataTable();
                    using (SqlCommand cmd = new SqlCommand(queryLocalidades, db))
                    {
                        var da = new SqlDataAdapter(cmd);
                        da.Fill(dataTableLocalidades);
                        //dataTable = dts.Tables[0];
                    }
                    var querryLocalidades = "SELECT * " +
                    "FROM Localidades " +
                    "WHERE Actualizador > 0 OR Actualizador IS NULL " +
                    "ORDERBY Actualizador";

                    //dbSqlite.BeginTransaction();
                    //var registro = dbSqlite.Query<ActivosFijos>(querry);

                    //frm.ShowDialog();
                    foreach (var row in dataTableLocalidades.Select())
                    {
                        object[] props = row.ItemArray;
                        Localidades activos = new Localidades
                        {
                            Localidad = Convert.ToInt32(props[0].ToString()),
                            Parent = Convert.ToInt32(props[1].ToString()),
                            CodigoERP = props[2].ToString(),
                            Descripcion = props[3].ToString(),
                            Descripcion_Abreviada = props[4].ToString(),
                            Activo = Convert.ToInt32(props[5].ToString()),
                            Areas = Convert.ToInt32(props[6].ToString()),
                            Fecha_Ingreso = props[7].ToString(),
                            Usuario_Ingreso = props[8].ToString(),
                            Fecha_Modificado = props[9].ToString(),
                            Usuario_Modificado = props[10].ToString(),
                            Direccion1 = props[11].ToString(),
                            Direccion2 = props[12].ToString(),
                            Telefono_Central = props[13].ToString(),
                            Plano = props[14].ToString()
                        };
                        activos.Descripcion = activos.Descripcion.Replace("'", "''");
                        dbSqlite.Insert(activos);

                        //se actualizan los datos
                    }

                    #endregion

                    #region Area Sync

                    string queryAreas = "SELECT * " +
                    "FROM [dbo].[Areas]" +
                     "ORDER BY Actualizador";
                    DataTable dataTableAreas = new DataTable();
                    using (SqlCommand cmd = new SqlCommand(queryAreas, db))
                    {
                        var da = new SqlDataAdapter(cmd);
                        da.Fill(dataTableAreas);
                        //dataTable = dts.Tables[0];
                    }
                    var querryAreas = "SELECT * " +
                    "FROM Areas " +
                    "WHERE Actualizador > 0 OR Actualizador IS NULL " +
                    "ORDERBY Actualizador";

                    //dbSqlite.BeginTransaction();
                    //var registro = dbSqlite.Query<ActivosFijos>(querry);

                    //frm.ShowDialog();
                    foreach (var row in dataTableAreas.Select())
                    {
                        object[] props = row.ItemArray;
                        Areas activos = new Areas
                        {
                            Area = Convert.ToInt32(props[0].ToString()),
                            Parent = Convert.ToInt32(props[1].ToString()),
                            CodigoERP = props[2].ToString(),
                            Descripcion = props[3].ToString(),
                            Descripcion_Abreviada = props[4].ToString(),
                            Activo = Convert.ToInt32(props[5].ToString()),
                            Departamentos = Convert.ToInt32(props[6].ToString()),
                            Fecha_Ingreso = props[7].ToString(),
                            Usuario_Ingreso = props[8].ToString(),
                            Fecha_Modificado = props[9].ToString(),
                            Usuario_Modificado = props[10].ToString(),
                            Plano = props[11].ToString()
                        };
                        activos.Descripcion = activos.Descripcion.Replace("'", "''");
                        dbSqlite.Insert(activos);

                        //se actualizan los datos
                    }

                    #endregion

                    #region Departamentos Sync

                    string queryDepartamentos = "SELECT * " +
                    $"FROM [{cloudName}].[dbo].[Departamentos]" +
                     "ORDER BY Actualizador";
                    DataTable dataTableDepartamento = new DataTable();
                    using (SqlCommand cmd = new SqlCommand(queryDepartamentos, db))
                    {
                        var da = new SqlDataAdapter(cmd);
                        da.Fill(dataTableDepartamento);
                        //dataTable = dts.Tables[0];
                    }
                    var querryDepartamento = "SELECT * " +
                    "FROM Departamentos " +
                    "WHERE Actualizador > 0 OR Actualizador IS NULL " +
                    "ORDERBY Actualizador";

                    //dbSqlite.BeginTransaction();
                    //var registro = dbSqlite.Query<ActivosFijos>(querry);

                    //frm.ShowDialog();
                    foreach (var row in dataTableDepartamento.Select())
                    {
                        object[] props = row.ItemArray;
                        Departamentos activos = new Departamentos
                        {
                            Departamento = Convert.ToInt32(props[0].ToString()),
                            Parent = Convert.ToInt32(props[1].ToString()),
                            CodigoERP = props[2].ToString(),
                            Descripcion = props[3].ToString(),
                            Descripcion_Abreviada = props[4].ToString(),
                            Telefono_Directo = props[5].ToString(),
                            Extension = props[6].ToString(),
                            Encargado = props[7].ToString(),
                            Activo = Convert.ToInt32(props[8].ToString()),
                            Divisiones = Convert.ToInt32(props[9].ToString()),
                            Fecha_Ingreso = props[10].ToString(),
                            Usuario_Ingreso = props[11].ToString(),
                            Fecha_Modificado = props[12].ToString(),
                            Usuario_Modificado = props[13].ToString()
                            
                        };
                        activos.Descripcion = activos.Descripcion.Replace("'", "''");
                        dbSqlite.Insert(activos);

                        //se actualizan los datos
                    }

                    #endregion

                    #region Oficinas Sync

                    string queryOficinas = "SELECT * " +
                    $"FROM [{cloudName}].[dbo].[Oficinas]" +
                     "ORDER BY Actualizador";
                    DataTable dataTableOficinas = new DataTable();
                    using (SqlCommand cmd = new SqlCommand(queryOficinas, db))
                    {
                        var da = new SqlDataAdapter(cmd);
                        da.Fill(dataTableOficinas);
                        //dataTable = dts.Tables[0];
                    }
                    var querryOficinas = "SELECT * " +
                    "FROM Oficinas " +
                    "WHERE Actualizador > 0 OR Actualizador IS NULL " +
                    "ORDERBY Actualizador";

                    //dbSqlite.BeginTransaction();
                    //var registro = dbSqlite.Query<ActivosFijos>(querry);

                    //frm.ShowDialog();
                    foreach (var row in dataTableOficinas.Select())
                    {
                        object[] props = row.ItemArray;
                        Oficinas activos = new Oficinas
                        {
                            Oficina = Convert.ToInt32(props[0].ToString()),
                            Parent = Convert.ToInt32(props[1].ToString()),
                            CodigoERP = props[2].ToString(),
                            Descripcion = props[3].ToString(),
                            Descripcion_Abreviada = props[4].ToString(),
                            Activo = Convert.ToInt32(props[5].ToString()),
                            Fecha_Ingreso = props[6].ToString(),
                            Usuario_Ingreso = props[7].ToString(),
                            Fecha_Modificado = props[8].ToString(),
                            Usuario_Modificado = props[9].ToString(),
                            Izquierda = string.IsNullOrEmpty(props[10].ToString()) ? 0 : Convert.ToInt32(props[10].ToString()),
                            Tope = string.IsNullOrEmpty(props[11].ToString()) ? 0 : Convert.ToInt32(props[11].ToString()),
                            Derecha = string.IsNullOrEmpty(props[12].ToString()) ? 0 : Convert.ToInt32(props[12].ToString()),
                            Fondo = string.IsNullOrEmpty(props[13].ToString()) ? 0 : Convert.ToInt32(props[13].ToString()),
                            Ancho = string.IsNullOrEmpty(props[14].ToString()) ? 0 : Convert.ToInt32(props[14].ToString()),
                            Alto = string.IsNullOrEmpty(props[15].ToString()) ? 0 : Convert.ToInt32(props[15].ToString()),
                            Mapeando = string.IsNullOrEmpty(props[16].ToString()) ? 0 : Convert.ToInt32(props[16].ToString()),
                            Plantilla_ID = string.IsNullOrEmpty(props[17].ToString()) ? 0 : Convert.ToInt32(props[17].ToString()),
                            Empleado = string.IsNullOrEmpty(props[18].ToString()) ? 0 : Convert.ToInt32(props[18].ToString()),
                            Departamento = string.IsNullOrEmpty(props[19].ToString()) ? 0 : Convert.ToInt32(props[19].ToString()),
                            EmpleadoCodigoERP = props[20].ToString()
                        };
                        activos.Descripcion = activos.Descripcion.Replace("'", "''");
                        if (activos.Izquierda == 0)
                            activos.Izquierda = null;
                        if (activos.Tope == 0)
                            activos.Tope = null;
                        if (activos.Derecha == 0)
                            activos.Derecha = null;
                        if (activos.Fondo == 0)
                            activos.Fondo = null;
                        if (activos.Ancho == 0)
                            activos.Ancho = null;
                        if (activos.Alto == 0)
                            activos.Alto = null;
                        if (activos.Mapeando == 0)
                            activos.Mapeando = null;
                        if (activos.Plantilla_ID == 0)
                            activos.Plantilla_ID = null;
                        if (activos.Empleado == 0)
                                activos.Empleado = null;
                        if (activos.Departamento == 0)
                            activos.Departamento = null;
                        dbSqlite.Insert(activos);

                        //se actualizan los datos
                    }

                    #endregion

                    #region Empleados Sync

                    string queryEmpleado = "SELECT * " +
                    $"FROM [{cloudName}].[dbo].[Empleados]" +
                     "ORDER BY Actualizador";
                    DataTable dataTableEmpleado = new DataTable();
                    using (SqlCommand cmd = new SqlCommand(queryEmpleado, db))
                    {
                        var da = new SqlDataAdapter(cmd);
                        da.Fill(dataTableEmpleado);
                        //dataTable = dts.Tables[0];
                    }
                    var querryEmpleado = "SELECT * " +
                    "FROM Empleados " +
                    "WHERE Actualizador > 0 OR Actualizador IS NULL " +
                    "ORDERBY Actualizador";

                    //dbSqlite.BeginTransaction();
                    //var registro = dbSqlite.Query<ActivosFijos>(querry);

                    //frm.ShowDialog();
                    foreach (var row in dataTableEmpleado.Select())
                    {
                        object[] props = row.ItemArray;
                        Empleados activos = new Empleados
                        {
                            Empleado = Convert.ToInt32(props[0].ToString()),
                            CodigoERP = props[1].ToString(),
                            CodigoRFID = props[2].ToString(),
                            Descripcion = props[3].ToString(),
                            Descripcion_Abreviada = props[4].ToString(),
                            Telefono_Directo = props[5].ToString(),
                            Extension = props[6].ToString(),
                            Superior = string.IsNullOrEmpty(props[7].ToString()) ? 0 : Convert.ToInt32(props[7].ToString()),
                            Activo = Convert.ToInt32(props[8].ToString()),
                            Fecha_Ingreso = props[9].ToString(),
                            Usuario_Ingreso = props[10].ToString(),
                            Fecha_Modificado = props[11].ToString(),
                            Usuario_Modificado = props[12].ToString(),
                            NombreFoto = props[13].ToString(),
                            CentroCosto = props[14].ToString(),
                            Cargo = props[15].ToString(),
                            Oficina = string.IsNullOrEmpty(props[16].ToString()) ? 0 : Convert.ToInt32(props[16].ToString()),
                            Departamento = string.IsNullOrEmpty(props[17].ToString()) ? 0 : Convert.ToInt32(props[17].ToString())

                        };
                        activos.Descripcion = activos.Descripcion.Replace("'", "''");
                        if (activos.Superior == 0)
                            activos.Superior = null;
                        if (activos.Oficina == 0)
                            activos.Oficina = null;
                        if (activos.Departamento == 0)
                            activos.Departamento = null;
                        dbSqlite.Insert(activos);

                        //se actualizan los datos
                    }

                    #endregion

                    #region Clases de Activos Sync

                    string queryClases = "SELECT * " +
                    $"FROM [{cloudName}].[dbo].[ClasesActivos]" +
                     "ORDER BY Actualizador";
                    DataTable dataTableClases = new DataTable();
                    using (SqlCommand cmd = new SqlCommand(queryClases, db))
                    {
                        var da = new SqlDataAdapter(cmd);
                        da.Fill(dataTableClases);
                        //dataTable = dts.Tables[0];
                    }
                    var querryClases = "SELECT * " +
                    "FROM ClasesActivos " +
                    "WHERE Actualizador > 0 OR Actualizador IS NULL " +
                    "ORDERBY Actualizador";

                    //dbSqlite.BeginTransaction();
                    //var registro = dbSqlite.Query<ActivosFijos>(querry);

                    //frm.ShowDialog();
                    foreach (var row in dataTableClases.Select())
                    {
                        object[] props = row.ItemArray;
                        ClasesActivos activos = new ClasesActivos
                        {
                            ClaseActivo = Convert.ToInt32(props[0].ToString()),
                            CodigoERP = props[1].ToString(),
                            Descripcion = props[2].ToString(),
                            Descripcion_Abreviada = props[3].ToString(),
                            Activo = Convert.ToInt32(props[4].ToString()),
                            Fecha_Ingreso = props[5].ToString(),
                            Usuario_Ingreso = props[6].ToString(),
                            Fecha_Modificado = props[7].ToString(),
                            Usuario_Modificado = props[8].ToString(),
                            PedirSerie = Convert.ToInt32(props[9].ToString()),
                            PedirEmpleado = Convert.ToInt32(props[10].ToString()),
                            PedirIDFoto = Convert.ToInt32(props[11].ToString()),
                            PedirCodigoERP = Convert.ToInt32(props[12].ToString()),
                            Parent = Convert.ToInt32(props[13].ToString())
                        };
                        activos.Descripcion = activos.Descripcion.Replace("'", "''");
                        dbSqlite.Insert(activos);

                        //se actualizan los datos
                    }

                    #endregion

                    //se hace commit a los cambios de los datos
                    dbSqlite.Commit();
                    App.Current.MainPage.DisplayAlert("Aviso", "Se Sincronizaron los datos correctamente!", "ok");
                }
                else
                {
                    throw new Exception("No esta conectado al servidor.");
                }


            //}
            //catch (Exception ex)
            //{
            //    App.Current.MainPage.DisplayAlert("Error", ex.Message, "ok");

            //}
            
            LastSync syn = new LastSync();
            LastSync = DateTime.Now.ToString();
            syn.id = 1;
            LastSync = LastSync;
            dbSqlite.Update(syn);
        }

        public void intelligentSync()
        {
            var dbSqlite = new SQLiteConnection(Preferences.Get("DB_PATH", ""));
            try
            {
                visible = true;
                SqlConnection db = new SqlConnection($"Data Source={cloud};Initial Catalog=SGAFDB_TOTAL;Persist Security Info=True;User ID={cloudUser};Password={cloudPassword}");
                if (Ping()) {

                    #region Descripciones

                    //Lista de descripciones locales
                    var descripcionesActualizar = new List<DescripcionesActivos>();
                    //Lista de descripciones del Server
                    var descripcionesActualizarCloud = new List<DescripcionesActivos>();

                    #region Llenado de tablas a sincronizar
                    
                    //string queryDescrip = "SELECT * " +
                    //"FROM [dbo].[DescripcionesActivos]" +
                    // "ORDER BY Actualizador";
                    //DataTable dataTableDescrip = new DataTable();
                    //using (SqlCommand cmd = new SqlCommand(queryDescrip, db))
                    //{
                    //    var da = new SqlDataAdapter(cmd);
                    //    da.Fill(dataTableDescrip);
                    //    //dataTable = dts.Tables[0];
                    //}
                    //se borran los productos modificados en la base de datos local

                    var querryDescrip = "SELECT * " +
                    "FROM DescripcionesActivos " +
                    "WHERE Actualizador > 0 " +
                    "ORDER BY Actualizador";

                    dbSqlite.BeginTransaction();
                    var registroDescrip = dbSqlite.Query<DescripcionesActivos>(querryDescrip);
                    try {
                        if (registroDescrip.First() == null)
                        {

                        }
                        else
                        {
                            descripcionesActualizar = registroDescrip;
                        }
                    }
                    catch
                    {
                    }

                    //foreach (var row in dataTableDescrip.Select())
                    //{
                    //    object[] props = row.ItemArray;
                    //    DescripcionesActivos activos = new DescripcionesActivos
                    //    {
                    //        DescripcionActivo = Convert.ToInt32(props[0].ToString()),
                    //        Descripcion = props[1].ToString(),
                    //        ClasesActivos = Convert.ToInt32(props[2].ToString()),
                    //        Fecha_Ingreso = props[3].ToString(),
                    //        Usuario_Ingreso = props[4].ToString(),
                    //        Fecha_Modificado = props[5].ToString(),
                    //        Usuario_Modificado = props[6].ToString(),
                    //        PedirSerie = Convert.ToInt32(props[7].ToString()),
                    //        PedirEmpleado = Convert.ToInt32(props[8].ToString()),
                    //        PedirIDFoto = Convert.ToInt32(props[9].ToString()),
                    //        CodigoERP = props[10].ToString(),
                    //        PedirCodigoERP = Convert.ToInt32(props[11].ToString()),
                    //        Actualizador = Convert.ToInt32(props[12].ToString())

                    //    };
                    //    activos.Descripcion = activos.Descripcion.Replace("'", "''");
                    //    descripcionesActualizarCloud.Add(activos);
                    //}
                    #endregion

                    #region sincronizacion de inserts

                    foreach(var el in descripcionesActualizar)
                    {
                        string querys = "UPDATE [dbo].[DescripcionesActivos]" +
                                   "SET" +
                                   $"Descripcion = '{el.Descripcion}'" +
                                   $",ClasesActivos = '{el.ClasesActivos}'" +
                                   $",Fecha_Ingreso = '{el.Fecha_Ingreso}'" +
                                   $",Usuario_Ingreso = '{el.Usuario_Ingreso}'" +
                                   $",Fecha_Modificado = '{el.Fecha_Modificado}'" +
                                   $",Usuario_Modificado = '{el.Usuario_Modificado}'" +
                                   $",PedirSerie = '{el.PedirSerie}'" +
                                    $",PedirEmpleado = '{el.PedirEmpleado}'" +
                                   $",PedirIDFoto = '{el.PedirIDFoto}'" +
                                   $",CodigoERP = '{el.CodigoERP}'" +
                                   $",PedirCodigoERP = '{el.PedirCodigoERP}'" +
                                   $",Actualizador = 0" +
                                   $"WHERE DescripcionActivo = {el.DescripcionActivo}";

                        using (SqlCommand cmd = new SqlCommand(querys, db))
                        {
                            db.Open();
                            cmd.ExecuteNonQuery();
                            db.Close();
                        }
                    }

                    #endregion

                    #endregion

                    #region Marca Modelos

                    //Lista de descripciones locales
                    var marcasModelosActualizar = new List<Marcas_Modelos>();
                    //Lista de descripciones del Server
                    var marcaModelosActualizarCloud = new List<Marcas_Modelos>();

                    #region Llenado de tablas a sincronizar

                    //string queryBrand = "SELECT * " +
                    //"FROM [dbo].[Marcas_Modelos]" +
                    // "ORDER BY Actualizador";
                    //DataTable dataTableBrand = new DataTable();
                    //using (SqlCommand cmd = new SqlCommand(queryBrand, db))
                    //{
                    //    var da = new SqlDataAdapter(cmd);
                    //    da.Fill(dataTableBrand);
                    //    //dataTable = dts.Tables[0];
                    //}
                    //se borran los productos modificados en la base de datos local

                    var queryBrandSqlite = "SELECT * " +
                    "FROM Marcas_Modelos " +
                    "WHERE Actualizador > 0 " +
                    "ORDER BY Actualizador";

                    var registroBrand = dbSqlite.Query<Marcas_Modelos>(querryDescrip);
                    try
                    {
                        if (registroBrand.First() == null)
                        {

                        }
                        else
                        {
                            marcasModelosActualizar = registroBrand;
                        }
                    }
                    catch
                    {
                    }

                    //foreach (var row in dataTableBrand.Select())
                    //{
                    //    object[] props = row.ItemArray;
                    //    Marcas_Modelos activos = new Marcas_Modelos
                    //    {
                    //        Marca = Convert.ToInt32(props[0].ToString()),
                    //        Descripcion = props[1].ToString(),
                    //        DescripcionActivo = Convert.ToInt32(props[2].ToString()),
                    //        Fecha_Ingreso = props[3].ToString(),
                    //        Usuario_Ingreso = props[4].ToString(),
                    //        Fecha_Modificado = props[5].ToString(),
                    //        Usuario_Modificado = props[6].ToString(),
                    //        CodigoERP = props[7].ToString(),
                    //        CodigoFoto = props[8].ToString(),
                    //        CodigoFotoEtiquetado = props[9].ToString(),
                    //        Descripcion_Etiquetado = props[10].ToString()
                    //    };
                    //    activos.Descripcion = activos.Descripcion.Replace("'", "''");
                    //    marcaModelosActualizarCloud.Add(activos);
                    //}
                    #endregion

                    #region sincronizacion de inserts

                    foreach (var el in marcasModelosActualizar)
                    {
                        string querys = "UPDATE [dbo].[Marcas_Modelos]" +
                            "SET" +
                                    $"Descripcion = '{el.Descripcion}'" +
                                    $",DescripcionActivo = '{el.DescripcionActivo}'" +
                                    $",Fecha_Ingreso = '{el.Fecha_Ingreso}'" +
                                    $",Usuario_Ingreso = '{el.Usuario_Ingreso}'" +
                                    $",Fecha_Modificado = '{el.Fecha_Modificado}'" +
                                    $",Usuario_Modificado = '{el.Usuario_Modificado}'" +
                                    $",CodigoERP = '{el.CodigoERP}'" +
                                    $",CodigoFoto = '{el.CodigoFoto}'" +
                                    $",CodigoFotoEtiquetado = '{el.CodigoFotoEtiquetado}'" +
                                    $",Descripcion_Etiquetado = '{el.Descripcion_Etiquetado}'" +
                                    $",Actualizador = 0" +
                                    $"WHERE Marca = {el.Marca}";

                        using (SqlCommand cmd = new SqlCommand(querys, db))
                        {
                            db.Open();
                            cmd.ExecuteNonQuery();
                            db.Close();
                        }
                    }

                    #endregion

                    #endregion

                    #region Activos

                    //Lista de descripciones locales
                    var activosActualizar = new List<ActivosFijos>();
                    //Lista de descripciones del Server
                    var activosActualizarCloud = new List<ActivosFijos>();

                    #region Llenado de tablas a sincronizar

                    //string queryActivo = "SELECT * " +
                    //"FROM [dbo].[ActivosFijos]" +
                    // "ORDER BY Actualizador";
                    //DataTable dataTableActivo = new DataTable();
                    //using (SqlCommand cmd = new SqlCommand(queryActivo, db))
                    //{
                    //    var da = new SqlDataAdapter(cmd);
                    //    da.Fill(dataTableActivo);
                    //    //dataTable = dts.Tables[0];
                    //}
                    //se borran los productos modificados en la base de datos local

                    var querryActivo = "SELECT * " +
                    "FROM ActivosFijos " +
                    "WHERE Actualizador > 0 " +
                    "ORDER BY Actualizador";

                    
                    var registroActivo = dbSqlite.Query<ActivosFijos>(querryActivo);
                    try
                    {
                        if (registroActivo.First() == null)
                        {

                        }
                        else
                        {
                            activosActualizar = registroActivo;
                        }
                    }
                    catch
                    {
                    }

                    //foreach (var row in dataTableActivo.Select())
                    //{
                    //    object[] props = row.ItemArray;

                    //    ActivosFijos activos = new ActivosFijos
                    //    {
                    //        ActivoFijo = Convert.ToInt32(props[0].ToString()),
                    //        Compania = Convert.ToInt32(props[1].ToString()),
                    //        Localidad = Convert.ToInt32(props[2].ToString()),
                    //        Area = Convert.ToInt32(props[3].ToString()),
                    //        Departamento = Convert.ToInt32(props[4].ToString()),
                    //        Oficina = Convert.ToInt32(props[5].ToString()),
                    //        DescripcionActivo = Convert.ToInt32(props[6].ToString()),
                    //        ClaseActivo = Convert.ToInt32(props[7].ToString()),
                    //        MarcaModelo = Convert.ToInt32(props[8].ToString()),
                    //        Empleado = Convert.ToInt32(props[9].ToString()),
                    //        TagRFID = props[10].ToString(),
                    //        Descripcion = props[11].ToString(),
                    //        CodigoERP = props[12].ToString(),
                    //        DescripcionERP = props[13].ToString(),
                    //        Imagen = string.IsNullOrEmpty(props[14].ToString()) ? 0 : Convert.ToInt32(props[14].ToString()),
                    //        FotoID = props[15].ToString(),
                    //        NumeroSerie = props[16].ToString(),
                    //        Fecha_Ingreso = props[17].ToString(),
                    //        Usuario_Ingreso = props[18].ToString(),
                    //        Fecha_Modificado = props[19].ToString(),
                    //        Usuario_Modificado = props[20].ToString(),
                    //        Activo = Convert.ToInt32(props[21].ToString()),
                    //        Fecha_Ultima_Auditoria = props[22].ToString(),
                    //        Movible = props[23].ToString(),
                    //        Asignado = props[24].ToString(),
                    //        CapturadoVia = props[25].ToString(),
                    //        TagCodigo = props[26].ToString(),
                    //        ComentarioInactivo = props[27].ToString(),
                    //        ComentarioActivo = props[28].ToString(),
                    //        Comentarios = props[29].ToString(),
                    //        Origen = Convert.ToInt32(props[30].ToString()),
                    //        RecepcionID = string.IsNullOrEmpty(props[31].ToString()) ? 0 : Convert.ToInt32(props[31].ToString()),
                    //        Fecha_Fin_Garantia = props[32].ToString(),
                    //        Actualizador = Convert.ToInt32(props[33].ToString())

                    //    };
                    //    activos.Descripcion = activos.Descripcion.Replace("'", "''");
                    //    activosActualizarCloud.Add(activos);
                    //}
                    #endregion

                    #region sincronizacion de inserts

                    foreach (var el in activosActualizar)
                    {
                        int? num = ((el.Imagen == null) ? null : el.Imagen);
                        int? num2 = ((el.Origen == null) ? null : el.Origen);
                        int? num3 = ((el.RecepcionID == null) ? null : el.RecepcionID);
                        string querys = $"INSERT INTO [{cloudName}].[dbo].[ActivosFijos]" +
                                   " ([Compania]," +
                                        "[Localidad]," +
                                        "[Area]," +
                                        "[Departamento]," +
                                        "[Oficina]," +
                                        "[DescripcionActivo]," +
                                        "[ClaseActivo]," +
                                        "[MarcaModelo]," +
                                        "[Empleado]," +
                                        "[TagRFID]," +
                                        "[Descripcion]," +
                                        "[CodigoERP]," +
                                        "[DescripcionERP]," +
                                        "[Imagen]," +
                                        "[FotoID]," +
                                        "[NumeroSerie]," +
                                        "[Fecha_Ingreso]," +
                                        "[Usuario_Ingreso]," +
                                        "[Fecha_Modificado]," +
                                        "[Usuario_Modificado]," +
                                        "[Activo]," +
                                        "[Fecha_Ultima_Auditoria]," +
                                        "[Movible]," +
                                        "[Asignado]," +
                                        "[CapturadoVia]," +
                                        "[TagCodigo]," +
                                        "[ComentarioInactivo]," +
                                        "[ComentarioActivo]," +
                                        "[Comentarios]," +
                                        "[Origen]," +
                                        "[RecepcionID]," +
                                        "[Fecha_Fin_Garantia]," +
                                        "[Actualizador])" +
                             " VALUES(" +
                                   $"'{el.Compania}'" +
                                   $",'{el.Localidad}'" +
                                   $",'{el.Area}'" +
                                   $",'{el.Departamento}'" +
                                   $",'{el.Oficina}'" +
                                   $",'{el.DescripcionActivo}'" +
                                   $",'{el.ClaseActivo}'" +
                                    $",'{el.MarcaModelo}'" +
                                   $",'{el.Empleado}'" +
                                   $",'{el.TagRFID}'" +
                                   $",'{el.Descripcion}'" +
                                   $",'{(string.IsNullOrEmpty(el.CodigoERP) ? "NULL" : el.CodigoERP)}'" +
                                   $",'{(string.IsNullOrEmpty(el.DescripcionERP) ? "NULL" : el.DescripcionERP)}'" +
                                   $",'{num}'" +
                                   $",'{(string.IsNullOrEmpty(el.FotoID) ? "NULL" : el.FotoID)}'" +
                                   $",'{(string.IsNullOrEmpty(el.NumeroSerie) ? "NULL" : el.NumeroSerie)}'" +
                                   $",'{el.Fecha_Ingreso}'" +
                                   $",'{(string.IsNullOrEmpty(el.Usuario_Ingreso) ? "NULL" : el.Usuario_Ingreso)}'" +
                                   $",{(string.IsNullOrEmpty(el.Fecha_Modificado) ? "NULL" : el.Fecha_Modificado)}" +
                                   $",'{(string.IsNullOrEmpty(el.Usuario_Modificado) ? "NULL" : el.Usuario_Modificado)}'" +
                                   $",'{el.Activo}'" +
                                   $",{(string.IsNullOrEmpty(el.Fecha_Ultima_Auditoria) ? "NULL" : el.Fecha_Ultima_Auditoria)}" +
                                   $",'{(string.IsNullOrEmpty(el.Movible) ? "NULL" : el.Movible)}'" +
                                   $",'{(string.IsNullOrEmpty(el.Asignado) ? "NULL" : el.Asignado)}'" +
                                   $",'{(string.IsNullOrEmpty(el.CapturadoVia) ? "NULL" : el.CapturadoVia)}'" +
                                   $",'{(string.IsNullOrEmpty(el.TagCodigo) ? "NULL" : el.CapturadoVia)}'" +
                                   $",'{(string.IsNullOrEmpty(el.ComentarioInactivo) ? "NULL" : el.ComentarioInactivo)}'" +
                                   $",'{(string.IsNullOrEmpty(el.ComentarioActivo) ? "NULL" : el.ComentarioActivo)}'" +
                                   $",'{(string.IsNullOrEmpty(el.Comentarios) ? "NULL" : el.Comentarios)}'" +
                                   $",'{num2}'" +
                                   $",'{num3}'" +
                                   $",{(string.IsNullOrEmpty(el.Fecha_Fin_Garantia) ? "NULL" : el.Fecha_Fin_Garantia)}" +
                                   $",'0'" +
                                   $")";

                        using (SqlCommand cmd = new SqlCommand(querys, db))
                        {
                            db.Open();
                            cmd.ExecuteNonQuery();
                            db.Close();
                        }
                    }

                    #endregion

                    #endregion

                    #region Borrado Local
                    
                    dbSqlite.Execute("DELETE FROM DescripcionesActivos");
                    dbSqlite.Execute("DELETE FROM Marcas_Modelos");
                    dbSqlite.Execute("DELETE FROM ActivosFijos");
                    dbSqlite.Execute("DELETE FROM Companias");
                    dbSqlite.Execute("DELETE FROM Localidades");
                    dbSqlite.Execute("DELETE FROM Areas");
                    dbSqlite.Execute("DELETE FROM Departamentos");
                    dbSqlite.Execute("DELETE FROM Oficinas");
                    dbSqlite.Execute("DELETE FROM Empleados");
                    dbSqlite.Execute("DELETE FROM ClasesActivos");

                    #endregion

                    #region Insertado Local
                    dbSqlite.Commit();
                    sync();

                    #endregion
                }
                else
                {
                    throw new Exception("No esta conectado al servidor.");
                }
                visible = false;
            }
            catch (Exception ex)
            {
                visible = false;
                App.Current.MainPage.DisplayAlert("Error", ex.Message, "ok");

            }
        }

        public void bruteSync()
        {
            var dbSqlite = new SQLiteConnection(Preferences.Get("DB_PATH", ""));

            try
            {
                dbSqlite.Execute("DELETE FROM DescripcionesActivos");
                dbSqlite.Execute("DELETE FROM Marcas_Modelos");
                dbSqlite.Execute("DELETE FROM ActivosFijos");
                dbSqlite.Execute("DELETE FROM Companias");
                dbSqlite.Execute("DELETE FROM Localidades");
                dbSqlite.Execute("DELETE FROM Areas");
                dbSqlite.Execute("DELETE FROM Departamentos");
                dbSqlite.Execute("DELETE FROM Oficinas");
                dbSqlite.Execute("DELETE FROM Empleados");
                dbSqlite.Execute("DELETE FROM ClasesActivos");

                sync();
            }
            catch(Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Error", ex.Message, "ok");
            }
        }
        
    }
}
