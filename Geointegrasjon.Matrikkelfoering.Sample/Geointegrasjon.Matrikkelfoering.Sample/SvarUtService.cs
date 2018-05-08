using no.geointegrasjon.rep.matrikkelfoering;
using KommIT.FIKS.AdapterAltinnSvarUt.Services.WS.SvarUt.Forsendelse8;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geointegrasjon.Matrikkelfoering.Sample
{
    public class SvarUtService
    {

       public void Send(ByggesakType byggesak, string sendToOrganizationNumber, string sendToName)
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
                //dokumenter = 
            };

            //TODO send forsendelse to svarut

        }


    }
}
