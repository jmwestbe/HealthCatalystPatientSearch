using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCatalystPatientSearch.Context;
using HealthCatalystPatientSearch.Models;
using NUnit.Framework;

namespace HealthCatalystPatientSearch.Tests.Context
{
    [TestFixture]
    public class PersonContextIntegTest
    {
        string FirstStreetLine1 = "123 S";
        string SecondStreetLine1 = "Apt 1";
        string City1 = "Anytown";
        string State1 = "Utah";
        string Zip1 = "88888";
        string Country1 = "USA";
        string FirstName1 = "XXXJohn";
        string LastName1 = "XXXDoe";
        string Interests1 = "archaeology, anthropology, apologists";
        readonly byte[] Image1 = new byte[] { 0x0, 0x1 };
        readonly DateTime DateOfBirth1 = new DateTime(1998, 04, 13);


        private Address _address;
        private Person _person;

        private PersonContext _testSubject;

        [SetUp]
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

        [TearDown]
        public void CleanUp()
        {
            using (_testSubject = new PersonContext())
            {
                //Delete Persons added during test
                _testSubject.Persons.Attach(_person);
                _testSubject.Addresses.Attach(_address);
                _testSubject.Persons.Remove(_person);
                _testSubject.Addresses.Remove(_address);
                _testSubject.SaveChanges();
            }
        }

        [Test]
        public void AddPersonToDBIsPersisted()
        {
            using (_testSubject = new PersonContext())
            {
                //Create and save new Person
                _testSubject.Persons.Add(_person);
                _testSubject.SaveChanges();

                var persons = _testSubject.Persons.Include(p => p.Address).ToList();
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
            }
        }
    }
}
