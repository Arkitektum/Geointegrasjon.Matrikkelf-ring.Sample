using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geointegrasjon.Matrikkelfoering.ReceiveSample
{
    //Automatically generated from the example response at https://github.com/ks-no/svarut-dokumentasjon/wiki/mottaksservice-REST
    public class Avsender
    {
        public string adresselinje1 { get; set; }
        public string adresselinje2 { get; set; }
        public string adresselinje3 { get; set; }
        public string navn { get; set; }
        public string poststed { get; set; }
        public string postnr { get; set; }
    }

    public class Mottaker
    {
        public string adresse1 { get; set; }
        public string adresse2 { get; set; }
        public string adresse3 { get; set; }
        public string postnr { get; set; }
        public string poststed { get; set; }
        public string navn { get; set; }
        public string land { get; set; }
        public string orgnr { get; set; }
        public string fnr { get; set; }
    }

    public class EkstraMetadata
    {
        public string key { get; set; }
        public string value { get; set; }
    }

    public class MetadataFraAvleverendeSystem
    {
        public int sakssekvensnummer { get; set; }
        public int saksaar { get; set; }
        public int journalaar { get; set; }
        public int journalsekvensnummer { get; set; }
        public int journalpostnummer { get; set; }
        public string journalposttype { get; set; }
        public string journalstatus { get; set; }
        public string journaldato { get; set; }
        public string dokumentetsDato { get; set; }
        public string tittel { get; set; }
        public string saksBehandler { get; set; }
        public List<EkstraMetadata> ekstraMetadata { get; set; }
    }

    public class MetadataForImport
    {
        public int sakssekvensnummer { get; set; }
        public int saksaar { get; set; }
        public string journalposttype { get; set; }
        public string journalstatus { get; set; }
        public string dokumentetsDato { get; set; }
        public string tittel { get; set; }
    }

    public class Filmetadata
    {
        public string filnavn { get; set; }
        public string mimetype { get; set; }
        public string sha256hash { get; set; }
        public string dokumentType { get; set; }
        public int size { get; set; }
    }

    public class SvarSendesTil
    {
        public string adresse1 { get; set; }
        public string adresse2 { get; set; }
        public object adresse3 { get; set; }
        public string postnr { get; set; }
        public string poststed { get; set; }
        public string navn { get; set; }
        public string land { get; set; }
        public string orgnr { get; set; }
        public string fnr { get; set; }
    }

    public class Lenker
    {
        public string ledetekst { get; set; }
        public string urlLenke { get; set; }
        public string urlTekst { get; set; }
    }

    public class Forsendelse
    {
        public Avsender avsender { get; set; }
        public Mottaker mottaker { get; set; }
        public string id { get; set; }
        public string tittel { get; set; }
        public long date { get; set; }
        public MetadataFraAvleverendeSystem metadataFraAvleverendeSystem { get; set; }
        public MetadataForImport metadataForImport { get; set; }
        public string status { get; set; }
        public string niva { get; set; }
        public List<Filmetadata> filmetadata { get; set; }
        public SvarSendesTil svarSendesTil { get; set; }
        public string svarPaForsendelse { get; set; }
        public string forsendelseType { get; set; }
        public string eksternRef { get; set; }
        public List<Lenker> lenker { get; set; }
        public string downloadUrl { get; set; }
    }
}
