using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using StudentManagement.Helper;
using StudentManagement.Models;

namespace StudentManagement.Control
{
    public partial class CourseManagement: UserControl {
        private ObservableCollection<Course> coursesList = DataManager.GetCourseList();
        public CourseManagement()
        {
            InitializeComponent();
        }

        private void dgCourses_Loaded(object sender,RoutedEventArgs e) {
            dgCourses.ItemsSource = coursesList;
        }
    }
}
