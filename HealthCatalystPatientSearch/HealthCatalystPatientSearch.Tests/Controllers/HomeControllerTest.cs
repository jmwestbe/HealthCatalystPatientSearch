using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using HealthCatalystPatientSearch;
using HealthCatalystPatientSearch.Context;
using HealthCatalystPatientSearch.Controllers;
using HealthCatalystPatientSearch.Models;
using Moq;
using NUnit.Framework;
using System.Collections;
using System.Linq.Expressions;

namespace HealthCatalystPatientSearch.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTest
    {
        private HomeController _testSubject;

        [Test]
        public void Index()
        {
            // Arrange
            _testSubject = new HomeController();

            // Act
            ViewResult result = _testSubject.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void Add()
        {
            //Setup Moq
            var contextMock = new Mock<IPersonContext>();

            //Setup request data
            var person = getPerson();

            contextMock.Setup(db => db.Add(person));

            //setup test subject
            _testSubject = new HomeController();
            _testSubject.SetPersonContext(contextMock.Object);

            //Execute
            JsonResult result = _testSubject.Add(person);
            string resultString = new JavaScriptSerializer().Serialize(result.Data);

            //assert
            Assert.AreEqual("\"Person\\n\\tName: James Potter\\n\\tAge: 0\\n\\tInterests: some interests\\n\\tAddress: 123 Big St Apt 2\\nAnytown, UT 88888\\nUSA\"", resultString);

            //verify
            contextMock.Verify(db => db.Add(person), Times.Once);
        }

        [Test]
        public void GetPersons()
        {
            var persons = new List<Person>() { getPerson(), getPerson() };
            var searchString = "FindMe";

            //Setup Moq
            var contextMock = new Mock<IPersonContext>();
            contextMock.Setup(db => db.Search(searchString)).Returns(persons);

            //setup test subject
            _testSubject = new HomeController();
            _testSubject.SetPersonContext(contextMock.Object);

            //Execute
            JsonResult result = _testSubject.GetPersons(searchString);
            string resultString = new JavaScriptSerializer().Serialize(result.Data);

            Console.WriteLine(resultString);

            //Assert
            Assert.AreEqual("[{\"Id\":0,\"FirstName\":\"James\",\"LastName\":\"Potter\",\"DateOfBirth\":\"\\/Date(1504159200000)\\/\",\"Interests\":\"some interests\",\"Address\":{\"Id\":0,\"StreetLine1\":\"123 Big St\",\"StreetLine2\":\"Apt 2\",\"City\":\"Anytown\",\"State\":\"UT\",\"PostalCode\":\"88888\",\"Country\":\"USA\",\"AsString\":\"123 Big St Apt 2\\nAnytown, UT 88888\\nUSA\"},\"Image\":[0,1],\"Age\":0},{\"Id\":0,\"FirstName\":\"James\",\"LastName\":\"Potter\",\"DateOfBirth\":\"\\/Date(1504159200000)\\/\",\"Interests\":\"some interests\",\"Address\":{\"Id\":0,\"StreetLine1\":\"123 Big St\",\"StreetLine2\":\"Apt 2\",\"City\":\"Anytown\",\"State\":\"UT\",\"PostalCode\":\"88888\",\"Country\":\"USA\",\"AsString\":\"123 Big St Apt 2\\nAnytown, UT 88888\\nUSA\"},\"Image\":[0,1],\"Age\":0}]", resultString);

            //verify
            contextMock.Verify(db => db.Search(searchString), Times.Once);
        }

        [Test]
        public void GetPersonsWithNull()
        {
            var persons = new List<Person>() { getPerson(), getPerson() };
            string searchString = null;

            //Setup Moq
            var contextMock = new Mock<IPersonContext>();
            contextMock.Setup(db => db.Search(searchString)).Returns(persons);

            //setup test subject
            _testSubject = new HomeController();
            _testSubject.SetPersonContext(contextMock.Object);

            //Execute
            JsonResult result = _testSubject.GetPersons(searchString);
            string resultString = new JavaScriptSerializer().Serialize(result.Data);

            Console.WriteLine(resultString);

            //Assert
            Assert.AreEqual("[{\"Id\":0,\"FirstName\":\"James\",\"LastName\":\"Potter\",\"DateOfBirth\":\"\\/Date(1504159200000)\\/\",\"Interests\":\"some interests\",\"Address\":{\"Id\":0,\"StreetLine1\":\"123 Big St\",\"StreetLine2\":\"Apt 2\",\"City\":\"Anytown\",\"State\":\"UT\",\"PostalCode\":\"88888\",\"Country\":\"USA\",\"AsString\":\"123 Big St Apt 2\\nAnytown, UT 88888\\nUSA\"},\"Image\":[0,1],\"Age\":0},{\"Id\":0,\"FirstName\":\"James\",\"LastName\":\"Potter\",\"DateOfBirth\":\"\\/Date(1504159200000)\\/\",\"Interests\":\"some interests\",\"Address\":{\"Id\":0,\"StreetLine1\":\"123 Big St\",\"StreetLine2\":\"Apt 2\",\"City\":\"Anytown\",\"State\":\"UT\",\"PostalCode\":\"88888\",\"Country\":\"USA\",\"AsString\":\"123 Big St Apt 2\\nAnytown, UT 88888\\nUSA\"},\"Image\":[0,1],\"Age\":0}]", resultString);

            //verify
            contextMock.Verify(db => db.Search(searchString), Times.Once);
        }

        [Test]
        public void GetPersonsWithEmpty()
        {
            var persons = new List<Person>() { getPerson(), getPerson() };
            var searchString = "";

            //Setup Moq
            var contextMock = new Mock<IPersonContext>();
            contextMock.Setup(db => db.Search(searchString)).Returns(persons);

            //setup test subject
            _testSubject = new HomeController();
            _testSubject.SetPersonContext(contextMock.Object);

            //Execute
            JsonResult result = _testSubject.GetPersons(searchString);
            string resultString = new JavaScriptSerializer().Serialize(result.Data);

            Console.WriteLine(resultString);

            //Assert
            Assert.AreEqual("[{\"Id\":0,\"FirstName\":\"James\",\"LastName\":\"Potter\",\"DateOfBirth\":\"\\/Date(1504159200000)\\/\",\"Interests\":\"some interests\",\"Address\":{\"Id\":0,\"StreetLine1\":\"123 Big St\",\"StreetLine2\":\"Apt 2\",\"City\":\"Anytown\",\"State\":\"UT\",\"PostalCode\":\"88888\",\"Country\":\"USA\",\"AsString\":\"123 Big St Apt 2\\nAnytown, UT 88888\\nUSA\"},\"Image\":[0,1],\"Age\":0},{\"Id\":0,\"FirstName\":\"James\",\"LastName\":\"Potter\",\"DateOfBirth\":\"\\/Date(1504159200000)\\/\",\"Interests\":\"some interests\",\"Address\":{\"Id\":0,\"StreetLine1\":\"123 Big St\",\"StreetLine2\":\"Apt 2\",\"City\":\"Anytown\",\"State\":\"UT\",\"PostalCode\":\"88888\",\"Country\":\"USA\",\"AsString\":\"123 Big St Apt 2\\nAnytown, UT 88888\\nUSA\"},\"Image\":[0,1],\"Age\":0}]", resultString);

            //verify
            contextMock.Verify(db => db.Search(searchString), Times.Once);
        }

        [Test]
        public void SlowGetPersons()
        {
            var persons = new List<Person>() { getPerson(), getPerson() };
            var searchString = "FindMe";

            //Setup Moq
            var contextMock = new Mock<IPersonContext>();
            contextMock.Setup(db => db.Search(searchString)).Returns(persons);

            //setup test subject
            _testSubject = new HomeController();
            _testSubject.SetPersonContext(contextMock.Object);

            //Execute
            JsonResult result = _testSubject.SlowGetPersons(searchString);
            string resultString = new JavaScriptSerializer().Serialize(result.Data);

            Console.WriteLine(resultString);

            //Assert
            Assert.AreEqual("[{\"Id\":0,\"FirstName\":\"James\",\"LastName\":\"Potter\",\"DateOfBirth\":\"\\/Date(1504159200000)\\/\",\"Interests\":\"some interests\",\"Address\":{\"Id\":0,\"StreetLine1\":\"123 Big St\",\"StreetLine2\":\"Apt 2\",\"City\":\"Anytown\",\"State\":\"UT\",\"PostalCode\":\"88888\",\"Country\":\"USA\",\"AsString\":\"123 Big St Apt 2\\nAnytown, UT 88888\\nUSA\"},\"Image\":[0,1],\"Age\":0},{\"Id\":0,\"FirstName\":\"James\",\"LastName\":\"Potter\",\"DateOfBirth\":\"\\/Date(1504159200000)\\/\",\"Interests\":\"some interests\",\"Address\":{\"Id\":0,\"StreetLine1\":\"123 Big St\",\"StreetLine2\":\"Apt 2\",\"City\":\"Anytown\",\"State\":\"UT\",\"PostalCode\":\"88888\",\"Country\":\"USA\",\"AsString\":\"123 Big St Apt 2\\nAnytown, UT 88888\\nUSA\"},\"Image\":[0,1],\"Age\":0}]", resultString);

            //verify
            contextMock.Verify(db => db.Search(searchString), Times.Once);
        }

        private static Person getPerson()
        {
            Person person = new Person()
            {
                FirstName = "James",
                LastName = "Potter",
                Address = new Address()
                {
                    StreetLine1 = "123 Big St",
                    StreetLine2 = "Apt 2",
                    City = "Anytown",
                    State = "UT",
                    PostalCode = "88888",
                    Country = "USA"
                },
                DateOfBirth = new DateTime(2017, 8, 31),
                Image = new byte[] { 0x0, 0x1 },
                Interests = "some interests"
            };
            return person;
        }
    }
}
