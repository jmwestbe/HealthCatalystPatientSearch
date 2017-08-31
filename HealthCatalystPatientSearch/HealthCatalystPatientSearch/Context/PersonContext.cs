using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using HealthCatalystPatientSearch.Models;

namespace HealthCatalystPatientSearch.Context
{
    public class PersonContext : DbContext
    {
        public PersonContext() : base("name=PersonContextDB")
        {
            Database.SetInitializer<PersonContext>(new PersonDataContextDbInitializer());
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Address> Addresses { get; set; }


    }
}