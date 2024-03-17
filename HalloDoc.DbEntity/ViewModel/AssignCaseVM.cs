using HalloDoc.DbEntity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DbEntity.ViewModel
{
    public class AssignCaseVM
    {
        public int reqId {  get; set; }

        public IEnumerable<Region>? region { get; set; }
        public int SelectedPhysicianName { get; set; }
        public string? AdminNotes { get; set; }
        public List<Physician>? Physicians { get; set; }
    }
}
