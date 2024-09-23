#nullable disable

using Core.Records.Bases;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Business.Models
{
    public class ProductModel : Record
    {
        #region Entity'den Kopyalanan Özellikler

        [Required]
        [StringLength(200)]
        [DisplayName("Product Name")]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
        [DisplayName("Stock")]
        public int StockAmount { get; set; }
        [DisplayName("Lot No")]
        public int LotNo { get; set; }
        [DisplayName("Unit Price")]
        public double UnitPrice { get; set; }

        [Required]
        [DisplayName("Category Name")]
        public int? CategoryId { get; set; }

        [Required]
        [DisplayName("Supplier Name")]
        public int? SupplierId { get; set; }

        #region Binary Data 
        [Column(TypeName = "image")]
        public byte[] Image { get; set; }

        [StringLength(5)]
        public string ImageExtension { get; set; }//.jpg, .png
        #endregion

        #endregion

        #region View'larda Gösterim veya Veri Girişi için Kullanacağımız Özellikler

        [DisplayName("Lot No")]
        public string LotNoDisplay { get; set; }

        [DisplayName("Unit Price")]
        public string UnitPriceDisplay { get; set; }

        [DisplayName("Category Name")]
        public string CategoryNameDisplay { get; set; }

        [DisplayName("Supplier Name")]
        public string SupplierNameDisplay { get; set; }

        #region Binary Data 
        [DisplayName("Image")]
        public string ImgSrcDisplay { get; set; }
        #endregion

        #endregion
    }
}
