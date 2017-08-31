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
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Title = "Patient Search Assignment";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Title = "Contact Me";
            return View();
        }


        [HttpPost]
        public JsonResult Add(Person person)
        {
            System.Diagnostics.Debug.WriteLine($"In Add method for: { person.ToString() }");

            using (var db = new PersonContext())
            {
                db.Persons.Add(person);
                db.SaveChanges();
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
            using (var db = new PersonContext())
            {
                List<Person> persons;
                if (searchString == null || searchString.IsEmpty())
                {
                    persons = db.Persons.Include(p => p.Address).OrderBy(p => p.LastName).ThenBy(p => p.FirstName).ToList();
                }
                else
                {

                    persons = db.Persons.Where(p =>
                            (p.LastName != null && p.LastName.ToUpper().Contains(searchString)) //Handle Last Name
                          ||
                            (p.FirstName != null && p.FirstName.ToUpper().Contains(searchString)) //Handle First Name
                          ).Include(p => p.Address).OrderBy(p => p.LastName).ThenBy(p => p.FirstName).ToList();
                }

                return Json(persons, JsonRequestBehavior.AllowGet);
            }
        }

        public bool MatchesSearch(Person p, string searchString = null)
        {
            if (searchString == null)
            {
                return true;
            }

            if (p == null || p.FirstName == null || p.LastName == null)
            {
                return false;
            }

            //Most scenarios caught in first to cases. Last 2 provided for when user enters space or comma in search string
            return p.FirstName.ToUpper().Contains(searchString.ToUpper())
                   || p.LastName.ToUpper().Contains(searchString.ToUpper())
                   || $"{p.FirstName.ToUpper()} {p.LastName.ToUpper()}".Contains(searchString.ToUpper())
                   || $"{p.LastName.ToUpper()}, {p.FirstName.ToUpper()}".Contains(searchString.ToUpper());
                
        }
    }
}