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
        private const string UrlMottakVelykket = "https://svarut.ks.no/tjenester/svarinn/kvitterMottak/forsendelse/{0}"; //+ forsendelsesid

        /// <summary>
        /// URLen man POSTer til i SvarUt for å si at man ikke kan gjøre noe nyttig med forsendelsen.
        /// </summary>
        private const string UrlMottakFeilet = "https://svarut.ks.no/tjenester/svarinn/mottakFeilet/forsendelse/{0}"; //+ forsendelsesid

        /// <summary>
        /// JSON man POSTer for å si ifra om at man ikke kan håndtere en melding som er kommet inn fordi den ikke er Geointegrasjon.Matrikkelføring
        /// </summary>
        private const string ErrorReceiptMessage = "{ \"feilmelding\":\"Kan ikke håndtere forsendelsestypen.\", \"permanent\":true}";

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
            //For each waiting message:
            foreach (JObject waitingMessage in responseList)
            {
                string tittel = (string)waitingMessage["tittel"];
                string id = (string)waitingMessage["id"];
                string downloadUrl = (string)waitingMessage["downloadUrl"];

                Console.WriteLine("--------------------------------");
                Console.WriteLine("Tittel: " + tittel);
                Console.WriteLine("Forsendelses-ID: " + id);

                //Check if it's the right type (making sure FIKS is not misconfigured)                
                if ((string)waitingMessage["forsendelseType"] != ForsendelsesTypeGeointegrasjonMatrikkel)
                {
                    //If not, deny handling the message by sending a negative receipt
                    Console.WriteLine("Kan ikke håndtere denne meldingen, sender  negativ håndteringsrespons");
                    await client.PostAsync(string.Format(UrlMottakFeilet, id), new StringContent(ErrorReceiptMessage));

                    continue;
                }

                Console.WriteLine("Melding er av riktig forsendelsestype.");

                //If OK, download files (Always a zip in our example, can be PDF)
                using (FileStream stream = new FileStream("forsendelse_" + id, FileMode.Create))
                {
                    HttpResponseMessage fileResponse = await client.GetAsync(downloadUrl);
                    await fileResponse.Content.CopyToAsync(stream);
                }

                //TODO: Decrypt with certificate (also add to instructions about generating and uploading cert)

                Console.WriteLine("Melding er lastet ned.");

                //Send receipt that message was handled
                //TODO: commented out right now for the sake of testing without having to push new meassages
                //await client.PostAsync(string.Format(UrlMottakVelykket, id), new StringContent(""));

                //Pretend we do something interesting with the data, then press any key
                Console.WriteLine("Trykk enter for å sende beskjed om at matrikkelen har (liksom) blitt oppdatert.");
                Console.ReadLine();

                //Send a return message that the data has been acted on
                //TODO: Implement stuff using SvarUtService here!


                Console.WriteLine("Trykk enter for å gå videre til neste melding.");
                Console.ReadLine();

            }
        }

        private static HttpClient CreateClient()
        {
            var client = new HttpClient();
            AuthenticationHeaderValue authHeader = createAuthHeader();
            client.DefaultRequestHeaders.Authorization = authHeader;
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
