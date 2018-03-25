using Chilkat;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.IO;


namespace OfferTest.Func
{
    public class RequestApiOffer
    {
        public class ModeRequest
        {
            string body = "";
            string redirectUrl = "";
            string redirectSuccees = "";
            string urllocation = "";
            public string Body
            {
                get
                {
                    return body;
                }

                set
                {
                    body = value;
                }
            }

            public string RedirectUrl
            {
                get
                {
                    return redirectUrl;
                }

                set
                {
                    redirectUrl = value;
                }
            }

            public string RedirectSuccees { get => redirectSuccees; set => redirectSuccees = value; }
            public string Urllocation { get => urllocation; set => urllocation = value; }
        }
        string UserAgentIOS = "Mozilla/5.0 (iPhone; CPU iPhone OS 11_2_1 like Mac OS X) AppleWebKit/604.4.7 (KHTML, like Gecko) Mobile/15C153";
        //   string UserAgentAndroid = "Mozilla/5.0 (Linux; Android 7.0; SM-G930V Build/NRD90M) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.125 Mobile Safari/537.36";
        string UserAgentAndroid = "Mozilla/5.0 (Linux; Android 7.0; SAMSUNG SM-G950F Build/NRD90M) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Mobile Safari/537.36";
        public string randomUserAgent()
        {
            string userAgent = "Mozilla/5.0 (Linux; Android 7.0; SM-G930V Build/NRD90M) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.125 Mobile Safari/537.36";

            if (new Random().Next(1, 100) > 50)
            {
                userAgent = UserAgentAndroid;
            }
            else
            {
                userAgent = UserAgentIOS;
            }
            return userAgent;
        }

        public string checkUrl303(string str)
        {
            Chilkat.Http hh = new Chilkat.Http();
         
            int lenghtstr = str.Length;
            foreach (string url in hh.GetUrlPath(str).Split('/'))
            {
                int ncount = str.IndexOf(url);

                if (url.Split('.').Length - 1 > 0)
                {

                    Ping p = new Ping();
                    try
                    {
                        PingReply reply = p.Send(url, 3000);
                        if (reply.Status == IPStatus.Success)
                            return "http://"+ str.Substring(ncount, lenghtstr - ncount);
                    }
                    catch { }

                }

            }
            return "";
        }
        public ModeRequest ChkRequest(string url, string useragent, string socksName, string socksPort,string username,string password)
        {
            Http http = new Http();
            bool success;
            http.SetRequestHeader("User-Agent", useragent);
          //  Console.WriteLine(username + "----" + password);
            if (socksName != "" && socksPort != "")
            {
                http.SocksHostname = socksName;//"62.210.220.176"; 
                http.SocksPort = int.Parse(socksPort);// 4277; 
                http.SocksVersion = 5;
                http.SocksUsername = username;
                http.SocksPassword = password;
                
            }
            http.S3Ssl = true;
            http.SslProtocol = "TLS 1.2";

            success = http.UnlockComponent("ADVRGL.CB1122018_CdZ5Qrc24DmP");
            if (success != true)
            {
                // Console.WriteLine(http.LastErrorText);
            }

            string html;
            http.SaveCookies = true;
            http.SendCookies = true;
            http.FollowRedirects = false;
            
            html = http.QuickGetStr(url);
            ModeRequest md = new ModeRequest();
          //  Console.WriteLine(http.LastErrorText);
            if (http.LastMethodSuccess != true)
            {
                Console.WriteLine("--------------- LastErrorText ------------------");
             

            }
            Console.WriteLine("============================================== Begin =============================================");
          //  Console.WriteLine("LastResponseHeader: " + http.LastResponseHeader);
            foreach (string header in http.LastResponseHeader.Split(new string[] { "\n" }, StringSplitOptions.None))
            {
                
              
                if (header.ToLower().Contains("location"))
                {
                   
                    md.Urllocation = header.Replace("Location: ", "");
                }
            }
       //     Console.WriteLine("LastHeader: " + http.LastHeader);
            md.Body = http.LastResponseBody;
            md.RedirectUrl = http.FinalRedirectUrl;
            Console.WriteLine("Localtion: " + md.Urllocation);
            Console.WriteLine("============================================== Begin =============================================");
            Console.WriteLine("Body: " + md.Body);
            Console.WriteLine("RedirectUrl: " + md.RedirectUrl);
            return md;
        }
        public string getssh(string url)
        {
            var jsonResponse = "";
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            myRequest.Method = "GET";
            using (System.IO.Stream s = myRequest.GetResponse().GetResponseStream())
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                {
                    jsonResponse = sr.ReadToEnd();
                    //  Console.WriteLine(String.Format("Response: {0}", jsonResponse));
                }
            }

            return jsonResponse;
        }


        public string getProxy(string countryCode)
        {
            Console.WriteLine("Country Code:"+countryCode);
            string jsonUrl = "http://rockettraffic.org/get/port?country=" + countryCode.ToLower();
            WebRequest request = WebRequest.Create(jsonUrl);
            request.Method = "GET";
            request.ContentType = "application/json; charset=utf-8";
            var response = (HttpWebResponse)request.GetResponse();
            System.IO.Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            String responseString = reader.ReadToEnd();
            var data = (JObject)JsonConvert.DeserializeObject(responseString);
            return data[countryCode].ToString();
        }
        //    public string getProxy(string countrycode, DateTime startTime)
        //{
        //    Chilkat.Global chilkatGlob = new Chilkat.Global();
        //    bool success = chilkatGlob.UnlockBundle("ADVRGL.CB1122018_CdZ5Qrc24DmP");
        //    if (success != true)
        //    {
        //        Console.WriteLine(chilkatGlob.LastErrorText);
        //        return "";
        //    }


        //    string[] arrSSH = getssh("http://tracking.tracktech.pw/sshapi/?action=read&geo=" + countrycode.ToUpper()).Split(new string[] { "\r\n" }, StringSplitOptions.None); //ChkRequest("http://tracking.tracktech.pw/sshapi/?action=read&geo=" + countrycode.ToUpper(), randomUserAgent(), "", "").Body.Split(new string[] { "\r\n" }, StringSplitOptions.None);
        //    if (arrSSH.Length - 2 > 0)
        //    {
        //        B1:
        //        Chilkat.SshTunnel tunnel = new Chilkat.SshTunnel();
        //        if (int.Parse(DateTime.Now.Subtract(startTime).TotalSeconds.ToString().Split('.')[0]) > 200)
        //        {
        //            return "Timeout Connect Proxy";
        //        }

        //        string[] arruserSSH = arrSSH[new Random().Next(0, arrSSH.Length - 2)].Split('|');
        //        if (arruserSSH.Length - 1 > 1)
        //        {

        //            string sshHostname = arruserSSH[0];
        //            Console.WriteLine(sshHostname);
        //            int sshPort = 22;
        //            tunnel.IdleTimeoutMs = 4000;
        //            tunnel.ConnectTimeoutMs = 4000;

        //            success = tunnel.Connect(sshHostname, sshPort);

        //            if (success != true)
        //            {

        //                goto B1;
        //            }

        //            success = tunnel.AuthenticatePw(arruserSSH[1], arruserSSH[2]);
        //            if (success != true)
        //            {

        //                goto B1;
        //            }


        //            tunnel.DynamicPortForwarding = true;

        //            tunnel.InboundSocksUsername = "quydaica123";
        //            tunnel.InboundSocksPassword = "quydaica";

        //            int port = new Random().Next(1000, 65000);
        //            success = tunnel.BeginAccepting(port);

        //            return "localhost:" + port;
        //        }
        //        else
        //        {
        //            return "";
        //        }
        //    }
        //    else
        //    {
        //        return "";
        //    }

        //}
        private string IsUrlValid(string str)
        {

            Regex regx = new Regex("https?://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\{\\}\\[\\]\\'\\,]*)?", RegexOptions.IgnoreCase);

            MatchCollection mactches = regx.Matches(str);
            foreach (Match match in mactches)
            {
                if (match.Value.Contains("http") && !match.Value.ToLower().Contains("w3.org"))
                {
                    string rdURL = "";
                    //   rdURL = getRedirectUrl(match.Value, os, countrycode, st, arr);
                    if (rdURL != "")
                    {
                        return rdURL;
                    }

                }
                return match.Value;
            }
            return "";

        }
        public string IsUrlSuccess(string str)
        {
            Match match = Regex.Match(str + "Quy", @"market:/([A-Za-z0-9\/?.&%=_-]+)Quy");
            if (match.Success)
            {
                string key = "market://" + match.Groups[1].Value;
                return key;
            }
            else
            {
                match = Regex.Match(str + "Quy", @"itunes.apple.com([A-Za-z0-9\/?.&%=_-]+)Quy");
                if (match.Success)
                {
                    if (match.Groups[1].Value.Contains("id"))
                    {
                        string key = "http://itunes.apple.com/" + match.Groups[1].Value;
                        return key;
                    }

                }
                else
                {
                    match = Regex.Match(str + "Quy", @"play.google.com([A-Za-z0-9\/?.&%=_-]+)Quy");
                    if (match.Success)
                    {
                        if (match.Groups[1].Value.Contains("id"))
                        {
                            string key = "http://play.google.com" + match.Groups[1].Value;
                            return key;
                        }
                    }
                    else
                    {
                        return "";
                    }

                }

            }

            return "";
        }

        public string runRedirectURL(string url, string os, string countrycode, DateTime startTime)
        {


            Thread.Sleep(5000);
            if (int.Parse(DateTime.Now.Subtract(startTime).TotalSeconds.ToString().Split('.')[0]) > 200)
            {
                return url;
            }

            string proxy = getProxy(countrycode); //"62.210.220.176:4274";
            Console.WriteLine("Proxy:" + proxy);
            if (proxy != "Timeout Connect Proxy")
            {
                string[] array = proxy.Split(':');
                Console.WriteLine(array.Length);
                if (array.Length - 1 == 1)
                {
                    string url1 = getRedirectUrl(url, os, countrycode, startTime, array);
                    Console.WriteLine("URl END" + url1);
                    if (url1 != "")
                    {
                        return url1;
                    }
                 

                }
            }
            else
            {
                return "Timeout Connect Proxy";
            }
            return url;

        }
        string urlendredirect = "";
        public string rcUrl(string url)
        {
            string str = checkUrl303(url);
            if (str != "")
            {
                return  str;
            }
            int index = url.IndexOf("'");
            if (index > 0)
            {
                return url.Substring(0, index);
            }
            index = url.IndexOf("\"");
            if (index > 0)
            {
                return url.Substring(0, index);
            }
            return url;
        }
        int demurl = 0;
        public string getRedirectUrl(string url, string os, string countrycode, DateTime startTime, string[] array)
        {
            string useragent = "";
            switch (os.Trim().ToLower())
            {
                case "ios": useragent = UserAgentIOS; break;
                case "android": useragent = UserAgentAndroid; break;
            }
            if (useragent != "")
            {
                string urlfirst = "";
                B1:
                Console.WriteLine(int.Parse(DateTime.Now.Subtract(startTime).TotalSeconds.ToString().Split('.')[0]));
                if (int.Parse(DateTime.Now.Subtract(startTime).TotalSeconds.ToString().Split('.')[0]) > 200)
                {
                    return url;
                }

                if (url.ToLower().Contains("itunes.apple.com/"))
                {
                    if (url.ToLower().Contains("id"))
                    {
                        Console.WriteLine("returnurl" + url);
                        return url;
                    }
                }
                if (url.ToLower().Contains("w3.org"))
                {
                    return urlfirst;
                }
                if (url.ToLower().Contains("ogp.me"))
                {
                    return urlfirst;
                }
                url = rcUrl(url);
                demurl++;
                urlfirst = url;
                Console.WriteLine("============================================== End =============================================");
                Console.WriteLine("url " + url);

                urlendredirect = url;
                string username = "";
                string pass = "";
                if (countrycode.ToLower().Trim().Equals("us") || countrycode.ToLower().Trim().Equals("gb"))
                {
                    username = "tieuhuy";
                    pass = "anhhuydeptrai1";
                }
                else
                {
                    username = "quydaica123";
                    pass = "quydaica";
                }
                ModeRequest md = ChkRequest(url, useragent, array[0], array[1],username,pass);
      
                string redirectUrl = md.RedirectUrl;

                Console.WriteLine("redirectURL " + redirectUrl);
                if (redirectUrl != "")
                {
                    string rdUrl = IsUrlSuccess(redirectUrl);
                    if (rdUrl != "")
                    {
                        return rdUrl;
                    }
                    else
                    {
                        url = redirectUrl;
                        goto B1;
                    }
                }
                else
                {
                    string body = IsUrlValid(md.Body);
                    Console.WriteLine("Body " + body);
                    Console.WriteLine("============================================== End =============================================");
                    string rdUrl = IsUrlSuccess(body);
                    if (rdUrl != "")
                    {
                        return rdUrl;
                    }
                    else
                    {
                        if (body != "")
                        {
                            url = body;
                            goto B1;
                        }
                        else
                        {
                            return url;
                        }
                    }
                }


          

            }
            return url;
        }
        public static string GetRandomIp()
        {
            Random random = new Random();
            return string.Format("{0}.{1}.{2}.{3}", new object[]
            {
                random.Next(1, 255),
                random.Next(0, 255),
                random.Next(0, 255),
                random.Next(0, 255)
            });
        }
        private WebClient Createclient()
        {
            return new WebClient
            {
                Headers =
                {
                    {
                        "x-forwarded-for",
                        GetRandomIp()
                    },
                    {
                        "user-agent",
                        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.115 Safari/537.36"
                    },
                    {
                        "X-Requested-With",
                        "XMLHttpRequest"
                    }
                }
            };
        }
        public class ResultRedrectUrl
        {
            string nameApp;
            string icon;

            public string NameApp
            {
                get
                {
                    return nameApp;
                }

                set
                {
                    nameApp = value;
                }
            }

            public string Icon
            {
                get
                {
                    return icon;
                }

                set
                {
                    icon = value;
                }
            }
        }
        public string getApp(string Destination, WebClient client = null)
        {

            if (client == null)
            {
                client = Createclient();
            }
            try
            {
                if (Destination != "Timeout Connect Proxy")
                {
                    client = new WebClient();

                    var values = new NameValueCollection();
                    values["url"] = Destination;


                    var response = client.UploadValues("http://rockettraffic.org/checkapplication", values);

                    var responseString = Encoding.Default.GetString(response);
                    JObject jObject = JObject.Parse(responseString);
                    Console.WriteLine("Get ICON " + responseString);
                    if ((string)jObject["message"] != "error")
                    {

                        string NameApp = ((string)jObject["title"]).Trim();
                        string Icon = ((string)jObject["icon"]).Trim();
                        string rs = "{ 'message':'Success' ,'NameApp': '" + NameApp + "','Icon': '" + Icon + "','Url':'" + Destination + "','Count': '" + demurl + "'}" ;
                        return rs;
                    }
                    return "{'message':'Error' ,'Url':'" + Destination + "','Count': '" + demurl + "'}";
                }
                else
                {
                    return "{'message':'Error' ,'Url':'" + Destination + "','Count': '" + demurl + "'}";
                }

            }
            catch 
            {

                return "{'message':'Error' ,'Url':'" + Destination + "','Count': '" + demurl + "'}";

            }

        }
    }
}
