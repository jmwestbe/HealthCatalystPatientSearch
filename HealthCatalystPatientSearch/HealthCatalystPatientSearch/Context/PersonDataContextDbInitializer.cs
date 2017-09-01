using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using HealthCatalystPatientSearch.Models;
using Newtonsoft.Json;

namespace HealthCatalystPatientSearch.Context
{
    public class PersonDataContextDbInitializer : DropCreateDatabaseAlways<PersonContext>
    {
        private const string FirstStreetLine1 = "123 S";
        private const string SecondStreetLine1 = "Apt 1";
        private const string City1 = "Anytown";
        private const string State1 = "Utah";
        private const string Zip1 = "88888";
        private const string Country1 = "USA";
        private static readonly DateTime DateOfBirth1 = new DateTime(1998, 04, 13);
        private const string FirstName1 = "XXXJohn";
        private const string LastName1 = "XXXDoe";
        private static readonly byte[] Image1 = new byte[] { 0 };
        private const string Interests1 = "archaeology, anthropology, apologists";


        protected override void Seed(PersonContext context)
        {
            System.Diagnostics.Debug.WriteLine("Begin Seeding Data...");

            if (HttpContext.Current != null) //only seed if httpcontext.current is available
            {
                var jsonPath = HttpContext.Current.Server.MapPath(@"~/App_Data/seed-data.json");
                var jsonData = System.IO.File.ReadAllText(jsonPath);

                List<Person> persons = JsonConvert.DeserializeObject<List<Person>>(
                    jsonData, new JsonSerializerSettings());

                context.Persons.AddRange(persons);
                context.SaveChanges();
            }
            System.Diagnostics.Debug.WriteLine("Finished Seeding Data...");
        }
    }
}