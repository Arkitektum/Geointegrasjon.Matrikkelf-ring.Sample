using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using no.geointegrasjon.rep.matrikkelfoering;

namespace Geointegrasjon.Matrikkelfoering.Sample
{
    class GenerateN2
    {
        public ByggesakType GenerateSample()
        {
            // Lars
            var byggesak = new ByggesakType();
            byggesak.adresse = "Byggestedgate 1";
            byggesak.tittel = "Igangsettingstillatelse for enebolig i Byggestedgate 1";
            byggesak.saksnummer = new SaksnummerType() { saksaar = "2018", sakssekvensnummer = "123456" };
            byggesak.kategori = new ProsesskategoriType() { kode = "IS", beskrivelse = "Søknad om igangsettingstillatelse" };
            byggesak.tiltakstype = new[]
            {
                new TiltaktypeType() {kode = "nyttbyggboligformal", beskrivelse = "Nytt bygg - boligformål"}
            };
            byggesak.vedtak = new VedtakType()
            {
                beskrivelse = "Vedtak om igangsettingstillatelse",
                status = new VedtakstypeType() { kode = "1", beskrivelse = "Godkjent" },
                vedtaksdato = DateTime.Now
            };

            byggesak.matrikkelopplysninger = new MatrikkelopplysningerType();
            byggesak.matrikkelopplysninger.adresse = new[]
            {
                new AdresseType() {adressekode = "1001", adressenavn = "Byggestedgate", adressenummer = "1"}
            };
            byggesak.matrikkelopplysninger.eiendomsidentifikasjon = new[]
            {
                new MatrikkelnummerType() {kommunenummer = "9999", gaardsnummer = "1", bruksnummer = "2"}
            };
            byggesak.matrikkelopplysninger.bygning = new[]
            {
                new BygningType()
                {
                    bebygdAreal = 110.5,
                    bebygdArealSpecified = true,
                    bygningstype = new BygningstypeType() {kode = "111", beskrivelse = "Enebolig"}
                }
            };

            return byggesak;
        }

    }
}
