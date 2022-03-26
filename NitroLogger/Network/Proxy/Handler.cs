using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;
using Titanium.Web.Proxy.Network;

using NitroLogger.Network.Communication;

namespace NitroLogger.Network.Proxy
{
    public class Handler
    {

        private static ProxyServer proxyServer;
        private static ExplicitProxyEndPoint explicitProxyEndPoint;

        public static void Start()
        {
            proxyServer = new ProxyServer(true, false, false);
            proxyServer.CertificateManager.CertificateEngine = CertificateEngine.BouncyCastle;

            proxyServer.BeforeResponse += OnResponse;
            proxyServer.AfterResponse += AfterResponse;

            explicitProxyEndPoint = new ExplicitProxyEndPoint(System.Net.IPAddress.Any, 8080, true);

            Initialize();

        }

        public static void Stop()
        {
            proxyServer.Dispose();
        }

        private static void Initialize()
        {
            proxyServer.AddEndPoint(explicitProxyEndPoint);
            proxyServer.Start();

            proxyServer.SetAsSystemHttpProxy(explicitProxyEndPoint);
            proxyServer.SetAsSystemHttpsProxy(explicitProxyEndPoint);
        }

        private static string TryFindSocketUrl(string body)
        {

            string socketUrl = "";
            string matchF = "";

            Dictionary<string, string> matchesSocketUrl = new Dictionary<string, string>
            {
                ["\"socket.url\": \"(.+)\","] = "\"socket.url\": \"",
                ["\"socket.url\" : \"(.+)\","] = "\"socket.url\" : \"",
                ["\"socket.url\"  :\"(.+)\","] = "\"socket.url\"  :\"",

                ["socket.url\":\"(.+?)\","] = "socket.url\":\"",
                ["{\"socket.url\":\"(.+)\","] = "{\"socket.url\":\"",
                ["{\"socket.url\" : \"(.+)\","] = "{\"socket.url\" : \"",
                ["{\"socket.url\"  :\"(.+)\","] = "{\"socket.url\"  :\""

            };

            foreach (KeyValuePair<string, string> match_ in matchesSocketUrl)
            {
                string match = match_.Key;

                Match tryMatch = Regex.Match(body, match);

                char k = '"';

                if (tryMatch.Success && !tryMatch.Groups[1].Value.Contains(k.ToString()))
                {
                    socketUrl = tryMatch.Groups[1].Value;
                    matchF = match_.Value;
                }

            }

            return socketUrl + "?" + matchF;

        }

        private static Task OnResponse(object sender, SessionEventArgs e)
        {
            try
            {
                
                    string body = e.GetResponseBodyAsString().Result;

                    if (body.Contains("socket.url"))
                    {
                        string[] result = TryFindSocketUrl(body).Split('?');
                        string socketUrl = result[0];
                        string toReplace = result[1];

                        string json = toReplace + socketUrl.Trim() + "\",";

                        e.SetResponseBodyString(body.Replace(json, toReplace + "ws://127.0.0.1:9092\","));

                        Client.Start(socketUrl);

                        Stop();

                    }
                
            }

            catch { }
            
            return Task.CompletedTask;
            

        }

        private static Task AfterResponse(object sender, SessionEventArgs e) 
        {
            if (e.HttpClient.Request.Url.Contains(".json") && e.HttpClient.Request.Url.Contains("?"))
                e.HttpClient.Request.Url = $"{e.HttpClient.Request.Url.Split('?')[0]}";
            return Task.CompletedTask;
        }


    }
}
