using System;
namespace SGAFComplete.Models
{
    public class Auditorias_Detalles
    {
        public int Secuencia { get; set; }
        public int Auditoria { get; set; }
        public int ActivosFijo { get; set; }
        public string Estatus { get; set; }
        public string TagRFID { get; set; }
        public string Fecha_Captura { get; set; }
        public string Usuario_Captura { get; set; }
        public string Auditoria_Control { get; set; }
        public string Fecha_Ingreso { get; set; }
        public string Usuario_Ingreso { get; set; }
        public string Fecha_Modificado { get; set; }
        public string Usuario_Modificado { get; set; }
        public int Activo { get; set; }
        public string ComentarioInactivo { get; set; }
        public string Fecha_Inactivo { get; set; }
        public string Usuario_Inactivo { get; set; }
        public string TagCodigo { get; set; }
        public int Compania { get; set; }
        public int Localidad { get; set; }
        public int Area { get; set; }
        public int Departamento { get; set; }
        public int Oficina { get; set; }
        public int Veces_Capturado { get; set; }
    }
}
