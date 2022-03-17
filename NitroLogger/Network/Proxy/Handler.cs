using NitroLogger.Network.Communication;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;
using Titanium.Web.Proxy.Network;

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

        private static async Task OnResponse(object sender, SessionEventArgs e)
        {
            string body = e.GetResponseBodyAsString().Result;

            if (body.Contains("socket.url"))
            {
                string socketUrl = Regex.Match(body, "\"socket.url(.+)\"").Groups[1].Value.Split(',')[0].Split('"')[2];
                string json = "\"socket.url\": \"" + socketUrl.Trim() + "\",";
                e.SetResponseBodyString(body.Replace(json, "\"socket.url\": \"ws://127.0.0.1:9092\","));
                proxyServer.Stop();
                Client.Start(socketUrl);
            }
        }

    }
}
