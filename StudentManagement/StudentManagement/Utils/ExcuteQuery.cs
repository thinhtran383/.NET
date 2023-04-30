using System;
using System.Data.SqlClient;
using System.Windows;

namespace StudentManagement.Utils {
    public class ExecuteQuery {
        private static string connectionString = @"Data Source=THINHTRAN\MSSQLSERVER02;Initial Catalog=SinhVienDb;Integrated Security=True;";

        public static void executeNonQuery(string sql) {

            try {
                using (var connection = new SqlConnection(connectionString)) {
                    connection.Open();
                    using (var command = new SqlCommand(sql, connection)) {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException e) {
                MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu");
                throw;
            }
        }

        public static SqlDataReader executeReader(string sql) {
            try {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();
                return command.ExecuteReader();
            }
            catch (SqlException e) {
                MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu");
                throw;
            }
        }
    }
}