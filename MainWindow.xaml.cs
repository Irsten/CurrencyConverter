using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

namespace CurrencyConverter
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BindCurrency();
        }

        private void BindCurrency()
        {
            DataTable dtCurrency= new DataTable();
            dtCurrency.Columns.Add("Currency");
            dtCurrency.Columns.Add("Value");

            // Add rows in the Datatable
            dtCurrency.Rows.Add("SELECT CURRENCY", 0);
            dtCurrency.Rows.Add("PLN", 1);
            dtCurrency.Rows.Add("EUR", 4.6);
            dtCurrency.Rows.Add("DOL", 4.08);
            dtCurrency.Rows.Add("GBP", 5.38);
            dtCurrency.Rows.Add("CHF", 4.4);
            dtCurrency.Rows.Add("JPY", 0.036);

            cmbFrom.ItemsSource = dtCurrency.DefaultView;
            cmbFrom.DisplayMemberPath = "Currency";
            cmbFrom.SelectedValuePath = "Value";
            cmbFrom.SelectedIndex = 0;

            cmbTo.ItemsSource = dtCurrency.DefaultView;
            cmbTo.DisplayMemberPath = "Currency";
            cmbTo.SelectedValuePath = "Value";
            cmbTo.SelectedIndex = 0;
        }

        private void CourseCheck()
        {
            // Parse values of From and To comboboxes
            double from = double.Parse(cmbFrom.SelectedValue.ToString());
            double to = double.Parse(cmbTo.SelectedValue.ToString());
            double Course = from / to;

            // Set course in Course label
            lblCourse.Content = Course.ToString("N2");
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
                double from = double.Parse(cmbFrom.SelectedValue.ToString());
                double to = double.Parse(cmbTo.SelectedValue.ToString());
                double amount = double.Parse(txtAmount.Text);
                ConvertedValue = from * amount / to;

                lblConvertedCurrency.Content = txtAmount.Text + " " + cmbFrom.Text + " = " + ConvertedValue.ToString("N2") + " " + cmbTo.Text;
            }

            CourseCheck();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            txtAmount.Text = "";
            cmbFrom.SelectedIndex = 0;
            cmbTo.SelectedIndex = 0;
            lblCourse.Content = "";
            lblConvertedCurrency.Content = "";
        }
    }
}
