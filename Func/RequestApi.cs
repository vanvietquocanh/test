using Chilkat;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OfferTest.Func
{
    public class RequestApi
    {
        string urlRedirectEnd = "";
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
        public string splitRedirectUrlErr(string strErrRedirect)
        {
            string[] arrErr = strErrRedirect.Split(new string[] { "url", "<br>" }, StringSplitOptions.RemoveEmptyEntries);
            if (arrErr.Length - 1 > 0)
            {
                foreach (string url in arrErr)
                {
                    string urlRedirectSuccess = getRedirectUrl(url);
                    if (urlRedirectSuccess != "")
                    {
                        return urlRedirectSuccess;
                    }
                }
            }
            return "";
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
                // Console.WriteLine(http.LastErrorText);
            }

            string html;

            http.FollowRedirects = false;
            html = http.QuickGetStr(url);
            ModeRequest md = new ModeRequest();
            md.RedirectSuccees = splitRedirectUrlErr(http.LastErrorHtml);
            if (http.LastMethodSuccess != true)
            {
                Console.WriteLine("--------------- LastErrorText ------------------");
                //  Console.WriteLine(http.LastErrorText);

            }


            md.Body = http.LastResponseBody;
            md.RedirectUrl = http.FinalRedirectUrl;
            //if (http.WasRedirected != true)
            //{
            //    md.RedirectUrl = "Not RedirectUrl";
            //}
            return md;
        }
        public class ModeRequest
        {
            string body = "";
            string redirectUrl = "";
            string redirectSuccees = "";
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
        }
        public string getProxy(string countrycode,DateTime startTime)
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
                if (int.Parse(DateTime.Now.Subtract(startTime).TotalSeconds.ToString().Split('.')[0]) > 200)
                {
                    return "Timeout Connect Proxy";
                }

                string[] arruserSSH = arrSSH[new Random().Next(0, arrSSH.Length - 2)].Split('|');
                if (arruserSSH.Length - 1 > 1)
                {
                    string sshHostname = arruserSSH[0];
                    int sshPort = 22;
                    tunnel.IdleTimeoutMs = 4000;
                    tunnel.ConnectTimeoutMs = 4000;

                    success = tunnel.Connect(sshHostname, sshPort);

                    if (success != true)
                    {

                        goto B1;
                    }

                    success = tunnel.AuthenticatePw(arruserSSH[1], arruserSSH[2]);
                    if (success != true)
                    {

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
        private string IsUrlValid(string str, string os, string countrycode, DateTime st,string[] arr)
        {

            Regex regx = new Regex("https?://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\{\\}\\[\\]\\'\\,]*)?", RegexOptions.IgnoreCase);

            MatchCollection mactches = regx.Matches(str);
            foreach (Match match in mactches)
            {
                if (match.Value.Contains("http"))
                {
                    string rdURL = "";
                    rdURL = getRedirectUrl(match.Value, os, countrycode, st, arr);
                    if (rdURL != "")
                    {
                        return rdURL;
                    }
                  
                }
                return match.Value;
            }
            return "";

        }

       
        public string geturlBody(string str, string os, string countrycode, DateTime st,string[] arr)
        {
            string result = "";
            result = IsUrlValid(str, os, countrycode, st,arr);
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
                    if (match.Groups[1].Value.Contains("id"))
                    {
                        string key = "Http://itunes.apple.com/" + match.Groups[1].Value;
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
                            string key = "Http://play.google.com" + match.Groups[1].Value;
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

            B2:
            if (int.Parse(DateTime.Now.Subtract(startTime).TotalSeconds.ToString().Split('.')[0]) > 200)
            {
                return urlendredirect;
            }
            string proxy = getProxy(countrycode, startTime);

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
                    else
                    {
                        Console.WriteLine("URl urlendredirect" + urlendredirect);
                        if (urlendredirect != url)
                        {
                            return urlendredirect;
                        }
                        else
                        {
                            goto B2;
                        }
                    }
                }
            }
            else
            {
                return "Timeout Connect Proxy";
            }
            return "";

        }
        string urlendredirect = "";
        public string rcUrl(string url)
        {
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

        public string getRedirectUrl(string url, string os, string countrycode, DateTime startTime,string[] array)
        {
            string useragent = "";
            switch (os.Trim().ToLower())
            {
                case "ios": useragent = UserAgentIOS; break;
                case "android": useragent = UserAgentAndroid; break;
            }
            if (useragent != "")
            {
                    B1:
                    Console.WriteLine(int.Parse(DateTime.Now.Subtract(startTime).TotalSeconds.ToString().Split('.')[0]));
                    if (int.Parse(DateTime.Now.Subtract(startTime).TotalSeconds.ToString().Split('.')[0]) > 200)
                    {
                        return "";
                    }
               
                    if (url.ToLower().Equals("itunes.apple.com/"))
                    {
                        Console.WriteLine("returnurl" + url);
                        return url;
                    }
                    if (url == "www.w3.org")
                    {
                        return "";
                    }
                    url = rcUrl(url);
                   Console.WriteLine("url " + url);
                   urlendredirect = url;
                    ModeRequest md = ChkRequest(url, useragent, array[0], array[1]);
                    if (md.RedirectSuccees != "")
                    {
                        return md.RedirectSuccees;
                    }
                    string redirectUrl = md.RedirectUrl;
                   
                    Console.WriteLine("redirectURL" + redirectUrl);
                    if (redirectUrl != "")
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
                        string body = geturlBody(md.Body, os, countrycode, startTime,array);
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
                                return "";
                            }
                        }
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
                if (Destination != "Timeout Connect Proxy")
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
                    return "{'message':'Error' ,'Url':'" + Destination + "'}";
                }
                else
                {
                    return "{'message':'"+ Destination + "'}";
                }
             
            }
            catch (Exception ex)
            {

                return "{'message':'Error' ,'Url':'" + Destination + "'}";

            }

        }
    }
}
