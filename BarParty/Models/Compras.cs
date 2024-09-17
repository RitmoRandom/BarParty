using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarParty.Models
{
    public class Compras
    {
        public int Id { get; set; }
        public int Cantidad { get; set; }
        public int type { get; set; }
        public string Calculo { get; set; }
    }
}
