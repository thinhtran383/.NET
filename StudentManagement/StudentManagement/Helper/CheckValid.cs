using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace StudentManagement.Helper {
    public static class CheckValid {
        public static bool isFieldEmpty<T>(T field,Label errorLabel,string errorMessage) { // phuong thuc generic
            if(field is TextBox && string.IsNullOrEmpty((field as TextBox).Text)) {
                errorLabel.Content = errorMessage;
                return true;
            } else if(field is ComboBox && (field as ComboBox).SelectedItem == null) {
                errorLabel.Content = errorMessage;
                return true;
            } else if(field is DatePicker && (field as DatePicker).SelectedDate == null) {
                errorLabel.Content = errorMessage;
                return true;
            } else {
                errorLabel.Content = "";
                return false;
            }
        }

        public static void isSelected(ComboBox cb,Label errLabel) {
            if(cb.SelectedItem != null) {
                errLabel.Content = "";
            }
        }

        public static bool isExitsEmail(string email,ObservableCollection<Models.Student> observableList,Label errLabel) {
            foreach(Models.Student student in observableList) {
                if(student.Email.Equals(email)) {
                    errLabel.Content = "Email đã tồn tại";
                    return true;
                }
            }
            return false;
        }

        public static bool isValidate(string email,string dienThoai,Label errPhone,Label errEmail) {
            Regex regex = new Regex(Constant.Regex.EMAIL);
            Regex regexPhone = new Regex(Constant.Regex.PHONE);
            if(!regex.IsMatch(email)) {
                errEmail.Content = "Email không hợp lệ";
                return false;
            }

            if(!regexPhone.IsMatch(dienThoai)) {
                errPhone.Content = "Số điện thoại không hợp lệ";
                return false;
            }

            return true;
        }

      


    }
}