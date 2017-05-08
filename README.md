# IdentityServer4Proof
Proof of concept for Authentication and Authorization with IdentityServer 4

<div>
<h2>Inhaltsverzeichnis</h2>
<ol>
  <li>Struktur der Doku</li>
  <li>Ziel</li>
  <li>Ein Flüchtiger Blick auf das Projekt</li>
  <li>Crashkurs: OAuth 2.0 und OpenID Connect</li>
  <li>IdentityServer - Tanz der Versionen</li>
  <li>Das Projekt</li>
</ol>
</div>

<div>
<br/><h2>1. Struktur der Doku</h2>
<p> Mit dieser Dokumentation wird ein möglichst einfacher Einstieg in das vorliegende Projekt gewährleistet. Zunächst wird die Absicht, das Ziel mit dem das Projekt entstanden ist, herausgestellt und ein Überblick über die darin befindlichen Komponenten verschafft. Wer sich zuvor nie mit OAuth 2.0 und OpenID Connect befasst hat, oder sein Wissen auffrischen möchte, findet in einer Kurzanleitung die grundlegenden, groben Eckpunkte, die es zu diesen Themen zu wissen gibt.<br/>Auf diesem Fundament wird schließlich detaillierter auf das Projekt und jede einzelne Komponente eingegangen.</p>
</div>

<div>
<br/><h2>2. Ziel</h2>
<p>Man nehme eine Web API, Clients die Ressourcen über die API anfragen und sichere das Ganze mittels SSL-Verschlüsselung. Was fällt auf? Es fehlen Authentifizierung und Autorisierung!<br/>Mit einer solchen Problemstellung hat dieses Projekt begonnen. Das Ziel ist es demnach eine bestehende Web API mit sicheren Verfahren zur Authentifizierung und Autorisierung von Anwendern und Clients zu schützen. Diese Aufgabe übernimmt unser Held: <b>IdentityServer</b>. Das Projekt stellt eine exemplarische Implementierung möglicher (nicht aller) Authentifizierungs- und Autorisierungsverfahren mittels IdentityServer dar./p>
</div>

<div>
<br/><h2>3. Überblick - Ein Flüchtiger Blick auf das Projekt</h2>
<p>Dieses Projekt ist eine exemplarische Umsetzung verschiedener Authentifizierungs- und Autorisierungsverfahrens mit dem IdentityServer, welcher den OAuth-2.0-Standard und den offene Standard OpenID Connect implementiert.</p>
<br/>
<p>Das Projekt besteht aus den vier Komponenten:</p>
<ul>
<li>AuthServer<br/>Der Autorisierungsserver. Über Ihn werden Clients und Benutzer authentifiziert und Zugriffe auf Ressourcen autorisiert.</li>
<li>WebAPIwithIS3<br/>Die Web API mit geschützten Ressourcen. Die Autorisierung der Zugriffe auf ihre geschützen Ressourcen delegiert sie an den Autorisieringsserver "AuthServer". Diese Web API läuft unter dem <i>Standard .NET Framework</i> und verwendet die Bibliothek <i>IdentityServer3</i> für die Kommunikation mit dem Autorisierungsserver.</li>
<li>WebAPIwithIS3<br/>Die Web API mit geschützten Ressourcen. Die Autorisierung der Zugriffe auf ihre geschützen Ressourcen delegiert sie an den Autorisierungsserver "AuthServer". Diese Web API läuft unter dem <i>.NET Core</i> Framework und verwendet die Bibliothek <i>IdentityServer4</i> für die Kommunikation mit dem Autorisierungsserver.</li>
<li>ClientAppWithIS4<br/>Eine in Visual Studio initial erstellte Webanwendung mit einem geschützten Kontaktfomular. Die Authentifizierung und Autorisierung werden über das implizite Genehmigungsverfahren mit dem Autorisierungsserver "AuthServer" umgesetzt. Die Webanwendung läuft unter <i>.NET Core</i> Framework und verwendet die Bibliothek <i>IdentityServer4</i> für die Kommunikation mit dem Autorisierungsserver. </li>
</ul>
<br/>
<p>Jede der beiden Web APIs besitzt einen Test-Client, über den die geschützten Ressourcen der jeweiligen Web API abgerufen werden. Der Test-Client fragt, um Zugriff auf die geschützten Ressourcen zu erhalten, einen Access-Token beim Autorisierungsserver an und holt sich mit diesem Token die geschützten Ressourcen bei der Web API. Wie dies im Detail funktioniert ist in der Projektbeschreibung unten erklärt.</p>
</div>

<div>
<br/><h2>4. Crashkurs: OAuth 2.0 und OpenID Connect</h2>
<h3>4.1. OAuth 2.0 - kurz und knapp</h3>
<p>OAuth 2.0 ist ein Standard, in dem Verfahren für begrenzenten Zugriff auf HTTP-Ressourcen definiert werden. Dabei kann ein sogenannter Ressource-Owner seine Ressourcen kontrolliert an Clients und Dritte bereitstellen. Ohne OAuth würde dies über die Weitergabe der persönlichen Anmeldedaten des Ressource-Owners erfolgen, womit jedoch zum einen Sicherheitsdefiziete entstehen, da der Client oder Dritte dann vollen Zugriff auf alle Ressourcen des Ressource-Owners erhält und alle damit verbundenen Rechte. Zudem müssten, um dem Client die Rechte wieder zu entziehen, die Anmeldedaten geändert werden, was jedoch zum Entzug der Rechte für alle autorisierten Clients führt. Deshalb kommt OAuth zum Einsatz und bringt Verfahren, mit denen diese Probleme gelöst werden.<br/>
Im Standard ist ein Protokollfluss definiert, der Zugriffe mittels Token kontrolliert.<br/>
Es sind vier Rollen am Protokollfluss beteiligt:</p>
<ul>
  <li>Client</li>
  <li>Ressourceninhaber</li>
  <li>Autorisierungsserver</li>
  <li>Ressourcenserver</li>
</ul>
<p>
Der <b>Client</b> ist die Anwendung, die auf die geschützten Ressourcen zugreifen will. Der <b>Ressourceninhaber</b> (Ressource Owner) ist eine Person, der die Ressourcen auf dem <b>Ressourcenserver</b> (Ressource Server) gehören. Der <b>Autorisierungsserver</b> (Authorization Server) stellt dem Client bei erfolgreicher Autorisierung ein Access-Token aus, mit denem dieser auf angefragte Ressourcen zugegriffen werden kann.</p>
<br/><br/>
</div>

<div>
<p>Die folgende Darstellung stellt den Kommunikationsfluss zwischen den genannten Parteien dar. Die Kommunikation erfolgt aussschließlich über das HTTP Protokoll.</p>
<br/>
</div>

![OAuth 2.0 Protokollfluss](https://github.com/cchichlow/IdentityServer4Proof/blob/master/_img/OAuth_Protokollfluss_engl_small.png)

<div>
<br/>
<p>Dem Ablauf zufolge kann der Client, nachdem er vom Ressourceninhaber eine Autorisierungsgenehmigung erhalten hat, beim Autorisierungsserver einen Access Token anfragen. Dabei gibt er an auf welche Ressourcen er zugreifen möchte und erhält einen Access Token, der nur für ebendiesen Client und nur für die angefragten Ressourcen gültig ist. Mit dem Access Token werden anschließend die Ressourcen beim Ressourcenserver angefragt.
Der Ressourcenserver und Autorisierungsserver können auf dem gleichen Server laufen, müssen also nicht getrennt verwaltet werden.</p>
<p>Der beschriebene Protokollfluss ist nur eine schematische Darstellung und wird über Genehmigungsverfahren realisiert.</p>
<p>Vier <b>Genehmigungsverfahren</b> sind im OAuth-2.0-Standard definiert:</p>

<ul>
<li>Autorisierungscode<br/>
Beim Autorisierungscode wird der Benutzer vom Client an den Autorisierungsserver weitergeleitet und autorisiert dort den Client. Ist der Client erfolgreich autorisiert, erhält er einen Autorisierungscode, mit dem er ein Access Token beim Autorisierungsserver anfragen kann. Erst mit dem Access Token werden Ressourcen beim Ressourcenserver angefragt.</li>
<li>Implizites Genehmigung<br/>
Das implizite Genehmigungsverfahren ist im Prinzip eine Vereinfachung des Autorisierungscode-Verfahrens. Dabei wird dem Client statt einem Autorisierungscode direkt ein Access Token erstellt. Das implizite Genehmigungsverfahren eignet sich besonders für in-browser Clients oder Clients die in einer Skripsprache (wie JavaScript) geschrieben sind.</li>
<li>Ressourceninhaber<br/>
Bei der Genehmigung über die Anmeldedaten des Ressourceninhabers fragt der Client ein Access-Token direkt mit den Anmeldedaten des Ressourceninhabers an und muss von diesem nicht separat am Autorisierungsserver autorisiert werden.</li>
<li>Clientdaten<br/>
Beim Verfahren Clientdaten ist ein Client im Autorisierungsserver registriert und kann über seine Client-Identifikation und ein Client-Secret ein Access-Token anfragen.</li>
</ul>
<br/>

<p>Wie bereits erwähnt, wird nach OAuth-2.0-Standard nur mittels HTTP kommuniziert. Die Endpoints, die dabei angesprochen werden, sind im Standard definiert, als:</p>
<ul>
  <li>Autorisierungs-Endpoint (Autorisierungsserver)</li>
  <li>Token-Endpoint (Autorisierungsserver)</li>
  <li>Redirection-Endpoint (Client)</li>
</ul>
<p>Die URIs der Endpoints müssen für jeden OAuth-Anbieter separat ermittelt werden. Gängig sind jedoch "/authorization" für den Autorisierungs-Endpoint und "/token" für den Token-Endpoint. Die Verwendung der Endpoints verriert je nach Genehmigungsverfahren.</p>
<p>Beim Autorisierungscode-Fluss leitet der Client den Benutzer an den Autorisierungs-Endpoint weiter, um einen Autorisierungscode zu erhalten. Der Autorisierungsserver leitet den Benutzer nach erfolgreicher Autorisierung an den Redirection-Endpoint des Clients weiter. Mit dem Autorisierungscode kann der Client anschließend ein Access-Token am Token-Endpoint und mit diesem schließlich die geschützte Ressource anfragen. Bei einer Anfrage am Autorisierungs-Enpoint im impliziten Genehmigungsverfahren, wird das Access-Token direkt am Autorisierungs-Endpoint ausgestellt und der Benutzer an den Redirection-Enpoint des Clients weitergeleitet. Beim Ressourceninhaber-Fluss wird das Access-Token am Token-Enpoint angefragt. Ebenso bei dem Genehmigungsverfahren über Clientdaten.</p>
<p>OAuth 2.0 ist ein reines Autorisierungsverfahren und bietet keine Mechanismen für eine sicher Authentifizierung. Deshalb wurde mit OpenID ein Protokoll entwickelt, welches auf dem OAuth 2.0 Standard aufsetzend Authentifizierungsmechanismen für einen sicheren Umgang mit Benutzerdaten und Single-Sign-On im Netz ermöglicht.</p>
</div>

<div>
<br/><h3>4.2. OpenID Connect</h3>
<p>OpenID Connect ist eine OpenID Spezifikation, die als Identitätsschicht auf dem O-Auth 2.0 Protokoll mit dem JWT Token Format aufbaut. Die Genehmigungsverfahren bleiben die gleichen, nur, dass nun auch Benutzerdaten wie Ressourcen erfragt werden können. Diese werden als ID Token zurückgegeben und enthalten unter anderem Informationen über den Benutzer und die Authentifizierung eines bestimmten Clients. Ein ID Token ist nur in der Kombination aus Endanwender, Client und dem OpenID Provider gültig. Damit darf ein ID Token für den gleichen Endanwender nicht von un-terschiedlichen Clients akzeptiert werden.</p>
<p>Im OpenID Connect Protokollfluss für den Erhalt von Benutzerdaten werden folgende Rollen unterschieden:</p>
<ul>
<li>Endanwender</li>
<li>Client</li>
<li>OpenID Provider</li>
</ul>
<p>Der OpenID Provider entspricht dabei dem Autorisierungsserver und dem Ressourcenserver aus dem vo OAuth bekannten Protokollfluss.</p>
<p>Ein ID Token ist laut der Spezifikation ein Sicherheitstoken, welches Informationen zur Authentifizierung eines Endanwenders über einen Autorisierungsserver und weitere, angefragte Informationen enthält. Die Spezifikation definiert die Repräsentation des ID Token als JSON Web Token (JWT). Es enthält für alle OAuth 2.0 Informationsflüsse eine Reihe von benötigten, sowie optionalen Claims. Zusätzlich kann ein ID Token eige-ne definierte Claims enthalten, womit auch definiert ist, dass Claims, welche nicht er-kannt werden, server-, wie client-seitig, ignoriert werden müssen.</p>
<br/>
</div>

![OpenID Connect Protokollfluss](https://github.com/cchichlow/IdentityServer4Proof/blob/master/_img/OpenID_Protokollfluss_deut_small.png)

<div>
<p>Obwohl die Technologien für Authentifizierung und Autorisierung mit den genannten Standards einheitlich definiert sind, bleibt ein ernormer Aufwand für die Implementierung einer sicheren Architektur nach OAuth 2.0 und OpenID Connect ein enormer Aufwand. Es liegt im Interesse eines Softwareentwicklers, der die Funktionen lediglich verwenden und nicht erst implementieren möchte, ein Framework zu verwenden, welches ihm die Arbeit abnimmt.</p>
<p>Damit wird unter anderem eine einfache Umsetzung von Single-Sign-On und Zugriffsbeschränkungen für Webapplikationen und -schnittstellen ermöglicht.</p>
<p>OpenID Connect (und damit implizit auch OAuth 2.0) ist vielfältig und in unterschiedlichen Sprachen implementiert. Im .NET Umfeld ist das am häufigsten Verwendete Framework der IdentityServer von Thinktecture. Die neueste Version, der IdentityServer 4, ist seit Ende 2016 auf dem Markt.</p>

<p><b>TODO</b>: Endpoints. /.well-known/openid-configuration, /connect/token </p>
</div>





<div>
<br/><h2>5. IdentityServer - Tanz der Versionen</h2>
<p>Der IdentityServer liegt aktuell (19.04.2017) mit der neuesten Version bei Version 4. Wenn man sich jetzt aber denkt:
<br/><i>Na dann nehme ich doch bedenkenlos die neueste Version und bin damit auf der sicheren, zukunftsfähigen Seite!</i>
<br/>der hat sich geirrt, denn der IdentityServer in der <b>Version 4 ist nur für das .NET Core</b> Framework verfügbar.
<br/>Dies stellt eine entscheidende Einschränkung dar, ist doch das .NET Core Framework eine neue Technologie, die eben ihre ersten Schritte auf dem Boden des .NET Ökosystems macht. An vielen Stellen ist es fehlerhaft oder unvollständig, niemand kann genaue Aussagen über die zukunftsfähigkeit machen und der Umbau bestehender Applikationen, um den IdentityServer4 einzubinden, birgt einen hohen Aufwand. Trotzdem ist dieses Projekt ausschließlich mit IdentityServer4 und auf dem .NET Core Framework umgesetzt.
<br/>Ist das nicht ziemlich widersprüchlich und dumm?
<br/>Nein. Denn Thinktecture verspricht mit dem IdentityServer 4 eine Abwärtskompatibiliät zur Version 3, womit bestehende Webschnittstellen auf dem .NET Standard Framework bleiben können und trotzdem mit einem OpenID Provider für .NET Core ihre Ressourcen schützen können. Dies ist in diesem Projekt mit der Komponente "WebAPIwithIS3" geprüft und bestätigt worden.</p>
</div>





<div>
<br/><h2>6. Das Projekt</h2>
<p>Wie bereits erwähnt, besteht das vorliegende Projekt aus vier Komponenten:</p>
<ul>
  <li>Autorisierungsserver <b>AuthServer</b></li>
  <li>Webanwendung mit IdentityServer 4 <b>ClientAppWithIS4</b></li>
  <li>WebAPI mit IdentityServer 4 <b>WebAPIwithIS4</b></li>
  <li>WebAPI mit IdentityServer 3 <b>WebAPIwithIS3</b></li>
</ul>
<p>Der <b>AuthServer</b> ist ein OpenID Provider. Er führt Informationen zu allen registrierten Usern, den zu schützenden Ressourcen und den autorisierten Clients. Die <b>Client-Anwendung</b> ist eine Anwendung mit teilweise frei verfügbaren und teilweise über den AuthServer geschützten Ressourcen. Die <b>Web API</b> ist eine Schnittstelle, die den Zugriff auf geschützte Ressourcen ebenfalls über den AuthServer verwaltet. Ein Client, der Ressourcen bei der Web API anfragt, muss erst über den Autorisierungsserver autorisiert werden und erhält dann die benötigten Ressourcen.</p>
<p>Alle Komponenten, außer der WebAPI mit IS3, sind nur auf dem .NET Core Framework lauffähig und können demnach auch nur mit der Visual Studio Version 2017 entwickelt werden. Abgesehen von denen, die mit Nerd-Vodoo ihre .NET Core Applikationen auch mit VS2015 zum Laufen bringen. (Ich hörte, das soll es geben...)</p>
<p>Die Abbildung stellt schematisch die Kommunikation zwischen den Komponenten dar. Hierbei ist die geschützte Ressource (Protected Ressource) separat dargestellt, wobei sie im Projekt immer in der Web API integriert ist. 
</div>

![Kommunikation zwischen Entitäten](https://github.com/cchichlow/IdentityServer4Proof/blob/master/_img/Communication_IdServ4Proof_small.png)

<div>
<p>Im Abschnitt "OAuth kurz und knapp" wurden Genehmigungsverfahren beschrieben, von denen drei in diesem Projekt eingesetzt werden: implizite Genehmigung, Ressourceninhaber und Clientdaten. Jeder Client verwendet ein anderes Verfahren:</p>
<table>
  <tr>
  <td><b>Client</b></td>
    <td><b>Genehmigungsverfahren</b></td>
  </tr>
  <tr>
    <td>ClientAppWithIS4</td>
    <td>Implizit</td>
  </tr>
  <tr>
    <td>WebAPIwithIS4</td>
    <td>Ressourceninhaber</td>
  </tr>
    <tr>
    <td>WebAPIwithIS3</td>
    <td>Clientdaten</td>
  </tr>
</table>
<p>Nachfolgend wird jede Komponente einzeln in Hinblick auf ihre Implementierung detaillierter betrachtet.</p>
<div>

</div>
<br/><h3>6.1. AuthServer</h3>
<p> Im vorliegenden AuthServer-Projekt sind die Clients, User und Ressourcen vorkonfiguriert. Alle sind aus bestehenden Klassen in eine SQLite Datenbank migriert.</p>
<h4>6.1.1. Clients</h4>
<p>Die drei genannten Clients werden in der Klasse AuthServer.InMemoryStores.Clients definiert. Die Web API <i>WebAPIwithIS3</i> wird über die ID <i>IS3api</i> angesprochen, die <i>WebAPIwithIS4</i> über die ID <i>testApi</i> und die Client-Anwendung <i>ClientAppWithIS4</i> über die ID <i>openIdConnectClient</i>. NAchfolgende Tabelle gibt eine Übersicht über die konfigurierten Werte zu dem jeweiligen Client:
Client|ID|GrantType|Secret|Scopes|RedirectsUri
WebAPIwithIS3|IS3api|ClientCredentials|superSecretPassword|customAPI.read|-
WebAPIwithIS4|testApi|ResourceOwnerPassword|secret|OpenId Profile Email role customAPI.read|-
openIdConnectClient|openIdConnectClient|Implicit|-|OpenId Profile Email role customAPI| https://localhost:44342/signin-oidc
</p>
```csharp
                new Client
                {
                    ClientId = "IS3api",
                    ClientName = "Web API mit IdentityServer 3",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = new List<Secret> {
                        new Secret("superSecretPassword".Sha256())},
                    // Secret wird über eine von IS4 gebotene extension method gehasht
                    AllowedScopes = new List<string> {"customAPI.read"}
                    // Als Scope wird hier ein selbst implementiertes aus der Klasse Resources verwendet
                }
```
<h4>6.1.2. User</h4>
<p></p>
zwei User (alice und bob) mit pw alice und bob, besitzen jeweils User Claims, die über den Identity-Ressource Scope erreicht werden können.
<h4>6.1.1- Clients</h4>
<p>Identity- und Api-Ressourcen</p>
<p>
Alle Daten werden über das Identity Framework und einer SQLite Datenbank persisten gehalten. Beim Start des AuthServers werden alle Clients, Benutzer und Ressourcen in die Datenbank migriert, falls sie nicht bereits vorhanden sind.</p>

<p>
</p>
<a href=https://github.com/IdentityServer/IdentityServer4.Quickstart.UI></a>
<div>

</div>
<br/><h3>6.2. ClientAppWithIS4</h3>
<p>Die Client-Anwendung ist ein initiales, mit Visual Studio 2017 erstelltes und modifiziertes ASP.NET Projekt für Webanwendungen. Die geschützte Ressource ist das Kontak-Formular. Mit einem Klick auf das Formular wird der Anwender auf die Login-Seite des Autorisierungsserver weitergeleitet und kann darin die Client-Anwendung autorisieren. Anschließend ist der Zugriff auf die Kontakt-Daten erlaubt.
</p>
<div>

</div>
<br/><h3>6.3. WebAPIwithIS4</h3>
<p>Die Web API ist ein initiales, mit Visual Studio 2017 erstelltes und modifiziertes ASP.NET Projekt für Webschnittstellen. Die geschützte Ressource ist exemplarisch das Value-Objekt, welches eingangs zu Beispielzwecken mit dem Projekt erstellt wird. Der Zugriff erfolgt über die Benutzerdaten, indem zunächst ein Post-Request an den Autorisierungsserver gesendet wird, um ein Access Token zu erhalten. Der Content-Type Parameter im Header der Anfrage muss den Wert "x-www-form-urlencoded" haben. Außerdem müssen folgende Paramter im Body der Anfrage enthalten sein:</p>
<ul>
<li>grant_type</li>
<li>username</li>
<li>password</li>
<li>client_id</li>
<li>client_secret</li>
<li>scope</li>
</ul>
<p>
Demnach ist ein Client, der Ressourcen an der Web API anfragt, jede beliebige Anwendung, die über SSL geschütztes HTTP Anfragen senden kann. </p>
<p>Für das aktuelle Projekt sind folgende key-value-Paare anzugeben:<br/>
grant_type : password<br/>
username : alice<br/>
password : Password123!<br/>
client_id : webApi<br/>
client_secret : secret<br/>
scope : openID<br/>
<br/>
Der Grant-Type entspricht dem Ressource-Owner Genehmigungsverfahren, welches im Autorisierungsserver für diesen Client definiert ist. Auch die Client-ID und das Client-Secret sind im Autorisierungsserver hinterlegt. Der Scope gibt an, auf welche Ressourcen der Client zugreifen möchte. Die möglichen Scopes sind im Autorisierungsserver definiert. Der Benutzername und das Passwort für den Benutzer sind im Falle des Ressource-Owner Genehmigungsverfahrens ebenfalls im Autorisierungsserver hinterlegt.
Als Antwort erhält man ein Access Token, mit dem man die Ressourcen an der Web API anfragen kann. Die Web API prüft den Access Token beim Autorisierungsserver und gibt bei einem validen Access Token die gewünschten Ressourcen zurück.
</p>
<div>

</div>
<br/><h3>6.4. WebAPIwithIS3</h3>
<p>Aber sind wir damit nicht wieder auf dem Level der Unsicherheit, bei dem wir die eingangs genannten Vorteile von OAuth über Bord werfen? Nein. Denn die Benutzerdaten werden nur ein mal an den Autorisierungsserver gesendet und ein Token im Gegenzug erhalten. Der Client muss damit die Benutzerdaten nicht persistent speichern, sondern nur den Token. Außerdem bleiben mit dem Token auch die Vorteile der begrenzente und verwaltbaren Zugriffe erhalten.
</p>

</div>
