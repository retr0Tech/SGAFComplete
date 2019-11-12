using System;
using SQLite;

namespace SGAFComplete.Models
{
    public class DescripcionesActivos
    {
        //Esta clase tiene implementado el actualizador
        [PrimaryKey]
        public int DescripcionActivo { get; set; }
        public string Descripcion { get; set; }
        public int ClasesActivos { get; set; }
        public string Fecha_Ingreso { get; set; }
        public string Usuario_Ingreso { get; set; }
        public string Fecha_Modificado { get; set; }
        public string Usuario_Modificado { get; set; }
        public int PedirSerie { get; set; }
        public int PedirEmpleado { get; set; }
        public int PedirIDFoto { get; set; }
        public string CodigoERP { get; set; }
        public int PedirCodigoERP { get; set; }
        // 0:sincronizado, 1:insertado, 2:modificado
        public int Actualizador { get; set; }
    }
}
