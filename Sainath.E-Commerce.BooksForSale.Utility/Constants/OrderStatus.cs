using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sainath.E_Commerce.BooksForSale.Utility.Constants
{
    public static class OrderStatus
    {
        public const string STATUS_PENDING = "Pending";
        public const string STATUS_APPROVED = "Approved";
        public const string STATUS_PROCESSING = "Processing";
        public const string STATUS_SHIPPED = "Shipped";
        public const string STATUS_CANCELLED = "Cancelled";
        public const string STATUS_REFUNDED = "Refunded";
        public const string PAYMENT_STATUS_PENDING = "Pending";
        public const string PAYMENT_STATUS_APPROVED = "Approved";
        public const string PAYMENT_STATUS_DELAYED_PAYMENT = "ApprovedForDelayedPayment";
        public const string PAYMENT_STATUS_REJECTED = "Rejected";
        public const string PAYMENT_STATUS_PAID = "paid";
    }
}
