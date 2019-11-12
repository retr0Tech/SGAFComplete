using System;
namespace SGAFComplete.Models
{
    //Esta clase tiene actualizador implementado
    public class ActivosFijos
    {
        public int ActivoFijo { get; set; }
        public int Compania { get; set; }
        public int Localidad { get; set; }
        public int Area { get; set; }
        public int Departamento { get; set; }
        public int Oficina { get; set; }
        public int DescripcionActivo { get; set; }
        public int ClaseActivo { get; set; }
        public int MarcaModelo { get; set; }
        public int Empleado { get; set; }
        public string TagRFID { get; set; }
        public string Descripcion { get; set; }
        public string CodigoERP { get; set; }
        public string DescripcionERP { get; set; }
        public int? Imagen { get; set; }
        public string FotoID { get; set; }
        public string NumeroSerie { get; set; }
        public string Fecha_Ingreso { get; set; }
        public string Usuario_Ingreso { get; set; }
        public string Fecha_Modificado { get; set; }
        public string Usuario_Modificado { get; set; }
        public int Activo { get; set; }
        public string Fecha_Ultima_Auditoria { get; set; }
        public string Movible { get; set; }
        public string Asignado { get; set; }
        public string CapturadoVia { get; set; }
        public string TagCodigo { get; set; }
        public string ComentarioInactivo { get; set; }
        public string ComentarioActivo { get; set; }
        public string Comentarios { get; set; }
        public int? Origen { get; set; }
        public int? RecepcionID { get; set; }
        public string Fecha_Fin_Garantia { get; set; }
        // 0:sincronizado, 1:insertado, 2:modificado
        public int Actualizador { get; set; }
    }
}
