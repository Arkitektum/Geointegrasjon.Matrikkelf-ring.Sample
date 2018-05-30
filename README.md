Geointegrasjon.Matrikkelføring.Sample
======================================
Eksempler på hvordan man kan bruke FIKS for å sende informasjon fra eByggeSak
til matrikkelføringsklienter, i henhold til ny (kommende) Geointegrasjonsstandard for overføring.

Funksjonalitet
--------------

Eksemplet sender meldinger med forsendelsestype "`Geointegrasjon.Matrikkelføring`", slik at de kan plukkes opp av mottakstjenesten som lytter på dette. Det blir sendt meldinger med ulike nivåer av klarhet for matrikkelføring, fra nivå 0 (det har skjedd et vedtak) til nivå 4 (full situasjonsplan). Se koden for nærmere info om hva som gjøres i hvert nivå.

Konfigurasjon
-------------

Koden krever noen private nøkler som du må hente ut fra [KS Svarut sin testportal](https://test.svarut.ks.no).

For at ikke de skal havne ut i kildekontroll, legges de i filen LocalSettings.config, som er referert fra csproj-filene med lagt inn i .gitignore. Du må kopiere LocalSettings.default.config og omdøpe den, og deretter legge inn verdiene.

For sending:
 - `SvarUtUsername`: Navnet du har i SvarUt
 - `SvarUtPassword`: Service-passordet som er generert i SvarUt (må regeneres om du glemmer den)
 - `OrgNrReceiver`: Orgnummeret du skal sende til, typisk din egen i dette scenariet. Må være en du har tilgang til å lese meldinger til!

