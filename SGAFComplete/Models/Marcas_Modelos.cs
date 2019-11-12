using System;
namespace SGAFComplete.Models
{
    public class Marcas_Modelos
    {
        //Esta clase tiene imlpementado el actualizador
        public int Marca { get; set; }
        public string Descripcion { get; set; }
        public int DescripcionActivo { get; set; }
        public string Fecha_Ingreso { get; set; }
        public string Usuario_Ingreso { get; set; }
        public string Fecha_Modificado { get; set; }
        public string Usuario_Modificado { get; set; }
        public string CodigoERP { get; set; }
        public string CodigoFoto { get; set; }
        public string CodigoFotoEtiquetado { get; set; }
        public string Descripcion_Etiquetado { get; set; }
        // 0:sincronizado, 1:insertado, 2:modificado
        public int Actualizador { get; set; }
    }
}
