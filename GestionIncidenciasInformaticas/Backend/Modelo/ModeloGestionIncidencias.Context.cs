﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class gestion_incidenciasEntities : DbContext
    {
        public gestion_incidenciasEntities()
            : base("name=gestion_incidenciasEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<departamento> departamento { get; set; }
        public virtual DbSet<estados> estados { get; set; }
        public virtual DbSet<hardware> hardware { get; set; }
        public virtual DbSet<incidencia> incidencia { get; set; }
        public virtual DbSet<permisos> permisos { get; set; }
        public virtual DbSet<profesor> profesor { get; set; }
        public virtual DbSet<roles> roles { get; set; }
        public virtual DbSet<tipo_hardware> tipo_hardware { get; set; }
    }
}