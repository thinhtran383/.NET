using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using StudentManagement.Helper;
using StudentManagement.Models;
using StudentManagement.Utils;

namespace StudentManagement {
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private List<Account> adminAccounts = DataManager.GetAdminAccounts();

        private bool checkAccount() {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            foreach (Account account in adminAccounts) {
                if (account.getUsername() == username && account.getPassword() == password) {
                    return true;
                }
            }

            return false;

        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            // check null
            if (txtUsername.Text == "" || txtPassword.Password == "") {
                lbErr.Content = "*Vui lòng nhập đầy đủ thông tin";
                return;
            }

            if (checkAccount()) {
                MessageBox.Show("Đăng nhập thành công");
                ControlPanel control = new ControlPanel();
                this.Close();
                control.Show();
            }
            else {
                lbErr.Content = "*Tài khoản hoặc mật khẩu không chính xác";
            }


        }
        
        private int clickCount = 1;
       
        private void showPassword_MouseLeftButtonDown(object sender,System.Windows.Input.MouseButtonEventArgs e) {
            if(clickCount % 2 == 1) {
                showPassword.Text = "Hide";
                passwordUnmark.Visibility = Visibility.Visible;
                txtPassword.Visibility = Visibility.Hidden;
                passwordUnmark.Text = txtPassword.Password;
                clickCount++;
            } else {
                showPassword.Text = "Show";
                passwordUnmark.Visibility = Visibility.Hidden;
                txtPassword.Visibility = Visibility.Visible;
                txtPassword.Password = passwordUnmark.Text;
                clickCount++;
            }
        }
    }
}