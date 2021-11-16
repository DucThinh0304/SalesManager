using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for DangNhap.xaml
    /// </summary>
    public partial class DangNhap : BasePage
    {
        public DangNhap()
        {
            InitializeComponent();
        }


        private void DangKy_Click(object sender, RoutedEventArgs e)
        {

            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.DangKy;
        }
        public static string Encrypt(string toEncrypt)
        {
            string key = "";
            bool useHashing = true;
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        private void DangNhap_Click(object sender, RoutedEventArgs e)
        {
            if (!(CMND.Text == "" || MatKhau.Password== "" )) {
                SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                sqlConn.Open();
                string MatKhauMaHoa = Encrypt(MatKhau.Password);
                var sqlCommand = new SqlCommand("SELECT CMND,MATKHAU FROM NHANVIEN WHERE CMND='" + CMND.Text + "'and MATKHAU ='" + MatKhauMaHoa + "'", sqlConn);
                var reader = sqlCommand.ExecuteReader();
                if (reader.Read() == true)
                {
                    sqlConn.Close();
                    reader.Close();
                    NhapHangMoi.CMND = CMND.Text;
                    Home.CMND = CMND.Text;
                    TaoHoaDon.CMND = CMND.Text;
                    ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.Home;
                    ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).SideMenu = ApplicationPage.SideMenuControl;
                }
                else
                {
                    CMND.Clear();
                    MatKhau.Clear();
                    MessageBox.Show("Đăng nhập thất bại");
                }
                reader.Close();
            }
            else
                 MessageBox.Show("Xin hãy điền đầy đủ thông tin");

        }

        private void ForgotPassword_Click(object sender, MouseButtonEventArgs e)
        {
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.QuenMatKhau;
        }
    }
}
