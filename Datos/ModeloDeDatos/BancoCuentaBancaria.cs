//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Datos.ModeloDeDatos
{
    using System;
    using System.Collections.Generic;
    
    public partial class BancoCuentaBancaria
    {
        public int Id { get; set; }
        public int NumeroOperacion { get; set; }
        public int IdBancoCuenta { get; set; }
        public int IdGrupoCaja { get; set; }
        public int NumeroCierre { get; set; }
        public string CuentaDescripcion { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public Nullable<System.DateTime> FechaEfectiva { get; set; }
        public string DiaClearing { get; set; }
        public decimal Importe { get; set; }
        public string IdCliente { get; set; }
        public bool Conciliacion { get; set; }
        public System.DateTime FechaIngreso { get; set; }
        public string IdImputacion { get; set; }
        public Nullable<bool> Activo { get; set; }
        public Nullable<int> IdUsuario { get; set; }
        public Nullable<System.DateTime> UltimaModificacion { get; set; }
    
        public virtual BancoCuenta BancoCuenta { get; set; }
        public virtual GrupoCaja GrupoCaja { get; set; }
    }
}
