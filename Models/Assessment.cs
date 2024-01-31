using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApplication.Models
{
    [Table("assessment")]
    public class Assessment
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

        [Column("type")]
        public string Type { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("alerts")]
        public string Alerts { get; set; }      
        
        [Indexed]
        [Column("courseId")]
        public Guid CourseId { get; set; } // foreign key
    }
}
