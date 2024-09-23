#nullable disable

using Core.Records.Bases;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class Order : Record
    {
        public string Situation { get; set; }
        //public int OrderNumber { get; set; }
        public DateTime? Date { get; set; }
        [StringLength(500)]
        public string Description { get; set; }


        public int UserId { get; set; }
        public User User { get; set; }


        public List<OrderDetail> OrderDetails { get; set; }
    }
}