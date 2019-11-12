using System;
namespace SGAFComplete.Models
{
    public class Movimientos_Equipos
    {
        public int Movimientos { get; set; }
        public string Identificacion { get; set; }
        public int Area { get; set; }
        public int DescripcionActivo { get; set; }
        public int ClaseActivo { get; set; }
        public int MarcaModelo { get; set; }
        public string NumeroSerie { get; set; }
        public string Fecha_Entrada { get; set; }
        public string Fecha_Salida { get; set; }
        public int Contacto_En_Compania { get; set; }
        public string Tipo_Visitante { get; set; }
        public string Fecha_Ingreso { get; set; }
        public string Usuario_Ingreso { get; set; }
        public string Fecha_Modificado { get; set; }
        public string Usuario_Modificado { get; set; }
        public string Fecha { get; set; }
        public string TagRFID { get; set; }
        public string Comentarios { get; set; }
        public string TieneArmaDeFuego { get; set; }
    }
}
