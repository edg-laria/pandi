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
    
    public partial class Cheque
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cheque()
        {
            this.Caja = new HashSet<Caja>();
            this.CompraFacturaPago = new HashSet<CompraFacturaPago>();
            this.FactVentaCobro = new HashSet<FactVentaCobro>();
        }
    
        public int Id { get; set; }
        public int NumeroCheque { get; set; }
        public int IdBanco { get; set; }
        public System.DateTime Fecha { get; set; }
        public string DiaClearing { get; set; }
        public decimal Importe { get; set; }
        public int IdCliente { get; set; }
        public string Descripcion { get; set; }
        public string NumeroRecibo { get; set; }
        public Nullable<System.DateTime> FechaIngreso { get; set; }
        public System.DateTime FechaEgreso { get; set; }
        public string Destino { get; set; }
        public Nullable<int> IdMoneda { get; set; }
        public Nullable<int> IdGrupoCaja { get; set; }
        public string IdFactura { get; set; }
        public string NumeroPago { get; set; }
        public string Registro { get; set; }
        public string Proveedor { get; set; }
        public Nullable<bool> Endosado { get; set; }
        public Nullable<bool> Activo { get; set; }
        public Nullable<int> IdUsuario { get; set; }
        public Nullable<System.DateTime> UltimaModificacion { get; set; }
    
        public virtual BancoCuenta BancoCuenta { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Caja> Caja { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompraFacturaPago> CompraFacturaPago { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FactVentaCobro> FactVentaCobro { get; set; }
        public virtual TipoMoneda TipoMoneda { get; set; }
    }
}
