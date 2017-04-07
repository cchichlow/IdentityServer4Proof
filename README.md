# IdentityServer4Proof
Proof of concept for Authentication and Authorization with IdentityServer 4

<div>
<h2>Einleitende Worte</h2>
<p>Das Projekt ist eine exemplarische Umsetzung eines Authentifizierungs- und Autorisierungsverfahrens mit dem IdentityServer 4. Die dem IdentityServer zugrundeliegenden Standards sind zum einen der OAuth 2.0 Standard mit dem Open Authorization 2.0 Framework und der offene Standard OpenID Connect.</p>
<p>Obwohl die Technologien für Authentifizierung und Autorisierung mit den genannten Standards einheitlich definiert sind, bleibt ein ernormer Aufwand für die Implementierung einer sicheren Architektur nach OAuth 2.0 und OpenID Connect ein enormer Aufwand. Es liegt im Interesse eines Softwareentwicklers, der die Funktionen lediglich verwenden und nicht erst implementieren möchte, ein Framework zu verwenden, welches ihm die Arbeit abnimmt.</p>
<p>Im .NET Umfeld bietet der IdentityServer dabei einen entscheidenden Vorteil. Er implementiert beide Standards und erlaubt eine einfache Umsetzung von Single-Sign-On und Zugriffsbeschränkungen für Webapplikationen und -schnittstellen.</p>
</div>

<div>
<h2>Einschränkungen</h2>
<p>Der IdentityServer in der Version 4 ist nur unter dem .NET Core Framework
<br/>
<br/>
Dies stellt eine entscheidende Einschränkung dar, ist doch das .NET Core Framework eine neue Technologie, die eben erst ihre ersten Schritte auf dem Boden der .NET Ökosystems macht. An vielen Stellen ist es fehlerhaft oder unvollständig, niemand kann genaue Aussagen über die zukunftsfähigkeit machen und der Umbau bestehender Applikationen, um den IdentityServer4 einzubinden, birgt einen hohen Aufwand. Trotzdem ist dieses Projekt ausschließlich mit IdentityServer4 und auf dem .NEt Core Framework umgesetzt. Thinktecture verspricht mit dem IdServ4 eine abwärtskompatibiliät zur Version 3, womit auch bestehende Webschnittstellen mit einem OpenID Provider, der auf dem .NET Core Framework basiert, Ressourcen schützen können. </p>
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

<h4>Client</h4>
<p></p>
<h4>OpenID Provider</h4>
<p></p>
<h4>Ressourcen</h4>
<p></p>

</div>
<div>
<h2></h2>
<p></p>
</div>
