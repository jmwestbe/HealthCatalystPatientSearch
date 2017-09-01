using System;
using System.Collections.Generic;
using HealthCatalystPatientSearch.Models;

namespace HealthCatalystPatientSearch.Context
{
    public interface IPersonContext : IDisposable
    {
        void Add(Person person);
        List<Person> Search(string searchString = null);

    }
}