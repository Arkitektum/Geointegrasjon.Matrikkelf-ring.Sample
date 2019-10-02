using no.geointegrasjon.rep.matrikkelfoering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using no.geointegrasjon.rep.matrikkelfoering;

namespace Geointegrasjon.Matrikkelfoering.SendSample
{
    class GenerateN2N3_NOIS
    {

        /// <summary>
        /// Nivå 2 - Beskjed om godkjent vedtak på ett trinn søknad - med gjeldende tegninger og utfylt matrikkelopplysninger på ny enebolig
        ///Brukstilfelle matrikkel - 8.6.2 Nybygg, nytt bygg - igangsettingstillatelse gitt  (denne brukes for ett trinn)
        ///Søknaden kan etterfølges med endringssøknad, midlertidig brukstillatelse eller ferdigattest
        ///Mer om søknaden - https://dibk-utvikling.atlassian.net/wiki/spaces/FB/pages/31129622/S+knad+om+tillatelse+i+ett+trinn
        /// </summary>
        /// <returns>Byggesak med utfylt matrikkelopplysninger for ny enebolig</returns>
        public ByggesakType GenerateSampleRetorten()
        {

            // 
            // G3: ByggesaksBIM med Matrikkelopplysninger for Retorten i Trondheim
            // Vegadresse	Gunnerus gate 1
            // Adressekode 2470
            //A dressenavn Gunnerus gate
            //    Adressenummer   1
            // Matrikkelenhet  403 / 177
            // Type Grunneiendom
            // Kommune 5001
            // Bruksnavn:
            // Etableringsdato 14.09.1962
            // Areal   9662.8
            // Tinglyst Ja
            // Status:
            // Adresser:
            //Representasjonspunkt:
            //Koordinatsystem EUREF89 UTM Sone 32
            //Nord    7034069
            //Øst 569253
            //Bygninger    3
            //Kommunal tilleggsdel:

            // Hovedbygg 	10539765
            //Bygningsnummer  10539765
            //Bygningstype    621 - Univ./ høgskole m / auditor.leses
            //Næringsgruppe Undervisning
            //Status Tatt i bruk
            //Har heis    Ja
            //    Opprinnelseskode    Vanlig registrering
            //Etasjer. 7
            //Bruksenheter     1
            //Gunnerus gate 1 H0101
            //    Matrikkelenhet  403 / 177
            //Bruksenhetstype Annet enn bolig
            //Kjøkken Ikke kjøkken
            //    Bruksareal  9738
            //Vannforsyning:
            //Avløp:
            //Representasjonspunkt:
            //Koordinatsystem EUREF89 UTM Sone 32
            //Nord    7034069
            //Øst 569253
            //Bruksareal til annet    9738
            //Bruksareal total    9738
            //Bruttoareal til bolig   0
            //Bruttoareal til annet   0
            //Bruttoareal total   0
            //Kommunal tilleggsdel:
            //Bygningsendring 10539765 - 2
            //Bygningsnummer  10539765
            //BygningendringskodePåbygg
            //    Næringsgruppe   Undervisning
            //    Status  Ferdigattest
            //    Opprinnelseskode    Vanlig registrering
            //Etasjer. 1
            //Bruksenheter     1
            //Kontaktpersoner 1
            //Vannforsyning:
            //Avløp:
            //Representasjonspunkt:
            //Bruksareal til annet    350
            //Bruksareal total    350
            //Bruttoareal til bolig   0
            //Bruttoareal til annet   0
            //Bruttoareal total   0
            //Kommunal tilleggsdel:
            //Bygningsendring 10539765 - 1
            //Bygningsnummer  10539765
            //BygningendringskodeTilbygg
            //    Næringsgruppe   Undervisning
            //    Status  Tatt i bruk
            //    Opprinnelseskode    Vanlig registrering
            //Etasjer. 4
            //Bruksenheter     1
            //Kontaktpersoner 1
            //Vannforsyning:
            //Avløp:
            //Representasjonspunkt:
            //Bruksareal til annet    200
            //Bruksareal total    200
            //Bruttoareal til bolig   0
            //Bruttoareal til annet   0
            //Bruttoareal total   0
            //Kommunal tilleggsdel:


            var byggesak = new ByggesakType();
            byggesak.adresse = "Gunnerusgate 1";
            byggesak.tittel = "Ett trinn søknad for enebolig i Gunnerusgate 1";

            // LARS
            var posisjon = new Punkt();
            var koordinat = new Koordinat();
            koordinat.x = 569289;
            koordinat.y = 7034032;
            koordinat.zSpecified = false;

            posisjon.posisjon = koordinat;

            var koordinatsystemKode = new KoordinatsystemKode();
            var epsgCode = "25832";
            var sosiCode = "22";
            koordinatsystemKode.kodeverdi = sosiCode;
            posisjon.koordinatsystem = koordinatsystemKode; // EPSG eller SOSI kode?


            byggesak.saksnummer = new SaksnummerType() { saksaar = "2019", sakssekvensnummer = "1234567" };

            byggesak.kategori = new ProsesskategoriType() { kode = "ET", beskrivelse = "Søknad om tillatelse i ett trinn" };
            byggesak.tiltakstype = new[] { new TiltaktypeType() { kode = "nyttbyggboligformal", beskrivelse = "Nytt bygg - boligformål" } };
            byggesak.vedtak = new VedtakType() { beskrivelse = "Vedtak om byggetillatelse", status = new VedtakstypeType() { kode = "1", beskrivelse = "Godkjent" }, vedtaksdato = DateTime.Now };

            byggesak.matrikkelopplysninger = new MatrikkelopplysningerType();
            byggesak.matrikkelopplysninger.adresse = new[]
            {
                new AdresseType() {adressekode = "2470", adressenavn = "Gunnerusgate", adressenummer = "1"}
            };
            byggesak.matrikkelopplysninger.eiendomsidentifikasjon = new[]
            {
                new MatrikkelnummerType() {kommunenummer = "5001", gaardsnummer = "403", bruksnummer = "77"}
            };
            byggesak.matrikkelopplysninger.bygning = new[]
                {
                    new BygningType
                    {
                        bebygdAreal = 100,
                        bebygdArealSpecified = true,
                        bygningstype = new BygningstypeType()
                        {
                            kode = "111",
                            beskrivelse = "Enebolig" //ref kodeliste https://register.geonorge.no/subregister/byggesoknad/direktoratet-for-byggkvalitet/bygningstype
                        },
                        naeringsgruppe  = new NaeringsgruppeType()
                        {
                            kode = "X",
                            beskrivelse = "Bolig" //ref kodeliste https://register.geonorge.no/subregister/byggesoknad/direktoratet-for-byggkvalitet/naeringsgruppe
                        },
                        etasjer = new[]
                        {
                            new EtasjeType
                            {

                                etasjeplan = new EtasjeplanType
                                {
                                    kode = "H",
                                    beskrivelse = "H"
                                },
                                etasjenummer = "01",
                                antallBoenheter = "1",
                                bruksarealTotalt = 100,
                                bruksarealTotaltSpecified = true,
                                bruksarealTilAnnet = 0,
                                bruksarealTilAnnetSpecified = true,
                                bruksarealTilBolig = 100,
                                bruksarealTilBoligSpecified = true,
                                endring =  EndringsstatusType.Ny
                            }
                        },
                        bruksenheter = new[]
                        {
                            new BruksenhetType
                            {

                                bruksenhetsnummer = new BruksenhetsnummerType
                                {
                                    etasjeplan = new EtasjeplanType
                                    {
                                        kode = "H",
                                        beskrivelse = "H"
                                    },
                                    etasjenummer = "01",
                                    loepenummer = "01"
                                },
                                bruksenhetstype = new BruksenhetstypeKodeType
                                {
                                    kode = "B",
                                    beskrivelse = "Bolig"
                                },
                                kjoekkentilgang = new KjoekkentilgangKodeType
                                {
                                    kode = "0",
                                    beskrivelse = "Ikke oppgitt"
                                },
                                bruksareal = 100,
                                bruksarealSpecified = true,
                                antallRom = "3",
                                antallBad = "1",
                                antallWC = "1",
                                adresse = new AdresseType
                                {
                                    adressekode = "1234",
                                    adressenavn = "Gatenavn",
                                    adressenummer = "1",
                                    adressebokstav = "A"

                                },
                                endring =  EndringsstatusType.Ny
                            }
                        },
                        avlop = new AvloepstilknytningType()
                        {
                            kode = "OffentligKloakk",
                            beskrivelse = "Offentlig avløpsanlegg" //ref kodeliste https://register.geonorge.no/subregister/byggesoknad/direktoratet-for-byggkvalitet/avlopstilknytning
                        },

                        vannforsyning = new VanntilknytningType()
                        {
                            kode = "AnnenPrivatInnlagt", //ref https://register.geonorge.no/subregister/byggesoknad/direktoratet-for-byggkvalitet/vanntilknytning 
                            beskrivelse = "Annen privat vannforsyning, innlagt vann"
                        },

                        energiforsyning = new EnergiforsyningType()
                        {
                            varmefordeling = new[]
                            {
                                new VarmefordelingType()
                                {
                                    kode = "elektriskePanelovner", //ref https://register.geonorge.no/subregister/byggesoknad/direktoratet-for-byggkvalitet/varmefordeling
                                    beskrivelse = "Elektriske panelovner"
                                },
                                new VarmefordelingType()
                                {
                                    kode = "elektriskeVarmekabler",
                                    beskrivelse = "Elektriske varmekabler"
                                },
                            },
                            energiforsyning = new[]
                            {
                                new EnergiforsyningTypeType()
                                {
                                    kode = "biobrensel", // ref https://register.geonorge.no/subregister/byggesoknad/direktoratet-for-byggkvalitet/energiforsyningtype
                                    beskrivelse = "Biobrensel"
                                },
                                new EnergiforsyningTypeType()
                                {
                                    kode = "elektrisitet",
                                    beskrivelse = "Elektrisitet"
                                },
                            },
                            relevant = true,
                            relevantSpecified = true
                        },
                        harHeis = false,
                        harHeisSpecified = true,
                        endring =  EndringsstatusType.Ny
                    }
            };




            return byggesak;

        }

        /// <summary>
        /// Nivå 2 - Beskjed om godkjent vedtak på ett trinn søknad - med gjeldende tegninger og utfylt matrikkelopplysninger på ny enebolig
        ///Brukstilfelle matrikkel - 8.6.2 Nybygg, nytt bygg - igangsettingstillatelse gitt  (denne brukes for ett trinn)
        ///Søknaden kan etterfølges med endringssøknad, midlertidig brukstillatelse eller ferdigattest
        ///Mer om søknaden - https://dibk-utvikling.atlassian.net/wiki/spaces/FB/pages/31129622/S+knad+om+tillatelse+i+ett+trinn
        /// </summary>
        /// <returns>Byggesak med utfylt matrikkelopplysninger for ny enebolig</returns>
        public ByggesakType GenerateSample()
        {

            var byggesak = new ByggesakType();
            byggesak.adresse = "Byggestedgate 1";
            byggesak.tittel = "Ett trinn søknad for enebolig i Byggestedgate 1";



            byggesak.saksnummer = new SaksnummerType() { saksaar = "2019", sakssekvensnummer = "123456" };
            //  byggesak.saksnummer = new SaksnummerType() { saksaar = "2018", sakssekvensnummer = "123456" };

            byggesak.kategori = new ProsesskategoriType() { kode = "ET", beskrivelse = "Søknad om tillatelse i ett trinn" };
            byggesak.tiltakstype = new[] { new TiltaktypeType() { kode = "nyttbyggboligformal", beskrivelse = "Nytt bygg - boligformål" } };
            byggesak.vedtak = new VedtakType() { beskrivelse = "Vedtak om byggetillatelse", status = new VedtakstypeType() { kode = "1", beskrivelse = "Godkjent" }, vedtaksdato = DateTime.Now };

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
                    new BygningType
                    {
                        bebygdAreal = 100,
                        bebygdArealSpecified = true,
                        bygningstype = new BygningstypeType()
                        {
                            kode = "111",
                            beskrivelse = "Enebolig" //ref kodeliste https://register.geonorge.no/subregister/byggesoknad/direktoratet-for-byggkvalitet/bygningstype
                        },
                        naeringsgruppe  = new NaeringsgruppeType()
                        {
                            kode = "X",
                            beskrivelse = "Bolig" //ref kodeliste https://register.geonorge.no/subregister/byggesoknad/direktoratet-for-byggkvalitet/naeringsgruppe
                        },
                        etasjer = new[]
                        {
                            new EtasjeType
                            {

                                etasjeplan = new EtasjeplanType
                                {
                                    kode = "H",
                                    beskrivelse = "H"
                                },
                                etasjenummer = "01",
                                antallBoenheter = "1",
                                bruksarealTotalt = 100,
                                bruksarealTotaltSpecified = true,
                                bruksarealTilAnnet = 0,
                                bruksarealTilAnnetSpecified = true,
                                bruksarealTilBolig = 100,
                                bruksarealTilBoligSpecified = true,
                                endring =  EndringsstatusType.Ny
                            }
                        },
                        bruksenheter = new[]
                        {
                            new BruksenhetType
                            {

                                bruksenhetsnummer = new BruksenhetsnummerType
                                {
                                    etasjeplan = new EtasjeplanType
                                    {
                                        kode = "H",
                                        beskrivelse = "H"
                                    },
                                    etasjenummer = "01",
                                    loepenummer = "01"
                                },
                                bruksenhetstype = new BruksenhetstypeKodeType
                                {
                                    kode = "B",
                                    beskrivelse = "Bolig"
                                },
                                kjoekkentilgang = new KjoekkentilgangKodeType
                                {
                                    kode = "0",
                                    beskrivelse = "Ikke oppgitt"
                                },
                                bruksareal = 100,
                                bruksarealSpecified = true,
                                antallRom = "3",
                                antallBad = "1",
                                antallWC = "1",
                                adresse = new AdresseType
                                {
                                    adressekode = "1234",
                                    adressenavn = "Gatenavn",
                                    adressenummer = "1",
                                    adressebokstav = "A"

                                },
                                endring =  EndringsstatusType.Ny
                            }
                        },
                        avlop = new AvloepstilknytningType()
                        {
                            kode = "OffentligKloakk",
                            beskrivelse = "Offentlig avløpsanlegg" //ref kodeliste https://register.geonorge.no/subregister/byggesoknad/direktoratet-for-byggkvalitet/avlopstilknytning
                        },

                        vannforsyning = new VanntilknytningType()
                        {
                            kode = "AnnenPrivatInnlagt", //ref https://register.geonorge.no/subregister/byggesoknad/direktoratet-for-byggkvalitet/vanntilknytning 
                            beskrivelse = "Annen privat vannforsyning, innlagt vann"
                        },

                        energiforsyning = new EnergiforsyningType()
                        {
                            varmefordeling = new[]
                            {
                                new VarmefordelingType()
                                {
                                    kode = "elektriskePanelovner", //ref https://register.geonorge.no/subregister/byggesoknad/direktoratet-for-byggkvalitet/varmefordeling
                                    beskrivelse = "Elektriske panelovner"
                                },
                                new VarmefordelingType()
                                {
                                    kode = "elektriskeVarmekabler",
                                    beskrivelse = "Elektriske varmekabler"
                                },
                            },
                            energiforsyning = new[]
                            {
                                new EnergiforsyningTypeType()
                                {
                                    kode = "biobrensel", // ref https://register.geonorge.no/subregister/byggesoknad/direktoratet-for-byggkvalitet/energiforsyningtype
                                    beskrivelse = "Biobrensel"
                                },
                                new EnergiforsyningTypeType()
                                {
                                    kode = "elektrisitet",
                                    beskrivelse = "Elektrisitet"
                                },
                            },
                            relevant = true,
                            relevantSpecified = true
                        },
                        harHeis = false,
                        harHeisSpecified = true,
                        endring =  EndringsstatusType.Ny
                    }
            };




            return byggesak;
        }

        /// <summary>
        ///Nivå 2 - Beskjed om godkjent vedtak på rammesøknad - med mange adresser
        ///Brukstilfelle matrikkel - 8.6.1 Nybygg, nytt bygg - rammetillatelse gitt
        ///Søknaden må etterfølges med minst en igangsettingssøknad
        ///Mer om søknaden - https://dibk-utvikling.atlassian.net/wiki/spaces/FB/pages/31129620/S+knad+om+rammetillatelse
        /// </summary>
        /// <returns>Byggesak med utfylt matrikkelopplysninger for oppføring av 5 tomannsboliger</returns>
        public ByggesakType GenerateSample2()
        {

            var byggesak = new ByggesakType();
            byggesak.adresse = "Byggestedgate 3 - 11";
            byggesak.tittel = "Rammesøknad for oppføring av 5 tomannsboliger";
            byggesak.saksnummer = new SaksnummerType() { saksaar = "2018", sakssekvensnummer = "123456" };
            byggesak.kategori = new ProsesskategoriType() { kode = "RS", beskrivelse = "Søknad om rammetillatelse" };
            byggesak.tiltakstype = new[] { new TiltaktypeType() { kode = "nyttbyggboligformal", beskrivelse = "Nytt bygg - boligformål" } };
            byggesak.vedtak = new VedtakType() { beskrivelse = "Vedtak om rammetillatelse", status = new VedtakstypeType() { kode = "1", beskrivelse = "Godkjent" }, vedtaksdato = DateTime.Now };
            byggesak.saksbehandler = "Michael";
            byggesak.ansvarligSoeker = new PartType() { navn = "Arkitekt Flink", organisasjonsnummer = "123456789" };
            byggesak.tiltakshaver = new PartType() { navn = "Hans Utbygger", foedselsnummer = "12345678901" };

            byggesak.matrikkelopplysninger = new MatrikkelopplysningerType();
            byggesak.matrikkelopplysninger.adresse = new[]
            {
                new AdresseType() {adressekode = "1001", adressenavn = "Byggestedgate", adressenummer = "3", adressebokstav="A"},
                new AdresseType() {adressekode = "1001", adressenavn = "Byggestedgate", adressenummer = "3", adressebokstav="B"},
                new AdresseType() {adressekode = "1001", adressenavn = "Byggestedgate", adressenummer = "5", adressebokstav="A"},
                new AdresseType() {adressekode = "1001", adressenavn = "Byggestedgate", adressenummer = "5", adressebokstav="B"},
                new AdresseType() {adressekode = "1001", adressenavn = "Byggestedgate", adressenummer = "7", adressebokstav="A"},
                new AdresseType() {adressekode = "1001", adressenavn = "Byggestedgate", adressenummer = "7", adressebokstav="B"},
                new AdresseType() {adressekode = "1001", adressenavn = "Byggestedgate", adressenummer = "9", adressebokstav="A"},
                new AdresseType() {adressekode = "1001", adressenavn = "Byggestedgate", adressenummer = "9", adressebokstav="B"},
                new AdresseType() {adressekode = "1001", adressenavn = "Byggestedgate", adressenummer = "11", adressebokstav="A"},
                new AdresseType() {adressekode = "1001", adressenavn = "Byggestedgate", adressenummer = "11", adressebokstav="B"},
            };
            byggesak.matrikkelopplysninger.eiendomsidentifikasjon = new[]
            {
                new MatrikkelnummerType() {kommunenummer = "9999", gaardsnummer = "260", bruksnummer = "109"}
            };
            byggesak.matrikkelopplysninger.bygning = new[]
                {
                    new BygningType
                    {
                        bebygdAreal = 100,
                        bebygdArealSpecified = true,
                        bygningstype = new BygningstypeType()
                        {
                            kode = "121",
                            beskrivelse = "Tomannsbolig, vertikaldelt" //ref kodeliste https://register.geonorge.no/subregister/byggesoknad/direktoratet-for-byggkvalitet/bygningstype
                        },
                        naeringsgruppe  = new NaeringsgruppeType()
                        {
                            kode = "X",
                            beskrivelse = "Bolig" //ref kodeliste https://register.geonorge.no/subregister/byggesoknad/direktoratet-for-byggkvalitet/naeringsgruppe
                        },
                        etasjer = new[]
                        {
                            new EtasjeType
                            {

                                etasjeplan = new EtasjeplanType
                                {
                                    kode = "H",
                                    beskrivelse = "H"
                                },
                                etasjenummer = "01",
                                antallBoenheter = "1",
                                bruksarealTotalt = 200,
                                bruksarealTotaltSpecified = true,
                                bruksarealTilAnnet = 0,
                                bruksarealTilAnnetSpecified = true,
                                bruksarealTilBolig = 200,
                                bruksarealTilBoligSpecified = true,
                                endring =  EndringsstatusType.Ny
                            }
                        },
                        bruksenheter = new[]
                        {
                            new BruksenhetType
                            {

                                bruksenhetsnummer = new BruksenhetsnummerType
                                {
                                    etasjeplan = new EtasjeplanType
                                    {
                                        kode = "H",
                                        beskrivelse = "H"
                                    },
                                    etasjenummer = "01",
                                    loepenummer = "01"
                                },
                                bruksenhetstype = new BruksenhetstypeKodeType
                                {
                                    kode = "B",
                                    beskrivelse = "Bolig"
                                },
                                kjoekkentilgang = new KjoekkentilgangKodeType
                                {
                                    kode = "0",
                                    beskrivelse = "Ikke oppgitt"
                                },
                                bruksareal = 100,
                                bruksarealSpecified = true,
                                antallRom = "3",
                                antallBad = "1",
                                antallWC = "1",
                                adresse = new AdresseType
                                {
                                    adressekode = "1001",
                                    adressenavn = "Byggestedgate",
                                    adressenummer = "3",
                                    adressebokstav = "A"

                                },
                                endring =  EndringsstatusType.Ny
                            },
                            new BruksenhetType
                            {

                                bruksenhetsnummer = new BruksenhetsnummerType
                                {
                                    etasjeplan = new EtasjeplanType
                                    {
                                        kode = "H",
                                        beskrivelse = "H"
                                    },
                                    etasjenummer = "01",
                                    loepenummer = "01"
                                },
                                bruksenhetstype = new BruksenhetstypeKodeType
                                {
                                    kode = "B",
                                    beskrivelse = "Bolig"
                                },
                                kjoekkentilgang = new KjoekkentilgangKodeType
                                {
                                    kode = "0",
                                    beskrivelse = "Ikke oppgitt"
                                },
                                bruksareal = 100,
                                bruksarealSpecified = true,
                                antallRom = "3",
                                antallBad = "1",
                                antallWC = "1",
                                adresse = new AdresseType
                                {
                                    adressekode = "1001",
                                    adressenavn = "Byggestedgate",
                                    adressenummer = "3",
                                    adressebokstav = "B"

                                },
                                endring =  EndringsstatusType.Ny
                            }
                        },
                        avlop = new AvloepstilknytningType()
                        {
                            kode = "OffentligKloakk",
                            beskrivelse = "Offentlig avløpsanlegg" //ref kodeliste https://register.geonorge.no/subregister/byggesoknad/direktoratet-for-byggkvalitet/avlopstilknytning
                        },

                        vannforsyning = new VanntilknytningType()
                        {
                            kode = "TilknyttetOffVannverk", //ref https://register.geonorge.no/subregister/byggesoknad/direktoratet-for-byggkvalitet/vanntilknytning 
                            beskrivelse = "Offentlig vannverk"
                        },
                        harHeis = false,
                        harHeisSpecified = true,
                        endring =  EndringsstatusType.Ny
                    },
                    new BygningType
                    {
                        bebygdAreal = 100,
                        bebygdArealSpecified = true,
                        bygningstype = new BygningstypeType()
                        {
                            kode = "121",
                            beskrivelse = "Tomannsbolig, vertikaldelt" //ref kodeliste https://register.geonorge.no/subregister/byggesoknad/direktoratet-for-byggkvalitet/bygningstype
                        },
                        naeringsgruppe  = new NaeringsgruppeType()
                        {
                            kode = "X",
                            beskrivelse = "Bolig" //ref kodeliste https://register.geonorge.no/subregister/byggesoknad/direktoratet-for-byggkvalitet/naeringsgruppe
                        },
                        etasjer = new[]
                        {
                            new EtasjeType
                            {

                                etasjeplan = new EtasjeplanType
                                {
                                    kode = "H",
                                    beskrivelse = "H"
                                },
                                etasjenummer = "01",
                                antallBoenheter = "1",
                                bruksarealTotalt = 200,
                                bruksarealTotaltSpecified = true,
                                bruksarealTilAnnet = 0,
                                bruksarealTilAnnetSpecified = true,
                                bruksarealTilBolig = 200,
                                bruksarealTilBoligSpecified = true,
                                endring =  EndringsstatusType.Ny
                            }
                        },
                        bruksenheter = new[]
                        {
                            new BruksenhetType
                            {

                                bruksenhetsnummer = new BruksenhetsnummerType
                                {
                                    etasjeplan = new EtasjeplanType
                                    {
                                        kode = "H",
                                        beskrivelse = "H"
                                    },
                                    etasjenummer = "01",
                                    loepenummer = "01"
                                },
                                bruksenhetstype = new BruksenhetstypeKodeType
                                {
                                    kode = "B",
                                    beskrivelse = "Bolig"
                                },
                                kjoekkentilgang = new KjoekkentilgangKodeType
                                {
                                    kode = "0",
                                    beskrivelse = "Ikke oppgitt"
                                },
                                bruksareal = 100,
                                bruksarealSpecified = true,
                                antallRom = "3",
                                antallBad = "1",
                                antallWC = "1",
                                adresse = new AdresseType
                                {
                                    adressekode = "1001",
                                    adressenavn = "Byggestedgate",
                                    adressenummer = "5",
                                    adressebokstav = "A"

                                },
                                endring =  EndringsstatusType.Ny
                            },
                            new BruksenhetType
                            {

                                bruksenhetsnummer = new BruksenhetsnummerType
                                {
                                    etasjeplan = new EtasjeplanType
                                    {
                                        kode = "H",
                                        beskrivelse = "H"
                                    },
                                    etasjenummer = "01",
                                    loepenummer = "01"
                                },
                                bruksenhetstype = new BruksenhetstypeKodeType
                                {
                                    kode = "B",
                                    beskrivelse = "Bolig"
                                },
                                kjoekkentilgang = new KjoekkentilgangKodeType
                                {
                                    kode = "0",
                                    beskrivelse = "Ikke oppgitt"
                                },
                                bruksareal = 100,
                                bruksarealSpecified = true,
                                antallRom = "3",
                                antallBad = "1",
                                antallWC = "1",
                                adresse = new AdresseType
                                {
                                    adressekode = "1001",
                                    adressenavn = "Byggestedgate",
                                    adressenummer = "5",
                                    adressebokstav = "B"

                                },
                                endring =  EndringsstatusType.Ny
                            }
                        },
                        avlop = new AvloepstilknytningType()
                        {
                            kode = "OffentligKloakk",
                            beskrivelse = "Offentlig avløpsanlegg" //ref kodeliste https://register.geonorge.no/subregister/byggesoknad/direktoratet-for-byggkvalitet/avlopstilknytning
                        },

                        vannforsyning = new VanntilknytningType()
                        {
                            kode = "TilknyttetOffVannverk", //ref https://register.geonorge.no/subregister/byggesoknad/direktoratet-for-byggkvalitet/vanntilknytning 
                            beskrivelse = "Offentlig vannverk"
                        },
                        harHeis = false,
                        harHeisSpecified = true,
                        endring =  EndringsstatusType.Ny
                    },
                    new BygningType
                    {
                        bebygdAreal = 100,
                        bebygdArealSpecified = true,
                        bygningstype = new BygningstypeType()
                        {
                            kode = "121",
                            beskrivelse = "Tomannsbolig, vertikaldelt" //ref kodeliste https://register.geonorge.no/subregister/byggesoknad/direktoratet-for-byggkvalitet/bygningstype
                        },
                        naeringsgruppe  = new NaeringsgruppeType()
                        {
                            kode = "X",
                            beskrivelse = "Bolig" //ref kodeliste https://register.geonorge.no/subregister/byggesoknad/direktoratet-for-byggkvalitet/naeringsgruppe
                        },
                        etasjer = new[]
                        {
                            new EtasjeType
                            {

                                etasjeplan = new EtasjeplanType
                                {
                                    kode = "H",
                                    beskrivelse = "H"
                                },
                                etasjenummer = "01",
                                antallBoenheter = "1",
                                bruksarealTotalt = 200,
                                bruksarealTotaltSpecified = true,
                                bruksarealTilAnnet = 0,
                                bruksarealTilAnnetSpecified = true,
                                bruksarealTilBolig = 200,
                                bruksarealTilBoligSpecified = true,
                                endring =  EndringsstatusType.Ny
                            }
                        },
                        bruksenheter = new[]
                        {
                            new BruksenhetType
                            {

                                bruksenhetsnummer = new BruksenhetsnummerType
                                {
                                    etasjeplan = new EtasjeplanType
                                    {
                                        kode = "H",
                                        beskrivelse = "H"
                                    },
                                    etasjenummer = "01",
                                    loepenummer = "01"
                                },
                                bruksenhetstype = new BruksenhetstypeKodeType
                                {
                                    kode = "B",
                                    beskrivelse = "Bolig"
                                },
                                kjoekkentilgang = new KjoekkentilgangKodeType
                                {
                                    kode = "0",
                                    beskrivelse = "Ikke oppgitt"
                                },
                                bruksareal = 100,
                                bruksarealSpecified = true,
                                antallRom = "3",
                                antallBad = "1",
                                antallWC = "1",
                                adresse = new AdresseType
                                {
                                    adressekode = "1001",
                                    adressenavn = "Byggestedgate",
                                    adressenummer = "7",
                                    adressebokstav = "A"

                                },
                                endring =  EndringsstatusType.Ny
                            },
                            new BruksenhetType
                            {

                                bruksenhetsnummer = new BruksenhetsnummerType
                                {
                                    etasjeplan = new EtasjeplanType
                                    {
                                        kode = "H",
                                        beskrivelse = "H"
                                    },
                                    etasjenummer = "01",
                                    loepenummer = "01"
                                },
                                bruksenhetstype = new BruksenhetstypeKodeType
                                {
                                    kode = "B",
                                    beskrivelse = "Bolig"
                                },
                                kjoekkentilgang = new KjoekkentilgangKodeType
                                {
                                    kode = "0",
                                    beskrivelse = "Ikke oppgitt"
                                },
                                bruksareal = 100,
                                bruksarealSpecified = true,
                                antallRom = "3",
                                antallBad = "1",
                                antallWC = "1",
                                adresse = new AdresseType
                                {
                                    adressekode = "1001",
                                    adressenavn = "Byggestedgate",
                                    adressenummer = "7",
                                    adressebokstav = "B"

                                },
                                endring =  EndringsstatusType.Ny
                            }
                        },
                        avlop = new AvloepstilknytningType()
                        {
                            kode = "OffentligKloakk",
                            beskrivelse = "Offentlig avløpsanlegg" //ref kodeliste https://register.geonorge.no/subregister/byggesoknad/direktoratet-for-byggkvalitet/avlopstilknytning
                        },

                        vannforsyning = new VanntilknytningType()
                        {
                            kode = "TilknyttetOffVannverk", //ref https://register.geonorge.no/subregister/byggesoknad/direktoratet-for-byggkvalitet/vanntilknytning 
                            beskrivelse = "Offentlig vannverk"
                        },
                        harHeis = false,
                        harHeisSpecified = true,
                        endring =  EndringsstatusType.Ny
                    },
                    new BygningType
                    {
                        bebygdAreal = 100,
                        bebygdArealSpecified = true,
                        bygningstype = new BygningstypeType()
                        {
                            kode = "121",
                            beskrivelse = "Tomannsbolig, vertikaldelt" //ref kodeliste https://register.geonorge.no/subregister/byggesoknad/direktoratet-for-byggkvalitet/bygningstype
                        },
                        naeringsgruppe  = new NaeringsgruppeType()
                        {
                            kode = "X",
                            beskrivelse = "Bolig" //ref kodeliste https://register.geonorge.no/subregister/byggesoknad/direktoratet-for-byggkvalitet/naeringsgruppe
                        },
                        etasjer = new[]
                        {
                            new EtasjeType
                            {

                                etasjeplan = new EtasjeplanType
                                {
                                    kode = "H",
                                    beskrivelse = "H"
                                },
                                etasjenummer = "01",
                                antallBoenheter = "1",
                                bruksarealTotalt = 200,
                                bruksarealTotaltSpecified = true,
                                bruksarealTilAnnet = 0,
                                bruksarealTilAnnetSpecified = true,
                                bruksarealTilBolig = 200,
                                bruksarealTilBoligSpecified = true,
                                endring =  EndringsstatusType.Ny
                            }
                        },
                        bruksenheter = new[]
                        {
                            new BruksenhetType
                            {

                                bruksenhetsnummer = new BruksenhetsnummerType
                                {
                                    etasjeplan = new EtasjeplanType
                                    {
                                        kode = "H",
                                        beskrivelse = "H"
                                    },
                                    etasjenummer = "01",
                                    loepenummer = "01"
                                },
                                bruksenhetstype = new BruksenhetstypeKodeType
                                {
                                    kode = "B",
                                    beskrivelse = "Bolig"
                                },
                                kjoekkentilgang = new KjoekkentilgangKodeType
                                {
                                    kode = "0",
                                    beskrivelse = "Ikke oppgitt"
                                },
                                bruksareal = 100,
                                bruksarealSpecified = true,
                                antallRom = "3",
                                antallBad = "1",
                                antallWC = "1",
                                adresse = new AdresseType
                                {
                                    adressekode = "1001",
                                    adressenavn = "Byggestedgate",
                                    adressenummer = "9",
                                    adressebokstav = "A"

                                },
                                endring =  EndringsstatusType.Ny
                            },
                            new BruksenhetType
                            {

                                bruksenhetsnummer = new BruksenhetsnummerType
                                {
                                    etasjeplan = new EtasjeplanType
                                    {
                                        kode = "H",
                                        beskrivelse = "H"
                                    },
                                    etasjenummer = "01",
                                    loepenummer = "01"
                                },
                                bruksenhetstype = new BruksenhetstypeKodeType
                                {
                                    kode = "B",
                                    beskrivelse = "Bolig"
                                },
                                kjoekkentilgang = new KjoekkentilgangKodeType
                                {
                                    kode = "0",
                                    beskrivelse = "Ikke oppgitt"
                                },
                                bruksareal = 100,
                                bruksarealSpecified = true,
                                antallRom = "3",
                                antallBad = "1",
                                antallWC = "1",
                                adresse = new AdresseType
                                {
                                    adressekode = "1001",
                                    adressenavn = "Byggestedgate",
                                    adressenummer = "9",
                                    adressebokstav = "B"

                                },
                                endring =  EndringsstatusType.Ny
                            }
                        },
                        avlop = new AvloepstilknytningType()
                        {
                            kode = "OffentligKloakk",
                            beskrivelse = "Offentlig avløpsanlegg" //ref kodeliste https://register.geonorge.no/subregister/byggesoknad/direktoratet-for-byggkvalitet/avlopstilknytning
                        },

                        vannforsyning = new VanntilknytningType()
                        {
                            kode = "TilknyttetOffVannverk", //ref https://register.geonorge.no/subregister/byggesoknad/direktoratet-for-byggkvalitet/vanntilknytning 
                            beskrivelse = "Offentlig vannverk"
                        },
                        harHeis = false,
                        harHeisSpecified = true,
                        endring =  EndringsstatusType.Ny
                    },
                    new BygningType
                    {
                        bebygdAreal = 100,
                        bebygdArealSpecified = true,
                        bygningstype = new BygningstypeType()
                        {
                            kode = "121",
                            beskrivelse = "Tomannsbolig, vertikaldelt" //ref kodeliste https://register.geonorge.no/subregister/byggesoknad/direktoratet-for-byggkvalitet/bygningstype
                        },
                        naeringsgruppe  = new NaeringsgruppeType()
                        {
                            kode = "X",
                            beskrivelse = "Bolig" //ref kodeliste https://register.geonorge.no/subregister/byggesoknad/direktoratet-for-byggkvalitet/naeringsgruppe
                        },
                        etasjer = new[]
                        {
                            new EtasjeType
                            {

                                etasjeplan = new EtasjeplanType
                                {
                                    kode = "H",
                                    beskrivelse = "H"
                                },
                                etasjenummer = "01",
                                antallBoenheter = "1",
                                bruksarealTotalt = 200,
                                bruksarealTotaltSpecified = true,
                                bruksarealTilAnnet = 0,
                                bruksarealTilAnnetSpecified = true,
                                bruksarealTilBolig = 200,
                                bruksarealTilBoligSpecified = true,
                                endring =  EndringsstatusType.Ny
                            }
                        },
                        bruksenheter = new[]
                        {
                            new BruksenhetType
                            {

                                bruksenhetsnummer = new BruksenhetsnummerType
                                {
                                    etasjeplan = new EtasjeplanType
                                    {
                                        kode = "H",
                                        beskrivelse = "H"
                                    },
                                    etasjenummer = "01",
                                    loepenummer = "01"
                                },
                                bruksenhetstype = new BruksenhetstypeKodeType
                                {
                                    kode = "B",
                                    beskrivelse = "Bolig"
                                },
                                kjoekkentilgang = new KjoekkentilgangKodeType
                                {
                                    kode = "0",
                                    beskrivelse = "Ikke oppgitt"
                                },
                                bruksareal = 100,
                                bruksarealSpecified = true,
                                antallRom = "3",
                                antallBad = "1",
                                antallWC = "1",
                                adresse = new AdresseType
                                {
                                    adressekode = "1001",
                                    adressenavn = "Byggestedgate",
                                    adressenummer = "11",
                                    adressebokstav = "A"

                                },
                                endring =  EndringsstatusType.Ny
                            },
                            new BruksenhetType
                            {

                                bruksenhetsnummer = new BruksenhetsnummerType
                                {
                                    etasjeplan = new EtasjeplanType
                                    {
                                        kode = "H",
                                        beskrivelse = "H"
                                    },
                                    etasjenummer = "01",
                                    loepenummer = "01"
                                },
                                bruksenhetstype = new BruksenhetstypeKodeType
                                {
                                    kode = "B",
                                    beskrivelse = "Bolig"
                                },
                                kjoekkentilgang = new KjoekkentilgangKodeType
                                {
                                    kode = "0",
                                    beskrivelse = "Ikke oppgitt"
                                },
                                bruksareal = 100,
                                bruksarealSpecified = true,
                                antallRom = "3",
                                antallBad = "1",
                                antallWC = "1",
                                adresse = new AdresseType
                                {
                                    adressekode = "1001",
                                    adressenavn = "Byggestedgate",
                                    adressenummer = "11",
                                    adressebokstav = "B"

                                },
                                endring =  EndringsstatusType.Ny
                            }
                        },
                        avlop = new AvloepstilknytningType()
                        {
                            kode = "OffentligKloakk",
                            beskrivelse = "Offentlig avløpsanlegg" //ref kodeliste https://register.geonorge.no/subregister/byggesoknad/direktoratet-for-byggkvalitet/avlopstilknytning
                        },

                        vannforsyning = new VanntilknytningType()
                        {
                            kode = "TilknyttetOffVannverk", //ref https://register.geonorge.no/subregister/byggesoknad/direktoratet-for-byggkvalitet/vanntilknytning 
                            beskrivelse = "Offentlig vannverk"
                        },
                        harHeis = false,
                        harHeisSpecified = true,
                        endring =  EndringsstatusType.Ny
                    }
            };




            return byggesak;
        }
    }
}
