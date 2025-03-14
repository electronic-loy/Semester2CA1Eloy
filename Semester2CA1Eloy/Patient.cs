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
            O, A, AB, B
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

        // Constructor with name, full DateTime birthdate, and blood type
        public Patient(string name, DateTime dateOfBirth, BloodType bloodType)
        {
            Name = name;
            DateOfBirth = dateOfBirth;
            Type = bloodType;  
        }

        //Calculate to only show the age

        public int Age
        {
            get
            {
                DateTime today = DateTime.Today;
                int age = today.Year - DateOfBirth.Year;
                if (DateOfBirth > today.AddYears(-age)) age--; // Adjusts if birthday hasn't occurred yet (someone from 2023 can be 2 or 1 years old)
                return age;
            }
        }


        // Override ToString to display age instead of full birthdate
        public override string ToString()
        {
            return $"{Name}, Age: {Age}, Blood Type: {Type}";
        }


    }
}
