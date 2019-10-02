using KommIT.FIKS.AdapterAltinnSvarUt.Services.WS.SvarUt.Forsendelse8;
using no.geointegrasjon.rep.matrikkelfoering;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geointegrasjon.Matrikkelfoering.SendSample
{
    public sealed class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding { get { return Encoding.UTF8; } }
    }
    class Program
    {
        static void Main(string[] args)
        {

            int nivaa = 0;
            if (args.Length == 0)
            {
                // default, kjør alt
            }
            else if (args.Length == 1)
            {
                if (!Int32.TryParse(args[0], out nivaa))
                {
                    nivaa = 0;
                }
            }


            //Datamodell matrikkelføring
            var byggesakG0 = new GenerateN0().GenerateSample();
            List<dokument> dokumenter = new List<dokument>();

            string xml = writeByggesakXML(byggesakG0);
            dokument byggesakxml = new dokument()
            {
                dokumentType = "Byggesak",
                data = System.Text.Encoding.UTF8.GetBytes(xml),
                filnavn = "byggesak.xml",
                mimetype = "application/xml"
            };
            dokumenter.Add(byggesakxml);

            //
            // G0: Saksnummer / url på vedtak
            //
            if (nivaa <= 0)
            {
                //Rammesøknad
                SendByggesakToSvarut(byggesakG0, dokumenter);

                // Endringssøknad
                byggesakG0 = new GenerateN0().GenerateSample1();
                ReplaceByggesakXmlDoc(byggesakG0, dokumenter, byggesakxml);// LARS
                SendByggesakToSvarut(byggesakG0, dokumenter);

                // Igangsettingssøknad  av byggetrinn 1
                byggesakG0 = new GenerateN0().GenerateSample2();
                ReplaceByggesakXmlDoc(byggesakG0, dokumenter, byggesakxml);// LARS
                SendByggesakToSvarut(byggesakG0, dokumenter);

                // Igangsettingssøknad  av byggetrinn 2
                byggesakG0 = new GenerateN0().GenerateSample3();
                ReplaceByggesakXmlDoc(byggesakG0, dokumenter, byggesakxml);// LARS
                SendByggesakToSvarut(byggesakG0, dokumenter);

                // Midlertidig brukstillatelse
                byggesakG0 = new GenerateN0().GenerateSample4();
                ReplaceByggesakXmlDoc(byggesakG0, dokumenter, byggesakxml);// LARS
                SendByggesakToSvarut(byggesakG0, dokumenter);

                // Ferdigattest
                byggesakG0 = new GenerateN0().GenerateSample5();
                ReplaceByggesakXmlDoc(byggesakG0, dokumenter, byggesakxml);// LARS
                SendByggesakToSvarut(byggesakG0, dokumenter);

                // Ett trinn
                byggesakG0 = new GenerateN0().GenerateSample6();
                ReplaceByggesakXmlDoc(byggesakG0, dokumenter, byggesakxml);// LARS
                SendByggesakToSvarut(byggesakG0, dokumenter);

                // Tiltak uten ansvarsrett
                byggesakG0 = new GenerateN0().GenerateSample7();
                ReplaceByggesakXmlDoc(byggesakG0, dokumenter, byggesakxml);// LARS
                SendByggesakToSvarut(byggesakG0, dokumenter);

                Console.WriteLine("Sendte 7 meldinger med nivå 0, Saksnummer på vedtak");

            }

            //
            // G1: Gjeldende tegninger og (vanlig) situasjonsplan
            //
            var byggesakG1 = new GenerateN1().GenerateSample();
            var tegning1 = GetDokTegninger();
            dokumenter.Add(tegning1);
            var sitplan = GetDokSituasjonsPlan();
            dokumenter.Add(sitplan);
            if (nivaa <= 1)
            {
                ReplaceByggesakXmlDoc(byggesakG1, dokumenter, byggesakxml);// LARS
                SendByggesakToSvarut(byggesakG1, dokumenter);
                Console.WriteLine("Sendte melding med nivå 1, Gjeldende tegninger");
            }

            //
            // G2: Matrikkelopplysninger
            //
            // 
            var byggesakG2 = new GenerateN2().GenerateSample();
            if (nivaa <= 2)
            {

                ReplaceByggesakXmlDoc(byggesakG2, dokumenter, byggesakxml); // LARS

                SendByggesakToSvarut(byggesakG2, dokumenter);
                Console.WriteLine("Sendte melding med nivå 2, med matrikkelopplysninger for enebolig");
            }

            if (nivaa <= 2)
            {

                byggesakG2 = new GenerateN2().GenerateSample2();
                ReplaceByggesakXmlDoc(byggesakG2, dokumenter, byggesakxml);// LARS
                SendByggesakToSvarut(byggesakG2, dokumenter);
                Console.WriteLine("Sendte melding med nivå 2, med matrikkelopplysninger for 5 tomannsboliger");

                //TODO - delvis godkjent vedtak , tilbygg med løpenr/bygningsendringer? riving
                //seksjonerte eiendommer
            }

            //
            // G3: ByggesaksBIM med Matrikkelopplysninger
            //
            if (nivaa <= 3)
            {
                var bim = GetDokByggesaksBim();
                dokumenter.Add(bim);
                var byggesakG3 = new GenerateN2().GenerateSample();
                ReplaceByggesakXmlDoc(byggesakG3, dokumenter, byggesakxml);// LARS
                SendByggesakToSvarut(byggesakG3, dokumenter);
                Console.WriteLine("Sendte melding med nivå 3, med BIM");
            }


            // 
            // G3: ByggesaksBIM med Matrikkelopplysninger for Retorten i Trondheim
            //
            if (nivaa <= 4)
            {
                //dokumenter.Remove(tegning1);

                var bim = GetDokByggesaksBim(@"samplefiles\NTNU Retorten eByggesak.ifc");

                dokumenter.Add(bim);
                var byggesakG3 = new GenerateN2N3_NOIS().GenerateSampleRetorten();
                ReplaceByggesakXmlDoc(byggesakG3, dokumenter, byggesakxml);// LARS
                SendByggesakToSvarut(byggesakG3, dokumenter);
                Console.WriteLine("Sendte melding med nivå 3, med BIM");
            }


            //
            // G4: digital situasjonsplan
            //

            //Console.WriteLine("Sendte melding med nivå 4, med digital situasjonsplan");

            Console.WriteLine("pause");
        }

        private static void ReplaceByggesakXmlDoc(ByggesakType byggesak, List<dokument> dokumenter, dokument byggesakxml)
        {
            string xml = writeByggesakXML(byggesak);
            dokumenter.Remove(byggesakxml);
            byggesakxml.data = System.Text.Encoding.UTF8.GetBytes(xml);
            dokumenter.Add(byggesakxml);
        }

        private static string writeByggesakXML(ByggesakType byggesakG0)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(byggesakG0.GetType());
            var stringWriter = new Utf8StringWriter();
            serializer.Serialize(stringWriter, byggesakG0);
            string xml = stringWriter.ToString();
            return xml;
        }

        private static void SendByggesakToSvarut(ByggesakType byggesak, List<dokument> dokumenter)
        {
           


            //*** Vedleggstyper
            // Situasjonsplan, Avkjoerselsplan, 
            // TegningEksisterendePlan, TegningNyPlan, TegningEksisterendeSnitt, TegningNyttSnitt, TegningEksisterendeFasade, TegningNyFasade
            // ByggesaksBIM
            // Vedtak
            // - se beskrivelse https://dibk-utvikling.atlassian.net/wiki/spaces/FB/pages/270139400/Vedlegg

      
            //Opplasting FIKS
            var svarut = new SvarUtService();
            string orgnrTilKommunen = ConfigurationManager.AppSettings["OrgNrReceiver"];
            svarut.Send(byggesak, orgnrTilKommunen, "Matrikkelføring klient", dokumenter.ToArray());
        }

        private static dokument GetDokByggesaksBim(string fileName = @"samplefiles\bim.ifc")
        {
            dokument bim = new dokument()
            {
                dokumentType = "ByggesaksBIM",
                data = File.ReadAllBytes(fileName), // data = File.ReadAllBytes(@"samplefiles\bim.ifc"),
                filnavn = "bim.ifc",
                mimetype = "application/ifc"
            };
            return bim;
        }

        private static dokument GetDokTegninger(string fileName = @"samplefiles\DokTegning.pdf")
        {
            dokument tegning1 = new dokument()
            {
                dokumentType = "TegningNyttSnitt",
                data = new byte[1],
                filnavn = "tegning.pdf",
                mimetype = "application/pdf"
            };
            if (File.Exists(fileName))
            {
                tegning1.data = null;
                tegning1.data = File.ReadAllBytes(fileName);
            }

            return tegning1;
        }

        private static dokument GetDokSituasjonsPlan(string fileName = @"samplefiles\DokSitplan.pdf")
        {
            dokument sitplan = new dokument()
            {
                dokumentType = "Situasjonsplan",
                data = new byte[1],
                filnavn = "sitplan.pdf",
                mimetype = "application/pdf"
            };
            if (File.Exists(fileName))
            {
                sitplan.data = null;
                sitplan.data = File.ReadAllBytes(fileName);
            }

            return sitplan;
        }
    }
}
