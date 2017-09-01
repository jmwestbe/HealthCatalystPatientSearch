using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCatalystPatientSearch.Models;
using NUnit.Framework;

namespace HealthCatalystPatientSearch.Tests.Models
{
    [TestFixture]
    class PersonTest
    {
        [TestCase(1990,  8, 31, 27)]
        [TestCase(1990,  8, 30, 27)]
        [TestCase(1990,  9,  1, 26)]
        [TestCase(1980,  1, 31, 37)]
        [TestCase(1980,  9, 15, 36)]
        [TestCase(2000, 12, 31, 16)]
        [TestCase(2000,  2, 22, 17)] //leap year
        [TestCase(2000,  3, 31, 17)] //leap year
        [TestCase(2007,  8, 30, 10)]
        [TestCase(2007,  8, 31, 10)]
        [TestCase(2007,  9,  1,  9)]
        [TestCase(1990,  3, 11, 27)]
        public void GetAgeFromBirthDateAndCurrentYearIsCorrect(int year, int month, int day, int expectedAge)
        {
            Person p = new Person() { DateOfBirth = new DateTime(year, month, day, 0, 0, 0)};
            p.SetTimeProvider(new StaticTimeProvider());

            Assert.AreEqual(expectedAge, p.Age);

        }
    }

    //Concrete class to provide static time instead of  system time
    class StaticTimeProvider : ITimeProvider
    {
        //Always return August 31, 2017 00:00:00
        public DateTime Now { get { return new DateTime(2017, 08, 31, 12, 0, 0); } }

    }
}
