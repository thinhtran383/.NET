using System.Data.SqlClient;

namespace StudentManagement.Utils {
    public class ExecuteQuery {
        private static string connectionString = @"Data Source=THINHTRAN\MSSQLSERVER02;Initial Catalog=SinhVienDb;Integrated Security=True;";

        public static void executeNonQuery(string sql) {
            // Khởi tạo đối tượng SqlConnection để kết nối tới SQL Server
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                // Mở kết nối tới SQL Server
                connection.Open();

                // Khởi tạo đối tượng SqlCommand để thực thi câu lệnh SQL
                using (SqlCommand command = new SqlCommand(sql, connection)) {
                    // Thực thi câu lệnh SQL và không trả về bất kỳ dữ liệu nào
                    command.ExecuteNonQuery();
                }
            }
        }

        public static SqlDataReader executeReader(string sql) {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(sql, connection);
            connection.Open();
            return command.ExecuteReader();
        }
    }
}