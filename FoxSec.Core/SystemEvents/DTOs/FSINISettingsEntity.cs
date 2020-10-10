using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSec.Core.SystemEvents.DTOs
{
    public class FSINISettingsEntity : LogEntity
    {
        public string Name { get; set; }
        public byte[] Timestamp { get; set; }
        public virtual int SoftType { get; set; }
        public virtual int? SoftId { get; set; }
        public string Value { get; set; }
        public bool IsDeleted { get; set; }
    }
}
