using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthCatalystPatientSearch.Models
{
    public class SystemTimeProvider : ITimeProvider
    {
        public DateTime Now => DateTime.Now;
    }
}