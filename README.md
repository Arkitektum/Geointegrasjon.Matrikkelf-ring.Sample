Geointegrasjon.Matrikkelføring.Sample
======================================
Eksempler på hvordan man kan bruke FIKS for å sende informasjon fra eByggeSak
til matrikkelføringsklienter og hvordan disse klientene kan ta imot informasjonen, 
i henhold til ny (kommende) Geointegrasjonsstandard for overføring.

Funksjonalitet
--------------

Eksemplet `SendSample` sender meldinger med forsendelsestype 
`Geointegrasjon.Matrikkelføring`, slik at de kan plukkes opp av mottakstjenesten  som lytter på dette. Meldingen sendes med egenskapen "kun digital levering", slik 
at den ikke går videre til brevpost ved manglende henting.  Det blir sendt 
meldinger med ulike nivåer av klarhet for matrikkelføring, fra nivå 0 (det har 
skjedd et vedtak) til nivå 4 (full situasjonsplan). Se koden for nærmere info om 
hva som gjøres i hvert nivå.

Når du har sendt meldinger vil de være synlige i [KS Svarut sin testportal](https://test.svarut.ks.no).

Eksempelet `ReceiveSample` er en mottakstjeneste som lytter på slike meldinger. 
Den henter ut listen over mottatte meldinger, laster ned innholdet og dekrypterer 
det, og sender kvittering på at innholdet er korrekt mottatt. Når SvarInn mottar
kvitteringen vil den slette meldingen i sin ende.

Konfigurasjon
-------------

Koden krever noen private nøkler som du må hente ut fra [KS Svarut sin testportal](https://test.svarut.ks.no).

For at ikke de skal havne ut i kildekontroll, legges de i filen 
LocalSettings.config, som er referert fra csproj-filene med lagt inn i .gitignore. 
Du må kopiere LocalSettings.default.config og omdøpe den, og deretter legge inn verdiene.

__For sending:__
 - `SvarUtUsername`: Navnet du har i SvarUt
 - `SvarUtPassword`: Service-passordet som er generert i SvarUt (må regeneres om du glemmer den)
 - `OrgNrReceiver`: Orgnummeret du skal sende til, typisk din egen i dette scenariet. Må være en du har tilgang til å lese meldinger til!

__For mottak:__
- `MottakUserName`: Navnet på mottakersystemet som lytter på SvarInn
- `MottakPassword`: Service-passordet som er generert for mottakersystemet (må regeneres om du glemmer den)

I portalen må du konfigurere dette mottakersystemet til å lytte på orgnummeret du sender til i SendSample (`OrgNrReceiver`), og forsendelsestypen 
`Geointegrasjon.Matrikkelføring`. (Per mai 2018 er det ikke mulig å skrive inn 
forsendelsestypen selv, og man må derfor velge "Alle", som åpenbart ikke er gunstig
i en ekte konfigurasjon. Bugen er meldt inn til KS.)

I tillegg må du ha lastet opp et public sertifikat til portalen. Dette er fordi 
alle meldinger fra FIKS til mottager krypteres, og hvis du ikke laster opp 
sertifikatet vil alle meldinger du laster ned være en 1 KB tekstfil der det står 
"Mangler public key for mottaker, kan ikke sende".

Slik kan du generere nøkkel med OpenSSL (med Linux eller Linux på Windows): 

`openssl req -new -x509 -days 999 -nodes -out fiks_demo_private.pem -keyout fiks_demo_public.pem`

`fiks_demo_public.pem` må altså lastes opp til portalen.

For å installere den private delen av sertifikatet i din Windows User Certificate 
Store, må du konvertere den til et annet format:

`openssl pkcs12 -export -out fiks_demo_combined.p12 -in fiks_demo_public.pem -inkey fiks_demo_private.pem`

Den konverterte filen `fiks_demo_combined.p12` kan dobbeltklikkes for å importeres i Windows, og etter det
er den tilgjengelig for .Net og demokoden.