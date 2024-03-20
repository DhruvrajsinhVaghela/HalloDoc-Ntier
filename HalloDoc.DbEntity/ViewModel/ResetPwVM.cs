using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DbEntity.ViewModel
{
    public class ResetPwVM
    {
        public int AspId { get; set; }

        public string? Email { get; set; }
        [Required(ErrorMessage = "password is required")]
        public string? Password { get; set; }
    }
}
