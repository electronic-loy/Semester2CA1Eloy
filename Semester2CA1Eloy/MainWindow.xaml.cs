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
using static Semester2CA1Eloy.Patient;

namespace Semester2CA1Eloy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Ward> wards = new ObservableCollection<Ward>();

        //private ObservableCollection<Patient> patients = new ObservableCollection<Patient>();

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
            ValidateInputs();
        }

        private void tbxWardName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnAddWard_Click(object sender, RoutedEventArgs e)
        {
            string wardName = tbxWardName.Text;
            int capacity = (int)sliderCapacity.Value;

            if (!string.IsNullOrWhiteSpace(wardName))
            {
                Ward newWard = new Ward(wardName, capacity);
                wards.Add(newWard);
                lbxWards.SelectedItem = newWard;
            }
        }
        private BloodType ConvertRadioButtonToBloodType(string bloodTypeText)
        {
            switch (bloodTypeText)
            {
                case "O-": return BloodType.ONeg;
                case "O+": return BloodType.OPos;
                case "A-": return BloodType.ANeg;
                case "A+": return BloodType.APos;
                case "B-": return BloodType.BNeg;
                case "B+": return BloodType.BPos;
                case "AB-": return BloodType.ABNeg;
                case "AB+": return BloodType.ABPos;
                default:
                    throw new ArgumentException("Invalid blood type selection.");
            }
        }


        private void btnAddPatient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get name from input
                string name = tbxPatientName.Text.Trim();
                if (string.IsNullOrWhiteSpace(name))
                {
                    MessageBox.Show("Please enter the patient's name.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Get Date of Birth from DatePicker
                if (!dpPatientDOB.SelectedDate.HasValue)
                {
                    MessageBox.Show("Please select a valid date of birth.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                DateTime dateOfBirth = dpPatientDOB.SelectedDate.Value;

                // Get selected BloodType from RadioButtons
                BloodType? selectedBloodType = null;
                foreach (StackPanel panel in new List<StackPanel> { bloodTypePanel1, bloodTypePanel2 }) 
                {
                    foreach (RadioButton rb in panel.Children)
                    {
                        if (rb.IsChecked == true)
                        {
                            selectedBloodType = ConvertRadioButtonToBloodType(rb.Content.ToString());
                            break;
                        }
                    }
                }

                if (!selectedBloodType.HasValue)
                {
                    MessageBox.Show("Please select a blood type.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Create new Patient object
                Patient newPatient = new Patient(name, dateOfBirth, selectedBloodType.Value);

                // Add the new patient directly to the ListBox
                currentWard.Patients.Add(newPatient);


                // Clear inputs
                tbxPatientName.Clear();
                dpPatientDOB.SelectedDate = null;
                foreach (StackPanel panel in new List<StackPanel> { bloodTypePanel1, bloodTypePanel2 })
                {
                    foreach (RadioButton rb in panel.Children)
                    {
                        rb.IsChecked = false;
                    }
                }

                btnAddPatient.IsEnabled = false; // Reset the button until valid input is entered
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dpPatientDOB_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ValidateInputs();
        }
        private void BloodType_Checked(object sender, RoutedEventArgs e)
        {
            ValidateInputs();
        }
        private void ValidateInputs()
        {
            bool isNameValid = !string.IsNullOrWhiteSpace(tbxPatientName.Text);
            bool isDOBValid = dpPatientDOB.SelectedDate.HasValue;
            bool isBloodTypeSelected = false;

            // Check if any RadioButton is selected
            foreach (StackPanel panel in new List<StackPanel> { bloodTypePanel1, bloodTypePanel2 })
            {
                foreach (RadioButton rb in panel.Children)
                {
                    if (rb.IsChecked == true)
                    {
                        isBloodTypeSelected = true;
                        break;
                    }
                }
            }

            // Enable the button only if all conditions are met
            btnAddPatient.IsEnabled = isNameValid && isDOBValid && isBloodTypeSelected;
        }
        //startup code
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Ward w1 = new Ward() { Name = "Stark", Capacity = 7 };
            w1.Patients.Add(new Patient("Jon", 1995, 6, 15, BloodType.ONeg));
            w1.Patients.Add(new Patient("Arya", 2000, 10, 11, BloodType.ABNeg));

            Ward w2 = new Ward() { Name = "Lannister", Capacity = 5 };
            w2.Patients.Add(new Patient("Tyrion", 1979, 12, 3, BloodType.APos));

            List<Ward> wards = new List<Ward>();

            wards.Add(w1);
            wards.Add(w2);

            lbxWards.ItemsSource = wards;

            // Select the first ward by default
            if (wards.Count > 0)
                lbxWards.SelectedIndex = 0;

          

        }
        Ward currentWard = null; 

        private void lbxWards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Ward selectedWard = (Ward)lbxWards.SelectedItem;

            if (selectedWard != null)
            {
                // Update the currentWard to the selected ward
                currentWard = selectedWard;

                // Update the patients ListBox to display patients of the currentWard
                lbxPatients.ItemsSource = currentWard.Patients;

                // Optionally, select the first patient by default if there are any
                if (currentWard.Patients.Count > 0)
                {
                    lbxPatients.SelectedIndex = 0;
                }
            }
        }


        private void lbxPatients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Patient selectedPatient = (Patient)lbxPatients.SelectedItem;
            if (selectedPatient != null)
            {
                tblkPatientName.Text = selectedPatient.Name;
                lbxPatients.SelectedIndex = 0;
            }

        }
    }
}

