using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FoxSec.DomainModel.DomainObjects
{
    public class FSCamera
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual String status { get; set; }

        public virtual int ServerNr { get; set; }
        public virtual int CameraNr { get; set; }
        public virtual int Port { get; set; }
        public virtual int ResX { get; set; }
        public virtual int ResY { get; set; }
        public virtual int Skip { get; set; }
        public virtual int Delay { get; set; }
        public virtual int QuickPreviewSeconds { get; set; }

        public virtual bool Deleted { get; set; }
    }
}
