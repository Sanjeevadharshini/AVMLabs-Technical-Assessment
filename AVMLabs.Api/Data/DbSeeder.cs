using AVMLabs.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AVMLabs.Api.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (await context.Clients.AnyAsync())
                return;

            var clients = new List<Client>
            {
                new Client { ClientName = "ABC Hospital", ContactPerson = "Ramesh Kumar", Phone = "9876543210", Email = "ramesh@abchospital.com", City = "Coimbatore", Country = "India", CreditLimit = 50000, IsActive = true },
                new Client { ClientName = "City Care Diagnostics", ContactPerson = "Priya Sharma", Phone = "9876543211", Email = "priya@citycare.com", City = "Chennai", Country = "India", CreditLimit = 30000, IsActive = true },
                new Client { ClientName = "Apollo Medical Center", ContactPerson = "Arun Kumar", Phone = "9876543212", Email = "arun@apollomedical.com", City = "Bangalore", Country = "India", CreditLimit = 75000, IsActive = true },
                new Client { ClientName = "Green Life Hospital", ContactPerson = "Meena Raj", Phone = "9876543213", Email = "meena@greenlife.com", City = "Coimbatore", Country = "India", CreditLimit = 40000, IsActive = true },
                new Client { ClientName = "Sri Ram Diagnostics", ContactPerson = "Karthik Raj", Phone = "9876543214", Email = "karthik@sriramdiagnostics.com", City = "Dubai", Country = "UAE", CreditLimit = 45000, IsActive = true },
                new Client { ClientName = "MedCare Hospital", ContactPerson = "Anitha Devi", Phone = "9876543215", Email = "anitha@medcare.com", City = "Abu Dhabi", Country = "UAE", CreditLimit = 60000, IsActive = true },
                new Client { ClientName = "Health First Lab", ContactPerson = "Suresh Kumar", Phone = "9876543216", Email = "suresh@healthfirst.com", City = "Manama", Country = "Bahrain", CreditLimit = 35000, IsActive = true },
                new Client { ClientName = "Prime Care Hospital", ContactPerson = "Divya Raj", Phone = "9876543217", Email = "divya@primecare.com", City = "Riyadh", Country = "Saudi Arabia", CreditLimit = 55000, IsActive = true },
                new Client { ClientName = "Wellness Diagnostics", ContactPerson = "Vijay Anand", Phone = "9876543218", Email = "vijay@wellnessdiagnostics.com", City = "Muscat", Country = "Oman", CreditLimit = 25000, IsActive = true },
                new Client { ClientName = "Care Plus Medical Center", ContactPerson = "Lakshmi Priya", Phone = "9876543219", Email = "lakshmi@careplus.com", City = "Trichy", Country = "India", CreditLimit = 70000, IsActive = true },
                new Client { ClientName = "Global Health Lab", ContactPerson = "Mohan Raj", Phone = "9876543220", Email = "mohan@globalhealth.com", City = "Coimbatore", Country = "India", CreditLimit = 50000, IsActive = true },
                new Client { ClientName = "Sunrise Hospital", ContactPerson = "Deepa Kumar", Phone = "9876543221", Email = "deepa@sunrisehospital.com", City = "Chennai", Country = "India", CreditLimit = 65000, IsActive = true },
                new Client { ClientName = "City Medical Lab", ContactPerson = "Ravi Shankar", Phone = "9876543222", Email = "ravi@citymedical.com", City = "Bangalore", Country = "India", CreditLimit = 30000, IsActive = true },
                new Client { ClientName = "LifeCare Diagnostics", ContactPerson = "Nandhini Raj", Phone = "9876543223", Email = "nandhini@lifecare.com", City = "Doha", Country = "Qatar", CreditLimit = 45000, IsActive = true },
                new Client { ClientName = "Metro Health Center", ContactPerson = "Prakash Kumar", Phone = "9876543224", Email = "prakash@metrohealth.com", City = "Salalah", Country = "Oman", CreditLimit = 55000, IsActive = true }
            };

            context.Clients.AddRange(clients);
            await context.SaveChangesAsync();

            var tests = new List<Test>
            {
                new Test { TestCode = "CBC001", TestName = "Complete Blood Count", SampleType = "Blood", TATHours = 24, Rate = 500, IsOST = false, IsActive = true },
                new Test { TestCode = "LFT001", TestName = "Liver Function Test", SampleType = "Blood", TATHours = 24, Rate = 800, IsOST = false, IsActive = true },
                new Test { TestCode = "KFT001", TestName = "Kidney Function Test", SampleType = "Blood", TATHours = 24, Rate = 750, IsOST = false, IsActive = true },
                new Test { TestCode = "THY001", TestName = "Thyroid Profile", SampleType = "Blood", TATHours = 48, Rate = 1000, IsOST = false, IsActive = true },
                new Test { TestCode = "LIP001", TestName = "Lipid Profile", SampleType = "Blood", TATHours = 24, Rate = 650, IsOST = false, IsActive = true },
                new Test { TestCode = "HBA001", TestName = "HbA1c", SampleType = "Blood", TATHours = 12, Rate = 450, IsOST = false, IsActive = true },
                new Test { TestCode = "URI001", TestName = "Urine Routine", SampleType = "Urine", TATHours = 12, Rate = 300, IsOST = false, IsActive = true },
                new Test { TestCode = "CUL001", TestName = "Urine Culture", SampleType = "Urine", TATHours = 72, Rate = 1200, IsOST = true, IsActive = true },
                new Test { TestCode = "PCR001", TestName = "PCR Test", SampleType = "Swab", TATHours = 48, Rate = 1500, IsOST = true, IsActive = true },
                new Test { TestCode = "VIT001", TestName = "Vitamin D", SampleType = "Blood", TATHours = 48, Rate = 900, IsOST = true, IsActive = true }
            };

            context.Tests.AddRange(tests);
            await context.SaveChangesAsync();

            var workOrders = new List<WorkOrder>
            {
                new WorkOrder { ClientId = 1, WODate = DateTime.Today.AddDays(-2), Status = "Pending", TotalAmount = 2200, CreatedBy = "Admin" },
                new WorkOrder { ClientId = 1, WODate = DateTime.Today.AddDays(-5), Status = "Processing", TotalAmount = 2900, CreatedBy = "Admin" },
                new WorkOrder { ClientId = 2, WODate = DateTime.Today.AddDays(-1), Status = "Reported", TotalAmount = 1800, CreatedBy = "Admin" },
                new WorkOrder { ClientId = 3, WODate = DateTime.Today.AddDays(-20), Status = "Billed", TotalAmount = 3350, CreatedBy = "Admin" },
                new WorkOrder { ClientId = 3, WODate = DateTime.Today.AddDays(-15), Status = "Billed", TotalAmount = 2500, CreatedBy = "Admin" },
                new WorkOrder { ClientId = 2, WODate = DateTime.Today.AddDays(-7), Status = "Processing", TotalAmount = 2850, CreatedBy = "Admin" },
                new WorkOrder { ClientId = 2, WODate = DateTime.Today.AddDays(-10), Status = "Processing", TotalAmount = 4000, CreatedBy = "Admin" }
            };

            context.WorkOrders.AddRange(workOrders);
            await context.SaveChangesAsync();

            var workOrderItems = new List<WorkOrderItem>
            {
                new WorkOrderItem { WOId = 1, TestId = 1, Quantity = 2, Rate = 500, Amount = 1000, SampleStatus = "Received" },
                new WorkOrderItem { WOId = 1, TestId = 6, Quantity = 2, Rate = 450, Amount = 900, SampleStatus = "Received" },
                new WorkOrderItem { WOId = 1, TestId = 7, Quantity = 1, Rate = 300, Amount = 300, SampleStatus = "Received" },

                new WorkOrderItem { WOId = 2, TestId = 2, Quantity = 1, Rate = 800, Amount = 800, SampleStatus = "Received" },
                new WorkOrderItem { WOId = 2, TestId = 4, Quantity = 1, Rate = 1000, Amount = 1000, SampleStatus = "InTransit" },
                new WorkOrderItem { WOId = 2, TestId = 5, Quantity = 1, Rate = 650, Amount = 650, SampleStatus = "InTransit" },
                new WorkOrderItem { WOId = 2, TestId = 6, Quantity = 1, Rate = 450, Amount = 450, SampleStatus = "Received" },

                new WorkOrderItem { WOId = 3, TestId = 9, Quantity = 1, Rate = 1500, Amount = 1500, SampleStatus = "Received" },
                new WorkOrderItem { WOId = 3, TestId = 7, Quantity = 1, Rate = 300, Amount = 300, SampleStatus = "Received" },

                new WorkOrderItem { WOId = 4, TestId = 8, Quantity = 1, Rate = 1200, Amount = 1200, SampleStatus = "Received" },
                new WorkOrderItem { WOId = 4, TestId = 10, Quantity = 1, Rate = 900, Amount = 900, SampleStatus = "Received" },
                new WorkOrderItem { WOId = 4, TestId = 3, Quantity = 1, Rate = 750, Amount = 750, SampleStatus = "Received" },
                new WorkOrderItem { WOId = 4, TestId = 1, Quantity = 1, Rate = 500, Amount = 500, SampleStatus = "Received" },

                new WorkOrderItem { WOId = 5, TestId = 5, Quantity = 2, Rate = 650, Amount = 1300, SampleStatus = "Received" },
                new WorkOrderItem { WOId = 5, TestId = 6, Quantity = 2, Rate = 450, Amount = 900, SampleStatus = "Received" },
                new WorkOrderItem { WOId = 5, TestId = 7, Quantity = 1, Rate = 300, Amount = 300, SampleStatus = "Received" },

                new WorkOrderItem { WOId = 6, TestId = 9, Quantity = 1, Rate = 1500, Amount = 1500, SampleStatus = "InTransit" },
                new WorkOrderItem { WOId = 6, TestId = 10, Quantity = 1, Rate = 900, Amount = 900, SampleStatus = "Received" },
                new WorkOrderItem { WOId = 6, TestId = 6, Quantity = 1, Rate = 450, Amount = 450, SampleStatus = "Received" },

                new WorkOrderItem { WOId = 7, TestId = 2, Quantity = 2, Rate = 800, Amount = 1600, SampleStatus = "Received" },
                new WorkOrderItem { WOId = 7, TestId = 9, Quantity = 1, Rate = 1500, Amount = 1500, SampleStatus = "Received" },
                new WorkOrderItem { WOId = 7, TestId = 6, Quantity = 2, Rate = 450, Amount = 900, SampleStatus = "InTransit" }
            };

            context.WorkOrderItems.AddRange(workOrderItems);
            await context.SaveChangesAsync();

            var invoices = new List<Invoice>
            {
                new Invoice { ClientId = 3, WOId = 4, InvoiceDate = DateTime.Today.AddDays(-20), DueDate = DateTime.Today.AddDays(10), TotalAmount = 3350, Status = "Pending" },
                new Invoice { ClientId = 3, WOId = 5, InvoiceDate = DateTime.Today.AddDays(-15), DueDate = DateTime.Today.AddDays(15), TotalAmount = 2500, Status = "Pending" }
            };

            context.Invoices.AddRange(invoices);
            await context.SaveChangesAsync();

            var payments = new List<Payment>
            {
                new Payment { InvoiceId = 1, PaymentDate = DateTime.Today.AddDays(-3), Amount = 1000, Mode = "Online", GatewayFee = 20, NetAmount = 980 }
            };

            context.Payments.AddRange(payments);
            await context.SaveChangesAsync();
        }
    }
}