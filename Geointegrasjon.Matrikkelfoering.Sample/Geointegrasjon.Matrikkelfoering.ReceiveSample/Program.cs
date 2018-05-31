using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;

namespace Geointegrasjon.Matrikkelfoering.ReceiveSample
{
    class Program
    {
        /// <summary>
        /// Forsendelsetypen som mottakstjenester i FIKS filterer meldinger på for å hente matrikkelføringsbeskjeder
        /// fra eByggeSak til matrikkelklienter.
        /// </summary>
        private const string ForsendelsesTypeGeointegrasjonMatrikkel = "Geointegrasjon.Matrikkelføring";

        /// <summary>
        /// Forsendelsetypen som mottakstjenester i FIKS filterer meldinger på for å hente responsen fra matrikkelsystemet
        /// som skal tilbake til eBYggesak-systemet.
        /// </summary>
        private const string ForsendelsesTypeGeointegrasjonMatrikkelRespons = "Geointegrasjon.Matrikkelføringsrespons";

        // https://github.com/ks-no/svarut-dokumentasjon/wiki/mottaksservice-REST for dokumentasjon av APIet for SvarInn
        /// <summary>
        /// URLen man GETer fra i SvarUt for å hente ventende forsendelser.
        /// </summary>
        private const string UrlHentNyeForsendelser = "https://test.svarut.ks.no/tjenester/svarinn/mottaker/hentNyeForsendelser";

        /// <summary>
        /// URLen man POSTer til i SvarUt for å si at man har tatt imot forsendelsen
        /// </summary>
        private const string UrlMottakVelykket = "https://test.svarut.ks.no/tjenester/svarinn/kvitterMottak/forsendelse/{0}"; //+ forsendelsesid

        /// <summary>
        /// URLen man POSTer til i SvarUt for å si at man ikke kan gjøre noe nyttig med forsendelsen.
        /// </summary>
        private const string UrlMottakFeilet = "https://test.svarut.ks.no/tjenester/svarinn/mottakFeilet/forsendelse/{0}"; //+ forsendelsesid
      
        static void Main()
        {
            RunProgram().Wait();
        }

        static async Task RunProgram()
        {   
            HttpClient client = CreateClient();
            //Fetch list of waiting messages
            HttpResponseMessage response = await client.GetAsync(UrlHentNyeForsendelser);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Kunne ikke hente nye forsendelser, sjekk brukernavn og passord.");
            }

            //Download and parse incoming message
            string responseText = await response.Content.ReadAsStringAsync();
            JArray responseList = JArray.Parse(responseText);

            Console.WriteLine("Hentet liste over ventende meldinger:");
            if (responseList.Count > 0)
            {
                //For each waiting message:
                foreach (JObject waitingMessage in responseList)
                {
                    await ProcessWaitingMessage(client, waitingMessage);
                }
            }
            else
            {
                Console.WriteLine("Det er ingen ventende meldinger!");
            }
        }

        private static async Task ProcessWaitingMessage(HttpClient client, JObject waitingMessage)
        {
            MottattMelding message = new MottattMelding(waitingMessage);

            Console.WriteLine("--------------------------------");
            Console.WriteLine("Tittel: " + message.Tittel);
            Console.WriteLine("Forsendelses-ID: " + message.Id);

            //Check if it's the right type (making sure FIKS is not misconfigured)                
            if (message.Forsendelsestype != ForsendelsesTypeGeointegrasjonMatrikkel)
            {
                //If not, deny handling the message by sending a negative receipt
                Console.WriteLine("Kan ikke håndtere denne meldingen, sender  negativ håndteringsrespons");
                await DenyHandlingMessage(client, message.Id);
                return;
            }

            Console.WriteLine("Melding er av riktig forsendelsestype.");

            //If OK, download files (Always a zip in our example, can be a single PDF in other cases)

            await DownloadAndDecryptMessageFile(client, message.Id, message.DownloadUrl);

            Console.WriteLine("Melding er lastet ned og dekryptert.");

            //Send receipt that message was handled
            //TODO: commented out right now for the sake of testing without having to push new messages
            //await client.PostAsync(string.Format(UrlMottakVelykket, id), new StringContent(""));
            Console.WriteLine("Mottak av melding bekreftet til SvarInn");

            //Pretend we do something interesting with the data, then possibly send a response
            Console.WriteLine("Trykk y og enter for å sende returbeskjed med SvarUt om at matrikkelen har (liksom) blitt oppdatert, eller bare enter for å ikke gjøre det.");
            bool sendProcessedResponse = (Console.ReadLine() == "y");

            if (sendProcessedResponse)
            {
                //Send a return message that the data has been acted on
                //TODO: Implement stuff using SvarUtService here! Need to define the message format first!
                Console.WriteLine("Returbeskjed sendt, går videre til neste melding.");
            }
            else
            {
                Console.WriteLine("Går videre til neste melding uten å sende returbeskjed.");
            }
        }

        private static async Task DenyHandlingMessage(HttpClient client, string id)
        {
            string errorReceiptMessage = JsonConvert.SerializeObject(new { feilmelding = "Kan ikke håndtere forsendelsestypen.", permanent = true });
            await client.PostAsync(string.Format(UrlMottakFeilet, id), new StringContent(errorReceiptMessage));
        }

        private static async Task DownloadAndDecryptMessageFile(HttpClient client, string id, string downloadUrl)
        {
            HttpResponseMessage fileResponse = await client.GetAsync(downloadUrl);

            //Note that this only works for files that will fit into memory, so be careful with big BIMs
            byte[] filecontent = await fileResponse.Content.ReadAsByteArrayAsync();
            EnvelopedCms cmsData = new EnvelopedCms();
            cmsData.Decode(filecontent);
            //Remember to import the key as detailed in the readme
            cmsData.Decrypt();
            File.WriteAllBytes("forsendelse_" + id + ".zip", cmsData.ContentInfo.Content);
        }

        private static HttpClient CreateClient()
        {
            var client = new HttpClient();
            AuthenticationHeaderValue authHeader = CreateAuthHeader();
            client.DefaultRequestHeaders.Authorization = authHeader;
            return client;
        }

        private static AuthenticationHeaderValue CreateAuthHeader()
        {
            string mottakUserName = ConfigurationManager.AppSettings["MottakUserName"];
            string mottakPassword = ConfigurationManager.AppSettings["MottakPassword"];

            var byteArray = new UTF8Encoding().GetBytes(String.Format("{0}:{1}", mottakUserName, mottakPassword));
            AuthenticationHeaderValue authHeader = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            return authHeader;
        }

    }
}
