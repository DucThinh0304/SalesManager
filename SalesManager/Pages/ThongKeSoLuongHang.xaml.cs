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

namespace SalesManager
{
    /// <summary>
    /// Interaction logic for ThayDoiThongTinMatHang.xaml
    /// </summary>
    public partial class ThongKeSoLuongHang : BasePage
    {
        public ThongKeSoLuongHang()
        {
            InitializeComponent();
            RadialGradientBrush radialGradientBrush = new RadialGradientBrush();
            radialGradientBrush.GradientStops.Add(new GradientStop(Colors.Cyan, 0.0));
            radialGradientBrush.GradientStops.Add(new GradientStop(Colors.White, 1));
            Title.Background = radialGradientBrush;
            Sum_Border.Background = radialGradientBrush;
            SapXep_ComboBox.SelectedIndex = 0;
            SapXepTheo_ComboBox.SelectedIndex = 0;
            SapXep_ComboBox.SelectionChanged += SapXep_ComboBox_SelectionChanged;
            SapXepTheo_ComboBox.SelectionChanged += SapXep_ComboBox_SelectionChanged;
            TimKiem_TextBox.TextChanged += TimKiem_TextBox_TextChanged;
            LoadList();
        }

        private void SapXep_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvUsers.ItemsSource);
            view.SortDescriptions.Clear();
            if (SapXepTheo_ComboBox.SelectedIndex == 0)
            {
                switch (SapXep_ComboBox.SelectedIndex)
                {
                    case 0:
                        view.SortDescriptions.Add(new SortDescription("TENHANG", ListSortDirection.Ascending));
                        break;
                    case 1:
                        view.SortDescriptions.Add(new SortDescription("GIA", ListSortDirection.Ascending));
                        break;
                    case 2:
                        view.SortDescriptions.Add(new SortDescription("HSD", ListSortDirection.Ascending));
                        break;
                    case 3:
                        view.SortDescriptions.Add(new SortDescription("SOLUONG", ListSortDirection.Ascending));
                        break;
                    case 4:
                        view.SortDescriptions.Add(new SortDescription("TONGGIA", ListSortDirection.Ascending));
                        break;
                    default:
                        return;
                }
            }
            else
            {
                switch (SapXep_ComboBox.SelectedIndex)
                {
                    case 0:
                        view.SortDescriptions.Add(new SortDescription("TENHANG", ListSortDirection.Descending));
                        break;
                    case 1:
                        view.SortDescriptions.Add(new SortDescription("GIA", ListSortDirection.Descending));
                        break;
                    case 2:
                        view.SortDescriptions.Add(new SortDescription("HSD", ListSortDirection.Descending));
                        break;
                    case 3:
                        view.SortDescriptions.Add(new SortDescription("SOLUONG", ListSortDirection.Descending));
                        break;
                    case 4:
                        view.SortDescriptions.Add(new SortDescription("TONGGIA", ListSortDirection.Descending));
                        break;
                    default:
                        return;
                }
            }
            CollectionViewSource.GetDefaultView(lvUsers.ItemsSource).Refresh();
        }

        public class LoaiHangHoa
        {
            public string MAHANG { get; set; }
            public int MALO { get; set; }
            public string TENHANG { get; set; }
            public int SOLUONG { get; set; }
            public string HSD { get; set; }
            public int GIA { get; set; }
            public int TONGGIA { get; set; }
        }
        void LoadList()
        {
            int Sum = 0;
            SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            sqlConn.Open();
            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM LOAIHANG A, NHAPHANG B WHERE A.MAHANG = B.MAHANG AND SOLUONG != 0 ", sqlConn);
            SqlDataReader reader = sqlCommand.ExecuteReader();
            List<LoaiHangHoa> items = new List<LoaiHangHoa>();
            while (reader.Read())
            {
                items.Add(new LoaiHangHoa()
                {
                    MAHANG = reader.GetString(0),
                    TENHANG = reader.GetString(1),
                    MALO = reader.GetInt32(4),
                    SOLUONG = reader.GetInt32(7),
                    GIA = Convert.ToInt32(reader.GetDecimal(8)),
                    HSD = $"{reader.GetDateTime(6).Day}/{reader.GetDateTime(6).Month}/{reader.GetDateTime(6).Year}",
                    TONGGIA = Convert.ToInt32(reader.GetDecimal(8)) * reader.GetInt32(7)
                });
                lvUsers.ItemsSource = items;
            }
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvUsers.ItemsSource);
            view.SortDescriptions.Add(new SortDescription("TENHANG", ListSortDirection.Ascending));
            view.Filter = UserFilter;
            foreach (LoaiHangHoa item in items)
            {
                Sum += item.TONGGIA;
            }
            TongTien_TextBlock.Text = "Tổng tiền: " + Sum.ToString() + " VND";
            reader.Close();
            sqlConn.Close();
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(TimKiem_TextBox.Text))
                return true;
            else
                return ((item as LoaiHangHoa).TENHANG.IndexOf(TimKiem_TextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }
        private void TimKiem_TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(lvUsers.ItemsSource).Refresh();
        }
        private void SelectCurrentItem(object sender, KeyboardFocusChangedEventArgs e)
        {
            ListViewItem item = (ListViewItem)sender;
            item.IsSelected = true;
        }

    }
}
