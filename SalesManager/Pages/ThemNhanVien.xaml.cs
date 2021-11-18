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
using System.Security.Cryptography;

namespace SalesManager
{
    /// <summary>
    /// Interaction logic for ThemNhanVien.xaml
    /// </summary>
    public partial class ThemNhanVien : BasePage
    {
        public ThemNhanVien()
        {
            InitializeComponent();
            if (manv != "") load_ttsuaNV();
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
        public static string Decrypt(string toDecrypt)
        {
            string key = "";
            bool useHashing = true;
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

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

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        void load_ttsuaNV()
        {
            var sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            sqlConn.Open();
            var sqlCommand = new SqlCommand("SELECT * FROM NHANVIEN WHERE MANV=" + manv, sqlConn);
            var reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                tbMANV.Text = reader[0].ToString();
                tbHOTEN.Text = reader[1].ToString();
                ngSinh.Text = reader[2].ToString();
                tbCMND.Text = reader[3].ToString();
                tbDIACHI.Text = reader[4].ToString();
                ngVL.Text = reader[5].ToString();
                tbMK.Password = reader[6].ToString();
            }
            reader.Close();
            sqlConn.Close();
            tbMK.Password = Decrypt(tbMK.Password.ToString());
            tbRMK.Password = tbMK.Password;
        }
        private void Them_Click(object sender, RoutedEventArgs e)
        {

            if (tbMANV.Text == "" || tbHOTEN.Text == "" || ngSinh.Text == "" || tbCMND.Text == "" || tbDIACHI.Text == "" || ngVL.Text == "" || tbMK.Password == "" || tbRMK.Password == "") MessageBox.Show("Vui lòng nhập đủ thông tin", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (tbMK.Password != tbRMK.Password) MessageBox.Show("Mật khẩu nhập lại không chính xác. Vui lòng thử lại!", "THÔNG BÁO", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                string pass = Encrypt(tbMK.Password);
                var sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                sqlConn.Open();
                var sqlCommand = new SqlCommand("SELECT MANV FROM NHANVIEN", sqlConn);
                var reader = sqlCommand.ExecuteReader();
                bool flag = false;
                while (reader.Read())
                {
                    if (reader.GetString(0) == tbMANV.Text)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    reader.Close();
                    MessageBoxResult reSult = MessageBox.Show("Nhân viên đã tồn tại trong hệ thống, bạn có muốn ghi đè dữ liệu đang có không?", "", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (reSult == MessageBoxResult.Yes)
                    {
                        sqlCommand.CommandText = "UPDATE NHANVIEN SET HOTEN = @HOTEN, NGSINH = @NGSINH, CMND=@CMND, DIACHI= @DIACHI, NGVAOLAM=@NGVAOLAM, MATKHAU=@MATKHAU WHERE MANV = @MANV";
                        sqlCommand.Parameters.Add("@MANV", System.Data.SqlDbType.VarChar);
                        sqlCommand.Parameters["@MANV"].Value = tbMANV.Text;
                        sqlCommand.Parameters.Add("@HOTEN", System.Data.SqlDbType.NVarChar);
                        sqlCommand.Parameters["@HOTEN"].Value = tbHOTEN.Text;
                        sqlCommand.Parameters.Add("@NGSINH", System.Data.SqlDbType.SmallDateTime);
                        sqlCommand.Parameters["@NGSINH"].Value = ngSinh.Text;
                        sqlCommand.Parameters.Add("@CMND", System.Data.SqlDbType.VarChar);
                        sqlCommand.Parameters["@CMND"].Value = tbCMND.Text;
                        sqlCommand.Parameters.Add("@DIACHI", System.Data.SqlDbType.NVarChar);
                        sqlCommand.Parameters["@DIACHI"].Value = tbDIACHI.Text;
                        sqlCommand.Parameters.Add("@NGVAOLAM", System.Data.SqlDbType.SmallDateTime);
                        sqlCommand.Parameters["@NGVAOLAM"].Value = ngVL.Text;
                        sqlCommand.Parameters.Add("@MATKHAU", System.Data.SqlDbType.NVarChar);
                        sqlCommand.Parameters["@MATKHAU"].Value = pass;
                        sqlCommand.ExecuteNonQuery();
                        sqlCommand.Dispose();
                        MessageBox.Show("Cập nhật dữ liệu thành công");
                        tbMANV.Text = "";
                        tbHOTEN.Text = "";
                        ngSinh.Text = "";
                        tbCMND.Text = "";
                        tbDIACHI.Text = "";
                        ngVL.Text = "";
                        tbMK.Password = "";
                        ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.DanhSachNhanVien;
                    }
                }
                else
                {
                    reader.Close();
                    sqlCommand.CommandText = "INSERT INTO NHANVIEN (MANV,HOTEN,NGSINH,CMND,DIACHI,NGVAOLAM,MATKHAU) VALUES " + "(@MANV,@HOTEN,@NGSINH,@CMND,@DIACHI,@NGVAOLAM,@MATKHAU)";
                    sqlCommand.Parameters.Add("@MANV", System.Data.SqlDbType.VarChar);
                    sqlCommand.Parameters["@MANV"].Value = tbMANV.Text;
                    sqlCommand.Parameters.Add("@HOTEN", System.Data.SqlDbType.NVarChar);
                    sqlCommand.Parameters["@HOTEN"].Value = tbHOTEN.Text;
                    sqlCommand.Parameters.Add("@NGSINH", System.Data.SqlDbType.SmallDateTime);
                    sqlCommand.Parameters["@NGSINH"].Value = ngSinh.Text;
                    sqlCommand.Parameters.Add("@CMND", System.Data.SqlDbType.VarChar);
                    sqlCommand.Parameters["@CMND"].Value = tbCMND.Text;
                    sqlCommand.Parameters.Add("@DIACHI", System.Data.SqlDbType.NVarChar);
                    sqlCommand.Parameters["@DIACHI"].Value = tbDIACHI.Text;
                    sqlCommand.Parameters.Add("@NGVAOLAM", System.Data.SqlDbType.SmallDateTime);
                    sqlCommand.Parameters["@NGVAOLAM"].Value = ngVL.Text;
                    sqlCommand.Parameters.Add("@MATKHAU", System.Data.SqlDbType.NVarChar);
                    sqlCommand.Parameters["@MATKHAU"].Value = pass;
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.Dispose();
                    MessageBox.Show("Thêm dữ liệu thành công");
                    tbMANV.Text = "";
                    tbHOTEN.Text = "";
                    ngSinh.Text = "";
                    tbCMND.Text = "";
                    tbDIACHI.Text = "";
                    ngVL.Text = "";
                    tbMK.Password = "";
                    ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.DanhSachNhanVien;
                }
            }
        }

        private void Huy_Click(object sender, RoutedEventArgs e)
        {
            manv = "";
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.DanhSachNhanVien;
        }

    }
}
