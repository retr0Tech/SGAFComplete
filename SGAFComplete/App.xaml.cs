using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Android.Content;
using SGAFComplete.Models;
using SGAFComplete.Views;
using SQLite;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SGAFComplete
{
    public partial class App : Application
    {
        public App(Context context)
        {
            InitializeComponent();

            MainPage = new NavigationPage(new SgafMainPage(context));
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "SGAF.db");
            Preferences.Set("DB_PATH", dbPath);
            Preferences.Set("VERSION", "1.28.5.19.1");
            var db = new SQLiteConnection(dbPath);

            db.CreateTable<ActivosFijos>();
            db.CreateTable<Areas>();
            db.CreateTable<Auditorias>();
            db.CreateTable<Auditorias_Control>();
            db.CreateTable<Auditorias_Detalles>();
            db.CreateTable<ClasesActivos>();
            db.CreateTable<Companias>();
            db.CreateTable<Departamentos>();
            db.CreateTable<DescripcionesActivos>();
            db.CreateTable<Empleados>();
            db.CreateTable<Imagenes>();
            db.CreateTable<Localidades>();
            db.CreateTable<Marcas_Modelos>();
            db.CreateTable<Movimientos_Equipos>();
            db.CreateTable<Oficinas>();
            db.CreateTable<Parametros>();
            db.CreateTable<Plantilla_Captura>();
            db.CreateTable<ReceocionActivos>();
            db.CreateTable<ROLES>();
            db.CreateTable<ROLES_OPCIONES>();
            db.CreateTable<SYSTABLE>();
            db.CreateTable<ServidorDescrip>();
            db.CreateTable<LastSync>();

            if ((Preferences.Get("FIRST_BOOT", true)))
            {
                //Indices de la Tabla
                Task.Factory.StartNew(() =>
                {
                    Debug.WriteLine("Creating indexes");
                    db.CreateIndex("ActivosFijos", "ActivoFijo", true);
                    Debug.Write("Done! Creating indexes");
                });

                ServidorDescrip server = new ServidorDescrip();

                server.Timeout = 1000;
                server.ServerIp = "192.168.1.103";
                server.Database = "SGAFDB_TOTAL";
                server.Port = 9100;
                server.User = "vocollect";
                server.Password = "130060401";

                db.Insert(server);

                LastSync sync = new LastSync();
                sync.lastSync = DateTime.Now.ToString();
                db.Insert(sync);

                Preferences.Set("FIRST_BOOT", false);
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
