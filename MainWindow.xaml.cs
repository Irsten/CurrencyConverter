using System;
using System.Collections.Generic;
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
