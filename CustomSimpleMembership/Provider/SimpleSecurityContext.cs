using CustomSimpleMembership.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace CustomSimpleMembership.Provider
{
    public class SimpleSecurityContext : DbContext
    {
        public DbSet<UserProfile> Users { get; set; }

        public SimpleSecurityContext()
        {
            this.Configuration.LazyLoadingEnabled = true;
            this.Configuration.ProxyCreationEnabled = false;
        }

        public SimpleSecurityContext(string connStringName) :
            base(connStringName)
        {
            this.Configuration.LazyLoadingEnabled = true;
            this.Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new UserMapping());
        }
    }

    public class UserMapping : EntityMappingBase<UserProfile>
    {
        public UserMapping()
        {
            this.Property(x => x.UserName);
            this.Property(x => x.DisplayName);
            this.Property(x => x.Password);
            this.Property(x => x.Email);

            this.ToTable("UserProfile");
        }
    }

    public abstract class EntityMappingBase<T> : EntityTypeConfiguration<T> where T : Entity
    {
        protected EntityMappingBase()
        {
            this.HasKey(x => x.Id);

            this.Property(x => x.CreatedDate).IsRequired();
            this.Property(x => x.CreatedBy).IsRequired();
            this.Property(x => x.ModifiedDate).IsOptional();
        }
    }
}