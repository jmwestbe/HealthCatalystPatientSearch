using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading;
using System.Web.WebPages;
using HealthCatalystPatientSearch.Context;
using HealthCatalystPatientSearch.Models;

namespace HealthCatalystPatientSearch.Controllers
{
    public class HomeController : Controller
    {
        private IPersonContext _personContext;

        public HomeController() : base()
        {
            _personContext = new PersonContext();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Add(Person person)
        {
            System.Diagnostics.Debug.WriteLine($"In Add method for: { person.ToString() }");

            using (_personContext)
            {
                _personContext.Add(person);
            }

            return Json(person.ToString());
        }

        public JsonResult SlowGetPersons(string searchString)
        {
            Thread.Sleep(5_000); //5 second wait

            return GetPersons(searchString);
        }

        //GET
        public JsonResult GetPersons(string searchString = null)
        {
            using (_personContext)
            {
                List<Person> persons = _personContext.Search(searchString);

                return Json(persons, JsonRequestBehavior.AllowGet);
            }
        }

        public void SetPersonContext(IPersonContext personContext)
        {
            _personContext = personContext;
        }
    }
}