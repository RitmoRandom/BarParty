using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarParty.Models
{
    [Table("CoctelxParty")]
    public class CoctelxParty
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public int RecID { get; set; }
        public int PartyID { get; set; }
    }
}
