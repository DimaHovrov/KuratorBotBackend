using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class TemporalLink
    {
        public string Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime DestroyDate { get; set; }
        public UInt64 GroupId { get; set; }

    }
}
