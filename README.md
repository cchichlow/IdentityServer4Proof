# IdentityServer4Proof
Proof of concept for Authentication and Authorization with IdentityServer 4

</div>
<div>
<h2>Inhaltsverzeichnis</h2>
<ol>
<li>Einleitende Worte</li>
<li>Grundlagen zu OAuth 2.0 und OpenID Connect</li>
<li>Technologische Einschränkungen</li>
<li>Das Projekt</li>
</ol>
<br/>
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
Im Standard ist ein Protokollfluss definiert, der Zugriffe mittels Token kontrolliert.<br/><br/>
Es sind vier Rollen am Protokollfluss beteiligt:</p>
<ul>
<li>Client</li>
<li>Ressourceninhaber</li>
<li>Autorisierungsserver</li>
<li>Ressourcenserver</li>
</ul>
<p>
Der Client ist die Anwendung, die auf die geschützten Ressourcen zugreifen will. Der Ressourceninhaber (Ressource Owener) ist eine Person, der die Ressourcen auf dem Ressourcenserver (Ressource Server) gehören. Der Autorisierungsserver (Authorization Server) stellt bei erfolgreicher Autorisierung Access Token aus, mit denen auf angefragte Ressourcen zugegriffen werden kann.<br/><br/>
</p>
</div>

![OAuth 2.0 Protokollfluss](https://github.com/cchichlow/IdentityServer4Proof/blob/master/_img/OAuth_Protokollfluss_engl.png)

<div>

<p>
Dem Ablauf zufolge kan der Client, nachdem er vom Ressourceninhaber eine Autorisierungsgenehmigung erhalten hat,
</p>
<h4>OpenID Connect</h4>
<p></p>
<h6>Client</h6>
<p></p>
<h6>OpenID Provider</h6>
<p></p>
<h6>Ressourcen</h6>
<p></p>
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
