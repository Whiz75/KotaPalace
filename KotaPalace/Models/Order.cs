using Android.Text.Format;
using Java.Sql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KotaPalace.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Customer_Id { get; set; }
        public string Status { get; set; }
        public int BusinessId { get; set; }
        public virtual ICollection<OrderItems> OrderItems { get; set; }
        //public string OrderDate { get; set; }
        public DateTime OrderDateUtc { get; set; }

    }
}
