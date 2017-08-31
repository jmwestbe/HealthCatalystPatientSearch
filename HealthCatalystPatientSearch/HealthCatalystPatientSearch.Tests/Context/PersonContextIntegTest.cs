using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCatalystPatientSearch.Context;
using HealthCatalystPatientSearch.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace HealthCatalystPatientSearch.Tests.Context
{
    [TestClass]
    public class PersonContextIntegTest
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
        private static readonly byte[] Image1 = new byte[] {0};
        private static readonly string Interests1 = "archaeology, anthropology, apologists";


        private Address _address;
        private Person _person;


        [TestInitialize]
        public void SetUp()
        {
            _address = new Address()
            {
                StreetLine1 = FirstStreetLine1,
                StreetLine2 = SecondStreetLine1,
                City = City1,
                State = State1,
                PostalCode = Zip1,
                Country = Country1
            };

            _person = new Person()
            {
                Address = _address,
                FirstName = FirstName1,
                LastName = LastName1,
                Image = Image1,
                Interests = Interests1,
                DateOfBirth = DateOfBirth1
            };
        }

        [TestCleanup]
        public void CleanUp()
        {
            using (PersonContext db = new PersonContext())
            {
                //Delete Persons added during test
                db.Persons.Attach(_person);
                db.Addresses.Attach(_address);
                db.Persons.Remove(_person);
                db.Addresses.Remove(_address);
                db.SaveChanges();
            }
        }

        [TestMethod]
        public void AddPersonToDBIsPersisted()
        {
            using (PersonContext db = new PersonContext())
            {
                //Create and save new Person
                db.Persons.Add(_person);
                db.SaveChanges();

                var persons = db.Persons.Include(p => p.Address).ToList();
                Assert.IsNotNull(persons);
                Assert.AreEqual(1, persons.Count);
                Assert.IsNotNull(persons[0]);
                var person = persons[0];
                Assert.AreEqual(FirstName1, person.FirstName);
                Assert.AreEqual(LastName1, person.LastName);
                Assert.AreEqual(DateOfBirth1, person.DateOfBirth);
                Assert.AreEqual(Interests1, person.Interests);
                Assert.IsNotNull(person.Id);
                Assert.IsNotNull(person.Address);
                Assert.AreEqual(FirstStreetLine1, person.Address.StreetLine1);
                Assert.AreEqual(SecondStreetLine1, person.Address.StreetLine2);
                Assert.AreEqual(City1, person.Address.City);
                Assert.AreEqual(State1, person.Address.State);
                Assert.AreEqual(Zip1, person.Address.PostalCode);
                Assert.AreEqual(Country1, person.Address.Country);

                Console.WriteLine(person);
            }
        }
    }
}
