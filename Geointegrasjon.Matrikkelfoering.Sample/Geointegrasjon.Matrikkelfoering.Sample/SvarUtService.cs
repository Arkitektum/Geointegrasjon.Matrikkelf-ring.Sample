using no.geointegrasjon.rep.matrikkelfoering;
using KommIT.FIKS.AdapterAltinnSvarUt.Services.WS.SvarUt.Forsendelse8;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Geointegrasjon.Matrikkelfoering.SendSample
{
    public class SvarUtService
    {
        /// <summary>
        /// Forsendelsetypen som mottakstjenester i FIKS filterer meldinger på for å hente matrikkelføringsbeskjeder.
        /// </summary>
        private const string ForsendelsesTypeGeointegrasjonMatrikkel = "Geointegrasjon.Matrikkelføring";

        public void Send(ByggesakType byggesak, string sendToOrganizationNumber, string sendToName, dokument[] dokumenter)
        {
            string tittel = byggesak.tittel;
            string systemId = byggesak.systemId;

            // LARS: saksaar & sakssekvensnummer from Byggesak
            Send(tittel, systemId, sendToOrganizationNumber, sendToName, dokumenter,byggesak.saksnummer.saksaar, byggesak.saksnummer.sakssekvensnummer);

        }

        public static void Send(string tittel, string systemId, string sendToOrganizationNumber, string sendToName, dokument[] dokumenter,
            string saksAar, string saksSekvensnummer)
        {
            string avgiverSystem = "eByggesak system";
            string forsendelseType = ForsendelsesTypeGeointegrasjonMatrikkel;

            Send(avgiverSystem, forsendelseType, tittel, systemId, sendToOrganizationNumber, sendToName, dokumenter, saksAar, saksSekvensnummer);
        }

        public static void Send(string avgiverSystem, string forsendelseType, string tittel, string systemId, string sendToOrganizationNumber, string sendToName, dokument[] dokumenter,
            string saksAar="2018", string saksSekvensnummer="12345")
        {
            forsendelse forsendelse = new forsendelse
            {
                avgivendeSystem = avgiverSystem,
                eksternref = systemId,

                metadataFraAvleverendeSystem = new noarkMetadataFraAvleverendeSakssystem
                {
                    tittel = tittel
                },
                metadataForImport = new noarkMetadataForImport //// LARS: saksaar & sakssekvensnummer from Byggesak
                {
                    saksaar = Convert.ToInt32(saksAar), // saksaar = 2018,
                    sakssekvensnummer = Convert.ToInt32(saksSekvensnummer), // sakssekvensnummer = 12345,
                    tittel = tittel,
                    journalposttype = "I"
                },
                forsendelseType = forsendelseType,
                mottaker = new adresse()
                {
                    digitalAdresse = new organisasjonDigitalAdresse()
                    {
                        orgnr = sendToOrganizationNumber
                    },
                    postAdresse = new postAdresse()
                    {
                        navn = sendToName,
                        postnr = "9999",
                        poststed = "Digital levering"
                    }

                },
                kunDigitalLevering = true,
                tittel = tittel,
                dokumenter = dokumenter
            };



            using (var client = GetWebServiceClient())
            {

                try
                {
                    string forsendelseResponse = client.sendForsendelse(forsendelse);
                }

                catch (Exception e)
                {
                    throw e;
                }

            }
        }

        private static ForsendelsesServiceV8Client GetWebServiceClient()
        {
            var client = new ForsendelsesServiceV8Client
            {
                ClientCredentials =
                {
                    UserName =
                    {
                        UserName = ConfigurationManager.AppSettings["SvarUtUsername"],
                        Password = ConfigurationManager.AppSettings["SvarUtPassword"]
                    }
                }
            };
            return client;
        }
    }
}
