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
            noarkMetadataForImport metadataForImport = null;
            if (byggesak.saksnummer != null)
            {
                metadataForImport = new noarkMetadataForImport
                {
                    saksaar = Convert.ToInt32(byggesak.saksnummer.saksaar),
                    sakssekvensnummer = Convert.ToInt32(byggesak.saksnummer.saksaar),
                    journalposttype = "I"
                };
            }

            forsendelse forsendelse = new forsendelse
            {
                avgivendeSystem = "Saksbehandlingssystem",
                eksternref = byggesak.systemId,
                metadataForImport = metadataForImport,
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
