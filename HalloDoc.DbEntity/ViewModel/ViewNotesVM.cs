using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DbEntity.ViewModel
{
    public class ViewNotesVM
    {
        public int? reqId {  get; set; }
        public string? AdminNotes { get; set; }
        public string? PhysicianName { get; set; }
        public DateTime? Date {  get; set; }
        public List<string>? StatusLogNotes { get; set; }
    }
}
