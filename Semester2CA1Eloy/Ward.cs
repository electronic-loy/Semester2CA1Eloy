using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester2CA1Eloy
{
    public class Ward
    {
        // Variable to count for wards, this helps to update the Ward List (x)
        public static int WardCount { get; private set; }
        public string Name { get; set; }
        public int Capacity { get; set; }

        // Patients, using ObservableCollection to update like seen in previous Labs
        public ObservableCollection<Patient> Patients { get; set; }

        // Constructors
        public Ward()
        {
            Patients = new ObservableCollection<Patient>();
            WardCount++;
        }
        public Ward(string name, int capacity) : this()
        {
            Name = name;
            Capacity = capacity;
        }

        public override string ToString()
        {
            // Name is the "family" like the Marx 
            return $"{Name} (Limit:{Capacity})";
        }
    }
}
