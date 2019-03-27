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
        NORTHWNDEntities db = new NORTHWNDEntities();
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
    }
}
