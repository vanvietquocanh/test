using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OfferTest.Controllers
{
    [Produces("application/json")]
    [Route("api/Offer")]
    public class OfferController : Controller
    {
        string[] arr;
        public JObject Post([FromBody] dataPost data)
        {
            arr = getCountry();
            if (arr != null)
            {
                if (data != null)
                {
                    string bl = Post(data.User, data.Pass, data.Ipaddress);
                    Console.WriteLine(data.User + "--" + data.Pass + "--" + data.Ipaddress+"---"+ bl);
                    if (bl.ToLower() == "true")
                    {
                        if (arr.Contains(data.Country.ToLower()))
                        {
                            Func.RequestApiOffer func = new OfferTest.Func.RequestApiOffer();
                            string redirectUrl = func.getApp(func.runRedirectURL(data.Url, data.Os, data.Country.ToLower(), DateTime.Now));
                            return JObject.Parse(redirectUrl);
                        }
                        else
                        {
                            return JObject.Parse("{\"message\": \"Country Not Found\"}"); ;
                        }
                    }
                    else
                    {
                        return JObject.Parse("{\"message\": \"Dua Zoi Bo\"}"); ;
                    }
                }
                else
                {
                    return JObject.Parse("{\"message\": \"Data Post Error\"}");
                }
            }
            else
            {
                return JObject.Parse("{\"message\": \"Get Country Error\"}");
            }
          
        }
        public static string[] getCountry()
        {
            string jsonUrl = host + "/getcountry";
            try
            {
                WebRequest request = WebRequest.Create(jsonUrl);
                request.Method = "GET";
                //   request.ContentType = "application/json; charset=utf-8";
                var response = (HttpWebResponse)request.GetResponse();
                System.IO.Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                string responseString = reader.ReadToEnd();
                var data = (JObject)JsonConvert.DeserializeObject(responseString);
      
                return data["country"].ToString().Split(',');
            }
            catch
            {
                return null;
            }


        }
        public static string host = "http://rockettraffic.org";//"http://128.199.163.213";
        public static string Post(string user, string pass, string ipadress)
        {

            string url = host + "/checkstt";

            try
            {
                Console.WriteLine(ipadress);
                var request = (HttpWebRequest)WebRequest.Create(url);

                var postData = "username=" + user;
                postData += "&password=" + pass;
                postData += "&ipAddress=" + ipadress;
                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
             //   Console.WriteLine("CheckSTT"+responseString);
                return responseString;
            }
            catch
            {
                return "";
            }


        }
        public class dataPost
        {
            string url;
            string os;
            string country;
            string user;
            string pass;
            string ipaddress;
           
            public string User
            {
                get
                {
                    return user;
                }

                set
                {
                    user = value;
                }
            }
            public string Pass
            {
                get
                {
                    return pass;
                }

                set
                {
                    pass = value;
                }
            }
            public string Ipaddress
            {
                get
                {
                    return ipaddress;
                }

                set
                {
                    ipaddress = value;
                }
            }
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
