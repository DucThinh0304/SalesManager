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
using System.Data;
using System.ComponentModel;
using System.Security.Cryptography;
namespace SalesManager
{
    /// <summary>
    /// Interaction logic for ThongTinNhanVien.xaml
    /// </summary>
    public partial class ThongTinNhanVien : BasePage
    {
        public ThongTinNhanVien()
        {
            InitializeComponent();
            Load();
            LoadListHD();
        }

        public class HOADON
        {
            public string MAHD { get; set; }
            public string NGHOADON { get; set; }
            public string TRIGIA { get; set; }
        }

        void LoadListHD()
        {
            var sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            sqlConn.Open();
            var sqlCommand = new SqlCommand("SELECT MAHD,NGHOADON,TRIGIA FROM HOADON WHERE MANV=" + manv, sqlConn);
            var reader = sqlCommand.ExecuteReader();
            List<HOADON> items = new List<HOADON>();
            while (reader.Read())
            {
                items.Add(new HOADON() { MAHD = reader[0].ToString(), NGHOADON = reader[1].ToString(), TRIGIA = reader[2].ToString() });
                lvHOADON.ItemsSource = items;
            }
            reader.Close();
            sqlConn.Close();
        }

        void Load()
        {
            var sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            sqlConn.Open();
            var sqlCommand = new SqlCommand("SELECT * FROM NHANVIEN WHERE MANV= '" + manv+ "'", sqlConn);
            var reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                lb_manv.Content = reader[0].ToString();
                lb_ten.Content = reader[1].ToString();
                lb_ngsinh.Content = reader[2].ToString();
                lb_cmnd.Content = reader[3].ToString();
                lb_diachi.Content = reader[4].ToString();
                lb_ngvl.Content = reader[5].ToString();
                lb_mk.Content = reader[6].ToString();
                lb_gmail.Content = reader[7].ToString();
            }
            reader.Close();
            sqlConn.Close();
            lb_mk.Content = Decrypt(lb_mk.Content.ToString());
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

        private void suaNV_Click(object sender, RoutedEventArgs e)
        {
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.SuaNhanVien;
        }

        private void XoaNV_Click(object sender, RoutedEventArgs e)
        {
            var sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            sqlConn.Open();
            var sqlCommand = new SqlCommand("DELETE FROM HOADON WHERE MANV=" + manv, sqlConn);
            sqlCommand.ExecuteNonQuery();
            sqlCommand.Dispose();

            sqlCommand.CommandText = "DELETE FROM NHANVIEN WHERE MANV=" + manv;
            sqlCommand.ExecuteNonQuery();
            sqlCommand.Dispose();
            sqlConn.Close();
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.DanhSachNhanVien;
        }

        private void taoPass_Click(object sender, RoutedEventArgs e)
        {
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.TaoMaNhanVien;
        }
    }
}
