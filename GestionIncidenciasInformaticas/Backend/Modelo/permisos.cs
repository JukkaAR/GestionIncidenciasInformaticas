//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GestionIncidenciasInformaticas.Backend.Modelo
{
    using System;
    using System.Collections.Generic;
    
    public partial class permisos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public permisos()
        {
            this.roles = new HashSet<roles>();
        }
    
        public int cod_permiso { get; set; }
        public string desc_permiso { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<roles> roles { get; set; }
    }
}
