using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BanHangOnline.Models.EF
{
    [Table("tb_Contact")]
    public class Contact : ComontAbstract
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên không được để trống")]
        [StringLength(150, ErrorMessage = "Không được quá 150 ký tự")]
        public string Name { get; set; }
        public string Website { get; set; }

        [StringLength(150, ErrorMessage = "Không được quá 150 ký tự")]
        public string Email { get; set; }

        [StringLength(4000)]
        public string Message { get; set; }
        public Boolean IsReal { get; set; }
    }
}