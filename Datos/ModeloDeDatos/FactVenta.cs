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
    
    public partial class FactVenta
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FactVenta()
        {
            this.FactVentaCobro = new HashSet<FactVentaCobro>();
            this.ItemImpre = new HashSet<ItemImpre>();
            this.Retencion = new HashSet<Retencion>();
        }
    
        public int Id { get; set; }
        public Nullable<int> IdFacturaElectronica { get; set; }
        public int IdTipoComprobante { get; set; }
        public int NumeroFactura { get; set; }
        public string Codigo { get; set; }
        public Nullable<int> IdCliente { get; set; }
        public decimal TotalDolares { get; set; }
        public decimal Total { get; set; }
        public decimal Parcial { get; set; }
        public decimal Saldo { get; set; }
        public System.DateTime Fecha { get; set; }
        public System.DateTime Vencimiento { get; set; }
        public string Tipo { get; set; }
        public string Baja { get; set; }
        public string Impre { get; set; }
        public Nullable<System.DateTime> FechaCobro { get; set; }
        public string TipoIva { get; set; }
        public string Concepto { get; set; }
        public Nullable<int> IdDto { get; set; }
        public string Marca { get; set; }
        public string Condicion { get; set; }
        public int IdProvincia { get; set; }
        public int IdPais { get; set; }
        public int IdImputacion { get; set; }
        public int NumeroCobro { get; set; }
        public decimal Cotiza { get; set; }
        public int IdMoneda { get; set; }
        public string ORef { get; set; }
        public string YRef { get; set; }
        public decimal Gasto { get; set; }
        public string Descuento { get; set; }
        public decimal CotizaP { get; set; }
        public string TipoFac { get; set; }
        public int CodigoDiario { get; set; }
        public string Recibo { get; set; }
        public int Periodo { get; set; }
        public string NumeroTra { get; set; }
        public double Combria { get; set; }
        public double IngBr { get; set; }
        public string Anula { get; set; }
        public Nullable<bool> Activo { get; set; }
        public Nullable<int> IdUsuario { get; set; }
        public Nullable<System.DateTime> UltimaModificacion { get; set; }
    
        public virtual Cliente Cliente { get; set; }
        public virtual TipoComprobanteVenta TipoComprobanteVenta { get; set; }
        public virtual TipoMoneda TipoMoneda { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FactVentaCobro> FactVentaCobro { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ItemImpre> ItemImpre { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Retencion> Retencion { get; set; }
        public virtual FacturaElectronica FacturaElectronica { get; set; }
    }
}
