using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

using System.Net.Http;
using Chilkat;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using static OfferTest.Func.RequestApi;

namespace OfferTest.Controllers
{
    [Authorize]
    [RoutePrefix("api/Offer")]
    public class OfferController : ApiController
    {
       

        public string Post([FromBody] dataPost data)
        {
            if(data != null)
            {
                
                Func.RequestApi func = new OfferTest.Func.RequestApi();
                string redirectUrl =func.getApp(func.getRedirectUrl(data.Url, data.Os, data.Country));

                return redirectUrl;
            }
            return null;


        }
        public string Get(string url,string os,string country)
        {
           
           // string redirectUrl = "{ 'Url': '"+ func.getRedirectUrl(Base64Decode(url), os,country) +"'}";
          //  return JObject.Parse(redirectUrl);
          return url;
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }


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
