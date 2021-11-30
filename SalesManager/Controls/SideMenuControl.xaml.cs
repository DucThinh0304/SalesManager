using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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


namespace SalesManager
{
    /// <summary>
    /// Interaction logic for SideMenuControl.xaml
    /// </summary>
    public partial class SideMenuControl : BaseControl
    {
        #region Public Variables
        bool ShowThemHangContent = false;
        static public bool NotificationCheck = false;
        static public int Count = 0;
        #endregion

        #region Constructor
        public SideMenuControl()
        {
            InitializeComponent();
            NotificationControl.listNotification.CollectionChanged += ListNotification_CollectionChanged;
            if (NotificationCheck == true)
            {
                this.CoThongBao.Visibility = Visibility.Hidden;
            }
            if (NotificationControl.listNotification.Count == 0 || NotificationControl.listNotification.Count == Count)
            {
                this.CoThongBao.Visibility = Visibility.Hidden;
            }
            Count = NotificationControl.listNotification.Count;
            var sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            sqlConn.Open();
            var sqlCommand = new SqlCommand($"DELETE FROM NHAPHANG WHERE HANSD < GETDATE()", sqlConn);
            var reader = sqlCommand.ExecuteReader();
        }
        #endregion

        #region UI for SideMenu

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            ResetColor();
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.Home;
        }

        private void ResetColor()
        {
            BrushConverter bc = new BrushConverter();
            ThemHang_Button.Background = (Brush)bc.ConvertFrom("#00BCD4");
            DanhSachNV_Button.Background = (Brush)bc.ConvertFrom("#00BCD4");
            ThemSuaNV_Button.Background = (Brush)bc.ConvertFrom("#00BCD4");
            ThongKe_Button.Background = (Brush)bc.ConvertFrom("#00BCD4");
            TaoHoaDon_Button.Background = (Brush)bc.ConvertFrom("#00BCD4");
            ThongKeSoLuongHang_Button.Background = (Brush)bc.ConvertFrom("#00BCD4");
        }
        #endregion

        #region Notification Component
        private void ListNotification_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (NotificationControl.listNotification.Count == 0 || NotificationControl.listNotification.Count == Count)
            {
                this.CoThongBao.Visibility = Visibility.Hidden;
            }
            else
            {
                this.CoThongBao.Visibility = Visibility.Visible;
                NotificationCheck = false;
            }
        }

        private void Notification_Click(object sender, RoutedEventArgs e)
        {
            NotificationCheck = true;
            ResetColor();
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).SideMenu = ApplicationPage.NotificationControl;
        }

        #endregion

        #region Staff Component
        private void Staff_Click(object sender, RoutedEventArgs e)
        {
            ResetColor();
            this.Staff.Visibility = Visibility.Visible;
            this.Manager.Visibility = Visibility.Hidden;
            this.Setting.Visibility = Visibility.Hidden;
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.Home;
        }
        private void ThemHang_Click(object sender, RoutedEventArgs e)
        {
            ResetColor();
            BrushConverter bc = new BrushConverter();
            if (ShowThemHangContent == false)
            {
                ThemHang_Button.Background = (Brush)bc.ConvertFrom("#0A5E5A");
                ThemLoaiHangMoi_Button.Visibility = Visibility.Visible;
                ThemLoaiHangDaCo_Button.Visibility = Visibility.Visible;
                ShowThemHangContent = true;
            }
            else
            {
                ThemLoaiHangMoi_Button.Visibility = Visibility.Collapsed;
                ThemLoaiHangDaCo_Button.Visibility = Visibility.Collapsed;
                ShowThemHangContent = false;
            }
        }
        private void ThemLoaiHangDaCo_Click(object sender, RoutedEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            ResetColor();
            ThemHang_Button.Background = (Brush)bc.ConvertFrom("#0A5E5A");
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.NhapHangMoi;
        }
        private void ThemLoaiHangMoi_Click(object sender, RoutedEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            ResetColor();
            ThemHang_Button.Background = (Brush)bc.ConvertFrom("#0A5E5A");
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.NhapLoaiHangMoi;
        }
        private void ThongKeSoLuongHang_Click(object sender, RoutedEventArgs e)
        {
            ResetColor();
            BrushConverter bc = new BrushConverter();
            ThongKeSoLuongHang_Button.Background = (Brush)bc.ConvertFrom("#0A5E5A");
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.ThongKeSoLuongHang;
        }

        private void TaoHoaDon_Click(object sender, RoutedEventArgs e)
        {
            ResetColor();
            BrushConverter bc = new BrushConverter();
            TaoHoaDon_Button.Background = (Brush)bc.ConvertFrom("#0A5E5A");
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.TaoHoaDon;
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).SideMenu = ApplicationPage.VisualOff;
        }
        #endregion

        #region Manager Component
        private void Manager_Click(object sender, RoutedEventArgs e)
        {
            string Manv = "";
            var sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            sqlConn.Open();
            var sqlCommand = new SqlCommand($"SELECT MANV FROM NHANVIEN WHERE CMND = " + Home.CMND, sqlConn);
            var reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                Manv = reader.GetString(0);
            }
            if (Manv != "NVQL")
            {
                MessageBox.Show("Chỉ có chủ cửa hàng mới có thể sử dụng những chức năng này");
            }
            else
            {
                ResetColor();
                this.Manager.Visibility = Visibility.Visible;
                this.Staff.Visibility = Visibility.Hidden;
                this.Setting.Visibility = Visibility.Hidden;
                ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.Home;
            }
        }
        private void DSNV_Click(object sender, RoutedEventArgs e)
        {
            ResetColor();
            BrushConverter bc = new BrushConverter();
            DanhSachNV_Button.Background = (Brush)bc.ConvertFrom("#0A5E5A");
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.DanhSachNhanVien;
        }
        private void ThemNV_Click(object sender, RoutedEventArgs e)
        {
            ResetColor();
            BrushConverter bc = new BrushConverter();
            ThemSuaNV_Button.Background = (Brush)bc.ConvertFrom("#0A5E5A");
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.ThemNhanVien;
        }
        private void ThongKeDT_Click(object sender, RoutedEventArgs e)
        {
            ResetColor();
            BrushConverter bc = new BrushConverter();
            ThongKe_Button.Background = (Brush)bc.ConvertFrom("#0A5E5A");
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.ThongKeDoanhThu;
        }
        #endregion

        #region Setting Component
        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            this.Staff.Visibility = Visibility.Hidden;
            this.Manager.Visibility = Visibility.Hidden;
            this.Setting.Visibility = Visibility.Visible;
        }
        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.DangNhap;
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).SideMenu = ApplicationPage.VisualOff;
        }
        private void SuaMatKhau_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Trâm thêm vô đi");
        }
        private void ThongTinCuaHang_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Bình thêm vô đi");
        }
        #endregion

    }
}
