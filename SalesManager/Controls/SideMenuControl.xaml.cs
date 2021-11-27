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
        bool ShowThemHangContent = false;
        static public bool NotificationCheck = false;
        static public int Count = 0;
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


        private void ResetColor()
        {
            var bc = new BrushConverter();
            ThemHang_Button.Background = (Brush)bc.ConvertFrom("#00BCD4");
            DanhSachNV_Button.Background = (Brush)bc.ConvertFrom("#00BCD4");
            ThemSuaNV_Button.Background = (Brush)bc.ConvertFrom("#00BCD4");
            ThongKe_Button.Background = (Brush)bc.ConvertFrom("#00BCD4");
            TaoHoaDon_Button.Background = (Brush)bc.ConvertFrom("#00BCD4");
            ThongKeSoLuongHang_Button.Background = (Brush)bc.ConvertFrom("#00BCD4");

        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.DangNhap;
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).SideMenu = ApplicationPage.VisualOff;
        }

        private void ThemHang_Click(object sender, RoutedEventArgs e)
        {
            ResetColor();
            var bc = new BrushConverter();
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

        private void DSNV_Click(object sender, RoutedEventArgs e)
        {
            ResetColor();
            var bc = new BrushConverter();
            DanhSachNV_Button.Background = (Brush)bc.ConvertFrom("#0A5E5A");
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.DanhSachNhanVien;
        }

        private void ThemNV_Click(object sender, RoutedEventArgs e)
        {
            ResetColor();
            var bc = new BrushConverter();
            ThemSuaNV_Button.Background = (Brush)bc.ConvertFrom("#0A5E5A");
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.ThemNhanVien;
        }

        private void ThongKeDT_Click(object sender, RoutedEventArgs e)
        {
            ResetColor();
            var bc = new BrushConverter();
            ThongKe_Button.Background = (Brush)bc.ConvertFrom("#0A5E5A");
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.ThongKeDoanhThu;
        }
        
        private void TaoHoaDon_Click(object sender, RoutedEventArgs e)
        {
            ResetColor();
            var bc = new BrushConverter();
            TaoHoaDon_Button.Background = (Brush)bc.ConvertFrom("#0A5E5A");
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.TaoHoaDon;
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).SideMenu = ApplicationPage.VisualOff;
        }
        private void Home_Click(object sender, RoutedEventArgs e)
        {
            ResetColor();
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.Home;
        }
        private void Notification_Click(object sender, RoutedEventArgs e)
        {
            NotificationCheck = true;
            //foreach (Border bor in NotificationControl.listNotification)
            //{
            //    bor.Parent.RemoveChild(bor);
            //}
            ResetColor();
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).SideMenu = ApplicationPage.NotificationControl;

        }
        private void Manager_Click(object sender, RoutedEventArgs e)
        {
            this.Manager.Visibility = Visibility.Visible;
            this.Staff.Visibility = Visibility.Hidden;
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.Home;
        }
        private void Staff_Click(object sender, RoutedEventArgs e)
        {
            this.Staff.Visibility = Visibility.Visible;
            this.Manager.Visibility = Visibility.Hidden;
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.Home;
        }
        private void ThongKeSoLuongHang_Click(object sender, RoutedEventArgs e)
        {
            ResetColor();
            var bc = new BrushConverter();
            ThongKeSoLuongHang_Button.Background = (Brush)bc.ConvertFrom("#0A5E5A");
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.ThongKeSoLuongHang;
        }
        private void ThemLoaiHangDaCo_Click(object sender, RoutedEventArgs e)
        {
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.NhapHangMoi;
        }
        private void ThemLoaiHangMoi_Click(object sender, RoutedEventArgs e)
        {
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.NhapLoaiHangMoi;
        }
    }
}
