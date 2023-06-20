namespace GestionIncidenciasInformaticas.Backend
{
    class RolesPermisos
    {
        public string rol { get; set; }
        public bool puedeAnyadirIncidencias { get; set; }
        public bool puedeModificarIncidencias { get; set; }
        public bool puedeEditarTipoHW { get; set; }
        public bool puedeEditarRoles { get; set; }
        public bool puedeImportarExportar { get; set; }
        public bool puedeGenerarInformes { get; set; }
    }
}
