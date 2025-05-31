using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DiplomStoreHouse.ModelDbase;

public partial class StoreHouseContext : DbContext
{
    public StoreHouseContext()
    {
    }

    public StoreHouseContext(DbContextOptions<StoreHouseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Operation> Operations { get; set; }

    public virtual DbSet<QualityControl> QualityControls { get; set; }

    public virtual DbSet<Reception> Receptions { get; set; }

    public virtual DbSet<Shipment> Shipments { get; set; }

    public virtual DbSet<TaskAssignment> TaskAssignments { get; set; }

    public virtual DbSet<Transfer> Transfers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WarehouseDivision> WarehouseDivisions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-OP2C2HC\\SQLEXPRESS; Database=StoreHouse; User=sa; Password=1234567890; Encrypt=false");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employee");

            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Posititon).HasMaxLength(50);
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.ToTable("Item");

            entity.Property(e => e.MeasurementUnit).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.PackageType).HasMaxLength(50);
            entity.Property(e => e.UnitWeight).HasMaxLength(50);
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.ToTable("Location");

            entity.Property(e => e.LocationId).HasMaxLength(10);
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.MaxCapacity).HasMaxLength(50);
        });

        modelBuilder.Entity<Operation>(entity =>
        {
            entity.Property(e => e.FinishLocationId).HasMaxLength(10);
            entity.Property(e => e.Quantity).HasMaxLength(50);
            entity.Property(e => e.StartLocationId).HasMaxLength(10);
            entity.Property(e => e.Timestamp).HasColumnType("datetime");
            entity.Property(e => e.TypeOperation).HasMaxLength(50);

            entity.HasOne(d => d.FinishLocation).WithMany(p => p.OperationFinishLocations)
                .HasForeignKey(d => d.FinishLocationId)
                .HasConstraintName("FK_Operations_Location1");

            entity.HasOne(d => d.Item).WithMany(p => p.Operations)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Operations_Item");

            entity.HasOne(d => d.StartLocation).WithMany(p => p.OperationStartLocations)
                .HasForeignKey(d => d.StartLocationId)
                .HasConstraintName("FK_Operations_Location");
        });

        modelBuilder.Entity<QualityControl>(entity =>
        {
            entity.ToTable("QualityControl");

            entity.Property(e => e.ControlDate).HasColumnType("datetime");
            entity.Property(e => e.Result).HasMaxLength(50);

            entity.HasOne(d => d.Item).WithMany(p => p.QualityControls)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QualityControl_Item");

            entity.HasOne(d => d.ResponsibleEmployeeNavigation).WithMany(p => p.QualityControls)
                .HasForeignKey(d => d.ResponsibleEmployee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QualityControl_Employee");
        });

        modelBuilder.Entity<Reception>(entity =>
        {
            entity.ToTable("Reception");

            entity.Property(e => e.ReceiptDate).HasColumnType("datetime");
            entity.Property(e => e.ReceivedQuantity).HasMaxLength(50);
            entity.Property(e => e.SupplierInvoiceNumber).HasMaxLength(50);

            entity.HasOne(d => d.Item).WithMany(p => p.Receptions)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reception_Item1");
        });

        modelBuilder.Entity<Shipment>(entity =>
        {
            entity.ToTable("Shipment");

            entity.Property(e => e.ClientName).HasMaxLength(50);
            entity.Property(e => e.DeliveryPoint).HasMaxLength(250);
            entity.Property(e => e.ExpectedDate).HasColumnType("datetime");
            entity.Property(e => e.State).HasMaxLength(50);

            entity.HasOne(d => d.Item).WithMany(p => p.Shipments)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Shipment_Item");
        });

        modelBuilder.Entity<TaskAssignment>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PK_TaskAssigment");

            entity.ToTable("TaskAssignment");

            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");

            entity.HasOne(d => d.Employee).WithMany(p => p.TaskAssignments)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskAssignment_Employee");

            entity.HasOne(d => d.Operation).WithMany(p => p.TaskAssignments)
                .HasForeignKey(d => d.OperationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskAssignment_Operations");
        });

        modelBuilder.Entity<Transfer>(entity =>
        {
            entity.Property(e => e.FromLocationId).HasMaxLength(10);
            entity.Property(e => e.Quantity).HasMaxLength(50);
            entity.Property(e => e.ToLocationId).HasMaxLength(10);
            entity.Property(e => e.TransferDate).HasColumnType("datetime");

            entity.HasOne(d => d.FromLocation).WithMany(p => p.TransferFromLocations)
                .HasForeignKey(d => d.FromLocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transfers_Location");

            entity.HasOne(d => d.Item).WithMany(p => p.Transfers)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transfers_Item");

            entity.HasOne(d => d.ToLocation).WithMany(p => p.TransferToLocations)
                .HasForeignKey(d => d.ToLocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transfers_Location1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        modelBuilder.Entity<WarehouseDivision>(entity =>
        {
            entity.HasKey(e => e.DivisionId);

            entity.ToTable("WarehouseDivision");

            entity.Property(e => e.Manager).HasMaxLength(70);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.ResponsibilityOperation).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
