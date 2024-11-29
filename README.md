# Machine QR
Auteur(s): Hayley Bachot, Quin Verhoeven, Liam Vereycken  
Reviewers: S. Princen, K. Provoost  
**Via commits wordt de laatste versie (inclusief datum) bijgehouden**
## 1	OVERZICHT
Onze opdracht is om een webapp oprichten die een overzicht geeft voor alle machines in het werkhuis hier op school. Er wordt zo onder andere de serienummers, veiligheidsinstructiekaarten, onderhoudsfiches, de handleidingen en foto’s getoond. Eventueel zou het overzicht ook de status van deze machines kunnen aantonen (bv. of deze defect is of online of offline is ).

## 2	CONTEXT
Om te controleren of er een machine nog werkt moet meneer De Poorter steeds alle gegevens handmatig nakijken, wat leidt tot een chaos. Door het maken van onze webapp hopen we zijn job een beetje te vergemakkelijken. Het is belangrijk omdat deze de job van meneer De Poorter makkelijker maakt en orde brengt. 

Enkele vragen die eventueel van toepass kunnen komen:
Hoe gaan we dit project aanpakken? Welke vorm gaan we onze webapp geven? Hoeveel informatie moet er opgeslagen worden? Welke machines moeten er allemaal in de database? Hoe gaan we zorgen voor een QR-code bij elke machine? Welke programma’s gaan we gebruiken?
Hierdoor leren we veel dingen die belangrijk kunnen zijn voor toekomstige projecten.

## 3	DOELEN EN NIET-DOELEN
## 3.1	Doelen (Requirements)
De bedoeling is om een overzicht te creëren voor alle machines die gebruikt worden in het werkhuis. De admin (In dit geval meneer De Poorter) heeft dan de mogelijkheid om alle bestanden te beheren. Hij zou voor elke machine een QR-code moeten scannen om deze in een lijst te zetten op onze webapp. Deze geeft dan allerlei kenmerken weer. (Misschien sorteren we de namen van de machines dan ook op alfabetische volgorde.) Als een machine defect is moet de admin ook de mogelijkheid hebben om de status te zien van machines (online of offline indien deze defect is). Hij zou dan eventueel een machine in de lijst kunnen opzoeken en/of verwijderen.
### 3.2	Niet-Doelen
Dit is uit zichzelf een vrij klein project. De bedoeling is dat we deze niet te ver proberen uit te breiden. 
## 4	PLANNING
### Todo: Hayley
 -Check software
 
-Install extensions vscode

-Brainstorm design elke pagina

-Front-end homepage

-Back-end homepage


### Todo: Quin
-Routing

-Config database

-Frontend machine bio

-Login back-end

-Login safety

### Todo: Liam
-Check software

-Install extensions vscode

-Front-end machine list

-Zoek QR code API en voeg toe in machine page

-Login front-end

-Back-end machine list
## 5	BENODIGDE MIDDELEN
Middelen vereist voor het ontwikkelen van het project: Webapp

Programmeertalen: HTML, CSS, C#

Programma’s: Visual Studio Code, PostgreSQL…

Versiebeheersystemen: Github

Frameworks: Asp.net (inclusief Bootstrap)
## 6	VERWACHTE PROBLEMEN
Momenteel nog geen problemen.
## 7	OPLOSSINGEN
## 7.1	Bestaande Oplossing
Beschrijf de huidige situatie en hoe gebruikers nu omgaan met het systeem. Geef een ge-bruikersverhaal of voorbeeld van hoe de data door het systeem stroomt.
Bestaan er reeds projecten, programma’s, websites, … om dit probleem op te lossen?
### 7.2	Voorgestelde Oplossing
Geef een overzicht van de voorgestelde technische oplossing:
•	Begin met het grote plaatje en werk naar de details toe.
•	Beschrijf de technische architectuur of aanpak.
•	Voeg indien nodig diagrammen en schema's toe om het proces visueel te maken.
•	Loop door een gebruikersverhaal om te laten zien hoe de nieuwe oplossing werkt.
### 7.3	Alternatieve Oplossingen
Welke andere oplossingen heb je overwogen?  
Wat zijn de voor- en nadelen van deze alternatieven?
## 8	TESTBAARHEID, MONITORING EN DETECTIE
We werken met branches in github: 
main (De main branch, alles komt hier terecht)
develop (Aanpassingen groot en klein, alle features worden hier eerst gemerged)
feature/”naam feature” (Nieuwe functies toevoegen)
release (Klaar om uit te brengen)
Bij een pull request wordt alles eerst getest voordat er een merge gebeurt.

## 9	AFHANDELING
Zijn er nog onopgeloste kwesties?  
Welke beslissingen moeten nog worden genomen?  
Zijn er toekomstgerichte voorstellen of ideeën die buiten de scope van dit project vallen?

