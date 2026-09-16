namespace AVMLabs.Api.Common
{
    public static class Constants
    {
        public static class WorkOrderStatus
        {
            public const string Pending = "Pending";
            public const string Processing = "Processing";
            public const string Reported = "Reported";
            public const string Billed = "Billed";
        }

        public static class SampleStatus
        {
            public const string Received = "Received";
            public const string InTransit = "InTransit";
        }

        public static class InvoiceStatus
        {
            public const string Pending = "Pending";
            public const string Paid = "Paid";
            public const string Overdue = "Overdue";
        }

        public static class PaymentMode
        {
            public const string Cash = "Cash";
            public const string Cheque = "Cheque";
            public const string Online = "Online";
        }
    }
}