using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApplication.Models
{
    [Table ("term")]
    public class Term
    {
        [PrimaryKey, AutoIncrement]
        [Column("id")]
        public Guid Id { get; set; } // primary key        

        [Column("name")]
        public string Name { get; set; }

        [Column("start")]
        public DateTime Start { get; set; }

        [Column("end")]
        public DateTime End { get; set; }          
    }
}
