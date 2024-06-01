using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace CG_AirLineApi.Models
{
    public partial class CG_AirlinesContext : DbContext
    {
        public CG_AirlinesContext()
        {
        }

        public CG_AirlinesContext(DbContextOptions<CG_AirlinesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Flight> Flights { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=LIN37001600\\SQLEXPRESS;Database=CG_Airlines;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Flight>(entity =>
            {
                entity.ToTable("Flight");

                entity.Property(e => e.FlightId)
                    .ValueGeneratedNever()
                    .HasColumnName("FlightID");

                entity.Property(e => e.ArrivalTime)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DeptTime)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Destination)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Fare).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.LaunchDate).HasColumnType("date");

                entity.Property(e => e.Origin)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.HasKey(e => e.TicketNo)
                    .HasName("Pk2_Tno");

                entity.ToTable("Reservation");

                entity.Property(e => e.DateofBooking).HasColumnType("date");

                entity.Property(e => e.Email)
                    .HasMaxLength(70)
                    .IsUnicode(false);

                entity.Property(e => e.FlightId).HasColumnName("FlightID");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.JourneyDate).HasColumnType("date");

                entity.Property(e => e.PassengerName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Ticketstatus)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("ticketstatus");

                entity.Property(e => e.TotalFare).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Flight)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.FlightId)
                    .HasConstraintName("fk_fid");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("fk_uid");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Password)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__users__RoleId__52593CB8");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
