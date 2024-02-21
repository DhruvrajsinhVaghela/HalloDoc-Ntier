using HalloDoc.DbEntity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DbEntity.ViewModel
{
    public class GetAllFormsVM
    {
        public string? Email { get; set; }
        public AspNetUser aspnetuser {  get; set; }

        public User? user { get; set; }

        public string RegName { get; set; }

    }
}
