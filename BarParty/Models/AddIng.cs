using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarParty.Models
{
    class AddIng
    {
        public int RecID { get; set; }
        public int IngID { get; set; }

        public string Cant { get; set; }
        public string Note { get; set; }

        public string NameIng { get; set; }
        public string Unit { get; set; }
    }
}
