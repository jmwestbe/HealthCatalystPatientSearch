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

        string FirstStreetLine2 = "456 S";
        string SecondStreetLine2 = "Apt 2";
        string City2 = "Anytown2";
        string State2 = "Delaware";
        string Zip2 = "44444";
        string Country2 = "USA";
        string FirstName2 = "XXXJames";
        string LastName2 = "XXXCarmichael";
        string Interests2 = "snakes, aliens";
        readonly byte[] Image2 = new byte[] { 0x2, 0x3 };
        readonly DateTime DateOfBirth2 = new DateTime(1994, 02, 13);

        private Person _person1;
        private Person _person2;

        private PersonContext _testSubject;



        [SetUp]
        public void SetUp()
        {
            _person1 = GetPerson(FirstName1, LastName1, Interests1, Image1, FirstStreetLine1,
                SecondStreetLine1, City1, State1, Zip1, Country1, DateOfBirth1);

            _person2 = GetPerson(FirstName2, LastName2, Interests2, Image2, FirstStreetLine2,
                SecondStreetLine2, City2, State2, Zip2, Country2, DateOfBirth2);
        }

        [TearDown]
        public void CleanUp()
        {
            using (_testSubject = new PersonContext())
            {
                //Delete Persons added during test
                try
                {
                    _testSubject.Persons.Attach(_person1);
                    _testSubject.Persons.Remove(_person1);
                    _testSubject.SaveChanges();

                }
                catch (Exception e)
                {
                    //ignored as person not always saved in every test case
                    Console.WriteLine(e.Message + "\n" + e.StackTrace);
                }
                try
                {
                    _testSubject.Persons.Attach(_person2);
                    _testSubject.Persons.Remove(_person2);
                    _testSubject.SaveChanges();

                }
                catch (Exception e)
                {
                    //ignored as person not always saved in every test case
                    Console.WriteLine(e.Message + "\n" + e.StackTrace);
                }
            }
        }

        [Test]
        public void AddPerson()
        {
            using (_testSubject = new PersonContext())
            {
                //Create and save new Person
                _testSubject.Add(_person1);

                var persons = _testSubject.Persons.Include(p => p.Address).ToList();
                Assert.IsNotNull(persons);
                Assert.AreEqual(1, persons.Count);
                Assert.IsNotNull(persons[0]);

                var person = persons[0];
                AssertPerson(person, FirstName1, LastName1, Interests1, Image1, FirstStreetLine1,
                    SecondStreetLine1, City1, State1, Zip1, Country1, DateOfBirth1);
            }
        }

        [Test]
        public void SearchPerson()
        {
            using (_testSubject = new PersonContext())
            {
                //Create and save new Person
                _testSubject.Add(_person1);
                _testSubject.Add(_person2);

                var persons = _testSubject.Search("John");
                Assert.IsNotNull(persons);
                Assert.AreEqual(1, persons.Count);
                Assert.IsNotNull(persons[0]);

                var person = persons[0];
                AssertPerson(person, FirstName1, LastName1, Interests1, Image1, FirstStreetLine1,
                    SecondStreetLine1, City1, State1, Zip1, Country1, DateOfBirth1);
            }
        }

        [Test]
        public void SearchPersonWithNull()
        {
            using (_testSubject = new PersonContext())
            {
                //Create and save new Person
                _testSubject.Add(_person1);
                _testSubject.Add(_person2);

                var persons = _testSubject.Search();
                Assert.IsNotNull(persons);
                Assert.AreEqual(2, persons.Count);
                Assert.IsNotNull(persons[0]);
                Assert.IsNotNull(persons[1]);

                //order is reversed (sorted alphabetically)
                AssertPerson(persons[0], FirstName2, LastName2, Interests2, Image2, FirstStreetLine2,
                        SecondStreetLine2, City2, State2, Zip2, Country2, DateOfBirth2);

                AssertPerson(persons[1], FirstName1, LastName1, Interests1, Image1, FirstStreetLine1,
                    SecondStreetLine1, City1, State1, Zip1, Country1, DateOfBirth1);
            }
        }

        [Test]
        public void SearchPersonWithEmptyString()
        {
            using (_testSubject = new PersonContext())
            {
                //Create and save new Person
                _testSubject.Add(_person1);
                _testSubject.Add(_person2);

                var persons = _testSubject.Search("");
                Assert.IsNotNull(persons);
                Assert.AreEqual(2, persons.Count);
                Assert.IsNotNull(persons[0]);
                Assert.IsNotNull(persons[1]);

                AssertPerson(persons[0], FirstName2, LastName2, Interests2, Image2, FirstStreetLine2,
                    SecondStreetLine2, City2, State2, Zip2, Country2, DateOfBirth2);

                AssertPerson(persons[1], FirstName1, LastName1, Interests1, Image1, FirstStreetLine1,
                    SecondStreetLine1, City1, State1, Zip1, Country1, DateOfBirth1);
            }
        }

        [Test]
        public void SearchPersonNoResults()
        {
            using (_testSubject = new PersonContext())
            {
                //Create and save new Person
                _testSubject.Add(_person1);
                _testSubject.Add(_person2);

                var persons = _testSubject.Search("SomeStringThatReturnsNoResults");
                Assert.IsNotNull(persons);
                Assert.AreEqual(0, persons.Count);
            }
        }


        Person GetPerson(string first, string last, string interests, byte[] image, 
            string line1, string line2, string city, string state, string postal, string country, DateTime dob)
        {
            var p = new Person()
            {
                FirstName = first,
                LastName = last,
                Interests = interests,
                Image = image,
                DateOfBirth = dob,
                Address = new Address()
                {
                    StreetLine1 = line1,
                    StreetLine2 = line2,
                    City = city,
                    State = state,
                    PostalCode = postal,
                    Country = country,
                }
            };

            return p;
        }

        void AssertPerson(Person person, string first, string last, string interests, byte[] image,
            string line1, string line2, string city, string state, string postal, string country, DateTime dob)
        {
            Assert.AreEqual(first, person.FirstName);
            Assert.AreEqual(last, person.LastName);
            Assert.AreEqual(dob, person.DateOfBirth);
            Assert.AreEqual(interests, person.Interests);
            Assert.IsNotNull(person.Id);
            Assert.IsNotNull(person.Address);
            Assert.AreEqual(line1, person.Address.StreetLine1);
            Assert.AreEqual(line2, person.Address.StreetLine2);
            Assert.AreEqual(city, person.Address.City);
            Assert.AreEqual(state, person.Address.State);
            Assert.AreEqual(postal, person.Address.PostalCode);
            Assert.AreEqual(country, person.Address.Country);
        }

    }
}
