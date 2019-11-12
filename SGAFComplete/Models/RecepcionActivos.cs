using System;
namespace SGAFComplete.Models
{
    public class ReceocionActivos
    {
        public int RecepcionID { get; set; }
        public string Fecha { get; set; }
        public string Descripcion { get; set; }
        public int Compania { get; set; }
        public int Localidad { get; set; }
        public int Area { get; set; }
        public int Departamento { get; set; }
        public int Oficina { get; set; }
        public string Suplidor { get; set; }
        public string Fabricante { get; set; }
        public string Oreden_Compra { get; set; }
        public string Factura { get; set; }
        public string Observaciones { get; set; }
        public string Fecha_Ingreso { get; set; }
        public string Usuario_Ingreso { get; set; }
        public string Fecha_Modificado { get; set; }
        public string Usuario_Modificado { get; set; }
        public int Cantidad { get; set; }
        public int Cerrada { get; set; }
    }
}
