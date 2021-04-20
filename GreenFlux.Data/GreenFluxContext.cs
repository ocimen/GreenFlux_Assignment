using System;
using System.Collections.Generic;
using GreenFlux.Domain;
using Microsoft.EntityFrameworkCore;

namespace GreenFlux.Data
{
    public class GreenFluxContext : DbContext
    {
        public GreenFluxContext()
        {
        }

        public GreenFluxContext(DbContextOptions<GreenFluxContext> options) : base(options)
        {
            FillGroup();
        }

        public DbSet<Group> Group { get; set; }
        public DbSet<ChargeStation> ChargeStation { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Product Version", "1.0.0.0");
            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("Group");
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Capacity).IsRequired();
                entity.HasMany(g => g.ChargeStations)
                    .WithOne()
                    .HasForeignKey(cs => cs.GroupId)
                    .HasConstraintName("FK_ChargeStation_Group_GroupId");
            });

            modelBuilder.Entity<ChargeStation>(entity =>
            {
                entity.ToTable("ChargeStation");
                entity.HasKey(cs => cs.Id).HasName("PK_ChargeStation_Id");
                entity.Property(cs => cs.Id).ValueGeneratedNever();

                entity.OwnsMany(cs => cs.Connectors, connectorBuilder =>
                {
                    connectorBuilder
                        .WithOwner()
                        .HasForeignKey(c => c.ChargeStationId)
                        .HasConstraintName("FK_Connector_ChargeStation_ChargeStationId");

                    connectorBuilder.Property(c => c.Id).ValueGeneratedNever();
                    connectorBuilder.HasKey(c => new {c.ChargeStationId, c.Id}).HasName("PK_Connector_ChargeStationId_Id");
                    connectorBuilder.Property(c => c.MaxCurrent).ValueGeneratedNever();
                });
            });
        }

        private void FillGroup()
        {
            var group = new Group(Guid.Parse("8C55849F-2329-4D59-B26D-1A4C8C48B08C"), "First Group", 50);
            var chargeStationId = Guid.Parse("243B6B5A-CCBB-4CAD-90DE-1F4E17D727F4");

            var connector = new Connector
            {
                Id = 1,
                MaxCurrent = 5,
                ChargeStationId = chargeStationId
            };

            var changeStation = new ChargeStation
            {
                Id = chargeStationId,
                Name = "First Charge Station",
                GroupId = group.Id,
                Connectors = new List<Connector>
                {
                    connector
                }
            };

            Group.Add(group);
            ChargeStation.Add(changeStation);
            SaveChanges();
        }
    }
}
