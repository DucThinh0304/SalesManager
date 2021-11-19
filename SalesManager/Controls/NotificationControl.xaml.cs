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

namespace SalesManager
{
    /// <summary>
    /// Interaction logic for NotificationControl.xaml
    /// </summary>
    public partial class NotificationControl : BaseControl
    {
        public NotificationControl()
        {
            InitializeComponent();

            var sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            sqlConn.Open();
            var sqlCommand = new SqlCommand("SELECT SUM(SOLUONG), TENHANG, A.MAHANG, DVT FROM NHAPHANG A, LOAIHANG B WHERE A.MAHANG=B.MAHANG  GROUP BY A.MAHANG, TENHANG, DVT", sqlConn);
            var reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                if (reader.GetInt32(0) < 10)
                {
                    var newItem = new Border();
                    newItem.Height = 100;
                    newItem.Width = 240;
                    newItem.Margin = new Thickness(0, 1, 0, 1);
                    var bc = new BrushConverter();
                    newItem.Background = (Brush)bc.ConvertFrom("#D8D03A");
                    newItem.BorderBrush = (Brush)bc.ConvertFrom("#C67634");
                    newItem.BorderThickness = new Thickness(1, 1, 1, 1);
                    newItem.CornerRadius = new CornerRadius(5);
                    StackPanel stack = new StackPanel();
                    newItem.Child = stack;
                    stack.Orientation = Orientation.Horizontal;
                    TextBlock textBlock = new TextBlock();
                    textBlock.Width = 100;
                    textBlock.Text = $"Mặt hàng {reader.GetString(1).ToLower()} sắp hết hàng!!!";
                    textBlock.TextWrapping = TextWrapping.Wrap;
                    textBlock.Foreground = (Brush)bc.ConvertFrom("#666217");
                    textBlock.FontSize = 20;
                    textBlock.Padding = new Thickness(3, 3, 0, 3);
                    textBlock.FontFamily = new FontFamily("Segoe UI");
                    Border border = new Border();
                    border.Width = 1;
                    border.Height = 90;
                    border.VerticalAlignment = VerticalAlignment.Center;
                    border.Background = Brushes.Brown;
                    TextBlock textBlock1 = new TextBlock();
                    textBlock1.Width = 120;
                    textBlock1.Text = $"Mã hàng: {reader.GetString(2)}\nSố lượng hàng còn lại: {reader.GetInt32(0)} {reader.GetString(3).ToLower()}";
                    textBlock1.TextWrapping = TextWrapping.Wrap;
                    textBlock1.Foreground = (Brush)bc.ConvertFrom("#666217");
                    textBlock1.FontSize = 15;
                    textBlock1.Padding = new Thickness(2, 3, 3, 3);
                    textBlock.FontFamily = new FontFamily("Segoe UI");
                    stack.Children.Add(textBlock);
                    stack.Children.Add(border);
                    stack.Children.Add(textBlock1);
                    this.mainPanel.Children.Add(newItem);
                }
            }
            reader.Close();
            sqlConn.Close();

            sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            sqlConn.Open();
            sqlCommand = new SqlCommand("SELECT * FROM NHAPHANG A, LOAIHANG B WHERE A.MAHANG = B.MAHANG", sqlConn);
            reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                if (DateTime.Compare(reader.GetDateTime(3),DateTime.Now) < 0) {
                    var newItem = new Border();
                    newItem.Height = 100;
                    newItem.Width = 240;
                    newItem.Margin = new Thickness(0, 1, 0, 1);
                    var bc = new BrushConverter();
                    newItem.Background = (Brush)bc.ConvertFrom("#D8633A");
                    newItem.BorderBrush = (Brush)bc.ConvertFrom("#7F3116");
                    newItem.BorderThickness = new Thickness(1, 1, 1, 1);
                    newItem.CornerRadius = new CornerRadius(5);
                    StackPanel stack = new StackPanel();
                    newItem.Child = stack;
                    stack.Orientation = Orientation.Horizontal;
                    TextBlock textBlock = new TextBlock();
                    textBlock.Width = 100;
                    textBlock.Text = $"Mặt hàng {reader.GetString(8).ToLower()} đã hết hạn!!!";
                    textBlock.TextWrapping = TextWrapping.Wrap;
                    textBlock.Foreground = (Brush)bc.ConvertFrom("#471A0A");
                    textBlock.FontSize = 20;
                    textBlock.Padding = new Thickness(3, 3, 0, 3);
                    textBlock.FontFamily = new FontFamily("Times New Roman");
                    Border border = new Border();
                    border.Width = 1;
                    border.Height = 90;
                    border.VerticalAlignment = VerticalAlignment.Center;
                    border.Background = Brushes.Brown;
                    TextBlock textBlock1 = new TextBlock();
                    textBlock1.Width = 120;
                    textBlock1.Text = $"Mã hàng: {reader.GetString(0)}\nMã lô: {reader.GetInt32(1)}\nHSD: {reader.GetDateTime(3).Day}/{reader.GetDateTime(3).Month}/{reader.GetDateTime(3).Year}\nSố lượng hàng: {reader.GetInt32(4)}";
                    textBlock1.TextWrapping = TextWrapping.Wrap;
                    textBlock1.Foreground = (Brush)bc.ConvertFrom("#471A0A");
                    textBlock1.FontSize = 15;
                    textBlock1.Padding = new Thickness(2, 3, 3, 3);
                    textBlock.FontFamily = new FontFamily("Segoe UI");
                    Button button = new Button();
                    button.Content = "x";
                    button.Padding = new Thickness(0);
                    button.Width = 20;
                    button.FontFamily = new FontFamily("Segoe UI");
                    button.FontSize = 15;
                    button.Background = Brushes.Transparent;
                    button.BorderBrush = Brushes.Transparent;
                    button.VerticalAlignment = VerticalAlignment.Top;
                    button.HorizontalAlignment = HorizontalAlignment.Right;
                    button.Foreground = (Brush)bc.ConvertFrom("#9E3510");
                    button.Tag = reader.GetString(0);
                    button.Click += Button_Click1;
                    stack.Children.Add(textBlock);
                    stack.Children.Add(border);
                    stack.Children.Add(textBlock1);
                    stack.Children.Add(button);
                    this.mainPanel.Children.Add(newItem);
                }
            }
            reader.Close();
            sqlConn.Close();

            sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            sqlConn.Open();
            sqlCommand = new SqlCommand("SELECT * FROM NHAPHANG A, LOAIHANG B WHERE A.MAHANG = B.MAHANG", sqlConn);
            reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                if (reader.GetDateTime(3).Date == DateTime.Now.AddDays(1).Date)
                {
                    var newItem = new Border();
                    newItem.Height = 100;
                    newItem.Width = 240;
                    newItem.Margin = new Thickness(0, 1, 0, 1);
                    var bc = new BrushConverter();
                    newItem.Background = (Brush)bc.ConvertFrom("#33CC33");
                    newItem.BorderBrush = (Brush)bc.ConvertFrom("#105410");
                    newItem.BorderThickness = new Thickness(1, 1, 1, 1);
                    newItem.CornerRadius = new CornerRadius(5);
                    StackPanel stack = new StackPanel();
                    newItem.Child = stack;
                    stack.Orientation = Orientation.Horizontal;
                    TextBlock textBlock = new TextBlock();
                    textBlock.Width = 100;
                    textBlock.Text = $"Mặt hàng {reader.GetString(8).ToLower()} sắp hết hạn!!!";
                    textBlock.TextWrapping = TextWrapping.Wrap;
                    textBlock.Foreground = (Brush)bc.ConvertFrom("#054405");
                    textBlock.FontSize = 20;
                    textBlock.Padding = new Thickness(3, 3, 0, 3);
                    textBlock.FontFamily = new FontFamily("Segoe UI");
                    Border border = new Border();
                    border.Width = 1;
                    border.Height = 90;
                    border.VerticalAlignment = VerticalAlignment.Center;
                    border.Background = Brushes.Brown;
                    TextBlock textBlock1 = new TextBlock();
                    textBlock1.Width = 120;
                    textBlock1.Text = $"Mã hàng: {reader.GetString(0)}\nMã lô: {reader.GetInt32(1)}\nHSD: {reader.GetDateTime(3).Day}/{reader.GetDateTime(3).Month}/{reader.GetDateTime(3).Year}\nSố lượng hàng: {reader.GetInt32(4)}";
                    textBlock1.TextWrapping = TextWrapping.Wrap;
                    textBlock1.Foreground = (Brush)bc.ConvertFrom("#054405");
                    textBlock1.FontSize = 15;
                    textBlock1.Padding = new Thickness(2, 3, 3, 3);
                    textBlock.FontFamily = new FontFamily("Segoe UI");
                    stack.Children.Add(textBlock);
                    stack.Children.Add(border);
                    stack.Children.Add(textBlock1);
                    this.mainPanel.Children.Add(newItem);
                }
            }
            reader.Close();
            sqlConn.Close();
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            string str = ((Button)sender).Tag.ToString();
            
            var sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            sqlConn.Open();
            var sqlCommand = new SqlCommand($"DELETE FROM NHAPHANG WHERE MAHANG = {str} AND HANSD < GETDATE()", sqlConn);
            var reader = sqlCommand.ExecuteReader();
            MessageBox.Show("Xóa thành công");
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).SideMenu = ApplicationPage.SideMenuControl;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((WindowViewModel)((MainWindow)Application.Current.MainWindow).DataContext).SideMenu = ApplicationPage.SideMenuControl;
        }

    }
}
