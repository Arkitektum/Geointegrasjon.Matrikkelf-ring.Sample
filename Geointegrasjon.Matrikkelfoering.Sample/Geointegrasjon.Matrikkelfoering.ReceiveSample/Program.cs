using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Geointegrasjon.Matrikkelfoering.ReceiveSample
{
    class Program
    {
        static void Main()
        {
            Task t = new Task(RunProgram);
            t.Start();
            Console.ReadLine();
        }

        static async void RunProgram()
        { 
            HttpClient client = CreateClient();
            //Fetch list of waiting messages
            var fetchWaitingMessagesRequest = new HttpRequestMessage(HttpMethod.Get, "/mottaker/hentNyeForsendelser");
            AuthenticationHeaderValue authHeader = createAuthHeader();
            fetchWaitingMessagesRequest.Headers.Authorization = authHeader;
            HttpResponseMessage response = await client.SendAsync(fetchWaitingMessagesRequest);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Kunne ikke hente nye forsendelser, sjekk brukernavn og passord.");
            }
            Console.WriteLine("Hentet liste over ventende meldinger:");
            //For each waiting message:
            Console.WriteLine(response.ToString());
            //Check if it's the right type (making sure FIKS is not misconfigured)
            //If not, deny handling the message

            //If OK, download files (Always a zip in our example, can be PDF)
            //Send receipt that message was handled
        }

        private static HttpClient CreateClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://test.svarut.ks.no/tjenester/svarinn");

            //AuthenticationHeaderValue authHeader = createAuthHeader();
            //client.DefaultRequestHeaders.Authorization = authHeader;
            return client;
        }

        private static AuthenticationHeaderValue createAuthHeader()
        {
            string mottakUserName = ConfigurationManager.AppSettings["MottakUserName"];
            string mottakPassword = ConfigurationManager.AppSettings["MottakPassword"];

            var byteArray = new UTF8Encoding().GetBytes(String.Format("{0}:{1}", mottakUserName, mottakPassword));
            AuthenticationHeaderValue authHeader = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            return authHeader;
        }
    }
}
