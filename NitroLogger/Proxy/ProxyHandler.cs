using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace NitroLogger.Proxy
{
    public class ProxyHandler
    {

        private static ProxyServer server;
        private static ExplicitProxyEndPoint endPoint;

        public static void Start() {
            server = new ProxyServer(true, false, false);
            endPoint = new ExplicitProxyEndPoint(IPAddress.Any, 8888, true);
            server.AddEndPoint(endPoint);
            server.BeforeResponse += OnResponse;
            server.Start();
            server.SetAsSystemHttpProxy(endPoint);
            server.SetAsSystemHttpsProxy(endPoint);
        }

        private static Task OnResponse(object sender, SessionEventArgs sess) {

            Match match;

            try {
                string body = sess.GetResponseBodyAsString().Result;
                if (sess.HttpClient.Request.Url.Contains(".json")) {
                    if (body.Contains("socket.url")) {

                        var reg_1 = Regex.Match(body, "\"socket.url\": \"(.+)\",");
                        var reg_2 = Regex.Match(body, "\"socket.url\" : \"(.+)\",");
                        var reg_3 = Regex.Match(body, "\"socket.url\" :\"(.+)\",");

                        if (reg_1.Success)
                            match = reg_1;
                        else if (reg_2.Success)
                            match = reg_2;
                        else if (reg_3.Success)
                            match = reg_3;
                        else
                            match = null;

                        string newBody = body.Replace("\"socket.url\": \""+match.Groups[1].Value+"\",", "\"socket.url\": \"ws://127.0.0.1:9092/\",");

                        sess.SetResponseBodyString(newBody);

                        Client.Start("ws://127.0.0.1:9092");

                    }
                }
            } catch { }

            return Task.CompletedTask;
        }

    }
}
