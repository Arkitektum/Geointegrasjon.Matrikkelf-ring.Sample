using no.geointegrasjon.rep.matrikkelfoering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geointegrasjon.Matrikkelfoering.Sample
{
    class Program
    {
        static void Main(string[] args)
        {


            //Datamodell matrikkelføring
            var byggesak = new ByggesakType();
            byggesak.adresse = "Byggestedgate 1";
            byggesak.tittel = "Rammesøknad for enebolig i Byggestedgate 1";
            byggesak.saksnummer = new SaksnummerType() { saksaar = "2018", sakssekvensnummer = "123456" };
            byggesak.vedtak = new VedtakType() { beskrivelse = "Vedtak om rammetillatelse", status = new VedtakstypeType() { kode = "1", beskrivelse = "Godkjent" }, vedtaksdato = DateTime.Now };

            byggesak.matrikkelopplysninger = new MatrikkelopplysningerType();
            byggesak.matrikkelopplysninger.adresse = new[] {
                new AdresseType(){ adressekode = "1001", adressenavn = "Byggestedgate", adressenummer = "1" } };
            byggesak.matrikkelopplysninger.eiendomsidentifikasjon = new[] {
                new MatrikkelnummerType() { kommunenummer = "9999", gaardsnummer = "1", bruksnummer = "2" } };
            byggesak.matrikkelopplysninger.bygning = new[] { new BygningType() { bebygdAreal = 110.5, bebygdArealSpecified = true, bygningstype = new BygningstypeType() { kode = "111", beskrivelse = "Enebolig" } } };
            
            //Osv


            //TODO Prossesskategori
            //TODO tiltakshaver, ansvarlig søker - Kontaktperson

            //Vedleggstyper - Situasjonsplan, 

            //Opplasting FIKS


        }
    }
}
