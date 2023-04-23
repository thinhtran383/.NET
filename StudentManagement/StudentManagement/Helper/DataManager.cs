using System.Collections.Generic;
using System.Data.SqlClient;
using DocumentFormat.OpenXml.Wordprocessing;
using StudentManagement.Models;
using StudentManagement.Utils;


namespace StudentManagement.Helper {
    public class DataManager {
        private static List<Account> adminAccounts = new List<Account>();
        private SqlDataReader _dataReader;

        private static DataManager instance = null;

        private DataManager() {
            initAccount();
        }
        
        private void initAccount() {
            _dataReader = ExecuteQuery.executeReader("Select * from AdminAccount");
            while (_dataReader.Read()) {
                string username = _dataReader.GetString(_dataReader.GetOrdinal("username"));
                string password = _dataReader.GetString(_dataReader.GetOrdinal("password"));
                Account account = new Account(username, password);
                adminAccounts.Add(account);
            }
        }

        public static List<Account> getAdminAccounts() {
            if(instance == null) {
                instance = new DataManager();
            }
            return adminAccounts;
        }

    }

}
