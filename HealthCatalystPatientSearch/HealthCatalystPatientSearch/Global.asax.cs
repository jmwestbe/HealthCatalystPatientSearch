using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using HealthCatalystPatientSearch.Context;
using HealthCatalystPatientSearch.Models;
using HealthCatalystPatientSearch.App_Start;

namespace HealthCatalystPatientSearch
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);



            /*
             * The following is a solution to POSTing large Json payloads to controller.  
             * Appears to be a known issue - where controller does not look at web.config setting for maxJsonLength value
             * 
             * Solution from:
             * https://forums.asp.net/t/1751116.aspx?How+to+increase+maxJsonLength+for+JSON+POST+in+MVC3
             * 
             * 
             * Replaces the serializer with the custom serializer that sets the maxJsonLength value
             */
            //find the default JsonVAlueProviderFactory
            JsonValueProviderFactory jsonValueProviderFactory = null;

            foreach (var factory in ValueProviderFactories.Factories)
            {
                if (factory is JsonValueProviderFactory)
                {
                    jsonValueProviderFactory = factory as JsonValueProviderFactory;
                }
            }

            //remove the default JsonVAlueProviderFactory
            if (jsonValueProviderFactory != null) ValueProviderFactories.Factories.Remove(jsonValueProviderFactory);

            //add the custom one
            ValueProviderFactories.Factories.Add(new CustomJsonValueProviderFactory());

        }
    }
}
