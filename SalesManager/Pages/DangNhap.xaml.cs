using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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
            Check();

        }
        private void Check()
        {
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
            SqlCommand sqlCommandCheck;
            try
            {
                sqlCommandCheck = new SqlCommand("SELECT COUNT (MACH) FROM CUAHANG", sqlConn);
                var reader = sqlCommandCheck.ExecuteScalar();
                if (Convert.ToInt32(reader) >= 1)
                {
                    DangKy.IsEnabled = false;
                }
                sqlConn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi kết nối dữ liệu!!!");
                sqlConn.Close();
                return;
            }

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
            if (!(CMND.Text == "" || MatKhau.Password == ""))
            {
                SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                sqlConn.Open();
                string MatKhauMaHoa = Encrypt(MatKhau.Password);
                try
                {
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
                        MessageBox.Show("CMND/CCCD hoặc mật khẩu của bạn chưa chính xác.");
                    }
                    reader.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Lỗi kết nối dữ liệu!!!");
                    sqlConn.Close();
                }

            }
            else
                MessageBox.Show("Bạn vui lòng nhập đầy đủ thông tin!!!");

        }

        private void ForgotPassword_Click(object sender, MouseButtonEventArgs e)
        {
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.QuenMatKhau;
        }

        private void CMND_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
