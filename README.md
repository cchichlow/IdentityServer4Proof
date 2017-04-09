# IdentityServer4Proof
Proof of concept for Authentication and Authorization with IdentityServer 4

<div>
<h2>Inhaltsverzeichnis</h2>
<ol>
<li>Einleitende Worte</li>
<li>Grundlagen zu OAuth 2.0 und OpenID Connect</li>
<li>Technologische Einschränkungen</li>
<li>Das Projekt</li>
</ol>
</div>
<div>
<h2>Einleitende Worte</h2>
<p>Das Projekt ist eine exemplarische Umsetzung eines Authentifizierungs- und Autorisierungsverfahrens mit dem IdentityServer 4. Die dem IdentityServer zugrundeliegenden Standards sind zum einen der OAuth 2.0 Standard mit dem Open Authorization 2.0 Framework und der offene Standard OpenID Connect.</p>
<p>Obwohl die Technologien für Authentifizierung und Autorisierung mit den genannten Standards einheitlich definiert sind, bleibt ein ernormer Aufwand für die Implementierung einer sicheren Architektur nach OAuth 2.0 und OpenID Connect ein enormer Aufwand. Es liegt im Interesse eines Softwareentwicklers, der die Funktionen lediglich verwenden und nicht erst implementieren möchte, ein Framework zu verwenden, welches ihm die Arbeit abnimmt.</p>
<p>Im .NET Umfeld bietet der IdentityServer dabei einen entscheidenden Vorteil. Er implementiert beide Standards und erlaubt eine einfache Umsetzung von Single-Sign-On und Zugriffsbeschränkungen für Webapplikationen und -schnittstellen.</p>
</div>
<div>
<h2>Grundlagen zu OAuth 2.0 und OpenID Connect</h2>
<h4>OAuth 2.0 - kurz und knapp</h4>
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

<h4>OpenID Connect</h4>
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
<p>Der IdentityServer in der Version 4 ist nur für das .NET Core Framework verfügrar.
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

<p>Der AuthServer ist ein OpenID Provider. Er führt Informationen zu allen registrierten Usern, den zu schützenden Ressourcen und den autorisierten Clients. Die Client-Anwendung ist eine Anwendung mit Ressourcen, die über den AuthServer geschützt werden. Die Web API ist eine Schnittstelle, die den Zugriff auf geschützte Ressourcen ebenfalls über den AuthServer verwaltet.</p>
</div>

![Kommunikation zwischen Entitäten](https://github.com/cchichlow/IdentityServer4Proof/blob/master/_img/Communication_IdServ4Proof.jpg)


<div>
<h2></h2>
<p></p>
</div>
