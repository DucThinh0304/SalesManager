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
    /// Interaction logic for DangKy.xaml
    /// </summary>
    public partial class DangKy : BasePage
    {
        public DangKy()
        {
            InitializeComponent();
        }

        private void Huy_Click(object sender, RoutedEventArgs e)
        {
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.DangNhap;
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
        private void HoanTat_Click(object sender, RoutedEventArgs e)
        {

            if (TenCuaHang.Text == "" || MaCuHang.Text == "" || NgayThanhLap.Text == "" || TenQuanLi.Text == "" || MaQuanLi.Text == ""
                || NgaySinh.Text == "" || SDT.Text == "" || DiaChi.Text == "" || CMND.Text == "" || MatKhau.Password == "" || XNMatKhau.Password == "")
            {
                MessageBox.Show("Đăng ký không thành công");
                return;
            }
            if (MatKhau.Password != XNMatKhau.Password)
            {
                MessageBox.Show("Đăng ký không thành công");
                return;
            }
            else
            {
                SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                sqlConn.Open();
                var sqlCommandCheck = new SqlCommand("SELECT MACH FROM CUAHANG WHERE MACH='" + MaCuHang.Text + "'", sqlConn);
                var reader = sqlCommandCheck.ExecuteReader();
                if (reader.Read() == true)
                {
                    MessageBox.Show("Mã cửa hàng đã tồn tại!!!");
                    reader.Close();
                    return;
                }

                sqlConn.Close();
                sqlConn.Open();
                sqlCommandCheck = new SqlCommand("SELECT CMND FROM NHANVIEN WHERE CMND='" + CMND.Text + "'", sqlConn);
                reader = sqlCommandCheck.ExecuteReader();
                if (reader.Read() == true)
                {
                    MessageBox.Show("CMND/CCCD đã tồn tại!!!");
                    reader.Close();
                    return;
                }

                reader.Close();
                string MatKhauMaHoa = Encrypt(MatKhau.Password);
                string CH = "insert into CUAHANG values ('" + MaCuHang.Text + "',N'" + TenCuaHang.Text + "','" + MaQuanLi.Text + "','" + NgayThanhLap.Text + "')";
                string NV = "insert into NHANVIEN values ('" + MaQuanLi.Text + "',N'" + TenQuanLi.Text + "','" + NgaySinh.Text + "','" + CMND.Text + "',N'" + DiaChi.Text + "','" + NgayThanhLap.Text + "','" + MatKhauMaHoa + "')";
                SqlCommand sqlCommandCH, sqlCommandNV;
                sqlCommandNV = sqlConn.CreateCommand();
                sqlCommandNV.CommandText = NV;
                sqlCommandNV.ExecuteNonQuery();
                sqlCommandCH = sqlConn.CreateCommand();
                sqlCommandCH.CommandText = CH;
                sqlCommandCH.ExecuteNonQuery();
                if (sqlConn != null)
                    sqlConn.Close();
                MessageBox.Show("Tạo tài khoản thành công.");
                TenCuaHang.Clear();
                MaCuHang.Clear();
                NgayThanhLap.Text = "";
                TenQuanLi.Clear();
                MaQuanLi.Clear();
                NgaySinh.Text = "";
                SDT.Clear();
                DiaChi.Clear();
                CMND.Clear();
                MatKhau.Clear();
                XNMatKhau.Clear();
                sqlConn.Close();
            }
        }
    }
}
