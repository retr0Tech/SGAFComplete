using System;
namespace SGAFComplete.ViewModels
{
    public class DondeEstoyViewModel
    {
        public string Empresa { get; set; }
        public string Edificio { get; set; }
        public string Piso { get; set; }
        public string CentroCosto { get; set; }
        public string Oficina { get; set; }
        public string Descripcion { get; set; }
        public string Empleado { get; set; }

        public DondeEstoyViewModel()
        {
            Empresa = MainCapturaDeActivosViewModel._Empresa;
            Edificio = MainCapturaDeActivosViewModel._Edificio;
            Piso = MainCapturaDeActivosViewModel._Piso;
            CentroCosto = MainCapturaDeActivosViewModel._CentroCosto;
            Oficina = MainCapturaDeActivosViewModel._Oficina;
            Descripcion = MainCapturaDeActivosViewModel._Descripcion;
            Empleado = MainCapturaDeActivosViewModel._Empleado;
            if (string.IsNullOrEmpty(Empresa))
            {
                Empresa = "No especificado";
            }
            if (string.IsNullOrEmpty(Edificio))
            {
                Edificio = "No especificado";
            }
            if (string.IsNullOrEmpty(Piso))
            {
                Piso = "No especificado";
            }
            if (string.IsNullOrEmpty(CentroCosto))
            {
                CentroCosto = "No especificado";
            }
            if (string.IsNullOrEmpty(Oficina))
            {
                Oficina = "No especificado";
            }
            if (string.IsNullOrEmpty(Descripcion))
            {
                Descripcion = "No especificado";
            }
            if (string.IsNullOrEmpty(Empleado))
            {
                Empleado = "No especificado";
            }
        }
    }
}
