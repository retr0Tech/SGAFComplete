using System;
namespace SGAFComplete.Models
{
    public class Auditorias
    {
        public int Auditoria { get; set; }
        public string Fecha { get; set; }
        public int Compania { get; set; }
        public int Localidad { get; set; }
        public int Area { get; set; }
        public int Departamento { get; set; }
        public int Oficina { get; set; }
        public int Activos { get; set; }
        public int Ubicados { get; set; }
        public int Desubicados { get; set; }
        public int Faltantes { get; set; }
        public int Sobrantes { get; set; }
        public string Fecha_Ingreso { get; set; }
        public string Usuario_Ingreso { get; set; }
        public string Fecha_Modificado { get; set; }
        public string Usuario_Modificado { get; set; }
        public string Identificador { get; set; }
        public string Estado { get; set; }
        public string Fecha_Cierre { get; set; }
        public string Usuario_Cierre { get; set; }
        public string Descripcion { get; set; }
        public string Fecha_Borrado { get; set; }
        public string Usuario_Borrado { get; set; }
        public int Auditoria_Control { get; set; }
        public int Auditoria_Nivel { get; set; }
    }
}
