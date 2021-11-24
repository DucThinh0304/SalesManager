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
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace SalesManager
{
    /// <summary>
    /// Interaction logic for ThongTinChinhSuaDanhSach.xaml
    /// </summary>
    public partial class ThongTinChinhSuaDanhSach : Page
    {       
        public static string mahang;
        public static int malo;
        public ThongTinChinhSuaDanhSach()
        {
            InitializeComponent();
            load();         
            
        }
        void load()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            MAHG.Text = mahang;
            MALOO.Text = malo.ToString();
            var cmd = new SqlCommand("SELECT  NGNHAP, HANSD, SOLUONG, DONGIA  FROM NHAPHANG WHERE MAHANG='" + mahang + "' AND MALO= '" + malo + "'", con);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                NGAY.Text = dr.GetDateTime(0).ToString("MM/dd/yyyy");
                HAN.Text = dr.GetDateTime(1).ToString("MM/dd/yyyy");
                SL.Text = dr.GetInt32(2).ToString();
                GIA.Text = dr.GetDecimal(3).ToString();
            }
            dr.Close();
            cmd = new SqlCommand("SELECT TENHANG,DVT FROM LOAIHANG WHERE MAHANG='" + mahang + "'", con);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                TENHG.Text = dr.GetString(0);
                DONVI.Text= dr.GetString(1);
            }
            dr.Close();
            DONVI.Items.Add("Cái");
            DONVI.Items.Add("Kg");
            DONVI.Items.Add("Lốc");
            DONVI.Items.Add("Chục");
        }
        private void trolai(object sender, RoutedEventArgs e)
        {
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.ThongTinDanhSachHangHoa;
        }

        private void xoathongtin(object sender, RoutedEventArgs e)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();

            if (MessageBox.Show("Bạn có chắc muốn xóa thông tin hàng này???", "CAU HỎI", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                var cmd = new SqlCommand("DELETE FROM NHAPHANG WHERE MAHANG='" + mahang + "' and MALO ='" + int.Parse(MALOO.Text) + "'", con);
                var dr= cmd.ExecuteReader();
                dr.Close();
                MessageBox.Show("Bạn đã xóa thành công thông tin này, Vui lòng kiểm tra trong danh sách!!!");
                MAHG.Text = "";
                MALOO.Text = "";
                NGAY.Text = "";
                TENHG.Text = "";
                DONVI.Text = "";
                HAN.Text = "";
                SL.Text = "";
                GIA.Text = "";
            }
            con.Close();
        }

        private void suathongtin(object sender, RoutedEventArgs e)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            if (MAHG.Text == "" || MALOO.Text == "" || TENHG.Text == "" || NGAY.Text == "" || HAN.Text == "" || SL.Text == "" || DONVI.Text == "" || GIA.Text == "")
                MessageBox.Show("Vui lòng nhập đủ thông tin", "", MessageBoxButton.OK, MessageBoxImage.Error);
            
            else
            {
                var cmd = new SqlCommand("UPDATE NHAPHANG SET HANSD = @HANSD WHERE MAHANG='" + mahang + "' AND MALO= '" + malo + "'", con);
                cmd.Parameters.Add("@HANSD", System.Data.SqlDbType.SmallDateTime);
                cmd.Parameters["@HANSD"].Value = HAN.SelectedDate;
                var dr = cmd.ExecuteReader();
                dr.Close();
                cmd = new SqlCommand("UPDATE LOAIHANG SET TENHANG = '" + TENHG.Text + "' WHERE MAHANG='" + mahang + "'", con);
                dr = cmd.ExecuteReader();
                dr.Close();
                cmd = new SqlCommand("UPDATE DVT SET TENHANG = '" + DONVI.Text + "' WHERE MAHANG='" + mahang + "'", con);
                dr = cmd.ExecuteReader();
                dr.Close();
                cmd = new SqlCommand("UPDATE NHAPHANG SET SOLUONG = " + int.Parse(SL.Text) + " WHERE MAHANG='" + mahang + "' AND MALO= '" + malo + "'", con);
                dr = cmd.ExecuteReader();
                dr.Close();
                cmd = new SqlCommand("UPDATE NHAPHANG SET DONGIA = " + Decimal.Parse(GIA.Text) + " WHERE MAHANG='" + mahang + "' AND MALO= '" + malo + "'", con);
                dr = cmd.ExecuteReader();
                dr.Close();

                con.Close();
                MessageBox.Show("Sửa thông tin thành công!");
                MAHG.Text = "";
                MALOO.Text = "";
                NGAY.Text = "";
                TENHG.Text = "";
                DONVI.Text = "";
                HAN.Text = "";
                SL.Text = "";
                GIA.Text = "";
            }
        }
    }
}
