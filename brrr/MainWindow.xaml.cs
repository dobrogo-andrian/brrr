using System.Configuration;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Linq;
using Mindbox.Data.Linq;

namespace brrr
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataClasses1DataContext dataContext;
        public MainWindow()
        {
            InitializeComponent();
            string conectionstring = ConfigurationManager.ConnectionStrings["brrr.Properties.Settings.sqllinqConnectionString"].ConnectionString;
            dataContext = new DataClasses1DataContext(conectionstring);
            DeleteJame();

        }


        public void InsertUniversities()
        {
            dataContext.ExecuteCommand("delete from University");
            University yale = new University();
            yale.Name = "Yale";
            University beijingTech = new University();
            beijingTech.Name = "Beijing Tech";
            dataContext.Universities.InsertOnSubmit(beijingTech);
            dataContext.Universities.InsertOnSubmit(yale);
            dataContext.SubmitChanges();

            MainDataGrid.ItemsSource = dataContext.Universities;
        }

        public void InsertStudent()
        {
            University yale = dataContext.Universities.First(un => un.Name.Equals("Yale"));
            University beijingTech = dataContext.Universities.First(un => un.Name.Equals("Beijing Tech"));
            List<Student> students = new List<Student>();
            students.Add(new Student { Name = "Carla", Gender = "female", UniversityId = yale.Id });
            students.Add(new Student { Name = "Toni", Gender = "male", University = yale });
            students.Add(new Student { Name = "Leyle", Gender = "female", University = beijingTech });
            students.Add(new Student { Name = "Jame", Gender = "trans-gender", University = beijingTech });
            dataContext.Students.InsertAllOnSubmit(students);
            dataContext.SubmitChanges();
            MainDataGrid.ItemsSource = dataContext.Students;
        }

        public void InsertLectures()
        {
            dataContext.Lectures.InsertOnSubmit(new Lecture { Name = "Math" });
            dataContext.Lectures.InsertOnSubmit(new Lecture { Name = "History" });

            dataContext.SubmitChanges();

            MainDataGrid.ItemsSource= dataContext.Lectures;
        }

        public void InsertStudentsLectureAssociations()
        {
            Student Carla = dataContext.Students.First(st => st.Name.Equals("Carla"));
            Student Toni = dataContext.Students.First(st => st.Name.Equals("Toni"));
            Student Leyle = dataContext.Students.First(st => st.Name.Equals("Leyle"));
            Student Jame = dataContext.Students.First(st => st.Name.Equals("Jame"));


            Lecture Math = dataContext.Lectures.First(lc => lc.Name.Equals("Math"));
            Lecture History = dataContext.Lectures.First(lc => lc.Name.Equals("History"));

            dataContext.StudentLectures.InsertOnSubmit(new StudentLecture { Student = Carla, Lecture = Math });
            dataContext.StudentLectures.InsertOnSubmit(new StudentLecture { Student = Toni, Lecture = Math });
            dataContext.StudentLectures.InsertOnSubmit(new StudentLecture { Student = Leyle, Lecture = History });
            dataContext.SubmitChanges();
            MainDataGrid.ItemsSource= dataContext.StudentLectures;

        }

        public void GetUniversityOfToni()
        {
            Student Toni = dataContext.Students.First(st => st.Name.Equals("Toni"));
            University tonisUniversity = Toni.University;

            List<University> universities = new List<University>();
            universities.Add(tonisUniversity);

            MainDataGrid.ItemsSource = universities;
        }
        public void GetLecturesOfToni()
        {
            Student Toni = dataContext.Students.First(st => st.Name.Equals("Toni"));

            var tonisLectures = from sl in Toni.StudentLectures select sl.Lecture;
            MainDataGrid.ItemsSource = tonisLectures;
        }

        public void GetAllStudentsFromYale()
        {
            var studentsFtomYale= 
                from student in dataContext.Students
                where student.University.Name == "Yale"
                select student;
            MainDataGrid.ItemsSource = studentsFtomYale;
        }

        public void GetAllUniversitiesWithTransgender()
        {
            var transGenderUniversities =
                from student in dataContext.Students
                join university in dataContext.Universities
                    on student.University equals university
                where student.Gender == "trans-gender"
                select university;
            MainDataGrid.ItemsSource = transGenderUniversities;
        }

        public void lecturesFromBeijingTech()
        {
            var lecturesFromBeijingTech = from sl in dataContext.StudentLectures
                join student in dataContext.Students on sl.StudentId equals student.Id
                where student.University.Name == "Beijing Tech"
                    select sl.Lecture;
            MainDataGrid.ItemsSource = lecturesFromBeijingTech;
        }

        public void UpdateToni()
        {
            Student toni = dataContext.Students.FirstOrDefault(st => st.Name == "Toni");
            toni.Name = "Antonio";
            dataContext.SubmitChanges();
            MainDataGrid.ItemsSource = dataContext.Students;
        }

        public void DeleteJame()
        {
            Student Jame = dataContext.Students.FirstOrDefault(st => st.Name == "Jame");
            dataContext.Students.DeleteOnSubmit(Jame);
            dataContext.SubmitChanges();
            MainDataGrid.ItemsSource = dataContext.Students;
        }
    }
}