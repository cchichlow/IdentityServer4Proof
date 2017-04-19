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
<h2>Struktur der Doku</h2>
</div>
<div>
<h2>Ziel</h2>
</div>
<div>
<h2>Ein Flüchtiger Blick auf das Projekt</h2>
</div>
<div>
<h2>Crashkurs: OAuth 2.0 und OpenID Connect</h2>
</div>
<div>
<h2>IdentityServer - Tanz der Versionen</h2>
</div>
<div>
<h2>Das Projekt</h2>
</div>





<div>
<h2>Einleitende Worte</h2>
<p>Das Projekt ist eine exemplarische Umsetzung eines Authentifizierungs- und Autorisierungsverfahrens mit dem IdentityServer 4. Die dem IdentityServer zugrundeliegenden Standards sind zum einen der OAuth 2.0 Standard mit dem Open Authorization 2.0 Framework und der offene Standard OpenID Connect.</p>
<p>Obwohl die Technologien für Authentifizierung und Autorisierung mit den genannten Standards einheitlich definiert sind, bleibt ein ernormer Aufwand für die Implementierung einer sicheren Architektur nach OAuth 2.0 und OpenID Connect ein enormer Aufwand. Es liegt im Interesse eines Softwareentwicklers, der die Funktionen lediglich verwenden und nicht erst implementieren möchte, ein Framework zu verwenden, welches ihm die Arbeit abnimmt.</p>
<p>Im .NET Umfeld bietet der IdentityServer dabei einen entscheidenden Vorteil. Er implementiert beide Standards und erlaubt eine einfache Umsetzung von Single-Sign-On und Zugriffsbeschränkungen für Webapplikationen und -schnittstellen.</p>
</div>
<div>
<h2>Grundlagen zu OAuth 2.0 und OpenID Connect</h2>
<h3>OAuth 2.0 - kurz und knapp</h3>
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
Der <b>Client</b> ist die Anwendung, die auf die geschützten Ressourcen zugreifen will. Der <b>Ressourceninhaber</b> (Ressource Owener) ist eine Person, der die Ressourcen auf dem <b>Ressourcenserver</b> (Ressource Server) gehören. Der <b>Autorisierungsserver</b> (Authorization Server) stellt bei erfolgreicher Autorisierung Access Token aus, mit denen auf angefragte Ressourcen zugegriffen werden kann.<br/><br/><br/></p>
</div>

![OAuth 2.0 Protokollfluss](https://github.com/cchichlow/IdentityServer4Proof/blob/master/_img/OAuth_Protokollfluss_engl.png)

<div>
<br/>
<p>Dem Ablauf zufolge kann der Client, nachdem er vom Ressourceninhaber eine Autorisierungsgenehmigung erhalten hat, beim Autorisierungsserver einen Access Token anfragen. Dabei gibt er an auf welche Ressourcen er zugreifen möchte und erhält einen Access Token, der nur für ebendiesen Client und nur für die angefragten Ressourcen gültig ist. Mit dem Access Token werden anschließend die Ressourcen beim Ressourcenserver angefragt.
Der Ressourcenserver und Autorisierungsserver können auf dem gleichen Server laufen, müssen also nicht getrennt verwaltet werden.</p>
<p>Der beschriebene Protokollfluss ist nur eine schematische Darstellung und wird über Genehmigungsverfahren realisiert, von denen die über einen Autorisierungscode und die implizite Genehmigung, die Verfahren sind, die am häufigsten Anwendung finden. Beim <b>Autorisierungscode</b> wird der Benutzer vom Client an den Autorisierungsserver weitergeleitet und autorisiert dort den Client. Ist der Client erfolgreich autorisiert, erhält er einen Autorisierungscode, mit dem er ein Access Token beim Autorisierungsserver anfragen kann. Erst mit dem Access Token werden Ressourcen beim Ressourcenserver angefragt. Das implizite Genehmigungsverfahren ist im Prinzip eine Vereinfachung des Autorisierungscode-Verfahrens. Dabei wird dem Client statt einem Autorisierungscode direkt ein Access Token erstellt. Das implizite Genehmigungsverfahren eignet sich besonders für in-browser Clients oder Clients die in einer Skripsprache (wie JavaScript) geschrieben sind.<br/><br/></p>

<h3>OpenID Connect</h3>
<p>OAuth 2.0 ist ein reines Autorisierungsverfahren und bietet keine Mechanismen für eine sicher Authentifizierung. Deshalb wurde mit OpenID ein Protokoll entwickelt, welches auf dem OAuth 2.0 Standard aufsetzend Authentifizierungsmechanismen für einen sicheren Umgang mit Benutzerdaten und Single-Sign-On im Netz ermöglicht. <b>OpenID Connect</b> ist eine OpenID Spezifikation, die als Identitätsschicht auf dem O-Auth 2.0 Protokoll mit dem JWT Token Format aufbaut. Die <b>Genehmigungsverfahren</b> bleiben die gleichen, nur, dass nun auch Benutzerdaten wie Ressourcen erfragt werden können. Diese werden als <b>ID Token</b> zurückgegeben und enthalten unter anderem Informationen über den Benutzer und die Authentifizierung eines bestimmten Clients. Ein ID Token ist nur in der Kombination aus Endanwender, Client und dem OpenID Provider gültig. Damit darf ein ID Token für den gleichen Endanwender nicht von un-terschiedlichen Clients akzeptiert werden.</p>
<p>Im OpenID Connect Protokollfluss für den Erhalt von Benutzerdaten werden folgende Rollen unterschieden:</p>
<ul>
<li>Endanwender</li>
<li>Client</li>
<li>OpenID Provider</li>
</ul>
<p>Der OpenID Provider entspricht dabei dem Autorisierungsserver und dem Ressourcenserver aus dem vo OAuth bekannten Protokollfluss.</p>
<p>Ein ID Token ist laut der Spezifikation ein Sicherheitstoken, welches Informationen zur Authentifizierung eines Endanwenders über einen Autorisierungsserver und weitere, angefragte Informationen enthält. Die Spezifikation definiert die Repräsentation des ID Token als JSON Web Token (JWT). Es enthält für alle OAuth 2.0 Informationsflüsse eine Reihe von benötigten, sowie optionalen Claims. Zusätzlich kann ein ID Token eige-ne definierte Claims enthalten, womit auch definiert ist, dass Claims, welche nicht er-kannt werden, server-, wie client-seitig, ignoriert werden müssen.<br/></p>
<p>OpenID Connect (und damit implizit auch OAuth 2.0) sind vielfältig und in unterschiedlichen Sprachen implementiert. Im .NET Umfeld ist das am häufigsten Verwendete Framework der IdentityServer von Thinktecture. Die neueste Version, der IdentityServer 4, ist seit Ende 2016 auf dem Markt.</p>
</div>

<div>
<h2>Technologische Einschränkungen</h2>
<p>Der IdentityServer in der Version 4 ist nur für das .NET Core Framework verfügbar.
<br/>
Dies stellt eine entscheidende Einschränkung dar, ist doch das .NET Core Framework eine neue Technologie, die eben erst ihre ersten Schritte auf dem Boden des .NET Ökosystems macht. An vielen Stellen ist es fehlerhaft oder unvollständig, niemand kann genaue Aussagen über die zukunftsfähigkeit machen und der Umbau bestehender Applikationen, um den IdentityServer4 einzubinden, birgt einen hohen Aufwand. Trotzdem ist dieses Projekt ausschließlich mit IdentityServer4 und auf dem .NET Core Framework umgesetzt. Thinktecture verspricht mit dem IdentityServer 4 eine Abwärtskompatibiliät zur Version 3, womit auch bestehende Webschnittstellen mit einem OpenID Provider, der auf dem .NET Core Framework basiert, Ressourcen schützen können. </p>
</div>

<div>
<h2>Das Projekt</h2>
<p>Das vorliegende Projekt besteht aus drei Komponenten:</p>
<ul>
  <li>AuthServer</li>
  <li>Client-Anwendung</li>
  <li>Web API</li>
</ul>

<p>Der <b>AuthServer</b> ist ein OpenID Provider. Er führt Informationen zu allen registrierten Usern, den zu schützenden Ressourcen und den autorisierten Clients. Die <b>Client-Anwendung</b> ist eine Anwendung mit teilweise frei verfügbaren und teilweise über den AuthServer geschützten Ressourcen. Die <b>Web API</b> ist eine Schnittstelle, die den Zugriff auf geschützte Ressourcen ebenfalls über den AuthServer verwaltet. Ein Client, der Ressourcen bei der Web API anfragt, muss erst über den Autorisierungsserver autorisiert werden und erhält dann die benötigten Ressourcen.</p>
<p>Alle drei Komponenten sind für das .NET Core Framework implementiert und nur auf diesem Lauffähig.</p>
</div>

![Kommunikation zwischen Entitäten](https://github.com/cchichlow/IdentityServer4Proof/blob/master/_img/Communication_IdServ4Proof.jpg)

<div>
<p>Im Abschnitt "OAuth kurz und knapp" wurden die zwei gängigsten Genehmigungsverfahren beschrieben, das Verfahreung über einen Autorisierungscode und das implizite Verfahren. Ein weiteres Verfahren stellt die Genehmigung über die Benutzerdaten des Ressourceninhabers dar.</p>
<p>Aber sind wir damit nicht wieder auf dem Level der Unsicherheit, bei dem wir die eingangs genannten Vorteile von OAuth über Bord werfen? Nein. Denn die Benutzerdaten werden nur ein mal an den Autorisierungsserver gesendet und ein Token im Gegenzug erhalten. Der Client muss damit die Benutzerdaten nicht persistent speichern, sondern nur den Token. Außerdem bleiben mit dem Token auch die Vorteile der begrenzente und verwaltbaren Zugriffe erhalten.</p>
<h3>AuthServer</h3>
<p> Im AuthServer sind die Clients in der Klasse <i>Clients</i> implementiert. Er bietet der Web API die eben beschriebene Zugriffsmöglichkeit über die Benutzerdaten des Ressourceninhabers. Die Client-Anwendung greift über den impliziten Fluss auf die geschützten Ressourcen zu. Alle Daten werden über das Identity Framework und einer SQLite Datenbank persisten gehalten. Beim Start des AuthServers werden alle Clients, Benutzer und Ressourcen in die Datenbank migriert, falls sie nicht bereits vorhanden sind. 
</p>
<h3>Client-Anwendung</h3>
<p>Die Client-Anwendung ist ein initiales, mit Visual Studio 2017 erstelltes und modifiziertes ASP.NET Projekt für Webanwendungen. Die geschützte Ressource ist das Kontak-Formular. Mit einem Klick auf das Formular wird der Anwender auf die Login-Seite des Autorisierungsserver weitergeleitet und kann darin die Client-Anwendung autorisieren. Anschließend ist der Zugriff auf die Kontakt-Daten erlaubt.</p>
<h3>Web API</h3>
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
</div>


<div>
<h2></h2>
<p></p>
</div>
