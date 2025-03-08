using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

namespace Semester2CA1Eloy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Ward> wards = new ObservableCollection<Ward>();
       
        public MainWindow()
        {
            InitializeComponent();

        }



        private void sliderCapacity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            textBlock2.Text = string.Format("{0:F0}", sliderCapacity.Value);
        }

        //save data
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //string json = JsonConvert.SerializeObject(a, Formatting.Indented);

            using (StreamWriter sw = new StreamWriter(@"c:\temp\Semester2CA1Eloy.json"))
            {
                //sw.Write(json);

            }
        }

        private void tbxPatientName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void tbxWardName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnAddWard_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddPatient_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
