#nullable disable

using Core.Records.Bases;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class Product : Record
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
        public int StockAmount { get; set; }
        public int LotNo { get; set; }
        public double UnitPrice { get; set; }


        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }


        public List<OrderDetail> OrderDetails { get; set; }

        #region Binary Data 
        [Column(TypeName = "image")]
        public byte[] Image { get; set; }

        [StringLength(5)]
        public  string ImageExtension { get; set; }//.jpg, .png
        #endregion


    }
}