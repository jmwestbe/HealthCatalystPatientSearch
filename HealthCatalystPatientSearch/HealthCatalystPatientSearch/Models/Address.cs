using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HealthCatalystPatientSearch.Models
{
    public class Address
    {
        public int Id { get; set; }

        [Required]
        public string StreetLine1 { get; set; }
        public string StreetLine2 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string Country { get; set; }

        [NotMapped]
        public string AsString => ToString();

        public override string ToString()
        {
            return
                $"{StreetLine1} {StreetLine2}\n{City}, {State} {PostalCode}\n{Country}";
        }
    }
}