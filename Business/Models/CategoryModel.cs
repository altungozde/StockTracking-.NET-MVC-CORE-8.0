#nullable disable

using Core.Records.Bases;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class CategoryModel : Record
    {
        #region Entity'den Kopyalanan Özellikler
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(100, ErrorMessage = "{0} must be maximum {1} characters!")]
        [DisplayName("Category Name")]
        public string Name { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        #endregion

        #region View'larda Gösterim veya Veri Girişi için Kullanacağımız Özellikler

        [DisplayName("Product Count")]
        public int ProductsCount { get; set; }

        [DisplayName("Product Name")]
        public string ProductNameDisplay { get; set; }
        #endregion
    }
}
