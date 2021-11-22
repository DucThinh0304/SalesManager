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

namespace SalesManager
{
    /// <summary>
    /// Interaction logic for ThongTinDanhSachHangHoa.xaml
    /// </summary>
    public partial class ThongTinDanhSachHangHoa : BasePage
    {

        public class HANG
        {
            public string MAHANG { get; set; }
            public int MALO { get; set; }
            public string TENHANG { get; set; }
            public System.DateTime NGNHAP { get; set; }
            public System.DateTime HANSD { get; set; }
            public int SOLUONG { get; set; }
            public string DVT { get; set; }
            public System.Decimal DONGIA { get; set; }
        }
        
        public ThongTinDanhSachHangHoa()
        {
            InitializeComponent();
            Loadhang();
        }
        void Loadhang()
        {
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            sqlConn.Open();
            var sqlCommand = new SqlCommand("SELECT LOAIHANG.MAHANG, MALO, TENHANG, NGNHAP, HANSD, SOLUONG,DVT, " +
                "DONGIA  FROM LOAIHANG inner join NHAPHANG on LOAIHANG.MAHANG=NHAPHANG.MAHANG", sqlConn);
            var reader = sqlCommand.ExecuteReader();
            List<HANG> items = new List<HANG>();
            while (reader.Read())
            {
                items.Add(new HANG()
                {
                    MAHANG = reader.GetString(0),
                    MALO = reader.GetInt32(1),
                    TENHANG = reader.GetString(2),
                    NGNHAP = reader.GetDateTime(3),
                    HANSD = reader.GetDateTime(4),
                    SOLUONG = reader.GetInt32(5),
                    DVT = reader.GetString(6),
                    DONGIA = reader.GetDecimal(7)
                }) ;
                listhanghoa.ItemsSource = items;
            }
            reader.Close();
            sqlConn.Close();
        }
        private void SelectCurrentItem(object sender, KeyboardFocusChangedEventArgs e)
        {
            ListViewItem item = (ListViewItem)sender;
            item.IsSelected = true;
        }
        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ThongTinChinhSuaDanhSach.mahang = ((HANG)listhanghoa.SelectedItem).MAHANG;
            ThongTinChinhSuaDanhSach.malo = ((HANG)listhanghoa.SelectedItem).MALO;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.Home;
        }

        private void CHINHSUA(object sender, RoutedEventArgs e)
        {
            if (ThongTinChinhSuaDanhSach.mahang == "") MessageBox.Show("Nháy đúp chọn hàng trước khi sửa thông tin!");
            else
            {
                ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.ThongTinChinhSuaDanhSach;
            }
            
        }

        private void TROLAI(object sender, RoutedEventArgs e)
        {
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.Home;
        }
    }
}
