﻿using System;
namespace SGAFComplete.Models
{
    public class ClasesActivos
    {
        public int ClaseActivo { get; set; }
        public string CodigoERP { get; set; }
        public string Descripcion { get; set; }
        public string Descripcion_Abreviada { get; set; }
        public int Activo { get; set; }
        public string Fecha_Ingreso { get; set; }
        public string Usuario_Ingreso { get; set; }
        public string Fecha_Modificado { get; set; }
        public string Usuario_Modificado { get; set; }
        public int PedirSerie { get; set; }
        public int PedirEmpleado { get; set; }
        public int PedirIDFoto { get; set; }
        public int PedirCodigoERP { get; set; }
        public int Parent { get; set; }
    }
}
