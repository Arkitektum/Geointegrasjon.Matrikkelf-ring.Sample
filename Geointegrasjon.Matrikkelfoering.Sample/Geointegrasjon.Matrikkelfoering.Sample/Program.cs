using KommIT.FIKS.AdapterAltinnSvarUt.Services.WS.SvarUt.Forsendelse8;
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


       


            //*** Vedleggstyper
            // Situasjonsplan, Avkjoerselsplan, 
            // TegningEksisterendePlan, TegningNyPlan, TegningEksisterendeSnitt, TegningNyttSnitt, TegningEksisterendeFasade, TegningNyFasade
            // ByggesaksBIM
            // Vedtak
            // - se beskrivelse https://dibk-utvikling.atlassian.net/wiki/spaces/FB/pages/270139400/Vedlegg

            dokument byggesakxml = new dokument() { dokumentType = "Byggesak", data = System.Text.Encoding.UTF8.GetBytes(xml), filnavn = "byggesak.xml", mimetype = "application/xml" };

            dokument sitplan = new dokument() { dokumentType = "Situasjonsplan", data = new byte[1], filnavn = "sitplan.pdf", mimetype = "application/pdf" };
            dokument tegning1 = new dokument() { dokumentType = "TegningNyttSnitt", data = new byte[1], filnavn = "tegning.pdf", mimetype = "application/pdf" };

            dokument bim = new dokument() { dokumentType = "ByggesaksBIM", data = new byte[1], filnavn = "bim.ifc", mimetype = "application/ifc" };


            List<dokument> dokumenter = new List<dokument>();
            dokumenter.Add(byggesakxml);
            dokumenter.Add(sitplan);
            dokumenter.Add(tegning1);
            dokumenter.Add(bim);

            //Opplasting FIKS
            var svarut = new SvarUtService();
            string orgnrTilKommunen = "123456789";
            svarut.Send(byggesak, orgnrTilKommunen, "Matrikkelføring klient", dokumenter.ToArray());

        }
    }
}
