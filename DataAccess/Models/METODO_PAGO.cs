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
    
    public partial class METODO_PAGO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public METODO_PAGO()
        {
            this.TRANSACCION = new HashSet<TRANSACCION>();
        }
    
        public long ID { get; set; }
        public string METODO_NOMBRE { get; set; }
        public string TARJETA_NUMERO { get; set; }
        public string TARJETA_NOMBRE_TITULAR { get; set; }
        public System.DateTime FECHA_VENCIMIENTO { get; set; }
        public System.DateTime CREADO_EN { get; set; }
        public System.DateTime ACTUALIZADO_EN { get; set; }
        public long USUARIO_ID { get; set; }
    
        public virtual USUARIO USUARIO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TRANSACCION> TRANSACCION { get; set; }
    }
}
