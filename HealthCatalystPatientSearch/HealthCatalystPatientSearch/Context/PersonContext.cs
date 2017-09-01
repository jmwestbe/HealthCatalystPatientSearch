using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.WebPages;
using HealthCatalystPatientSearch.Models;

namespace HealthCatalystPatientSearch.Context
{
    public class PersonContext : DbContext, IPersonContext
    {
        public PersonContext() : base("name=PersonContextDB")
        {
            Database.SetInitializer<PersonContext>(new PersonDataContextDbInitializer());
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public void Add(Person person)
        {
            Persons.Add(person);
            SaveChanges();
        }

        public List<Person> Search(string searchString = null)
        {
            List<Person> persons;

            if (searchString == null || searchString.IsEmpty())
            {
                persons = Persons.Include(p => p.Address).OrderBy(p => p.LastName).ThenBy(p => p.FirstName).ToList();
            }
            else
            {

                persons = Persons.Where(p =>
                        (p.LastName != null && p.LastName.ToUpper().Contains(searchString)) //Handle Last Name
                        ||
                        (p.FirstName != null && p.FirstName.ToUpper().Contains(searchString)) //Handle First Name
                ).Include(p => p.Address).OrderBy(p => p.LastName).ThenBy(p => p.FirstName).ToList();
            }

            return persons;
        }
    }
}