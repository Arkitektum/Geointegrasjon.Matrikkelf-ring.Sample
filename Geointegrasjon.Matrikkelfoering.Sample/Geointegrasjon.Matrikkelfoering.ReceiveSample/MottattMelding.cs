using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geointegrasjon.Matrikkelfoering.ReceiveSample
{
    //Manually constructed since we only need four properties, but you could generate from the json structure as well.
    struct MottattMelding
    {
        public readonly string Id;
        public readonly string Tittel;
        public readonly string DownloadUrl;
        public readonly string Forsendelsestype;

        public MottattMelding(JObject receivedMessage)
        {
            Tittel = (string)receivedMessage["tittel"];
            Id = (string)receivedMessage["id"];
            DownloadUrl = (string)receivedMessage["downloadUrl"];
            Forsendelsestype = (string)receivedMessage["forsendelseType"];
        }
    }
}
