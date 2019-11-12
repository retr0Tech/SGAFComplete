using System;
namespace SGAFComplete.Models
{
    public class Oficinas
    {
        public int Oficina { get; set; }
        public int Parent { get; set; }
        public string CodigoERP { get; set; }
        public string Descripcion { get; set; }
        public string Descripcion_Abreviada { get; set; }
        public int Activo { get; set; }
        public string Fecha_Ingreso { get; set; }
        public string Usuario_Ingreso { get; set; }
        public string Fecha_Modificado { get; set; }
        public string Usuario_Modificado { get; set; }
        public int? Izquierda { get; set; }
        public int? Tope { get; set; }
        public int? Derecha { get; set; }
        public int? Fondo { get; set; }
        public int? Ancho { get; set; }
        public int? Alto { get; set; }
        public int? Mapeando { get; set; }
        public long? Plantilla_ID { get; set; }
        public int? Empleado { get; set; }
        public int? Departamento { get; set; }
        public string EmpleadoCodigoERP { get; set; }
    }
}
