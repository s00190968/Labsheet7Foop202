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

namespace Exercise2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AdventureLiteEntities ADVENTURE = new AdventureLiteEntities();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var q = ADVENTURE.SalesOrderHeaders.
                OrderBy(a => a.Customer.CompanyName).
                Select(a => a.Customer.CompanyName);

            customersLbx.ItemsSource = q.ToList().Distinct();
        }

        private void CustomersLbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string customer = customersLbx.SelectedValue as string;

            if (customer != null)
            {
                var q = ADVENTURE.SalesOrderHeaders.
                    Where(a => a.Customer.CompanyName.Equals(customer)).
                    Select(a => new OrderSummary
                    {
                        SalesOrderID = a.SalesOrderID,
                        OrderDate = a.OrderDate,
                        TotalDue = a.TotalDue
                    });

                orderSummaryLbx.ItemsSource = q.ToList();
            }
        }

        private void OrderSummaryLbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int orderId = Convert.ToInt32(orderSummaryLbx.SelectedValue);

            if(orderId > 0)
            {
                var q = ADVENTURE.SalesOrderDetails.
                    Where(a => a.SalesOrderID == orderId).Select(
                    a => new
                    {
                        ProductName = a.Product.Name,
                        a.UnitPrice,
                        a.UnitPriceDiscount,
                        a.OrderQty,
                        a.LineTotal
                    });
                OrderDetailLbx.ItemsSource = q.ToList();
            }
        }

    }
    public class OrderSummary : SalesOrderHeader
    {
        public override string ToString()
        {
            return string.Format(
                "{0}:{1} {2:C}",
                OrderDate.ToShortDateString(),
                SalesOrderID,
                TotalDue
                );
        }
    }
}
