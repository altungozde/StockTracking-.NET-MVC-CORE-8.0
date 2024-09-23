#nullable disable

using Core.Records.Bases;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class RoleModel : Record
    {
        #region Entity'den Kopyalanan Özellikler
        [Required]
        [StringLength(30)]
        public string RoleName { get; set; }
        #endregion
    }
}
