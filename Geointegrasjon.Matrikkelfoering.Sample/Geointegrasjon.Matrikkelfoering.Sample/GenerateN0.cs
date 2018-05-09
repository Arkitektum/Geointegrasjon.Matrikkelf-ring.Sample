using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using no.geointegrasjon.rep.matrikkelfoering;

namespace Geointegrasjon.Matrikkelfoering.Sample
{
    /// <summary>
    /// Eksempler på Nivå 0 forsendelser fra eByggesak til Matrikkelføring
    /// Det refereres til aktuelle Brukstilfeller matrikkel se https://www.test.matrikkel.no:7004/matrikkel/docs/SamlaSystemspesifikasjonVer.3.12.pdf
    /// </summary>
    class GenerateN0
    {
        /// <summary>
        ///Nivå 0 - kun beskjed om godkjent vedtak på rammesøknad med saksnummer - matrikkelfører må selv finne korrekt underlag i saken
        ///Brukstilfelle matrikkel - 8.6.1 Nybygg, nytt bygg - rammetillatelse gitt
        ///Søknaden må etterfølges med minst en igangsettingssøknad
        ///Mer om søknaden - https://dibk-utvikling.atlassian.net/wiki/spaces/FB/pages/31129620/S+knad+om+rammetillatelse
        /// </summary>
        /// <returns></returns>
        public ByggesakType GenerateSample()
        {
            var byggesak = new ByggesakType();
            byggesak.adresse = "Byggestedgate 1";
            byggesak.tittel = "Rammesøknad for enebolig i Byggestedgate 1";
            byggesak.saksnummer = new SaksnummerType() { saksaar = "2018", sakssekvensnummer = "123456" };
            byggesak.kategori = new ProsesskategoriType() { kode = "RS", beskrivelse = "Søknad om rammetillatelse" };
            byggesak.tiltakstype = new[] { new TiltaktypeType() { kode = "nyttbyggboligformal", beskrivelse = "Nytt bygg - boligformål" } };
            byggesak.vedtak = new VedtakType() { beskrivelse = "Vedtak om rammetillatelse", status = new VedtakstypeType() { kode = "1", beskrivelse = "Godkjent" }, vedtaksdato = DateTime.Now };


            return byggesak;
        }

        /// <summary>
        ///Nivå 0 - kun beskjed om godkjent vedtak på endringssøknad med saksnummer - matrikkelfører må selv finne korrekt underlag i saken
        ///Endringssøknad kan komme i alle stadier etter den initielle søknaden (feks ramme, ett trinn eller tiltak uten ansvarsrett)
        ///Brukstilfelle matrikkel - 8.6.12 Endre bygningsdata
        ///Mer om søknaden - https://dibk-utvikling.atlassian.net/wiki/spaces/FB/pages/133365793/S+knad+om+endring+av+gitt+tillatelse
        /// </summary>
        /// <returns></returns>
        public ByggesakType GenerateSample1()
        {
            var byggesak = new ByggesakType();
            byggesak.adresse = "Byggestedgate 1";
            byggesak.tittel = "Endringssøknad for enebolig i Byggestedgate 1";
            byggesak.saksnummer = new SaksnummerType() { saksaar = "2018", sakssekvensnummer = "123456" };
            byggesak.kategori = new ProsesskategoriType() { kode = "ES", beskrivelse = "Søknad om endring av tillatelse" };
            byggesak.tiltakstype = new[] { new TiltaktypeType() { kode = "nyttbyggboligformal", beskrivelse = "Nytt bygg - boligformål" } };
            byggesak.vedtak = new VedtakType() { beskrivelse = "Vedtak om endring av tillatelse", status = new VedtakstypeType() { kode = "1", beskrivelse = "Godkjent" }, vedtaksdato = DateTime.Now };


            return byggesak;
        }


        /// <summary>
        ///Nivå 0 - kun beskjed om godkjent vedtak på igangsettingsøknad med saksnummer - matrikkelfører må selv finne korrekt underlag i saken
        ///Brukstilfelle matrikkel - 8.6.2 Nybygg, nytt bygg - igangsettingstillatelse gitt
        ///Mer om søknaden - https://dibk-utvikling.atlassian.net/wiki/spaces/FB/pages/20021280/S+knad+om+igangsettingstillatelse
        /// </summary>
        /// <returns></returns>
        public ByggesakType GenerateSample2()
        {
            var byggesak = new ByggesakType();
            byggesak.adresse = "Byggestedgate 1";
            byggesak.tittel = "Igangsettingssøknad for enebolig i Byggestedgate 1 - byggetrinn 1";
            byggesak.saksnummer = new SaksnummerType() { saksaar = "2018", sakssekvensnummer = "123456" };
            byggesak.kategori = new ProsesskategoriType() { kode = "IG", beskrivelse = "Søknad om igangsettingstillatelse" };
            byggesak.tiltakstype = new[] { new TiltaktypeType() { kode = "nyttbyggboligformal", beskrivelse = "Nytt bygg - boligformål" } };
            byggesak.vedtak = new VedtakType() { beskrivelse = "Vedtak om igangsettingstillatelse av byggetrinn 1", status = new VedtakstypeType() { kode = "1", beskrivelse = "Godkjent" }, vedtaksdato = DateTime.Now };

            return byggesak;
        }

        /// <summary>
        ///Nivå 0 - kun beskjed om godkjent vedtak på igangsettingsøknad med saksnummer - matrikkelfører må selv finne korrekt underlag i saken
        ///Brukstilfelle matrikkel - 8.6.2 Nybygg, nytt bygg - igangsettingstillatelse gitt
        ///Mer om søknaden - https://dibk-utvikling.atlassian.net/wiki/spaces/FB/pages/20021280/S+knad+om+igangsettingstillatelse
        /// </summary>
        /// <returns></returns>
        public ByggesakType GenerateSample3()
        {
            var byggesak = new ByggesakType();
            byggesak.adresse = "Byggestedgate 1";
            byggesak.tittel = "Igangsettingssøknad for enebolig i Byggestedgate 1 - byggetrinn 2";
            byggesak.saksnummer = new SaksnummerType() { saksaar = "2018", sakssekvensnummer = "123456" };
            byggesak.kategori = new ProsesskategoriType() { kode = "IG", beskrivelse = "Søknad om igangsettingstillatelse" };
            byggesak.tiltakstype = new[] { new TiltaktypeType() { kode = "nyttbyggboligformal", beskrivelse = "Nytt bygg - boligformål" } };
            byggesak.vedtak = new VedtakType() { beskrivelse = "Vedtak om igangsettingstillatelse av byggetrinn 2", status = new VedtakstypeType() { kode = "1", beskrivelse = "Godkjent" }, vedtaksdato = DateTime.Now };


            return byggesak;
        }

        /// <summary>
        ///Nivå 0 - kun beskjed om godkjent vedtak på midlertidig brukstillatelse med saksnummer - matrikkelfører må selv finne korrekt underlag i saken
        ///Brukstilfelle matrikkel - 8.6.4 Nybygg, eksisterende bygg – midlertidig brukstillatelse gitt
        ///Mer om søknaden - https://dibk-utvikling.atlassian.net/wiki/spaces/FB/pages/20021282/S+knad+om+midlertidig+brukstillatelse
        /// </summary>
        /// <returns></returns>
        public ByggesakType GenerateSample4()
        {
            var byggesak = new ByggesakType();
            byggesak.adresse = "Byggestedgate 1";
            byggesak.tittel = "Midlertidig brukstillatelse for enebolig i Byggestedgate 1";
            byggesak.saksnummer = new SaksnummerType() { saksaar = "2018", sakssekvensnummer = "123456" };
            byggesak.kategori = new ProsesskategoriType() { kode = "MB", beskrivelse = "Søknad om midlertidig brukstillatelse" };
            byggesak.tiltakstype = new[] { new TiltaktypeType() { kode = "nyttbyggboligformal", beskrivelse = "Nytt bygg - boligformål" } };
            byggesak.vedtak = new VedtakType() { beskrivelse = "Vedtak om midlertidig brukstillatelse", status = new VedtakstypeType() { kode = "1", beskrivelse = "Godkjent" }, vedtaksdato = DateTime.Now };

            return byggesak;
        }

        /// <summary>
        ///Nivå 0 - kun beskjed om godkjent vedtak på ferdigattest med saksnummer - matrikkelfører må selv finne korrekt underlag i saken
        ///Brukstilfelle matrikkel - 8.6.5 Nybygg, eksisterende bygg – ferdigattest gitt
        ///Mer om søknaden - https://dibk-utvikling.atlassian.net/wiki/spaces/FB/pages/23691266/S+knad+om+ferdigattest
        /// </summary>
        /// <returns></returns>
        public ByggesakType GenerateSample5()
        {
            var byggesak = new ByggesakType();
            byggesak.adresse = "Byggestedgate 1";
            byggesak.tittel = "Ferdigattest for enebolig i Byggestedgate 1";
            byggesak.saksnummer = new SaksnummerType() { saksaar = "2018", sakssekvensnummer = "123456" };
            byggesak.kategori = new ProsesskategoriType() { kode = "FA", beskrivelse = "Søknad om ferdigattest" };
            byggesak.tiltakstype = new[] { new TiltaktypeType() { kode = "nyttbyggboligformal", beskrivelse = "Nytt bygg - boligformål" } };
            byggesak.vedtak = new VedtakType() { beskrivelse = "Vedtak om ferdigattest", status = new VedtakstypeType() { kode = "1", beskrivelse = "Godkjent" }, vedtaksdato = DateTime.Now };


            return byggesak;
        }

        //******************************************


        /// <summary>
        ///Nivå 0 - kun beskjed om godkjent vedtak på ett trinn søknad med saksnummer - matrikkelfører må selv finne korrekt underlag i saken
        ///Brukstilfelle matrikkel - 8.6.2 Nybygg, nytt bygg - igangsettingstillatelse gitt  (denne brukes for ett trinn)
        ///Søknaden kan etterfølges med endringssøknad, midlertidig brukstillatelse eller ferdigattest
        ///Mer om søknaden - https://dibk-utvikling.atlassian.net/wiki/spaces/FB/pages/31129622/S+knad+om+tillatelse+i+ett+trinn
        /// </summary>
        /// <returns></returns>
        public ByggesakType GenerateSample6()
        {
            var byggesak = new ByggesakType();
            byggesak.adresse = "Byggestedgate 1";
            byggesak.tittel = "Ett trinn søknad for enebolig i Byggestedgate 1";
            byggesak.saksnummer = new SaksnummerType() { saksaar = "2018", sakssekvensnummer = "123456" };
            byggesak.kategori = new ProsesskategoriType() { kode = "ET", beskrivelse = "Søknad om tillatelse i ett trinn" };
            byggesak.tiltakstype = new[] { new TiltaktypeType() { kode = "nyttbyggboligformal", beskrivelse = "Nytt bygg - boligformål" } };
            byggesak.vedtak = new VedtakType() { beskrivelse = "Vedtak om byggetillatelse", status = new VedtakstypeType() { kode = "1", beskrivelse = "Godkjent" }, vedtaksdato = DateTime.Now };

            return byggesak;
        }

        /// <summary>
        ///Nivå 0 - kun beskjed om godkjent vedtak på tiltak uten ansvarsrett søknad med saksnummer - matrikkelfører må selv finne korrekt underlag i saken
        ///Brukstilfelle matrikkel - 8.6.2 Nybygg, nytt bygg - igangsettingstillatelse gitt  (denne brukes for tiltak uten ansvarsrett)
        ///Søknaden kan etterfølges med endringssøknad, midlertidig brukstillatelse eller ferdigattest
        ///Mer om søknaden - https://dibk-utvikling.atlassian.net/wiki/spaces/FB/pages/31129618/S+knad+om+tillatelse+til+tiltak+uten+ansvarsrett
        /// </summary>
        /// <returns></returns>
        public ByggesakType GenerateSample7()
        {
            var byggesak = new ByggesakType();
            byggesak.adresse = "Byggestedgate 1";
            byggesak.tittel = "Søknad om tiltak uten ansvarsrett for enebolig i Byggestedgate 1";
            byggesak.saksnummer = new SaksnummerType() { saksaar = "2018", sakssekvensnummer = "123456" };
            byggesak.kategori = new ProsesskategoriType() { kode = "TA", beskrivelse = "Søknad om tiltak uten ansvarsrett" };
            byggesak.tiltakstype = new[] { new TiltaktypeType() { kode = "nyttbyggboligformal", beskrivelse = "Nytt bygg - boligformål" } };
            byggesak.vedtak = new VedtakType() { beskrivelse = "Vedtak om byggetillatelse", status = new VedtakstypeType() { kode = "1", beskrivelse = "Godkjent" }, vedtaksdato = DateTime.Now };

            return byggesak;
        }
    }
}
