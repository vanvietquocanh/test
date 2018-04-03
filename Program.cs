using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OfferTest
{
    public class Program
    {
        // "http://128.199.163.213";
        public static string host ="http://rockettraffic.org";
        public static string[] getCountry()
        {
            string jsonUrl = host + "/getcountry";
            try
            {
                Console.WriteLine(jsonUrl);
                WebRequest request = WebRequest.Create(jsonUrl);
                request.Method = "GET";
                var response = (HttpWebResponse)request.GetResponse();
                System.IO.Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                string responseString = reader.ReadToEnd();
                var data = (JObject)JsonConvert.DeserializeObject(responseString);
                request.Abort();
                return data["country"].ToString().Split(',');
            }
            catch
            {
                return null;
            }


        }
        public static void addCountry()
        {
            Func.RequestApiOffer rq = new OfferTest.Func.RequestApiOffer();
            Func.Static.arrCountry = getCountry();
            foreach (string countrycode in Func.Static.arrCountry)
            {

                string proxy = rq.getProxy(countrycode);
                if (proxy.Contains(":"))
                {
                    Func.CountryProxy cProxy = new OfferTest.Func.CountryProxy();
                    cProxy.Country = countrycode;
                    cProxy.Proxy = proxy;
                    Func.Static.lsCountryProxy.Add(cProxy);
                }
            }
        }
        public static void Main(string[] args)
        {
            addCountry();
            BuildWebHost(args).Run();
            
          
           
        }

        public static IWebHost BuildWebHost(string[] args) =>

            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();

    }
}
