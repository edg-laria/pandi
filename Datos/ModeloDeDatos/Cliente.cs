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
    
    public partial class Cliente
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cliente()
        {
            this.FactVenta = new HashSet<FactVenta>();
            this.ClienteDireccion = new HashSet<ClienteDireccion>();
        }
    
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public Nullable<int> IdTipoiva { get; set; }
        public string Cuit { get; set; }
        public Nullable<int> DiasFactura { get; set; }
        public Nullable<int> IdImputacion { get; set; }
        public string Observaciones { get; set; }
        public string Email { get; set; }
        public Nullable<int> IdXml { get; set; }
        public Nullable<int> IdPieNota { get; set; }
        public Nullable<int> IdIdioma { get; set; }
        public Nullable<int> IdTipoCliente { get; set; }
        public Nullable<bool> Visible { get; set; }
        public Nullable<int> IdNotaPieB { get; set; }
        public Nullable<int> IdTipoMoneda { get; set; }
        public Nullable<int> IdGrupoPresupuesto { get; set; }
        public Nullable<bool> MiPyme { get; set; }
        public Nullable<bool> Activo { get; set; }
        public Nullable<int> IdUsuario { get; set; }
        public Nullable<System.DateTime> UltimaModificacion { get; set; }
    
        public virtual GrupoPresupuesto GrupoPresupuesto { get; set; }
        public virtual GrupoPresupuesto GrupoPresupuesto1 { get; set; }
        public virtual PieNota PieNota { get; set; }
        public virtual TipoCliente TipoCliente { get; set; }
        public virtual TipoIdioma TipoIdioma { get; set; }
        public virtual TipoMoneda TipoMoneda { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FactVenta> FactVenta { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClienteDireccion> ClienteDireccion { get; set; }
    }
}
