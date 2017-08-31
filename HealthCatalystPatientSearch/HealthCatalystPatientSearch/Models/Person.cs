using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HealthCatalystPatientSearch.Models
{
    public class Person
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Interests { get; set; }

        [Required]
        public Address Address { get; set; }

        public byte[] Image { get; set; }

        [NotMapped]
        public int Age => GetAge();

        public override string ToString()
        {
            return
                $"Person\n\tName: {FirstName} {LastName}\n\tAge: {GetAge()}\n\tInterests: {Interests}\n\tAddress: {Address.ToString()}";
        }

        public int GetAge()
        {
            DateTime now = DateTime.Now;

            int yearDiff = now.Year - DateOfBirth.Year;

            if (BeforeBirthday(now))
            {
                return yearDiff - 1;
            }

            return yearDiff;
        }

        private bool BeforeBirthday(DateTime now)
        {
            if (now.Month < DateOfBirth.Month)
            {
                return true;
            }

            if (now.Month == DateOfBirth.Month)
            {
                return now.Day < DateOfBirth.Day;
            }

            return false;
        }
    }
}