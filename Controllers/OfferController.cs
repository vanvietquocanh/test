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
        string[] arr = new string[] { "au", "kr", "de", "in", "id", "tw", "jp", "vn", "th", "br", "es", "tr", "ru", "fr", "sa", "ae", "kw", "mx", "cn", "us", "gb"};
        public JObject Post([FromBody] dataPost data)
        {

            if (data != null)
            {
                if (arr.Contains(data.Country.ToLower()))
                {
                    //Func.RequestApi func = new OfferTest.Func.RequestApi();
                    //string redirectUrl = func.getApp(func.runRedirectURL(data.Url, data.Os, data.Country, DateTime.Now));
                    Func.RequestApiOffer func = new OfferTest.Func.RequestApiOffer();
                    string redirectUrl = func.getApp(func.runRedirectURL(data.Url, data.Os, data.Country.ToLower(), DateTime.Now));
                    return JObject.Parse(redirectUrl);
                }
                else
                {
                    return JObject.Parse("{\"message\": \"Country Not Found\"}"); ;
                }

            }
            return JObject.Parse("{\"message\": \"Error\"}"); ;


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
