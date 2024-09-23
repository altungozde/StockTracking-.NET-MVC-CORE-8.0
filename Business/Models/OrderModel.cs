#nullable disable

using Core.Records.Bases;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class OrderModel : Record
    {
        #region Entity'den Kopyalanan Özellikler
        public string Situation { get; set; }
        public DateTime? Date { get; set; }
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [DisplayName("User")]
        public int UserId { get; set; }
        #endregion

        #region View'larda Gösterim veya Veri Girişi için Kullanacağımız Özellikler

        [DisplayName("User")]
        public string UserNameDisplay { get; set; }

        [DisplayName("Date")]
        public string DateDisplay { get; set; }

        [DisplayName("Product Name")]
        public string ProductNameDisplay { get; set; }

        [DisplayName("Product Name")]
        //[Required(ErrorMessage = "{0} are required!")]
        public List<int> ProductIds { get; set; }

        #endregion


    }
}
