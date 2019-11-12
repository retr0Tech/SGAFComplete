using System;
namespace SGAFComplete.Models
{
    public class Companias
    {
        public int Compania { get; set; }
        public string CodigoERP { get; set; }
        public string Descripcion { get; set; }
        public string Descripcion_Abreviada { get; set; }
        public int Activo { get; set; }
        public int Localidad { get; set; }
        public string Fecha_Ingreso { get; set; }
        public string Usuario_Ingreso { get; set; }
        public string Fecha_Modificado { get; set; }
        public string Usuario_Modificado { get; set; }
        public string Direccion1 { get; set; }
        public string Direccion2 { get; set; }
        public string Telefono_Central { get; set; }
        public string RNC { get; set; }
        public string Logo { get; set; }
        public string PrefijoRFID { get; set; }
    }
}
