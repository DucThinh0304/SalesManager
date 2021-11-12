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
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Threading;

namespace SalesManager
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : BasePage
    {
        public static string CMND;
        public Home()
        {
            InitializeComponent();

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            con.Open();
            var cmd = new SqlCommand("SELECT HOTEN FROM NHANVIEN WHERE CMND = "+ CMND , con);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                string HoTen = dr.GetString(0);
                TextBlock1.Text = $"Xin chào {HoTen}";
            }
            dr.Close();
            con.Close();

            DispatcherTimer LiveTime = new DispatcherTimer();
            LiveTime.Interval = TimeSpan.FromSeconds(1);
            LiveTime.Tick += timer_Tick;
            LiveTime.Start();
        }
        void timer_Tick(object sender, EventArgs e)
        {
            LiveTimeLabel.Content = DateTime.Now.ToString("HH:mm:ss");
        }
    }
}
