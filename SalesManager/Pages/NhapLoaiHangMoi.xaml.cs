using System;
using System.Collections.Generic;
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
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Configuration;
namespace SalesManager
{
    /// <summary>
    /// Interaction logic for NhapLoaiHangMoi.xaml
    /// </summary>
    public partial class NhapLoaiHangMoi : BasePage
    {
        public NhapLoaiHangMoi()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Nhap_Click(object sender, RoutedEventArgs e)
        {
            if (textMaHang.Text == "" || textTenHang.Text == "") MessageBox.Show("Vui lòng nhập đủ thông tin", "", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                var sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                sqlConn.Open();
                var sqlCommand = new SqlCommand("SELECT MAHANG FROM LOAIHANG", sqlConn);
                var reader = sqlCommand.ExecuteReader();
                bool flag = false;
                while (reader.Read())
                {
                    if (reader.GetString(0) == textMaHang.Text)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    reader.Close();
                    MessageBoxResult reSult = MessageBox.Show("Mã hàng đã tồn tại trong hệ thống, bạn có muốn ghi đè dữ liệu đang có không?", "", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (reSult==MessageBoxResult.Yes)
                    {
                        sqlCommand.CommandText = "UPDATE LOAIHANG SET TENHANG = @TENHANG, DVT = @DVT WHERE MAHANG = @MAHANG";
                        sqlCommand.Parameters.Add("@MAHANG", System.Data.SqlDbType.VarChar);
                        sqlCommand.Parameters["@MAHANG"].Value = textMaHang.Text;
                        sqlCommand.Parameters.Add("@TENHANG", System.Data.SqlDbType.NVarChar);
                        sqlCommand.Parameters["@TENHANG"].Value = textTenHang.Text;
                        sqlCommand.Parameters.Add("@DVT", System.Data.SqlDbType.NVarChar);
                        sqlCommand.Parameters["@DVT"].Value = comBoxDVT.Text;
                        sqlCommand.ExecuteNonQuery();
                        sqlCommand.Dispose();
                        MessageBox.Show("Cập nhật dữ liệu thành công");
                        textMaHang.Text = "";
                        textTenHang.Text = "";
                        comBoxDVT.Text = "Cái";
                    }    
                }
                else
                {
                    reader.Close();
                    sqlCommand.CommandText = "INSERT INTO LOAIHANG (MAHANG, TENHANG, DVT) VALUES " + "(@MAHANG,@TENHANG,@DVT)";
                    sqlCommand.Parameters.Add("@MAHANG", System.Data.SqlDbType.VarChar);
                    sqlCommand.Parameters["@MAHANG"].Value = textMaHang.Text;
                    sqlCommand.Parameters.Add("@TENHANG", System.Data.SqlDbType.NVarChar);
                    sqlCommand.Parameters["@TENHANG"].Value = textTenHang.Text;
                    sqlCommand.Parameters.Add("@DVT", System.Data.SqlDbType.NVarChar);
                    sqlCommand.Parameters["@DVT"].Value = comBoxDVT.Text;
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Dispose();
                    MessageBox.Show("Thêm dữ liệu thành công");
                    textMaHang.Text = "";
                    textTenHang.Text = "";
                    comBoxDVT.Text = "Cái";
                }
                sqlConn.Close();
            }
        }

        private void textMaHang_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Trove_click(object sender, RoutedEventArgs e)
        {
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.NhapHang_Question;
        }
    }
}
