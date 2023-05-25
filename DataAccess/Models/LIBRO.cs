//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class LIBRO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LIBRO()
        {
            this.DETALLE_CARRITO = new HashSet<DETALLE_CARRITO>();
            this.DETALLE_PEDIDO = new HashSet<DETALLE_PEDIDO>();
            this.MANTENIMIENTO = new HashSet<MANTENIMIENTO>();
            this.OFERTA = new HashSet<OFERTA>();
        }
    
        public long ID_LIBRO { get; set; }
        public string NOMBRE_LIBRO { get; set; }
        public string DESCRIPCION { get; set; }
        public string AUTOR { get; set; }
        public string EDITORIAL { get; set; }
        public decimal PRECIO_UNITARIO { get; set; }
        public long CANTIDAD_DISPONIBLE { get; set; }
        public string PORTADA { get; set; }
        public Nullable<System.DateTime> FECHA_PUBLICACION { get; set; }
        public string CATEGORIA { get; set; }
        public string ISBN { get; set; }
        public string SLUG { get; set; }
        public string THUMBNAIL { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DETALLE_CARRITO> DETALLE_CARRITO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DETALLE_PEDIDO> DETALLE_PEDIDO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MANTENIMIENTO> MANTENIMIENTO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OFERTA> OFERTA { get; set; }
    }
}
