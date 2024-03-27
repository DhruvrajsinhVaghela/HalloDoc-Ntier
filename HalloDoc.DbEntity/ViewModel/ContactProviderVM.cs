using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DbEntity.ViewModel
{
    public class ContactProviderVM
    {
        public int ProviderId { get; set; }
        public string? ContactType { get; set; }

        public string? Message { get; set; }
        
    }
}
