using no.geointegrasjon.rep.matrikkelfoering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geointegrasjon.Matrikkelfoering.Sample
{
    public sealed class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding { get { return Encoding.UTF8; } }
    }
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
            var serializer = new System.Xml.Serialization.XmlSerializer(byggesak.GetType());
            var stringWriter = new Utf8StringWriter();
            serializer.Serialize(stringWriter, byggesak);
            string xml = stringWriter.ToString();


            //TODO Prossesskategori
            //TODO tiltakshaver, ansvarlig søker - Kontaktperson

            //Vedleggstyper - Situasjonsplan, 

            //Opplasting FIKS
            var svarut = new SvarUtService();
            svarut.Send(byggesak, "123456789", "Matrikkelføring klient");

        }
    }
}
