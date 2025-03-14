using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    
    public partial class MainWindow : Window
    {
        private ObservableCollection<Ward> wards = new ObservableCollection<Ward>();

        //MainWindow will allow for the slider to return to like the start after adding new wards
        public MainWindow()
        {
            InitializeComponent();
            tbxWardName.TextChanged += tbxWardName_TextChanged;
            sliderCapacity.ValueChanged += sliderCapacity_ValueChanged; 
            UpdateAddWardButtonState(); 
        }
        //The following is to make it able for the ADD WARD and ADD PATIENT buttons to work excellently
        private void tbxWardName_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAddWardButtonState();
        }

        private void sliderCapacity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            textBlock2.Text = string.Format("{0:F0}", sliderCapacity.Value);
            UpdateAddWardButtonState(); 
        }

        private void UpdateAddWardButtonState()
        {
            btnAddWard.IsEnabled = !string.IsNullOrWhiteSpace(tbxWardName.Text) && sliderCapacity.Value > 0;
        }

       

        //save data
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string filePath = @"c:\temp\wards.json";
                string json = JsonConvert.SerializeObject(wards, Formatting.Indented);
                File.WriteAllText(filePath, json);
                //Message to acknowledge that it was saved
                MessageBox.Show("Wards and Patients saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //load data
        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string filePath = @"c:\temp\wards.json";

                // Check json file
                if (!File.Exists(filePath))
                {
                    MessageBox.Show("File not found!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Read json file
                string json = File.ReadAllText(filePath);
                var loadedWards = JsonConvert.DeserializeObject<ObservableCollection<Ward>>(json);

                if (loadedWards != null)
                {
                    wards.Clear();
                    foreach (var ward in loadedWards)
                    {
                        wards.Add(ward);
                    }
                }

                lbxWards.ItemsSource = wards;

               
                if (wards.Count > 0)
                    lbxWards.SelectedIndex = 0;

                MessageBox.Show("Wards and Patients loaded successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

       

        //startup code
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Basic instructions set in as wards

            Ward w1 = new Ward() { Name = "First time? Create a ward, save it, and load.", Capacity = 7 };
            w1.Patients.Add(new Patient("Lore", 1995, 6, 15, BloodType.O));
            w1.Patients.Add(new Patient("Ipsum", 2000, 10, 11, BloodType.AB));

            Ward w2 = new Ward() { Name = "Second or + time? Load your json!", Capacity = 5 };
            w2.Patients.Add(new Patient("Lore", 1979, 12, 3, BloodType.A));

            // Use the ObservableCollection<Ward> field instead of creating a new List<Ward>
            lbxWards.ItemsSource = wards;
            wards.Add(w1);
            wards.Add(w2);

            // Select the first ward by default
            if (wards.Count > 0)
                lbxWards.SelectedIndex = 0;
        }
        Ward currentWard = null;
        private void tbxPatientName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ValidateInputs();
        }

        //AddWard Button

        private void btnAddWard_Click(object sender, RoutedEventArgs e)
        {
            string wardName = tbxWardName.Text.Trim();
            int capacity = (int)sliderCapacity.Value;

            if (string.IsNullOrWhiteSpace(wardName) || capacity == 0)
            {
                MessageBox.Show("Please enter a ward name and a capacity greater than 0.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                Ward newWard = new Ward(wardName, capacity);
                wards.Add(newWard);
                lbxWards.SelectedItem = newWard;

                tbxWardName.Clear();
                sliderCapacity.Value = 1;

                UpdateAddWardButtonState(); // Update button state
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        //Info to convert the bloodtype before clicking the AddPatient button
        private Patient.BloodType ConvertRadioButtonToBloodType(string bloodTypeText)
        {
            switch (bloodTypeText)
            {
                case "O": return BloodType.O;
                case "A": return BloodType.A;
                case "B": return BloodType.B;
                case "AB": return BloodType.AB;
                default:
                    throw new ArgumentException("Invalid blood type selection.");
            }
        }

        //AddPatient Button

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
                Patient.BloodType? selectedBloodType = null;
                foreach (StackPanel panel in new List<StackPanel> { bloodTypePanel1 })
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

                // Check if the ward is at full capacity BEFORE adding the patient

                if (currentWard.Patients.Count >= currentWard.Capacity)
                {
                    MessageBox.Show($"Ward '{currentWard.Name}' is at full capacity ({currentWard.Capacity} patients).", "Capacity Reached", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return; // Prevent adding more patients, i.e. no more patients if capacity is full
                }

                // Create new Patient object
                Patient newPatient = new Patient(name, dateOfBirth, selectedBloodType.Value);

                // Add the new patient directly to the currentWard
                currentWard.Patients.Add(newPatient);

                // Clear inputs
                tbxPatientName.Clear();
                dpPatientDOB.SelectedDate = null;
                foreach (StackPanel panel in new List<StackPanel> { bloodTypePanel1 })
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

        //Validating if a Patient can be added or not

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
            foreach (StackPanel panel in new List<StackPanel> { bloodTypePanel1 })
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
        //Refreshing the UI with the selected ward

        private void lbxWards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Ward selectedWard = (Ward)lbxWards.SelectedItem;

            if (selectedWard != null)
            {
                // Update the currentWard to the selected ward
                currentWard = selectedWard;

                // Update the Patients ListBox to display Patients of the currentWard
                lbxPatients.ItemsSource = currentWard.Patients;

                // Optionally, select the first patient by default if there are any
                if (currentWard.Patients.Count > 0)
                {
                    lbxPatients.SelectedIndex = 0;
                }
            }
        }

        //Refreshing the UI again, now with Patient

        private void lbxPatients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // Get the selected patient
                var selectedPatient = lbxPatients.SelectedItem as Patient;
                if (selectedPatient != null)
                {
                    // Set the patient name to the info area
                    tblkPatientName.Text = selectedPatient.Name;
                    // Get the blood type image file name dynamically based on selected patient's blood type
                    string imagePath = GetBloodTypeImageFile(selectedPatient.Type);

                    // Construct the full path to the image in the images folder
                    string fullImagePath = System.IO.Path.Combine(Environment.CurrentDirectory, "images", imagePath);

                    // Map the blood type to the corresponding image
                    string bloodTypeImageFile = GetBloodTypeImageFile(selectedPatient.Type);

                    // Set the image source
                    patientImage.Source = new BitmapImage(new Uri(System.IO.Path.Combine(Environment.CurrentDirectory, "images", imagePath)));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading patient details: {ex.Message}");
            }
        }

        //Show the image that corresponds with the bloodtype

        private string GetBloodTypeImageFile(Patient.BloodType bloodType)
        {
            // Map blood types to their respective image filenames
            switch (bloodType)
            {
                case Patient.BloodType.A:
                    return "a.png";
                case Patient.BloodType.B:
                    return "b.png";
                case Patient.BloodType.AB:
                    return "ab.png";
                case Patient.BloodType.O:
                    return "0.png";
                default:
                    return "default.png"; // In case something goes wrong
            }
        }

        //Done!


    }
}

