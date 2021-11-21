using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Net;
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
using System.Data;

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
       
        private void SDT_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void CMND_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void Gmail_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9a-zA-Z]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void Huy_Click(object sender, RoutedEventArgs e)
        {
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.DangNhap;
        }
        string MaXacNhan;
        private void HoanTat_Click(object sender, RoutedEventArgs e)
        {
            string MaCuaHang = "CH";
            // Nhập chưa đủ thông tin
            if (TenCuaHang.Text == "" || NgayThanhLap.Text == "" || TenQuanLi.Text == "" || NgaySinh.Text == "" || SDT.Text == "" || DiaChi.Text == "" || CMND.Text == "" || MatKhau.Password == "" || XNMatKhau.Password == "" || Gmail.Text == "" || MXN.Text == "") 
            {
                MessageBox.Show("Bạn vui lòng nhập đủ thông tin!!!");
                return;
            }
            // Mã xác nhận không khớp
            if (MXN.Text != MaXacNhan)
            {
                MessageBox.Show("Mã xác nhận chưa chính xác. Vui lòng lấy lại mã xác nhận khác!!!");
                MXN.Clear();
                MXN.Focus();
                MaXacNhan = null;
                return;
            }
            //Mật khẩu không khớp
            if (MatKhau.Password != XNMatKhau.Password)
            {
                MessageBox.Show("Mật khẩu chưa trùng khớp!!!");
                return;
            }
            else
            {
                SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                if (sqlConn.State == ConnectionState.Closed){ sqlConn.Open(); }
                SqlCommand sqlCommandCheck ;
                // Tìm mã cửa hàng
                try
                {
                    sqlCommandCheck = new SqlCommand("SELECT COUNT (MACH) FROM CUAHANG", sqlConn);
                    var reader = sqlCommandCheck.ExecuteScalar();
                    MaCuaHang =MaCuaHang + Convert.ToString(Convert.ToInt32(reader) + 1);
                    sqlConn.Close();
                }
                catch(Exception)
                {
                    MessageBox.Show("Lỗi kết nối dữ liệu!!!");
                    sqlConn.Close();
                    return;
                }
                // Gmail tồn tại
                if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
                sqlCommandCheck = new SqlCommand("SELECT GMAIL FROM NHANVIEN WHERE GMAIL ='" + Gmail.Text + "@gmail.com'", sqlConn);
                try
                {
                    var reader = sqlCommandCheck.ExecuteReader();
                    if (reader.Read() == true)
                    {
                        MessageBox.Show("Gmail đã tồn tại!!!");
                        reader.Close();
                        sqlConn.Close();
                        return;
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Lỗi kết nối dữ liệu!!!");
                    sqlConn.Close();
                    return;
                }
                //CMND/CCCD tôn tại
                if (sqlConn.State == ConnectionState.Open) { sqlConn.Close(); }
                if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
                sqlCommandCheck = new SqlCommand("SELECT CMND FROM NHANVIEN WHERE CMND='" + CMND.Text + "'", sqlConn);
                try
                {
                    var reader = sqlCommandCheck.ExecuteReader();
                    if (reader.Read() == true)
                    {
                        MessageBox.Show("CMND/CCCD đã tồn tại!!!");
                        reader.Close();
                        sqlConn.Close();
                        return;
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Lỗi kết nối dữ liệu!!!");
                    sqlConn.Close();
                    return;
                }

                // Thêm data vào Sql
                if (sqlConn.State == ConnectionState.Open) { sqlConn.Close(); }
                if (sqlConn.State == ConnectionState.Closed) { sqlConn.Open(); }
                try
                {
                    string MatKhauMaHoa = Encrypt(MatKhau.Password);
                    string CH = "insert into CUAHANG values ('" + MaCuaHang + "',N'" + TenCuaHang.Text + "','NVQL01','" + NgayThanhLap.Text + "')";
                    string NV = "insert into NHANVIEN values ('NVQL01',N'" + TenQuanLi.Text + "','" + NgaySinh.Text + "','" + CMND.Text + "',N'" + DiaChi.Text + "','" + NgayThanhLap.Text + "','" + MatKhauMaHoa + "','" + Gmail.Text + "@gmail.com')";
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
                    NgayThanhLap.Text = "";
                    TenQuanLi.Clear();
                    NgaySinh.Text = "";
                    SDT.Clear();
                    DiaChi.Clear();
                    CMND.Clear();
                    Gmail.Clear();
                    MXN.Clear();
                    MaXacNhan = null;
                    MatKhau.Clear();
                    XNMatKhau.Clear();
                    sqlConn.Close();
                }
                catch(Exception)
                {
                    MessageBox.Show("Lỗi kết nối dữ liệu tc!!!");
                    sqlConn.Close();
                    return;
                }
               
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Random Ran = new Random();
            MaXacNhan = Ran.Next(100000, 999999).ToString();
            MailMessage Mess = new MailMessage("bihubihu2002@gmail.com", Gmail.Text+"@gmail.com", "[QUẢN LÍ BÁN HÀNG]", "Mã xác nhận: " + MaXacNhan);
            Mess.BodyEncoding = System.Text.Encoding.UTF8;
            Mess.SubjectEncoding = System.Text.Encoding.UTF8;
            Mess.IsBodyHtml = true;
            Mess.Sender = new MailAddress("bihubihu2002@gmail.com", "QUANLIBANHANG");
            SmtpClient Client = new SmtpClient("smtp.gmail.com",587);
            Client.EnableSsl = true;
            Client.Credentials = new NetworkCredential("bihubihu2002@gmail.com", "dungcomadua2");
            try
            {
                Client.Send(Mess);
                MessageBox.Show("Đã gửi mã xác nhận. Bạn vui lòng kiểm tra trong hộp thư gmail của bạn.");
            }
            catch(Exception )
            {
                MessageBox.Show("Gửi mã xác nhận thất bại.");
            }
          
        }

        private void Gmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            MXN.Clear();
            MaXacNhan = null;
        }
    }
    }
