using MobileApplication.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApplication.Services
{
    public static class DataAccessService
    {      
        static SQLiteConnection db;

        public static void Init()
        {
            // open db connection
            OpenDatabase();

            // create tables
            db.CreateTable<Term>();
            db.CreateTable<Course>();
            db.CreateTable<Assessment>();           
                                                
            // close db connection
            db.Close();
        }

        public static void OpenDatabase()
        {
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "C971.db");
            db = new SQLiteConnection(databasePath);
        }
                   
        public static void PopulateTables()
        {
            OpenDatabase();

            // populate database with default records
            var query1 = new Term
            {
                Id = new Guid(),
                Name = "Spring Term",
                Start = new DateTime(2024, 1, 1).ToUniversalTime(),
                End = new DateTime(2024, 6, 30).ToUniversalTime()
            };
            var results1 = db.Insert(query1);

            var query2 = new Course
            {
                Id = new Guid(),
                Name = "Cloud Foundations",
                Start = new DateTime(2024, 1, 1).ToUniversalTime(),
                End = new DateTime(2024, 4, 30).ToUniversalTime(),
                InstructorName = "Anika Patel",
                InstructorPhone = "555-123-4567",
                InstructorEmail = "anika.patel@strimeuniversity.edu",
                Status = "Plan to take",
                Notes = "This course uses VMs",
                Alerts = "On",
                TermId = query1.Id
            };            
            var results2 = db.Insert(query2);

            var query3 = new Assessment
            {
                Id = new Guid(),
                Name = "AWS Cloud Practitioner",
                Start = new DateTime(2024, 1, 1).ToUniversalTime(),
                End = new DateTime(2024, 2, 29).ToUniversalTime(),
                Type = "Objective",
                Alerts = "On",
                Description = "This assessment is conducted through the successful completion of the AWS Certified Cloud Practitioner certification exam.",
                CourseId = query2.Id
            };
            var results3 = db.Insert(query3);

            var query4 = new Assessment
            {
                Id = new Guid(),
                Name = "AWS Applications",
                Start = new DateTime(2024, 1, 1).ToUniversalTime(),
                End = new DateTime(2024, 3, 1).ToUniversalTime(),
                Type = "Performance",
                Alerts = "Off",
                Description = "This assessment is conducted through demonstrating the ability to write code and deploy applications using AWS technologies.",
                CourseId = query2.Id
            };
            var results4 = db.Insert(query4);

            db.Close();
        }

        #region Terms

        public static List<Term> GetAllTerms()
        {
            OpenDatabase();

            // get all terms to populate term list on home page           
            string query = $"SELECT * FROM term";
            
            var results = db.Query<Term>(query);          
                        
            db.Close();
            
            return results;          
        }
        public static Term GetTerm(Guid termId)
        {
            OpenDatabase();

            // to populate the selected term's detailed view    
            // get all courses where course.termId == (termId)          
            var results = db.Table<Term>().Where(t => t.Id == termId).ToList();

            db.Close();
            
            return results[0];
        }
        public static void AddTerm(Guid termId, string name, DateTime start, DateTime end)
        {
            OpenDatabase();

            var query = new Term
            {
                Id = termId,
                Name = name,
                Start = start.ToUniversalTime(),
                End = end.ToUniversalTime()
            };

            var results = db.Insert(query);
            
            db.Close();
        }
        public static void DeleteTerm(Guid termId)
        {   
            // get associated courses where course.termId == (termId)            
            var courses = GetCoursesByTerm(termId);

            // delete associated courses            
            if (courses.Any())
            {
                foreach (Course course in courses)
                {                                     
                    DeleteCourse(course.Id);
                }
            }                                          
                 
            OpenDatabase();

            // delete term where term.id == (termId)      
            db.Delete<Term>(termId);                                                
           
            db.Close();
        }
        public static void UpdateTerm(Guid termId, string name, DateTime start, DateTime end)
        {
            OpenDatabase();

            var query = new Term
            {
                Id = termId,
                Name = name,
                Start = start.ToUniversalTime(),
                End = end.ToUniversalTime()
            };

            var results = db.Update(query);

            db.Close();
        }

        #endregion

        #region Courses
        public static List<Course> GetAllCourses()
        {
            OpenDatabase();

            // get all courses
            // for testing purposes
            string query = $"SELECT * FROM course";

            var results = db.Query<Course>(query);

            db.Close();
          
            return results;
        }
        public static List<Course> GetCoursesByTerm(Guid termId) 
        {
            OpenDatabase();

            // to populate the selected term's course summary list   
            // get all courses where course.termId == (termId)
            var results = db.Table<Course>().Where(t => t.TermId == termId).ToList();

            db.Close();
            
            return results;
        }
        public static Course GetCourse(Guid courseId)
        {
            OpenDatabase();

            // to populate the selected course's detailed view   
            // get specific course where course.id == (courseId)
            var results = db.Table<Course>().Where(t => t.Id == courseId).ToList();

            db.Close();
           
            return results[0];
        }
        public static void AddCourse(Guid courseId, string name, DateTime start, DateTime end, string instructorName, string instructorPhone, string instructorEmail, string status, string notes, string alerts, Guid termId)
        {
            OpenDatabase();

            var query = new Course
            {
                Id = courseId,
                Name = name,
                Start = start.ToUniversalTime(),
                End = end.ToUniversalTime(),
                InstructorName = instructorName,
                InstructorPhone = instructorPhone,
                InstructorEmail = instructorEmail,
                Status = status,
                Notes = notes,
                Alerts = alerts,
                TermId = termId
            };

            var results = db.Insert(query);

            db.Close();
        }
        public static void DeleteCourse(Guid courseId)
        {            
            // get associated assessments where assessment.courseId == course.id
            var assessments = GetAssessmentsByCourse(courseId);
           
            // delete associated assessments
            if (assessments.Any())
            {
                foreach (Assessment assessment in assessments)
                {                                 
                    DeleteAssessment(assessment.Id);
                }
            }

            OpenDatabase();

            // delete course where course.id == (courseId)
            db.Delete<Course>(courseId);          

            db.Close();
        }
        public static void UpdateCourse(Guid courseId, string name, DateTime start, DateTime end, string instructorName, string instructorPhone, string instructorEmail, string status, string notes, string alerts, Guid termId)
        {
            OpenDatabase();

            var query = new Course
            {
                Id = courseId,
                Name = name,
                Start = start.ToUniversalTime(),
                End = end.ToUniversalTime(),
                InstructorName = instructorName,
                InstructorPhone = instructorPhone,
                InstructorEmail = instructorEmail,
                Status = status,
                Notes = notes,
                Alerts = alerts,
                TermId = termId
            };

            var results = db.Update(query);

            db.Close();
        }

        #endregion

        #region Assessments

        public static List<Assessment> GetAllAssessments()
        {
            OpenDatabase();

            // get all assessments
            // for testing purposes
            string query = $"SELECT * FROM assessment";

            var results = db.Query<Assessment>(query);

            db.Close();
          
            return results;
        }
        public static List<Assessment> GetAssessmentsByCourse(Guid courseId) 
        {
            OpenDatabase();

            // to populate the selected course's assessment summary list    
            // get all assessments where assessment.courseId = course.id (courseId)
            var results = db.Table<Assessment>().Where(t => t.CourseId == courseId).ToList();

            db.Close();
            
            return results;
        }
        public static Assessment GetAssessment(Guid assessmentId)
        {
            OpenDatabase();

            // to populate the selected assessment's detailed view    
            // get specific assessment where assessment.id = (assessmentId)
            var results = db.Table<Assessment>().Where(t => t.Id == assessmentId).ToList();

            db.Close();
            
            return results[0];
        }
        public static void AddAssessment(Guid assessmentId, string name, DateTime start, DateTime end, string type, string description, string alerts, Guid courseId)
        {
            OpenDatabase();

            var query = new Assessment
            {
                Id = assessmentId,
                Name = name,
                Start = start.ToUniversalTime(),
                End = end.ToUniversalTime(),
                Type = type,
                Description = description,
                Alerts = alerts,
                CourseId = courseId              
            };

            var results = db.Insert(query);

            db.Close();
        }
        public static void DeleteAssessment(Guid assessmentId)
        {
            OpenDatabase();

            // delete assessment where assessment.id == (assessmentId)
            db.Delete<Assessment>(assessmentId);

            db.Close();
        }
        public static void UpdateAssessment(Guid assessmentId, string name, DateTime start, DateTime end, string type, string description, string alerts, Guid courseId)
        {
            OpenDatabase();

            var query = new Assessment
            {
                Id = assessmentId,
                Name = name,
                Start = start.ToUniversalTime(),
                End = end.ToUniversalTime(),
                Type = type,
                Description = description,
                Alerts = alerts,
                CourseId = courseId
            };

            var results = db.Update(query);

            db.Close();
        }

        #endregion            
    }
}
