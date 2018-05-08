using no.geointegrasjon.rep.matrikkelfoering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geointegrasjon.Matrikkelfoering.Sample
{
    class GenerateN1
    {
        public ByggesakType GenerateSample()
        {

            //Nivå 0 - kun beskjed om godkjent vedtak på rammesøknad med saksnummer - matrikkelfører må selv finne korrekt underlag i saken
            var byggesak = new ByggesakType();
            byggesak.adresse = "Byggestedgate 1";
            byggesak.tittel = "Rammesøknad for enebolig i Byggestedgate 1";
            byggesak.saksnummer = new SaksnummerType() { saksaar = "2018", sakssekvensnummer = "123456" };
            byggesak.kategori = new ProsesskategoriType() { kode = "RS", beskrivelse = "Søknad om rammetillatelse" };
            byggesak.tiltakstype = new[] { new TiltaktypeType() { kode = "nyttbyggboligformal", beskrivelse = "Nytt bygg - boligformål" } };
            byggesak.vedtak = new VedtakType() { beskrivelse = "Vedtak om rammetillatelse", status = new VedtakstypeType() { kode = "1", beskrivelse = "Godkjent" }, vedtaksdato = DateTime.Now };

            
            return byggesak;
        }

        public ByggesakType GenerateSample1()
        {

            //Nivå 0 - kun beskjed om godkjent vedtak på endringssøknad med saksnummer - matrikkelfører må selv finne korrekt underlag i saken
            var byggesak = new ByggesakType();
            byggesak.adresse = "Byggestedgate 1";
            byggesak.tittel = "Endringssøknad for enebolig i Byggestedgate 1";
            byggesak.saksnummer = new SaksnummerType() { saksaar = "2018", sakssekvensnummer = "123456" };
            byggesak.kategori = new ProsesskategoriType() { kode = "ES", beskrivelse = "Søknad om endring av tillatelse" };
            byggesak.tiltakstype = new[] { new TiltaktypeType() { kode = "nyttbyggboligformal", beskrivelse = "Nytt bygg - boligformål" } };
            byggesak.vedtak = new VedtakType() { beskrivelse = "Vedtak om endring av tillatelse", status = new VedtakstypeType() { kode = "1", beskrivelse = "Godkjent" }, vedtaksdato = DateTime.Now };


            return byggesak;
        }


        public ByggesakType GenerateSample2()
        {

            //Nivå 0 - kun beskjed om godkjent vedtak på igangsettingsøknad med saksnummer - matrikkelfører må selv finne korrekt underlag i saken
            var byggesak = new ByggesakType();
            byggesak.adresse = "Byggestedgate 1";
            byggesak.tittel = "Igangsettingssøknad for enebolig i Byggestedgate 1 - byggetrinn 1";
            byggesak.saksnummer = new SaksnummerType() { saksaar = "2018", sakssekvensnummer = "123456" };
            byggesak.kategori = new ProsesskategoriType() { kode = "IG", beskrivelse = "Søknad om igangsettingstillatelse" };
            byggesak.tiltakstype = new[] { new TiltaktypeType() { kode = "nyttbyggboligformal", beskrivelse = "Nytt bygg - boligformål" } };
            byggesak.vedtak = new VedtakType() { beskrivelse = "Vedtak om igangsettingstillatelse av byggetrinn 1", status = new VedtakstypeType() { kode = "1", beskrivelse = "Godkjent" }, vedtaksdato = DateTime.Now };


            return byggesak;
        }

        public ByggesakType GenerateSample3()
        {

            //Nivå 0 - kun beskjed om godkjent vedtak på igangsettingsøknad med saksnummer - matrikkelfører må selv finne korrekt underlag i saken
            var byggesak = new ByggesakType();
            byggesak.adresse = "Byggestedgate 1";
            byggesak.tittel = "Igangsettingssøknad for enebolig i Byggestedgate 1 - byggetrinn 2";
            byggesak.saksnummer = new SaksnummerType() { saksaar = "2018", sakssekvensnummer = "123456" };
            byggesak.kategori = new ProsesskategoriType() { kode = "IG", beskrivelse = "Søknad om igangsettingstillatelse" };
            byggesak.tiltakstype = new[] { new TiltaktypeType() { kode = "nyttbyggboligformal", beskrivelse = "Nytt bygg - boligformål" } };
            byggesak.vedtak = new VedtakType() { beskrivelse = "Vedtak om igangsettingstillatelse av byggetrinn 2", status = new VedtakstypeType() { kode = "1", beskrivelse = "Godkjent" }, vedtaksdato = DateTime.Now };


            return byggesak;
        }

        public ByggesakType GenerateSample4()
        {

            //Nivå 0 - kun beskjed om godkjent vedtak på midlertidig brukstillatelse med saksnummer - matrikkelfører må selv finne korrekt underlag i saken
            var byggesak = new ByggesakType();
            byggesak.adresse = "Byggestedgate 1";
            byggesak.tittel = "Midlertidig brukstillatelse for enebolig i Byggestedgate 1";
            byggesak.saksnummer = new SaksnummerType() { saksaar = "2018", sakssekvensnummer = "123456" };
            byggesak.kategori = new ProsesskategoriType() { kode = "MB", beskrivelse = "Søknad om midlertidig brukstillatelse" };
            byggesak.tiltakstype = new[] { new TiltaktypeType() { kode = "nyttbyggboligformal", beskrivelse = "Nytt bygg - boligformål" } };
            byggesak.vedtak = new VedtakType() { beskrivelse = "Vedtak om midlertidig brukstillatelse", status = new VedtakstypeType() { kode = "1", beskrivelse = "Godkjent" }, vedtaksdato = DateTime.Now };


            return byggesak;
        }

        public ByggesakType GenerateSample5()
        {

            //Nivå 0 - kun beskjed om godkjent vedtak på ferdigattest med saksnummer - matrikkelfører må selv finne korrekt underlag i saken
            var byggesak = new ByggesakType();
            byggesak.adresse = "Byggestedgate 1";
            byggesak.tittel = "Ferdigattest for enebolig i Byggestedgate 1";
            byggesak.saksnummer = new SaksnummerType() { saksaar = "2018", sakssekvensnummer = "123456" };
            byggesak.kategori = new ProsesskategoriType() { kode = "FA", beskrivelse = "Søknad om ferdigattest" };
            byggesak.tiltakstype = new[] { new TiltaktypeType() { kode = "nyttbyggboligformal", beskrivelse = "Nytt bygg - boligformål" } };
            byggesak.vedtak = new VedtakType() { beskrivelse = "Vedtak om ferdigattest", status = new VedtakstypeType() { kode = "1", beskrivelse = "Godkjent" }, vedtaksdato = DateTime.Now };


            return byggesak;
        }
    }
}
