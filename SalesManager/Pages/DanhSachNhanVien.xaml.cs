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
using System.Data;
using System.ComponentModel;
namespace SalesManager
{
    /// <summary>
    /// Interaction logic for DanhSachNhanVien.xaml
    /// </summary>
    public partial class DanhSachNhanVien : BasePage
    {
        public DanhSachNhanVien()
        {
            InitializeComponent();
            LoadListNV();
        }
        public class User
        {
            public string STT { get; set; }
            public string MANV { get; set; }
            public string HOTEN { get; set; }
            public string DIACHI { get; set; }
            public string GIOITINH { get; set; }
        }
        void LoadListNV()
        {
            var sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            sqlConn.Open();
            var sqlCommand = new SqlCommand("SELECT STT,MANV, HOTEN, DIACHI, GIOITINH FROM NHANVIEN", sqlConn);
            var reader = sqlCommand.ExecuteReader();
            List<User> items = new List<User>();
            while (reader.Read())
            {
                items.Add(new User() { STT = reader[0].ToString(), MANV = reader.GetString(1), HOTEN = reader.GetString(2), DIACHI = reader.GetString(3), GIOITINH = reader.GetString(4) });
                lvUsers.ItemsSource = items;
            }
            reader.Close();
            sqlConn.Close();
        }

        private void ThemNV_Click(object sender, RoutedEventArgs e)
        {
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.ThemNhanVien;
        }

        private void XemThongTin_Click(object sender, RoutedEventArgs e)
        {
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.ThongTinNhanVien;
        }

        private void Thoat_Click(object sender, RoutedEventArgs e)
        {
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.Home;
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).SideMenu = ApplicationPage.SideMenuControl;
        }
    }
}
