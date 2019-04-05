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
using Entity;
using Popups;

namespace AvG___Abgabe_1
{
    public static class global
    {
        // Declaration of global variables
        public static List<Product> PList = new List<Product>();
        public static List<Supplier> SList = new List<Supplier>();

    }

    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Click event for "Load Databank" button. xxxxxxxx
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadDB_Click(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// Click event for "Save Databank" button. xxxxxxxx
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveDB_Click(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// Click event for "Refresh" button. xxxxxxxx
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            SGrid.ItemsSource = null;
            PGrid.ItemsSource = null;
            SGrid.ItemsSource = global.SList;
            PGrid.ItemsSource = global.PList;

        }

        /// <summary>
        /// Click event for the exit button. Closes the window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MWin.Close();
        }
    }
}
