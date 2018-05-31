using no.geointegrasjon.rep.matrikkelfoering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using no.geointegrasjon.rep.matrikkelfoering;

namespace Geointegrasjon.Matrikkelfoering.SendSample
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
            byggesak.kategori = new ProsesskategoriType() { kode = "IG", beskrivelse = "Søknad om igangsettingstillatelse" };
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
                    new BygningType
                    {
                        bebygdAreal = 100,
                        bebygdArealSpecified = true,
                        bygningsnummer = "1213234",
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
                                bruksarealTilBoligSpecified = true
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
                                    adressekode = "kode",
                                    adressenavn = "Gatenavn",
                                    adressenummer = "1",
                                    adressebokstav = "A",
                                    seksjonsnummer = "0"
                                }
                            }
                        },
                        avlop = new AvloepstilknytningType()
                        {
                            kode = "OffentligKloakk",
                            beskrivelse = "Offentlig avløpsanlegg" //ref kodeliste https://register.geonorge.no/subregister/byggesoknad/direktoratet-for-byggkvalitet/avlopstilknytning
                        },

                        vannforsyning = new VanntilknytningType()
                        {
                            kode = "Annen privat vannforsyning, innlagt vann",
                            beskrivelse = "Annen privat vannforsyning, innlagt vann"
                        },

                        energiforsyning = new EnergiforsyningType()
                        {
                            varmefordeling = new[]
                            {
                                new VarmefordelingType()
                                {
                                    kode = "elektriske panelovner",
                                    beskrivelse = "elektriske panelovner"
                                },
                                new VarmefordelingType()
                                {
                                    kode = "elektriske varmekabler",
                                    beskrivelse = "elektriske varmekabler"
                                },
                            },
                            energiforsyning = new[]
                            {
                                new EnergiforsyningTypeType()
                                {
                                    kode = "biobrensel",
                                    beskrivelse = "biobrensel"
                                },
                                new EnergiforsyningTypeType()
                                {
                                    kode = "elektrisitet",
                                    beskrivelse = "elektrisitet"
                                },
                            },
                            relevant = true,
                            relevantSpecified = true
                        },
                        harHeis = false,
                        harHeisSpecified = true
                    }
            };

            
                

            return byggesak;
        }

    }
}
