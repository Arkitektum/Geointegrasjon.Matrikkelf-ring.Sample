using no.geointegrasjon.rep.matrikkelfoering;
using KommIT.FIKS.AdapterAltinnSvarUt.Services.WS.SvarUt.Forsendelse8;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Geointegrasjon.Matrikkelfoering.Sample
{
    public class SvarUtService
    {

        public void Send(ByggesakType byggesak, string sendToOrganizationNumber, string sendToName, dokument[] dokumenter)
        {

            forsendelse forsendelse = new forsendelse
            {
                avgivendeSystem = "Saksbehandlingssystem",
                eksternref = byggesak.systemId,

                metadataFraAvleverendeSystem = new noarkMetadataFraAvleverendeSakssystem
                {
                    tittel = byggesak.tittel
                },
                metadataForImport = new noarkMetadataForImport
                {
                    saksaar = 2018,
                    sakssekvensnummer = 12345,
                    tittel = byggesak.tittel,
                    journalposttype = "I"
                },
                forsendelseType = "Geointegrasjon.Matrikkelføring",
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
                tittel = byggesak.tittel,
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
