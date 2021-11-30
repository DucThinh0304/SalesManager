﻿using System;
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using LiveCharts;
using LiveCharts.Wpf;

namespace SalesManager.Pages
{
    /// <summary>
    /// Interaction logic for DoanhThuNhanVien.xaml
    /// </summary>
    public partial class DoanhThuNhanVien : BasePage, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
        public DoanhThuNhanVien()
        {
            InitializeComponent();
            ColorConverter brush = new ColorConverter();
            RadialGradientBrush radialGradientBrush = new RadialGradientBrush();
            radialGradientBrush.GradientStops.Add(new GradientStop((Color)brush.ConvertFrom("#99ddff"), 0.0));
            radialGradientBrush.GradientStops.Add(new GradientStop(Colors.Transparent, 1));
            Title.Background = radialGradientBrush;
            DataContext = this;
            loadDTNV();
            loadtitle();
        }
        private void loadtitle()
        {
            var sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            sqlConn.Open();
            var sqlCommand = new SqlCommand("SELECT HOTEN FROM NHANVIEN WHERE MANV='" + manv + "'", sqlConn);
            var reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                tb_Title.Text = "DOANH THU NHÂN VIÊN " + reader.GetString(0).ToUpper();
            }
            reader.Close();
            sqlConn.Close();
            tb_Title1.Text = " THÁNG " + thang + " NĂM " + nam;
        }
        private void loadDTNV()
        {
            double S = 0, S1 = 0, S2 = 0, S3 = 0, S4 = 0, S5 = 0, S6 = 0, S7 = 0, S8 = 0, S9 = 0, S10 = 0, S11 = 0, S12 = 0, S13 = 0, S14 = 0, S15 = 0, S16 = 0, S17 = 0, S18 = 0, S19 = 0, S20 = 0, S21 = 0, S22 = 0, S23 = 0, S24 = 0, S25 = 0, S26 = 0, S27 = 0, S28 = 0, S29 = 0, S30 = 0, S31 = 0;
            string ngay = "";
            S = TongDT(S);
            tb_tongDT.Text = S.ToString() + " VND";
            S1 = TongDTngay(S1, ngay = "1");
            S2 = TongDTngay(S2, ngay = "2");
            S3 = TongDTngay(S3, ngay = "3");
            S4 = TongDTngay(S4, ngay = "4");
            S5 = TongDTngay(S5, ngay = "5");
            S6 = TongDTngay(S6, ngay = "6");
            S7 = TongDTngay(S7, ngay = "7");
            S8 = TongDTngay(S8, ngay = "8");
            S9 = TongDTngay(S9, ngay = "9");
            S10 = TongDTngay(S10, ngay = "10");
            S11 = TongDTngay(S11, ngay = "11");
            S12 = TongDTngay(S12, ngay = "12");
            S13 = TongDTngay(S13, ngay = "13");
            S14 = TongDTngay(S14, ngay = "14");
            S15 = TongDTngay(S15, ngay = "15");
            S16 = TongDTngay(S16, ngay = "16");
            S17 = TongDTngay(S17, ngay = "17");
            S18 = TongDTngay(S18, ngay = "18");
            S19 = TongDTngay(S19, ngay = "19");
            S20 = TongDTngay(S20, ngay = "20");
            S21 = TongDTngay(S21, ngay = "21");
            S22 = TongDTngay(S22, ngay = "22");
            S23 = TongDTngay(S23, ngay = "23");
            S24 = TongDTngay(S24, ngay = "24");
            S25 = TongDTngay(S25, ngay = "25");
            S26 = TongDTngay(S26, ngay = "26");
            S27 = TongDTngay(S27, ngay = "27");
            S28 = TongDTngay(S28, ngay = "28");
            S29 = TongDTngay(S29, ngay = "29");
            S30 = TongDTngay(S30, ngay = "30");
            S31 = TongDTngay(S31, ngay = "31");
            SeriesCollection = new SeriesCollection()
            {
                new ColumnSeries
                {
                    Title = "Doanh thu",
                    Values = new ChartValues<double> {  S1, S2, S3, S4, S5, S6, S7, S8, S9, S10, S11, S12,S13,S14,S15,S16,S17,S18,S19,S20,S21,S22,S23,S24,S25,S26,S27,S28,S29,S30,S31 }
                }
            };
            Labels = new[] { "Ngày 1", "Ngày 2", "Ngày 3", "Ngày 4", "Ngày 5", "Ngày 6", "Ngày 7", "Ngày 8", "Ngày 9", "Ngày 10", "Ngày 11", "Ngày 12", "Ngày 13", "Ngày 14", "Ngày 15", "Ngày 16", "Ngày 17", "Ngày 18", "Ngày 19", "Ngày20", "Ngày 21", "Ngày 22", "Ngày 23", "Ngày 24", "Ngày 25", "Ngày 26", "Ngày 27", "Ngày 28", "Ngày 29", "Ngày 30", "Ngày 31" };
            //Formatter = value => value.ToString("N");
        }

        private double TongDTngay(double S, string ngay)
        {
            var sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            sqlConn.Open();
            var sqlCommand = new SqlCommand("SELECT TRIGIA FROM HOADON WHERE YEAR(NGHOADON)=" + nam + "AND MONTH(NGHOADON)=" + thang + " AND DAY(NGHOADON)=" + ngay +"AND MANV='"+manv+"'", sqlConn);
            var reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                S += double.Parse(reader[0].ToString());
            }
            reader.Close();
            sqlConn.Close();
            return S;
        }
        double TongDT(double S)
        {
            var sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            sqlConn.Open();
            var sqlCommand = new SqlCommand("SELECT TRIGIA FROM HOADON WHERE YEAR(NGHOADON)=2021", sqlConn);
            var reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                S += double.Parse(reader[0].ToString());
            }
            reader.Close();
            sqlConn.Close();
            return S;
        }
        private void thoat_Click(object sender, RoutedEventArgs e)
        {
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).CurrentPage = ApplicationPage.ThongTinNhanVien;
        }
    }
}
