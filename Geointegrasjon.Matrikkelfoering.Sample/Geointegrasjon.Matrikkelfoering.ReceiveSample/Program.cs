using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.Pkcs;
using Newtonsoft.Json;
using Geointegrasjon.Matrikkelfoering.SendSample;
using no.geointegrasjon.rep.matrikkelfoeringsrespons;
using no.geointegrasjon.rep.matrikkelfoering;
using KommIT.FIKS.AdapterAltinnSvarUt.Services.WS.SvarUt.Forsendelse8;

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
            Console.ReadLine();
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
            List<Forsendelse> responseList = JsonConvert.DeserializeObject<List<Forsendelse>>(responseText);

            Console.WriteLine("Hentet liste over ventende meldinger:");
            if (responseList.Count > 0)
            {
                //For each waiting message:
                foreach (Forsendelse waitingMessage in responseList)
                {
                    await ProcessWaitingMessage(client, waitingMessage);
                }
            }
            else
            {
                Console.WriteLine("Det er ingen ventende meldinger!");
            }
        }

        private static async Task ProcessWaitingMessage(HttpClient client, Forsendelse message)
        {
            Console.WriteLine("--------------------------------");
            Console.WriteLine("Tittel: " + message.tittel);
            Console.WriteLine("Forsendelses-ID: " + message.id);

            //Check if it's the right type (making sure FIKS is not misconfigured)                
            if (message.forsendelseType != ForsendelsesTypeGeointegrasjonMatrikkel)
            {
                //If not, deny handling the message by sending a negative receipt
                Console.WriteLine("Kan ikke håndtere denne meldingen, sender  negativ håndteringsrespons");
                await DenyHandlingMessage(client, message.id);
                return;
            }

            Console.WriteLine("Melding er av riktig forsendelsestype.");

            //If OK, download files (Always a zip in our example, can be a single PDF in other cases)

            await DownloadAndDecryptMessageFile(client, message.id, message.downloadUrl);
            ByggesakType byggesak = await readByggeSakXml(message);

            Console.WriteLine("Melding er lastet ned, dekryptert og innlest.");

            //Send receipt that message was handled
            await client.PostAsync(string.Format(UrlMottakVelykket, message.id), new StringContent(""));
            Console.WriteLine("Mottak av melding bekreftet til SvarInn");

            //Pretend we do something interesting with the data, then possibly send a response
            Console.WriteLine("Trykk y og enter for å sende returbeskjed med SvarUt om at matrikkelen har (liksom) blitt oppdatert, eller bare enter for å ikke gjøre det.");
            bool sendProcessedResponse = (Console.ReadLine() == "y");

            if (sendProcessedResponse)
            {
                //Send a return message that the data has been acted on                
                SendMatrikkelFoeringsResponse(message, byggesak);
                Console.WriteLine("Returbeskjed sendt, går videre til neste melding.");
            }
            else
            {
                Console.WriteLine("Går videre til neste melding uten å sende returbeskjed.");
            }
        }

        private static void SendMatrikkelFoeringsResponse(Forsendelse message, ByggesakType byggesak)
        {
            string responseXml = PrepareResponseXML(message, byggesak);

            List<dokument> dokumenter = new List<dokument>();
            dokument matrikkelRespons = new dokument()
            {
                dokumentType = "ByggesakMatrikkelFøringsRespons",
                data = System.Text.Encoding.UTF8.GetBytes(responseXml),
                filnavn = "matrikkelrespons.xml",
                mimetype = "application/xml"
            };
            dokumenter.Add(matrikkelRespons);
            
            string orgnrTilKommunen = ConfigurationManager.AppSettings["OrgNrReceiver"];
            SvarUtService.Send("matrikkelsystem", ForsendelsesTypeGeointegrasjonMatrikkelRespons, "Matrikkelføringsrespons for " + message.tittel, "12345", orgnrTilKommunen, "Matrikkelføringsrespons klient", dokumenter.ToArray());
        }

        private static string PrepareResponseXML(Forsendelse message, ByggesakType byggesak)
        {
            string[] bygningsNummer;
            if (byggesak.matrikkelopplysninger == null)
            {
                //We make up something to pretend we've added a new building
                Random rnd = new Random();
                bygningsNummer = new string[] { rnd.Next(10000, 100000) .ToString() };
            }
            else
            {
                bygningsNummer = byggesak.matrikkelopplysninger.bygning.Select(bygning => bygning.bygningsnummer).ToArray();
            }

            MatrikkelFoeringsResponsType responseMessage = new MatrikkelFoeringsResponsType()
            {
                bygningsnummer = bygningsNummer,
                saksnummer = new no.geointegrasjon.rep.matrikkelfoeringsrespons.SaksnummerType()
                {
                    //Tostring because XSD.exe maps xml:integer to string because it's not bounded like int32
                    //TODO: Check assumption that ids may be larger than int32
                    saksaar = message.metadataFraAvleverendeSystem.saksaar.ToString(),
                    sakssekvensnummer = message.metadataFraAvleverendeSystem.sakssekvensnummer.ToString()
                }
            };
            return writeMatrikkelFoeringsResponsXML(responseMessage);
        }

        private static async Task<ByggesakType> readByggeSakXml(Forsendelse message)
        {
            string byggesakXML = await ReadByggesakXMLFromZip(message);
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(ByggesakType));
            using (var stringReader = new StringReader(byggesakXML))
            {
                return (ByggesakType)serializer.Deserialize(stringReader);
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

        private static async Task<string> ReadByggesakXMLFromZip(Forsendelse message)
        {
            //Get the filename from metadata

            //Read the file to extract the XML
            using (FileStream zipToOpen = new FileStream("forsendelse_" + message.id + ".zip", FileMode.Open))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    //TODO: byggesak.xml from dkument type!!!!
                    ZipArchiveEntry entry = archive.GetEntry("byggesak.xml");
                    using (StreamReader reader = new StreamReader(entry.Open()))
                    {
                        return await reader.ReadToEndAsync();
                    }
                }
            }
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

        private static string writeMatrikkelFoeringsResponsXML(MatrikkelFoeringsResponsType respons)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(respons.GetType());
            var stringWriter = new Utf8StringWriter();
            serializer.Serialize(stringWriter, respons);
            string xml = stringWriter.ToString();
            return xml;
        }
    }
}
