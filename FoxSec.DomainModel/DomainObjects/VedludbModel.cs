using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSec.DomainModel.DomainObjects
{
   public class VedludbModel
    {
        public string sertifikats { get; set; }
        public BuvniecibasLietasArray[] BuvniecibasLietas { get; set; }
    }
}
