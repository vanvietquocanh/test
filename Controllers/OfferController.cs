using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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

            Thread.Sleep(new Random().Next(2, 5) * 1000);
            if (data != null)
            {


                string bl = Post(data.User, data.Pass, data.Ipaddress);
                Console.WriteLine(data.User + "--" + data.Pass + "--" + data.Ipaddress + "---" + bl + "---" + data.Domain);
                if (bl.ToLower() == "true")
                {

                    if (Func.Static.arrCountry != null)
                    {
                        if (Func.Static.arrCountry.Contains(data.Country.ToLower()))
                        {
                            Func.RequestApiOffer func = new OfferTest.Func.RequestApiOffer();
                            string redirectUrl = func.runRedirectURL(data.Url, data.Os, data.Country.ToLower(), DateTime.Now);
                            string[] arrRs = redirectUrl.Split('|');
                            string rs = "{\"message\": \"" + arrRs[0] + "\",\"Count\": \"" + arrRs[1] + "\",\"TimeOut\": \"" + arrRs[2] + "\"}";

                            if (data.Domain == "host")
                            {
                                return JObject.Parse(rs);
                            }
                            else
                            {
                                host = data.Domain;
                                // Console.WriteLine(data.Domain);
                                if (arrRs[0].ToLower().Contains("market") || arrRs[0].ToLower().Contains("itunes.apple.com") || arrRs[0].ToLower().Contains("play.google.com"))
                                {

                                    Console.WriteLine(addDB(data.Url.ToLower().Replace("&", "&amp;"), data.Os.ToLower().Trim(), data.Country.ToLower().Trim(), arrRs[0].Replace("&", "+"), arrRs[1], "success"));
                                    return JObject.Parse(rs);

                                }
                                else
                                {
                                    Console.WriteLine(addDB(data.Url.ToLower().Replace("&", "&amp;"), data.Os.ToLower().Trim(), data.Country.ToLower().Trim(), arrRs[0].Replace("&", "+"), arrRs[1], "fail"));
                                    return JObject.Parse(rs);
                                }
                            }
                        }
                        else
                        {

                            return JObject.Parse("{\"message\": \"Country Not Found\"}"); ;
                        }
                    }
                    else
                    {

                        return JObject.Parse("{\"message\": \"Error\"}");
                    }

                }
                else
                {
                    Response.Clear();
                    return JObject.Parse("{\"message\": \"Dua Zoi Bo\"}");
                }


            }
            else
            {

                return JObject.Parse("{\"message\": \"Data Post Error\"}");
            }


        }



        public static string addDB(string url, string os, string country, string lead, string count, string status)
        {

            string uri = host + "/insert/links";

            try
            {
                WebClient client = new WebClient();
                var values = new NameValueCollection();
                values["url"] = url;
                values["os"] = os;
                values["country"] = country;
                values["lead"] = lead;
                values["count"] = count;
                values["status"] = status;
                var response = client.UploadValues(uri, values);
                var responseString = Encoding.Default.GetString(response);
                return responseString;
            }
            catch
            {
                return "";
            }


        }

        //"http://128.199.163.213";
        public static string host = "http://rockettraffic.org";
        public static string Post(string user, string pass, string ipadress)
        {

            string url = host + "/checkstt";

            try
            {
                // //Console.WriteLine(url);
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
                request.Abort();

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
            string domain;
            public string Domain
            {
                get
                {
                    return domain;
                }

                set
                {
                    domain = value;
                }
            }
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