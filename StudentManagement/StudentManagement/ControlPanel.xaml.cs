using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace StudentManagement {
    /// <summary>
    /// Interaction logic for ControlPanel.xaml
    /// </summary>
    public partial class ControlPanel:Window {
        public ControlPanel() {
            InitializeComponent();
        }

        private void listMenu_SelectionChanged(object sender,SelectionChangedEventArgs e) {
            ListBoxItem selectedItem = (ListBoxItem)listMenu.SelectedItem;
            string selectedItemContent = ((StackPanel) selectedItem.Content).Children.OfType<TextBlock>().FirstOrDefault().Text;
            switch (selectedItemContent) {
                case "Quản lý thông tin sinh viên":
                    contentControl.Content = new Control.StudentManagement();
                    break;
                case "Quản lý thông tin môn học":
                    contentControl.Content = new Control.CourseManagement();
                    break;
                case "Quản lý điểm":
                    contentControl.Content = new Control.GradeManagement();
                    break;
                case "Quản lý đăng kí":
                    contentControl.Content = new Control.Admin.EnrollManagement();
                    break;
                case "Thông báo":
                    contentControl.Content = new Control.Admin.Notification();
                    break;

            }
        }

        private void listMenu_Loaded(object sender,RoutedEventArgs e) { // mac dinh chon item dau tien
            listMenu.SelectedIndex = 0;
        }

        private void Button_Click(object sender,RoutedEventArgs e) {
            MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes) {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            } else {
                return;
            }
        }

       
    }
}
