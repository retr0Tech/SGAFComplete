using System;
namespace SGAFComplete.Models
{
    public class Empleados
    {
        public int Empleado { get; set; }
        public string CodigoERP { get; set; }
        public string CodigoRFID { get; set; }
        public string Descripcion { get; set; }
        public string Descripcion_Abreviada { get; set; }
        public string Telefono_Directo { get; set; }
        public string Extension { get; set; }
        public int? Superior { get; set; }
        public int Activo { get; set; }
        public string Fecha_Ingreso { get; set; }
        public string Usuario_Ingreso { get; set; }
        public string Fecha_Modificado { get; set; }
        public string Usuario_Modificado { get; set; }
        public string NombreFoto { get; set; }
        public string CentroCosto { get; set; }
        public string Cargo { get; set; }
        public int? Oficina { get; set; }
        public int? Departamento { get; set; }
    }
}
