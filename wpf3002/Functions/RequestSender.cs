using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.NetworkInformation;
using Newtonsoft.Json;    

namespace wpf3002.Functions
{
    static class RequestSender
    {
        public static async Task<String> GetPriceListAsync(String URL)
        {
            try
            {
                HttpClient client = new HttpClient();

                // http get request to validate token
                HttpResponseMessage response = await client.GetAsync(URL);

                // make sure the http reponse is successful
                response.EnsureSuccessStatusCode();

                // convert http response to string
                string responseString = await response.Content.ReadAsStringAsync();

                response.Dispose();
                client.Dispose();

                return responseString;
            }
            catch
            {
                return null;
            }
        }
    }
}
