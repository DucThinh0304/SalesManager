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
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;
namespace SalesManager
{
    /// <summary>
    /// Interaction logic for NhapHangMoi.xaml
    /// </summary>
    public partial class NhapHangMoi : Page
    {
        public static string MaNV = "NV001";
        public NhapHangMoi()
        {
            InitializeComponent();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT MAHANG FROM LOAIHANG", con);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                comMaHang.Items.Add(dr.GetString(0));
            }
            dr.Close();
            con.Close();
            NgayNhapHang.SelectedDate = DateTime.Now;
        }

        private void Check_SoLuong(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Check_DonGia(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void NhapHang_Click(object sender, RoutedEventArgs e)
        {
            if (comMaHang.Text == "" || NgayNhapHang.Text == "" || HSD.Text == "" || textSL.Text == "" || textDonGia.Text == "")
                MessageBox.Show("Vui lòng nhập đủ thông tin", "", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (HSD.SelectedDate < NgayNhapHang.SelectedDate) MessageBox.Show("Hạn sử dụng phải lớn hơn ngày nhập", "", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (Convert.ToInt32(textSL.Text) == 0) MessageBox.Show("Số lượng hàng nhập phải lớn hơn 0", "", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (Convert.ToInt32(textDonGia.Text) <= 100) MessageBox.Show("Giá mặt hàng phải lớn hơn 100 đồng", "", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                var sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                sqlConn.Open();
                var sqlCommand = new SqlCommand("SELECT MAHANG FROM LOAIHANG", sqlConn);
                var reader = sqlCommand.ExecuteReader();
                bool flag = false;
                while (reader.Read())
                {
                    if (reader.GetString(0) == comMaHang.Text)
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag) MessageBox.Show("Mã hàng không tồn tại trong hệ thống!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                {
                    reader.Close();
                    sqlCommand.CommandText = "SELECT MAX(MALO) FROM NHAPHANG WHERE MAHANG = " + comMaHang.Text;
                    reader = sqlCommand.ExecuteReader();
                    int MaLo = 0;
                    if (reader.Read())
                    {
                        if (!(reader.GetValue(0) is DBNull)) MaLo = reader.GetInt32(0);
                    }
                    reader.Close();
                    sqlCommand.CommandText = "INSERT INTO NHAPHANG VALUES (@MAHANG,@MALO,@NGNHAP,@HANSD,@SOLUONG,@DONGIA,@MANV)";
                    sqlCommand.Parameters.Add("@MAHANG", System.Data.SqlDbType.VarChar);
                    sqlCommand.Parameters["@MAHANG"].Value = comMaHang.Text;
                    sqlCommand.Parameters.Add("@MALO", System.Data.SqlDbType.Int);
                    sqlCommand.Parameters["@MALO"].Value = MaLo + 1;
                    sqlCommand.Parameters.Add("@NGNHAP", System.Data.SqlDbType.SmallDateTime);
                    sqlCommand.Parameters["@NGNHAP"].Value = NgayNhapHang.SelectedDate.ToString();
                    sqlCommand.Parameters.Add("@HANSD", System.Data.SqlDbType.SmallDateTime);
                    sqlCommand.Parameters["@HANSD"].Value = HSD.SelectedDate.ToString();
                    sqlCommand.Parameters.Add("@SOLUONG", System.Data.SqlDbType.Int);
                    sqlCommand.Parameters["@SOLUONG"].Value = Convert.ToInt32(textSL.Text);
                    sqlCommand.Parameters.Add("@DONGIA", System.Data.SqlDbType.Money);
                    sqlCommand.Parameters["@DONGIA"].Value = Convert.ToInt32(textDonGia.Text);
                    sqlCommand.Parameters.Add("@MANV", System.Data.SqlDbType.VarChar);
                    sqlCommand.Parameters["@MANV"].Value = MaNV;
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Dispose();
                    MessageBox.Show("Thêm hàng thành công!");
                    comMaHang.Text = "";
                    NgayNhapHang.SelectedDate = DateTime.Now;
                    HSD.Text = "";
                    textDonGia.Text = "";
                    textSL.Text = "";
                }
            }
        }

        private void TroVe_Click(object sender, RoutedEventArgs e)
        {
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.NhapHang_Question;
        }
    }
}
