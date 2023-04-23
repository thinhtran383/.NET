using System;
using System.Collections.Generic;
using System.Windows;
using StudentManagement.Helper;
using StudentManagement.Models;
using StudentManagement.Utils;

namespace StudentManagement {
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private List<Account> adminAccounts = DataManager.getAdminAccounts();

        private bool checkAccount() {
            string username = txtUsername.Text; 
            string password = txtPassword.Text;

            foreach (Account account in adminAccounts)
            {
                if (account.getUsername() == username && account.getPassword() == password)
                {
                    return true;
                }
            }

            return false;

        }

        private void Button_Click(object sender,RoutedEventArgs e) {
            if(checkAccount())
            {
                MessageBox.Show("dung");
            }
            else
            {
                MessageBox.Show("sai");
            }
        }
    }
}