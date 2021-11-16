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
    /// Interaction logic for TaoHoaDon.xaml
    /// </summary>
    public partial class TaoHoaDon : BasePage
    {
        public static string MaNV, CMND;
        private int STT = 0;
        public class MatHang
        {
            public int STT { get; set; }
            public string MH { get; set; }
            public string TenHang { get; set; }
            public int SoLuong { get; set; }
            public int DonGia { get; set; }
            public int ThanhTien { get; set; }
            public int MaLo { get; set; }
        }
        public List<MatHang> list = new List<MatHang>();
        public TaoHoaDon()
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
            var cmd2 = new SqlCommand("SELECT MANV FROM NHANVIEN WHERE CMND = " + CMND, con);
            var dr2 = cmd2.ExecuteReader();
            while (dr2.Read())
            {
                MaNV = dr2.GetString(0);
            }
            dr2.Close();
            con.Close();
            STT = 0;
        }
        private void Check_SoLuong(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void comMaHang_DropDownClosed(object sender, EventArgs e)
        {
            comMaLo.Items.Clear();
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
            reader.Close();
            if (flag)
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                con.Open();
                var cmd = new SqlCommand("SELECT MALO FROM NHAPHANG WHERE MAHANG = " + "'" + comMaHang.Text + "'", con);
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    comMaLo.Items.Add(Convert.ToString(dr.GetInt32(0)));
                }
            }
        }

        private void Xoa_Click(object sender, RoutedEventArgs e)
        {
            var sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            sqlConn.Open();
            int i = HangMua.SelectedIndex;
            string SL = Convert.ToString(list[i].SoLuong);
            string MH = list[i].MH;
            string ML = Convert.ToString(list[i].MaLo);
            var sqlCommand = new SqlCommand("UPDATE NHAPHANG SET SOLUONG = SOLUONG  + " + SL + " WHERE MALO = " + ML + " AND MAHANG = '" + MH + "'", sqlConn);
            sqlCommand.ExecuteNonQuery();
            ThanhTien.Text = Convert.ToString(Convert.ToInt32(ThanhTien.Text) - list[i].ThanhTien);
            list.RemoveAt(i);
            for (i = 0; i < list.Count; i++) 
            {
                list[i].STT = i + 1;
            }
            HangMua.Items.Clear();
            HangMua.ItemsSource = list;
            STT--;
        }

        private void TroVe_Click(object sender, RoutedEventArgs e)
        {
            for (int i=0; i<list.Count; i++)
            {
                var sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                sqlConn.Open();
                string SL = Convert.ToString(list[i].SoLuong);
                string MH = list[i].MH;
                string ML = Convert.ToString(list[i].MaLo);
                var sqlCommand = new SqlCommand("UPDATE NHAPHANG SET SOLUONG = SOLUONG  + " + SL + " WHERE MALO = " + ML + " AND MAHANG = '" + MH + "'", sqlConn);
                sqlCommand.ExecuteNonQuery();
            }
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.Home;

        }

        private void TaoHoaDon_Click(object sender, RoutedEventArgs e)
        {
            if (ThanhTien.Text == "0") MessageBox.Show("Không thể tạo hóa đơn rỗng", "", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {

                string MaHD = "HD";
                MaHD += DateTime.Now.Day.ToString();
                MaHD += DateTime.Now.Month.ToString();
                MaHD += DateTime.Now.Year.ToString();
                string NgayHD = DateTime.Now.ToString("dd/MM/yyyy");
                var sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                sqlConn.Open();
                int SoHD = 0;
                var sqlCommand = new SqlCommand("SELECT COUNT(*) FROM HOADON WHERE NGHOADON = @NGHOADON", sqlConn);
                sqlCommand.Parameters.Add("@NGHOADON", System.Data.SqlDbType.SmallDateTime);
                sqlCommand.Parameters["@NGHOADON"].Value = NgayHD;
                var reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    SoHD = reader.GetInt32(0);
                }
                sqlCommand.Parameters.Clear();
                MaHD += (SoHD + 1).ToString();
                reader.Close();
                sqlCommand.CommandText = "INSERT INTO HOADON VALUES ('" +
                                          MaHD + "',@NGHOADON,'" +
                                          MaNV + "'," +
                                          ThanhTien.Text + ")";
                sqlCommand.Parameters.Add("@NGHOADON", System.Data.SqlDbType.SmallDateTime);
                sqlCommand.Parameters["@NGHOADON"].Value = NgayHD;
                sqlCommand.ExecuteNonQuery();
                sqlCommand.Parameters.Clear();
                for (int i = 0; i < list.Count; i++)
                {
                    sqlCommand.CommandText = "INSERT INTO CTHD VALUES ('" +
                                              MaHD + "','" +
                                              list[i].MH + "'," +
                                              Convert.ToString(list[i].SoLuong) + ")";
                    sqlCommand.ExecuteNonQuery();
                }
                MessageBox.Show("Tạo thành công hóa đơn\nMã hóa đơn " + MaHD);
                HangMua.Items.Clear();
                list = new List<MatHang>();
                ThanhTien.Text = "0";
            }
        }

        private void NhapHang_Click(object sender, RoutedEventArgs e)
        {
            if (comMaHang.Text == "" || textSL.Text == "" || comMaLo.Text == "") 
                MessageBox.Show("Vui lòng nhập đủ thông tin", "", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (Convert.ToInt32(textSL.Text) == 0) MessageBox.Show("Số lượng hàng nhập phải lớn hơn 0", "", MessageBoxButton.OK, MessageBoxImage.Error);
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
                reader.Close();
                if (!flag) MessageBox.Show("Mã hàng không tồn tại trong hệ thống!", "", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                {
                    sqlCommand = new SqlCommand("SELECT SOLUONG FROM NHAPHANG WHERE MALO = " + comMaLo.Text + "AND MAHANG = '" +comMaHang.Text +"'", sqlConn);
                    reader = sqlCommand.ExecuteReader();
                    if (reader.Read())
                    {
                        if (reader.GetInt32(0) < Convert.ToInt32(textSL.Text))
                        {
                            MessageBox.Show("Số lượng hàng còn lại không đủ", "", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    reader.Close();
                    sqlCommand.CommandText = "UPDATE NHAPHANG SET SOLUONG = SOLUONG - " + textSL.Text + " WHERE MALO = " + comMaLo.Text + "AND MAHANG = '" + comMaHang.Text + "'";
                    sqlCommand.ExecuteNonQuery();
                    MatHang tmp = new MatHang();
                    sqlCommand.CommandText = "SELECT TENHANG FROM LOAIHANG WHERE MAHANG = '" + comMaHang.Text + "'";
                    reader = sqlCommand.ExecuteReader();
                    if (reader.Read())
                    {
                        tmp.TenHang = reader.GetString(0);
                    }
                    reader.Close();
                    tmp.SoLuong = Convert.ToInt32(textSL.Text);
                    sqlCommand.CommandText = "SELECT DONGIA FROM NHAPHANG WHERE MALO = " + comMaLo.Text + "AND MAHANG = '" + comMaHang.Text + "'";
                    reader = sqlCommand.ExecuteReader();
                    if (reader.Read())
                    {
                        tmp.DonGia = Convert.ToInt32(reader.GetDecimal(0));
                    }
                    tmp.ThanhTien = tmp.SoLuong * tmp.DonGia;
                    STT++;
                    tmp.STT = STT;
                    tmp.MaLo = Convert.ToInt32(comMaLo.Text);
                    tmp.MH = comMaHang.Text;
                    reader.Close();
                    list.Add(tmp);
                    HangMua.Items.Add(new MatHang() { STT = tmp.STT, DonGia = tmp.DonGia, SoLuong = tmp.SoLuong, TenHang = tmp.TenHang, ThanhTien = tmp.ThanhTien });
                    textSL.Text = "";
                    comMaHang.Text = "";
                    comMaLo.Text = "";
                    comMaLo.Items.Clear();
                    ThanhTien.Text = Convert.ToString(Convert.ToInt32(ThanhTien.Text) + tmp.ThanhTien);
                }
            }
        }
    }
}
