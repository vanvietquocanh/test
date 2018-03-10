using System;
using System.Collections.Generic;
using Chilkat;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Text;
using System.IO;
using System.Collections.Specialized;

namespace OfferTest.Func
{
    public class RequestApi:System.Web.UI.Page
    {
        string UserAgentIOS = "Mozilla/5.0 (iPhone; CPU iPhone OS 11_2_1 like Mac OS X) AppleWebKit/604.4.7 (KHTML, like Gecko) Mobile/15C153";
        string UserAgentAndroid = "Mozilla/5.0 (Linux; Android 7.0; SM-G930V Build/NRD90M) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.125 Mobile Safari/537.36";
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
        public ModeRequest ChkRequest(string url, string useragent, string socksName, string socksPort)
        {
            Http http = new Http();
            bool success;
            http.SetRequestHeader("User-Agent", useragent);
            if (socksName != "" && socksPort != "")
            {
                http.SocksHostname = socksName;//"62.210.220.176"; 
                http.SocksPort = int.Parse(socksPort);// 4277; 
                http.SocksVersion = 5;
                http.SocksUsername = "quydaica123";
                http.SocksPassword = "quydaica";
            }
            http.S3Ssl = true;
            http.SslProtocol = "TLS 1.2";

            success = http.UnlockComponent("ADVRGL.CB1122018_CdZ5Qrc24DmP");
            if (success != true)
            {
                Console.WriteLine(http.LastErrorText);
            }

            string html;


            html = http.QuickGetStr(url);

            if (http.LastMethodSuccess != true)
            {
                Console.WriteLine("--------------- LastErrorText ------------------");
                Console.WriteLine(http.LastErrorText);

            }

            ModeRequest md = new ModeRequest();
            md.Body = http.LastResponseBody;
            md.RedirectUrl = http.FinalRedirectUrl;
            if (http.WasRedirected != true)
            {
                md.RedirectUrl = "Not RedirectUrl";
            }
            return md;
        }
        public class ModeRequest
        {
            string body = "";
            string redirectUrl = "";

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
        }
        public string getProxy(string countrycode)
        {
            Chilkat.Global chilkatGlob = new Chilkat.Global();
            bool success = chilkatGlob.UnlockBundle("ADVRGL.CB1122018_CdZ5Qrc24DmP");
            if (success != true)
            {
                Console.WriteLine(chilkatGlob.LastErrorText);
                return "";
            }
            Chilkat.SshTunnel tunnel = new Chilkat.SshTunnel();
            
            string[] arrSSH = ChkRequest("http://tracking.tracktech.pw/sshapi/?action=read&geo=" + countrycode.ToUpper(), randomUserAgent(), "", "").Body.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            if (arrSSH.Length - 2 > 0)
            {
                B1:
                string[] arruserSSH = arrSSH[new Random().Next(0, arrSSH.Length - 2)].Split('|');
                if (arruserSSH.Length - 1 > 1)
                {
                    string sshHostname = arruserSSH[0];
                    int sshPort = 22;
                    tunnel.IdleTimeoutMs = 4000;

                    success = tunnel.Connect(sshHostname, sshPort);
                    if (success != true)
                    {
                        //        Console.WriteLine(tunnel.LastErrorText);
                        goto B1;
                    }

                    success = tunnel.AuthenticatePw(arruserSSH[1], arruserSSH[2]);
                    if (success != true)
                    {
                        //   Console.WriteLine(tunnel.LastErrorText);
                        goto B1;
                    }


                    tunnel.DynamicPortForwarding = true;

                    tunnel.InboundSocksUsername = "quydaica123";
                    tunnel.InboundSocksPassword = "quydaica";

                    int port = new Random().Next(1000, 65000);
                    success = tunnel.BeginAccepting(port);

                    return "localhost:" + port;
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }

        }
        private string IsUrlValid(string str, string start, string end, string os, string countrycode)
        {

            Regex regx = new Regex("https?://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\{\\}\\[\\]\\'\\,]*)?", RegexOptions.IgnoreCase);

            MatchCollection mactches = regx.Matches(str);
            foreach (Match match in mactches)
            {
                if (start == "http" || start == "https")
                {
                    string rdURL = "";
                    rdURL = getRedirectUrl(match.Value, os, countrycode);
                    if (rdURL != "")
                    {
                        return rdURL;
                    }
                    return rdURL;
                }

            }
            return "";

        }

        public string regexMatchBody(string str, string start, string end, string os, string countrycode)
        {
            Console.WriteLine("Regex:" + str + "Start" + start);
            Match match = Regex.Match(str, start + @"([A-Za-z0-9\/?.&%=;{}_-+])" + end);
            if (match.Success)
            {

                Console.WriteLine("Regex: " + match.Groups.Count);
                if (start == "http" && match.Groups.Count - 1 == 2 || start == "https" && match.Groups.Count - 1 == 2)
                {
                    string rdURL = "";

                    for (int i = 1; i <= match.Groups.Count - 1; i++)
                    {
                        rdURL = getRedirectUrl(start + match.Groups[i].Value, os, countrycode);
                        if (rdURL != "")
                        {
                            return rdURL;
                        }
                    }
                    return rdURL;
                }
                return start + match.Groups[1].Value;
            }
            else { return ""; }


        }
        public string geturlBody(string str, string os, string countrycode)
        {
            string result = "";
            result = IsUrlValid(str, "http", "", os, countrycode);
            if (result != "Err")
            {
                return result;
            }
            else
            {
                return "";
            }


        }

        public string getRedirectUrl(string str)
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
                    string key = "Http://itunes.apple.com/" + match.Groups[1].Value;
                    return key;
                }
                else
                {
                    match = Regex.Match(str + "Quy", @"play.google.com([A-Za-z0-9\/?.&%=_-]+)Quy");
                    if (match.Success)
                    {
                        string key = "Http://play.google.com" + match.Groups[1].Value;
                        return key;
                    }
                    else
                    {
                        return "";
                    }

                }

            }


        }
        public string getRedirectUrl(string url, string os, string countrycode)
        {
            string useragent = "";
            switch (os.Trim().ToLower())
            {
                case "ios": useragent = UserAgentIOS; break;
                case "android": useragent = UserAgentAndroid; break;
            }
            if (useragent != "")
            {

                string[] array = getProxy(countrycode).Split(':');
                Console.WriteLine(array.Length);
                if (array.Length - 1 == 1)
                {


                    B1:
                    Console.WriteLine("url" + url);
                    if (url.ToLower().Equals("itunes.apple.com/"))
                    {
                        Console.WriteLine("returnurl" + url);
                        return url;
                    }

                    ModeRequest md = ChkRequest(url, useragent, array[0], array[1]);
                    string redirectUrl = md.RedirectUrl;
                    if (redirectUrl != "")
                    {
                        
                        Console.WriteLine("redirectURL" + redirectUrl);
                        if (redirectUrl != "Not RedirectUrl")
                        {
                            string rdUrl = getRedirectUrl(redirectUrl);
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
                            string body = geturlBody(md.Body, os, countrycode);
                            Console.WriteLine("Body" + body);
                            string rdUrl = getRedirectUrl(body);
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

                }
                else
                {
                    return "The proxy does not exist";
                }
            }
            return "";
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
                client = new WebClient();
                
                var values = new NameValueCollection();
                values["url"] = Destination;
                    

                var response = client.UploadValues("https://rockettraffic.org/checkapplication", values);

                var responseString = Encoding.Default.GetString(response);
                JObject jObject = JObject.Parse(responseString);

                if ((string)jObject["message"] != "error")
                {

                    string NameApp = ((string)jObject["title"]).Trim();
                    string Icon = ((string)jObject["icon"]).Trim();
                    string rs = "{ 'NameApp': '" + NameApp + "','Icon': '" + Icon + "'}";
                    return rs;
                }

                return "{'message':'Error'}";
            }
            catch (Exception ex)
            {
           
                return "{'message':'Error'}";

            }

        }
      

    }
}