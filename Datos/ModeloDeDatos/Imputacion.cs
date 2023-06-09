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
    
    public partial class Imputacion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Imputacion()
        {
            this.BancoCuenta = new HashSet<BancoCuenta>();
            this.CompraFactura = new HashSet<CompraFactura>();
            this.Diario = new HashSet<Diario>();
            this.Proveedor = new HashSet<Proveedor>();
            this.Proveedor1 = new HashSet<Proveedor>();
        }
    
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public Nullable<int> IdSubRubro { get; set; }
        public Nullable<decimal> SaldoInicial { get; set; }
        public Nullable<decimal> SaldoFin { get; set; }
        public Nullable<int> IdTipo { get; set; }
        public string Alias { get; set; }
        public Nullable<decimal> Enero { get; set; }
        public Nullable<decimal> Febrero { get; set; }
        public Nullable<decimal> Marzo { get; set; }
        public Nullable<decimal> Abril { get; set; }
        public Nullable<decimal> Mayo { get; set; }
        public Nullable<decimal> Junio { get; set; }
        public Nullable<decimal> Julio { get; set; }
        public Nullable<decimal> Agosto { get; set; }
        public Nullable<decimal> Septiembre { get; set; }
        public Nullable<decimal> Octubre { get; set; }
        public Nullable<decimal> Noviembre { get; set; }
        public Nullable<decimal> Diciembre { get; set; }
        public Nullable<bool> Activo { get; set; }
        public Nullable<int> IdUsuario { get; set; }
        public Nullable<System.DateTime> UltimaModificacion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BancoCuenta> BancoCuenta { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CompraFactura> CompraFactura { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Diario> Diario { get; set; }
        public virtual SubRubro SubRubro { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Proveedor> Proveedor { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Proveedor> Proveedor1 { get; set; }
    }
}
