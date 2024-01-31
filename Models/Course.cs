using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApplication.Models
{
    [Table("course")]
    public class Course
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

        [Column("instructorName")]
        public string InstructorName { get; set; }

        [Column("instructorPhone")]
        public string InstructorPhone { get; set; }

        [Column("instructorEmail")]
        public string InstructorEmail { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("notes")]
        public string Notes { get; set; }

        [Column("alerts")]
        public string Alerts { get; set; }

        [Indexed]
        [Column("termId")]
        public Guid TermId { get; set; } // foreign key       
    }

   
}
