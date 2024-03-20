using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.DbEntity.ViewModel
{
    public class BlockCaseVM
    {
        public int ReqId {  get; set; }
        public string? PateintFirstName {  get; set; }
        public string? PatientLastName { get; set; }
        public string? ReasonForBlock {  get; set; }
    }
}
