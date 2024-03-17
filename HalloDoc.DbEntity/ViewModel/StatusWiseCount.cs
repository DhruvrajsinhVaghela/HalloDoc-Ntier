using HalloDoc.DbEntity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DbEntity.ViewModel
{
    public class StatusWiseCount
    {
        public int CountNewState { get; set; }

        public int CountPendingState { get; set; }

        public int CountActiveState { get; set; }

        public int CountConcludeState { get; set; }

        public int CountCloseState { get; set; }

        public int CountUnpaidState { get; set; }

        public IEnumerable<Region>? region { get; set; }
    }
}
