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

        static void Main(string[] args)
        {
            // The args parameter can be uses to process the last n messages, if not set, all messages are handled,
            // e.g.5 will process the last 5 messages.
            RunProgram(args).Wait();
            Console.ReadLine();
        }

        static async Task RunProgram(string[] args=null)
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
                int processLastMessages = 0;
                if (args.Length == 1)
                {
                    if (!Int32.TryParse(args[0], out processLastMessages))
                    {
                        processLastMessages = 0;
                    }
                }
                int idx = 0;
                int startIdx = 0;
                if (processLastMessages > 0)
                {
                    startIdx = responseList.Count - processLastMessages;
                    if (startIdx < 0)
                    {
                        startIdx = 0;
                    }
                }
                //For each waiting message:
                foreach (Forsendelse waitingMessage in responseList)
                {
                    if (idx >= startIdx)
                    {
                        await ProcessWaitingMessage(client, waitingMessage);
                    }
                    ++idx;
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
                //await DenyHandlingMessage(client, message.id); // denne tar tid
                return;
            }



            Console.WriteLine("Melding er av riktig forsendelsestype. ID:" + message.id);

            if (false)
            {
                if (message.id != "1815c285-1226-4b2f-ad66-731bfff24578")
                {
                    Console.WriteLine("Test på filter med ID. Kan ikke håndtere denne meldingen, sender  negativ håndteringsrespons");
                    //await DenyHandlingMessage(client, message.id); // denne tar tid
                    return;
                }
            }

            // The message.Date is of type Unixtime 
            Console.WriteLine("Meldingsdato:" + UnixTimeToDateTime(message.date));
            //dateTime dt = new DateTime(message.date);

            //If OK, download files (Always a zip in our example, can be a single PDF in other cases)

            await DownloadAndDecryptMessageFile(client, message.id, message.downloadUrl);
            ByggesakType byggesak = await readByggeSakXml(message);

            Console.WriteLine("Melding er lastet ned, dekryptert og innlest.");

            // Ikke send mottat melding her
            if (false)
            {
                //Send receipt that message was handled
                await client.PostAsync(string.Format(UrlMottakVelykket, message.id), new StringContent(""));
                Console.WriteLine("Mottak av melding bekreftet til SvarInn");
            }

            //Pretend we do something interesting with the data, then possibly send a response
            Console.WriteLine("Trykk y og enter for å sende returbeskjed med SvarUt om at matrikkelen har (liksom) blitt oppdatert, c for å avbryte, eller bare enter for å ikke gjøre det.");
            string responseConsole = Console.ReadLine();
            bool sendProcessedResponse = (responseConsole == "y");
            bool cancelProcessResponse = (responseConsole == "c");

            if (sendProcessedResponse)
            {
                if (true)
                {
                    //Send receipt that message was handled
                    await client.PostAsync(string.Format(UrlMottakVelykket, message.id), new StringContent(""));
                    Console.WriteLine("Mottak av melding bekreftet til SvarInn");
                    //await Task.Delay(100).ConfigureAwait(false); // TEST
                }

                //Send a return message that the data has been acted on                
                SendMatrikkelFoeringsResponse(message, byggesak);
                Console.WriteLine("Returbeskjed sendt, går videre til neste melding.");
            }
            else if (cancelProcessResponse)
            {
                // Simulerer at en sak er lastet ned, men bruker ønsker å avbryte.
                // En kopi av saken sendes da inn på nytt i mangel av metode for å gjøre dette mot SvarUt/SvarInn,
                if (true)
                {
                    //Send receipt that message was handled
                    await client.PostAsync(string.Format(UrlMottakVelykket, message.id), new StringContent(""));
                    Console.WriteLine("Mottak av melding bekreftet til SvarInn");
                    //await Task.Delay(100).ConfigureAwait(false); // TEST
                }

                //Send a return message that the data has cancelled               
                CopyMessageAndSendInAsNew(message);
                Console.WriteLine("Returbeskjed sendt, går videre til neste melding.");
            }
            else
            {
                Console.WriteLine("Går videre til neste melding uten å sende returbeskjed.");
                //await DenyHandlingMessage(client, message.id); // denne tar tid
           
            }
        }

        /// <summary>
        /// Convert Unix time value to a DateTime object.
        /// </summary>
        /// <param name="unixTime">The Unix time stamp you want to convert to DateTime.</param>
        /// <returns>Returns a DateTime object that represents value of the Unix time.</returns>
        public static DateTime UnixTimeToDateTime(long unixTime)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixTime).ToLocalTime();
            return dtDateTime;
        }

        /// <summary>
        /// Kopierer en eksisterende melding med dokument, og sender den inn på ny.
        /// Dette for å kunne låse en sak i en matrikkelklient, og frigjøre den dersom bruker avbryter.
        /// </summary>
        /// <param name="message"></param>
        private static void CopyMessageAndSendInAsNew(Forsendelse message)
        {
            List<dokument> dokumenter = ReadDocumentsAsync(message).GetAwaiter().GetResult();
            
            string orgnrTilKommunen = ConfigurationManager.AppSettings["OrgNrReceiver"];
            var saksaar = message.metadataForImport.saksaar.ToString();
            var sakssekvensnummer = message.metadataForImport.sakssekvensnummer.ToString();

            //message.
            string systemId = null;
            systemId = message.id;

            string avgiverSystem = message.avsender.navn;
            //avgiverSystem = "matrikkelsystem";
            avgiverSystem = "eByggesak system";
            SvarUtService.Send(avgiverSystem, ForsendelsesTypeGeointegrasjonMatrikkel, message.tittel, systemId,
                orgnrTilKommunen, "Matrikkelføring klient", dokumenter.ToArray(),
               saksaar, sakssekvensnummer);

        }
        
        /// <summary>
        /// Leser inn alle dokumenter i en sak
        /// </summary>
        /// <param name="message"></param>
        /// <returns>En liste med dokumenter</returns>
        private static async Task<List<dokument>> ReadDocumentsAsync(Forsendelse message)
        {
            if (message.filmetadata.Count > 0)
            {
                List<dokument> dokumenter = new List<dokument>();

                //Read the file to extract the XML
                using (FileStream zipToOpen = new FileStream("forsendelse_" + message.id + ".zip", FileMode.Open))
                {
                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                    {
                        foreach (var fil in message.filmetadata)
                        {
                            dokument doc = new dokument();
                            doc.dokumentType = fil.dokumentType;
                            doc.mimetype = fil.mimetype;
                            doc.filnavn = fil.filnavn;

                            ZipArchiveEntry entry = archive.GetEntry(fil.filnavn);
                            using (StreamReader reader = new StreamReader(entry.Open()))
                            {
                                var response =  await reader.ReadToEndAsync();
                                doc.data = System.Text.Encoding.UTF8.GetBytes(response);
                            }
                            dokumenter.Add(doc);
                        }
                    }
                }
                return dokumenter;
            }
            else
            {
                return null;
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
            // LARS: saksaar & sakssekvensnummer from Byggesak
            string systemId = message.id; //byggesak.systemId; //TODO: Check
            SvarUtService.Send("matrikkelsystem", ForsendelsesTypeGeointegrasjonMatrikkelRespons, "Matrikkelføringsrespons for " + message.tittel, systemId,
                orgnrTilKommunen, "Matrikkelføringsrespons klient", dokumenter.ToArray(),
                byggesak.saksnummer.saksaar, byggesak.saksnummer.sakssekvensnummer);
            //             SvarUtService.Send("matrikkelsystem", ForsendelsesTypeGeointegrasjonMatrikkelRespons, "Matrikkelføringsrespons for " + message.tittel, "12345", orgnrTilKommunen, "Matrikkelføringsrespons klient", dokumenter.ToArray(),
            // byggesak.saksnummer.saksaar, byggesak.saksnummer.sakssekvensnummer);

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

                if (bygningsNummer[0] == null)
                {
                    //We make up something to pretend we've added a new building
                    Random rnd = new Random();
                    bygningsNummer = new string[] { rnd.Next(10000, 100000).ToString() };
                }
            }

            MatrikkelFoeringsResponsType responseMessage = new MatrikkelFoeringsResponsType()
            {
                bygningsnummer = bygningsNummer,
                saksnummer = new no.geointegrasjon.rep.matrikkelfoeringsrespons.SaksnummerType()
                {
                 

                    //Tostring because XSD.exe maps xml:integer to string because it's not bounded like int32
                    //TODO: Check assumption that ids may be larger than int32
                    saksaar = message.metadataForImport.saksaar.ToString(), //saksaar = byggesak.saksnummer.saksaar, //message.metadataFraAvleverendeSystem.saksaar.ToString(),
                    sakssekvensnummer = message.metadataForImport.sakssekvensnummer.ToString() //sakssekvensnummer = byggesak.saksnummer.sakssekvensnummer //message.metadataFraAvleverendeSystem.sakssekvensnummer.ToString()
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
