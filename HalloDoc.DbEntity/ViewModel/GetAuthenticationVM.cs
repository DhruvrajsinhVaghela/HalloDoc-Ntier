using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DbEntity.ViewModel
{
    public class GetAuthenticationVM
    {
        public int Id { get; set; }

        public string? Email { get; set; }

        public string? PasswordHash { get; set; }

        public string? Passwordhashed { get; set; }



    }
}
