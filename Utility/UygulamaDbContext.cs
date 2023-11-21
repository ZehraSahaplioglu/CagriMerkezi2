using CagriMerkezi2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CagriMerkezi2.Utility
{
    public class UygulamaDbContext : IdentityDbContext
    {

        public UygulamaDbContext(DbContextOptions<UygulamaDbContext> options) : base(options) { }
        // bu bir constructor

        public DbSet<Birim> Birimler { get; set; }

        public DbSet<Departman> Departmanlar { get; set; }

        public DbSet<Sikayet> Sikayetler { get; set; }

        public DbSet<Calisan> Calisanlar { get; set; }

        public DbSet<CagriMerkezi> CagriMerkezis { get; set; }

        public DbSet<UygKullancisi> UygKullanicilari { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Birim ve Departman arasındaki one-to-many ilişkisi
            modelBuilder.Entity<Birim>()
                .HasMany(birim => birim.DepList)
                .WithOne(departman => departman.Birim)
                .HasForeignKey(departman => departman.BirimId);


            // Birim, Depatman ve Sikayet arasındaki one-to-many ilişkileri
            modelBuilder.Entity<Birim>()
                .HasMany(birim => birim.SikayetList)
                .WithOne(sikayet => sikayet.Birim)
                .HasForeignKey(sikayet => sikayet.BirimId);

            modelBuilder.Entity<Departman>()
                .HasMany(departman => departman.SikayetList)
                .WithOne(sikayet => sikayet.Departman)
                .HasForeignKey(sikayet => sikayet.DepId);


            // Birim, Depatman ve Calisan arasındaki one-to-many ilişkileri
            modelBuilder.Entity<Birim>()
                .HasMany(birim => birim.CalisanList)
                .WithOne(calisan => calisan.Birim)
                .HasForeignKey(calisan => calisan.BirimId);

            modelBuilder.Entity<Departman>()
                .HasMany(departman => departman.CalisanList)
                .WithOne(calisan => calisan.Departman)
                .HasForeignKey(calisan => calisan.DepId);

            // Birim, Depatman ve CagriMerkezi arasındaki one-to-many ilişkileri
            modelBuilder.Entity<Birim>()
                .HasMany(birim => birim.CagriList)
                .WithOne(cagriMerkezi => cagriMerkezi.Birim)
                .HasForeignKey(cagriMerkezi => cagriMerkezi.BirimId);

            modelBuilder.Entity<Departman>()
                .HasMany(departman => departman.CagriList)
                .WithOne(cagriMerkezi => cagriMerkezi.Departman)
                .HasForeignKey(cagriMerkezi => cagriMerkezi.DepId);


            // identity için:
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
            });

        }

    }

}
