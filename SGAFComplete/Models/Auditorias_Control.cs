using System;
namespace SGAFComplete.Models
{
    public class Auditorias_Control
    {
        public int Auditoria_Control { get; set; }
        public string Fecha { get; set; }
        public int Activos { get; set; }
        public int Ubicados { get; set; }
        public int Desubicados { get; set; }
        public int Faltantes { get; set; }
        public int Sobrantes { get; set; }
        public string Fecha_Ingreso { get; set; }
        public string Usuario_Ingreso { get; set; }
        public string Fecha_Modificado { get; set; }
        public string Usuario_Modificado { get; set; }
        public string Estado { get; set; }
        public string Fecha_Cierre { get; set; }
        public string Usuario_Cierre { get; set; }
        public string Descripcion { get; set; }
        public string Fecha_Borrado { get; set; }
        public string Usuario_Borrado { get; set; }
        public int Auditoria_Nivel { get; set; }
        public string Fecha_Inicio { get; set; }
        public string Usuario_Inicio { get; set; }

    }
}
