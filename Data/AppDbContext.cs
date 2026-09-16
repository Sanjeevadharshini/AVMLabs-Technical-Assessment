using AVMLabs.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AVMLabs.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<WorkOrder> WorkOrders { get; set; }
        public DbSet<WorkOrderItem> WorkOrderItems { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("Clients");

                entity.HasKey(x => x.ClientId);

                entity.Property(x => x.ClientName).HasMaxLength(150).IsRequired();
                entity.Property(x => x.ContactPerson).HasMaxLength(150).IsRequired();
                entity.Property(x => x.Phone).HasMaxLength(20).IsRequired();
                entity.Property(x => x.Email).HasMaxLength(150).IsRequired();
                entity.Property(x => x.City).HasMaxLength(100).IsRequired();
                entity.Property(x => x.Country).HasMaxLength(100).IsRequired(); 
                entity.Property(x => x.CreditLimit).HasPrecision(18, 2);
                entity.Property(x => x.IsActive).HasDefaultValue(true);
                entity.Property(x => x.CreatedOn).HasColumnType("datetime2(0)").HasDefaultValueSql("SYSDATETIME()");
                entity.Property(x => x.UpdatedOn).HasColumnType("datetime2(0)");

                entity.HasIndex(x => x.Email).IsUnique();
                entity.HasIndex(x => x.Country);
                entity.HasIndex(x => x.ClientName);

                entity.HasCheckConstraint("CK_Clients_CreditLimit", "[CreditLimit] > 0");
            });

            modelBuilder.Entity<Test>(entity =>
            {
                entity.ToTable("Tests");

                entity.HasKey(x => x.TestId);

                entity.Property(x => x.TestCode).HasMaxLength(50).IsRequired();
                entity.Property(x => x.TestName).HasMaxLength(150).IsRequired();
                entity.Property(x => x.SampleType).HasMaxLength(100).IsRequired();
                entity.Property(x => x.Rate).HasPrecision(18, 2);
                entity.Property(x => x.IsActive).HasDefaultValue(true);
                entity.Property(x => x.CreatedOn).HasColumnType("datetime2(0)").HasDefaultValueSql("SYSDATETIME()");

                entity.HasIndex(x => x.TestCode).IsUnique();

                entity.HasCheckConstraint("CK_Tests_TATHours", "[TATHours] > 0");
                entity.HasCheckConstraint("CK_Tests_Rate", "[Rate] >= 0");
            });

            modelBuilder.Entity<WorkOrder>(entity =>
            {
                entity.ToTable("WorkOrders");

                entity.HasKey(x => x.WOId);

                entity.Property(x => x.WODate).HasColumnType("date");
                entity.Property(x => x.Status).HasMaxLength(20).IsRequired();
                entity.Property(x => x.TotalAmount).HasPrecision(18, 2);
                entity.Property(x => x.CreatedBy).HasMaxLength(100).IsRequired();
                entity.Property(x => x.CreatedOn).HasColumnType("datetime2(0)").HasDefaultValueSql("SYSDATETIME()");

                entity.HasIndex(x => x.ClientId);
                entity.HasIndex(x => x.WODate);

                entity.HasCheckConstraint("CK_WorkOrders_Status", "[Status] IN ('Pending', 'Processing', 'Reported', 'Billed')");
                entity.HasCheckConstraint("CK_WorkOrders_TotalAmount", "[TotalAmount] >= 0");

                entity.HasOne(x => x.Client).WithMany(x => x.WorkOrders).HasForeignKey(x => x.ClientId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<WorkOrderItem>(entity =>
            {
                entity.ToTable("WorkOrderItems");

                entity.HasKey(x => x.WOItemId);

                entity.Property(x => x.Rate).HasPrecision(18, 2);
                entity.Property(x => x.Amount).HasPrecision(18, 2);
                entity.Property(x => x.SampleStatus).HasMaxLength(20).IsRequired();
                entity.Property(x => x.CreatedOn).HasColumnType("datetime2(0)").HasDefaultValueSql("SYSDATETIME()");

                entity.HasIndex(x => x.WOId);
                entity.HasIndex(x => x.TestId);
                entity.HasIndex(x => x.SampleStatus);

                entity.HasCheckConstraint("CK_WorkOrderItems_Quantity", "[Quantity] > 0");
                entity.HasCheckConstraint("CK_WorkOrderItems_Rate", "[Rate] >= 0");
                entity.HasCheckConstraint("CK_WorkOrderItems_Amount", "[Amount] = [Quantity] * [Rate]");
                entity.HasCheckConstraint("CK_WorkOrderItems_SampleStatus", "[SampleStatus] IN ('Received', 'InTransit')");

                entity.HasOne(x => x.WorkOrder).WithMany(x => x.WorkOrderItems).HasForeignKey(x => x.WOId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(x => x.Test).WithMany(x => x.WorkOrderItems).HasForeignKey(x => x.TestId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.ToTable("Invoices");

                entity.HasKey(x => x.InvoiceId);

                entity.Property(x => x.InvoiceDate).HasColumnType("date");
                entity.Property(x => x.DueDate).HasColumnType("date");
                entity.Property(x => x.TotalAmount).HasPrecision(18, 2);
                entity.Property(x => x.Status).HasMaxLength(20).IsRequired();
                entity.Property(x => x.CreatedOn).HasColumnType("datetime2(0)").HasDefaultValueSql("SYSDATETIME()");

                entity.HasIndex(x => x.ClientId);

                entity.HasCheckConstraint("CK_Invoices_Status", "[Status] IN ('Pending', 'Paid', 'Overdue')");
                entity.HasCheckConstraint("CK_Invoices_TotalAmount", "[TotalAmount] >= 0");
                entity.HasCheckConstraint("CK_Invoices_DueDate", "[DueDate] >= [InvoiceDate]");

                entity.HasOne(x => x.Client).WithMany(x => x.Invoices).HasForeignKey(x => x.ClientId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payments");

                entity.HasKey(x => x.PaymentId);

                entity.Property(x => x.PaymentDate).HasColumnType("date");
                entity.Property(x => x.Amount).HasPrecision(18, 2);
                entity.Property(x => x.Mode).HasMaxLength(20).IsRequired();
                entity.Property(x => x.GatewayFee).HasPrecision(18, 2);
                entity.Property(x => x.NetAmount).HasPrecision(18, 2);
                entity.Property(x => x.CreatedOn).HasColumnType("datetime2(0)").HasDefaultValueSql("SYSDATETIME()");

                entity.HasIndex(x => x.InvoiceId);
                entity.HasIndex(x => x.PaymentDate);

                entity.HasCheckConstraint("CK_Payments_Amount", "[Amount] > 0");
                entity.HasCheckConstraint("CK_Payments_Mode", "[Mode] IN ('Cash', 'Cheque', 'Online')");
                entity.HasCheckConstraint("CK_Payments_GatewayFee", "[GatewayFee] >= 0");
                entity.HasCheckConstraint("CK_Payments_NetAmount", "[NetAmount] >= 0");

                entity.HasOne(x => x.Invoice).WithMany(x => x.Payments).HasForeignKey(x => x.InvoiceId).OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}