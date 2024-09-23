#nullable disable

using Core.Records.Bases;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class SupplierModel : Record
    {
        #region Entity'den Kopyalanan Özellikler
        [Required]
        [StringLength(200)]
        [DisplayName("Supplier Name")]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
        public string Address { get; set; }
        [DisplayName("Contact Number ")]
        public int ContactNumber { get; set; }
        #endregion

    }
}
