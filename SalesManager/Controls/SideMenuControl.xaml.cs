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


namespace SalesManager
{
    /// <summary>
    /// Interaction logic for SideMenuControl.xaml
    /// </summary>
    public partial class SideMenuControl : BaseControl
    {
        public SideMenuControl()
        {
            InitializeComponent();
        }

        private void ResetColor()
        {
            var bc = new BrushConverter();
            ThemHang_Button.Background = (Brush)bc.ConvertFrom("#00BCD4");
            DanhSachNV_Button.Background = (Brush)bc.ConvertFrom("#00BCD4");
            ThemSuaNV_Button.Background = (Brush)bc.ConvertFrom("#00BCD4");
            ThongKe_Button.Background = (Brush)bc.ConvertFrom("#00BCD4");
            Danhsach_Button.Background = (Brush)bc.ConvertFrom("#00BCD4");
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
            ThemHang_Button.Background = (Brush)bc.ConvertFrom("#0A5E5A");
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.NhapHang_Question;
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
        private void Danhsach_1(object sender, RoutedEventArgs e)
        {
            ResetColor();
            var bc = new BrushConverter();
            Danhsach_Button.Background = (Brush)bc.ConvertFrom("#0A5E5A");
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.ThongTinDanhSachHangHoa;
        }
    }
}
