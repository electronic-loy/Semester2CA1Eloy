using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semester2CA1Eloy
{
    public class Patient
    {
        public enum BloodType
        {
            // BloodType = Probability
            ONeg = 1,
            OPos = 2,
            ANeg = 3,
            APos = 4,
            BNeg = 5,
            BPos = 6,
            ABNeg = 7,
            ABPos = 8
        };

        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public BloodType Type { get; set; }

        // Constructor with name, birthdate, and blood type
        public Patient(string name, int year, int month, int day, BloodType bloodType)
        {
            Name = name;
            DateOfBirth = new DateTime(year, month, day);
            Type = bloodType;
        }

        public Patient()
        {
        }

        public Patient(string name, DateTime dateOfBirth, BloodType value)
        {
            Name = name;
            DateOfBirth = dateOfBirth;
            Value = value;
        }

        //Calculate to only show the age

        public int Age
        {
            get
            {
                DateTime today = DateTime.Today;
                int age = today.Year - DateOfBirth.Year;
                if (DateOfBirth > today.AddYears(-age)) age--; // Adjust if birthday hasn't occurred yet
                return age;
            }
        }

        public BloodType Value { get; }

        // Override ToString to display age instead of full birthdate
        public override string ToString()
        {
            return $"{Name}, Age: {Age}, Blood Type: {Type}";
        }


    }
}
