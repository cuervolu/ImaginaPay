//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ws_ImaginaPay.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AUTH_PERMISSION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AUTH_PERMISSION()
        {
            this.AUTH_GROUP_PERMISSIONS = new HashSet<AUTH_GROUP_PERMISSIONS>();
            this.USUARIO_USER_PERMISSIONS = new HashSet<USUARIO_USER_PERMISSIONS>();
        }
    
        public long ID { get; set; }
        public string NAME { get; set; }
        public long CONTENT_TYPE_ID { get; set; }
        public string CODENAME { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AUTH_GROUP_PERMISSIONS> AUTH_GROUP_PERMISSIONS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USUARIO_USER_PERMISSIONS> USUARIO_USER_PERMISSIONS { get; set; }
    }
}
