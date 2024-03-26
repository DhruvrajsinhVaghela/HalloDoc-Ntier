using HalloDoc.DbEntity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DbEntity.ViewModel
{
    public class ProviderInformation
    {
        public List<Region>? RegionList { get; set; }
        public int? ProviderId { get; set; }
        public List<int>? IsChecked { get; set; }

        public string? FirstName { get; set; }

        public bool isChecked { get; set; }

    }
}
