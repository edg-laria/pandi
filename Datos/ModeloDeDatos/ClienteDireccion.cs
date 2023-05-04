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
    
    public partial class ClienteDireccion
    {
        public int Id { get; set; }
        public Nullable<int> IdCliente { get; set; }
        public string CodigoAfip { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public Nullable<int> IdPais { get; set; }
        public Nullable<int> IdProvincia { get; set; }
        public Nullable<int> IdLocalidad { get; set; }
        public string IdCodigoPostal { get; set; }
        public string Telefono { get; set; }
        public string Fax { get; set; }
        public string Cuit { get; set; }
        public string Email { get; set; }
        public string IdPieNota { get; set; }
        public Nullable<int> IdIdioma { get; set; }
        public string LocalA { get; set; }
        public Nullable<int> IdTipoMoneda { get; set; }
        public Nullable<bool> Activo { get; set; }
        public Nullable<int> IdUsuario { get; set; }
        public Nullable<System.DateTime> UltimaModificacion { get; set; }
    
        public virtual Cliente Cliente { get; set; }
        public virtual Pais Pais { get; set; }
        public virtual Provincia Provincia { get; set; }
        public virtual TipoIdioma TipoIdioma { get; set; }
        public virtual TipoMoneda TipoMoneda { get; set; }
        public virtual Localidad Localidad { get; set; }
        public virtual Localidad Localidad1 { get; set; }
    }
}
