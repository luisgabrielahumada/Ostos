using Core.Component.Library.Json;
using Core.Component.Library.WebTools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Common.Infrastructure.Connector
{
    public enum TypeAuth
    {
        Anonymous,
        Basic,
        Jwt
    }
    public static class ConnectorGeneric
    {
        /*
          RequestBecaServiceOBS _requestBecaServiceOBS = null;
            Dictionary<string, object> req = new Dictionary<string, object>()
            {
                {"data", new RequestBecaServiceOBS(){ }.SerializeJson() }
            };
            Dictionary<string, string> header = new Dictionary<string, string>()
            {
                {"header1", "" },
                {"header2", "" },
                {"header3", "" },
                {"header4", "" },
                {"header5", "" },
            };
            Dictionary<string, string> auth = new Dictionary<string, string>()
            {
                {"token", ""},
            };
            var resp= ConnectorGeneric.Run(req, header, _setting.Environments.Obs);
            if (resp.GetBoolean("Status"))
            {
                _requestBecaServiceOBS = OsbAdapter.MapeadorGetObs(resp);
            }
         */
        public static Dictionary<string, object> Run(Dictionary<string, object> req, Dictionary<string, string> headers, Dictionary<string, object> auth, string url, string verb = "POST", TypeAuth typeAuth = TypeAuth.Anonymous)
        {
            var body = req.SerializeJson();
            return Execute(body, headers, auth, url, verb, typeAuth);
        }

        public static Dictionary<string, object> Run(string req, Dictionary<string, string> headers, Dictionary<string, object> auth, string url, string verb = "POST", TypeAuth typeAuth = TypeAuth.Anonymous)
        {
            return Execute(req, headers, auth, url, verb, typeAuth);
        }

        private static Dictionary<string, object> Execute(string req, Dictionary<string, string> headers, Dictionary<string, object> auth, string url, string verb = "POST", TypeAuth typeAuth = TypeAuth.Anonymous)
        {
            try
            {
                Dictionary<string, object> result = new Dictionary<string, object>() {
                    { "Status" , false },
                    { "StatusCode" , HttpStatusCode.BadRequest },
                    { "Message" , string.Empty },
                    { "Data" , string.Empty  }
                };
                using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip }))
                {
                    client.BaseAddress = new Uri(url);
                    var method = new HttpMethod(verb);
                    var http = new HttpRequestMessage(method, url);
                    StringContent httpContent = null;
                    if (req != null)
                    {
                        httpContent = new StringContent(req, Encoding.UTF8, "application/json");
                        http.Content = httpContent;
                    }
                    // Add an Accept header for JSON format.
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    foreach (var head in headers)
                    {
                        client.DefaultRequestHeaders.TryAddWithoutValidation(head.Key, head.Value);
                    }
                    if (typeAuth != TypeAuth.Anonymous)
                    {
                        switch (typeAuth)
                        {
                            case TypeAuth.Basic:
                                var User = auth.GetString("User");
                                var Password = auth.GetString("Password");
                                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{User}:{Password}");
                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(plainTextBytes));
                                break;
                            case TypeAuth.Jwt:
                                var accessToken = auth.GetString("AccessToken");
                                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                                break;
                        }
                    }

                    // List data response.
                    client.Timeout = new TimeSpan(1,30,0);
                    HttpResponseMessage response = client.SendAsync(http).Result;
                    // Blocking call! Program will wait here until a response is received or a timeout occurs.
                    // Parse the response body.
                    var content = response.Content.ReadAsStringAsync();
                    result["Status"] = response.IsSuccessStatusCode;
                    result["StatusCode"] = response.IsSuccessStatusCode ? HttpStatusCode.OK : response.StatusCode;
                    result["Data"] = response.IsSuccessStatusCode ? JsonConvert.DeserializeObject<Dictionary<string, object>>(content.Result) : null;
                    result["Location"] = response.Headers.Location;
                    result["Headers"] = response.Headers;
                    result["Message"] = response.IsSuccessStatusCode ? string.Empty : content.Result;
                }
                return result;
            }
            catch (Exception ex)
            {
                return new Dictionary<string, object>() {
                    { "Status" , false },
                    { "StatusCode" , HttpStatusCode.BadRequest },
                    { "Message" , ex.ToString() },
                    { "Data" , string.Empty  }
                };
            }
        }
    }
}
