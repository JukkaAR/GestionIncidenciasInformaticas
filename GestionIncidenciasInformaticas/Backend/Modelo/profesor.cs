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
    
    public partial class profesor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public profesor()
        {
            this.incidencia = new HashSet<incidencia>();
            this.incidencia1 = new HashSet<incidencia>();
        }
    
        public string dni { get; set; }
        public string nombre { get; set; }
        public string apellidos { get; set; }
        public string email { get; set; }
        public string contraseña { get; set; }
        public int departamento_codigo_dep { get; set; }
        public string roles_rol { get; set; }
    
        public virtual departamento departamento { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<incidencia> incidencia { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<incidencia> incidencia1 { get; set; }
        public virtual roles roles { get; set; }
    }
}
