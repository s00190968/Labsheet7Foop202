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

namespace Labsheet7
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public enum StockLevel { Low, Normal, Overstocked };
    public partial class MainWindow : Window
    {
        NORTHWNDEntities1 db = new NORTHWNDEntities1();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //stocke level lbx
            stockLevelsLbx.ItemsSource = Enum.GetNames(typeof(StockLevel));

            //suppliers listbox
            var query1 = db.Suppliers.OrderBy(s => s.CompanyName)
                .Select(s => new
                {
                    SupplierName = s.CompanyName,
                    SupplierID = s.SupplierID,
                    Country = s.Country
                });

            suppliersLbx.ItemsSource = query1.ToList();

            //countries lbx
            var query2 = query1.OrderBy(s => s.Country).Select(s => s.Country);
            var countries = query2.ToList();

            countriesLbx.ItemsSource = countries.Distinct();
        }

        private void stockLevelsLbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var query = db.Products.Where(p => p.UnitsInStock < 50).OrderBy(p => p.ProductName).Select(p => p.ProductName);
            string selected = stockLevelsLbx.SelectedItem as string;

            switch (selected)
            {
                case "Low":
                    //do nothing here!
                    break;
                case "Normal":
                    query = db.Products.Where(p => p.UnitsInStock >= 50 && p.UnitsInStock <= 100)
                        .OrderBy(p => p.ProductName).Select(p => p.ProductName);
                    break;
                case "Overstocked":
                    query = db.Products.Where(p => p.UnitsInStock > 100)
                        .OrderBy(p => p.ProductName).Select(p => p.ProductName);
                    break;
            }

            //update products list
            ProductsLbx.ItemsSource = query.ToList();
        }

        private void suppliersLbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int supplierID = Convert.ToInt32(suppliersLbx.SelectedValue);

            var q = db.Products
                .Where(p => p.SupplierID == supplierID)
                .OrderBy(p => p.ProductName)
                .Select(p => p.ProductName);

            var results = q.ToList();

            ProductsLbx.ItemsSource = q.ToList();
        }

        private void countriesLbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string country = (string)(countriesLbx.SelectedValue);
            var q = db.Products.Where(p => p.Supplier.Country.Equals(country))
                .OrderBy(p => p.ProductName).Select(p => p.ProductName);

            ProductsLbx.ItemsSource = q.ToList();
        }
    }
}
