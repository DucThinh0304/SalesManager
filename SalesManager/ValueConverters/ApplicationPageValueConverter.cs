using System;
using System.Diagnostics;
using System.Globalization;

namespace SalesManager
{
    /// <summary>
    /// Converts the <see cref="ApplicationPage"/> to an actual view/page
    /// </summary>
    public class ApplicationPageValueConverter : BaseValueConverter<ApplicationPageValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Find the appropriate page
            switch ((ApplicationPage)value)
            {
                case ApplicationPage.Home:
                    return new Home();
                case ApplicationPage.NhapHang_Question:
                    return new NhapHang_Question();
                case ApplicationPage.NhapHangMoi:
                    return new NhapHangMoi();
                case ApplicationPage.NhapLoaiHangMoi:
                    return new NhapLoaiHangMoi();
                case ApplicationPage.DangNhap:
                    return new Pages.DangNhap();
                case ApplicationPage.DangKy:
                    return new Pages.DangKy();
                case ApplicationPage.QuenMatKhau:
                    return new Pages.QuenMatKhau();
                default:
                    Debugger.Break();
                    return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}