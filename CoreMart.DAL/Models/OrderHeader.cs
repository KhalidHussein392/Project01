using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreMart.DAL.Models
{
    public class OrderHeader
    {
        public int Id { get; set; }
        [ForeignKey("Customer")]

        public string CustomerId { get; set; }
        public ApplicationUser Customer { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }


        // Stripe Method

        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }



        //public virtual ICollection<shoppingCartDetails> ShoppingCartDetails { get; set; }



    }
}
