using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace OfferTest.Controllers
{
    [Produces("application/json")]
    [Route("api/Offer")]
    public class OfferController : Controller
    {
        public JObject Post([FromBody] dataPost data)
        {
            if (data != null)
            {

                Func.RequestApi func = new OfferTest.Func.RequestApi();
                string redirectUrl = func.getApp(func.runRedirectURL(data.Url, data.Os, data.Country, DateTime.Now));

                return JObject.Parse(redirectUrl);
            }
            return null;


        }
        public class dataPost
        {
            string url;
            string os;
            string country;

            public string Url
            {
                get
                {
                    return url;
                }

                set
                {
                    url = value;
                }
            }

            public string Os
            {
                get
                {
                    return os;
                }

                set
                {
                    os = value;
                }
            }

            public string Country
            {
                get
                {
                    return country;
                }

                set
                {
                    country = value;
                }
            }
        }
    }
}
