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
        public int ReqId {  get; set; }
        public IEnumerable<Region>? Region { get; set; }
        public int SelectedPhysicianName { get; set; }
        public string? AdminNotes { get; set; }
        public List<Physician>? Physicians { get; set; }
    }
}
