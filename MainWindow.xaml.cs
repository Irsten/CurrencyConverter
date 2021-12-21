using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
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
using Newtonsoft.Json;

namespace CurrencyConverter
{
    public partial class MainWindow : Window
    {
        Root val = new Root();
        public MainWindow()
        {
            InitializeComponent();
            GetValue();
        }
        private async void GetValue()
        {
            val = await GetData<Root>("https://openexchangerates.org/api/latest.json?app_id=f33fe06dc28547858c1dc801cd059d54");
            BindCurrency();
        }
        public static async Task<Root> GetData<T>(string url)
        {
            var myRoot = new Root();
            try
            {
                // HttpClient class provides a base class for sending/recieving the HTTP request/responses for URL
                using (var client = new HttpClient())
                {
                    // The timespan to wait before the request times out    
                    client.Timeout = TimeSpan.FromMinutes(1);
                    // HttpResponseMessage is a way of returning a message/date from your action
                    HttpResponseMessage response = await client.GetAsync(url);
                    // Check API response code ok 
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        // Serialize the HTTP content to a string as an asynchronous operation
                        var ResponceString = await response.Content.ReadAsStringAsync();
                        // JsonConvert.DeserializeObject to deserialize Json to a C#
                        var ResponceObject = JsonConvert.DeserializeObject<Root>(ResponceString);

                        // Return API responce
                        return ResponceObject;
                    }
                    return myRoot;
                }
            }
            catch
            {
                return myRoot;
            }
        }
        private void BindCurrency()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Currency");
            dt.Columns.Add("Value");

            // Add rows in the Datatable
            dt.Rows.Add("SELECT CURRENCY", 0);
            dt.Rows.Add("PLN", val.rates.PLN);
            dt.Rows.Add("USD", val.rates.USD);
            dt.Rows.Add("EUR", val.rates.EUR);
            dt.Rows.Add("GBP", val.rates.GBP);
            dt.Rows.Add("JPY", val.rates.JPY);
            dt.Rows.Add("CHF", val.rates.CHF);
            dt.Rows.Add("INR", val.rates.INR);
            dt.Rows.Add("NZD", val.rates.NZD);
            dt.Rows.Add("ISK", val.rates.ISK);

            // DataTable data asigned from the From combobox
            cmbFrom.ItemsSource = dt.DefaultView;
            cmbFrom.DisplayMemberPath = "Currency";
            cmbFrom.SelectedValuePath = "Value";
            cmbFrom.SelectedIndex = 0;

            // DataTable data asigned from the To combobox
            cmbTo.ItemsSource = dt.DefaultView;
            cmbTo.DisplayMemberPath = "Currency";
            cmbTo.SelectedValuePath = "Value";
            cmbTo.SelectedIndex = 0;
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            // Amount textbox accept only numbers
            Regex regex = new Regex(@"[^0-9,]");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            double ConvertedValue;

            // If Amount textbox is null or blank it will show messagebox and set focus on Amount textbox
            if (txtAmount.Text == null || txtAmount.Text.Trim() == "")
            {
                MessageBox.Show("Please enter amount of currency you want to convert",
                    "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                txtAmount.Focus();
                return;
            }
            // Else if combobox From or To select default value
            else if (cmbFrom.SelectedIndex == 0 || cmbTo.SelectedIndex == 0)
            {
                MessageBox.Show("Please select currencies", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                // If From combobox or From and To comboboxes select default values, set focus on combobox From
                if (cmbFrom.SelectedIndex == 0)
                {
                    cmbFrom.Focus();
                    return;
                }
                // Else if only To combobox select default value, set focus on combobox To
                else if (cmbFrom.SelectedIndex != 0 && cmbTo.SelectedIndex == 0)
                {
                    cmbTo.Focus();
                    return;
                }
            }
            // Check if From and To comboboxes selected values are the same
            if (cmbFrom.SelectedIndex == cmbTo.SelectedIndex)
            {
                // ConvertedValue set as Amount textbox value
                ConvertedValue = double.Parse(txtAmount.Text);
                lblConvertedCurrency.Content = txtAmount.Text + " " + cmbFrom.Text + " = " + ConvertedValue.ToString("N2") + " " + cmbTo.Text;
            }
            else
            {
                double from = double.Parse(cmbTo.SelectedValue.ToString());
                double to = double.Parse(cmbFrom.SelectedValue.ToString());
                double amount = double.Parse(txtAmount.Text);
                ConvertedValue = from * amount / to;

                lblConvertedCurrency.Content = txtAmount.Text + " " + cmbFrom.Text + " = " + ConvertedValue.ToString("N2") + " " + cmbTo.Text;
            }
        }
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            txtAmount.Text = "";
            cmbFrom.SelectedIndex = 0;
            cmbTo.SelectedIndex = 0;
            lblConvertedCurrency.Content = "";
        }

        private void Course_Click(object sender, RoutedEventArgs e)
        {
            // Parse values of From and To comboboxes
            double from = double.Parse(cmbTo.SelectedValue.ToString());
            double to = double.Parse(cmbFrom.SelectedValue.ToString());
            double Course = from / to;

            // Set course in Course label
            lblConvertedCurrency.Content = Course.ToString("N3");
        }
    }
}
