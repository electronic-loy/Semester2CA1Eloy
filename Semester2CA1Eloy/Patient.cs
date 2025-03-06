using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Semester2CA1Eloy.EnumBlood;

namespace Semester2CA1Eloy
{
    public class Patient
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public EnumBlood BloodType { get; set; }


        public Patient() { }
        public Patient(string name, DateTime dob, EnumBlood bloodType)
        {
            Name = name;
            DateOfBirth = dob;
            BloodType = bloodType;
        }
    }
}
