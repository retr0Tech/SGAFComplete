using System;
namespace SGAFComplete.Models
{
    public class Areas
    {
        public int Area { get; set; }
        public int Parent { get; set; }
        public string CodigoERP { get; set; }
        public string Descripcion { get; set; }
        public string Descripcion_Abreviada { get; set; }
        public int Activo { get; set; }
        public int Departamentos { get; set; }
        public string Fecha_Ingreso { get; set; }
        public string Usuario_Ingreso { get; set; }
        public string Fecha_Modificado { get; set; }
        public string Usuario_Modificado { get; set; }
        public string Plano { get; set; }
    }
}
