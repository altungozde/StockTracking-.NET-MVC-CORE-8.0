#nullable disable

using Core.Records.Bases;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class UserModel : Record
    {
        #region Entity'den Kopyalanan Özellikler

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(20)]
        public string Password { get; set; }

        [DisplayName("Role Name")]
        public int RoleId { get; set; }

        #endregion

        #region View'larda Gösterim veya Veri Girişi için Kullanacağımız Özellikler

        [DisplayName("Role Name")]
        public string RoleNameDisplay { get; set; }

        public RoleModel Role { get; set; }

        #endregion
    }
}

