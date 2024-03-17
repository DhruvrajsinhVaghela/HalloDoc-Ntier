using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DbEntity.ViewModel
{
    public class SendMailVM
    {
        public int reqId {  get; set; }
        public int reqType { get; set; }
        public string? FirstName { get; set; }
        public string? LastName{ get; set; }
        [Required(ErrorMessage = "Phone Number is required")]
        public string? PhoneNo { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = null!;

       
    }
}
