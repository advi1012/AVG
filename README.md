# AVG
> Diese Datei ist in Markdown geschrieben und kann mit `<Strg><Shift>v` in
> Visual Studio Code leicht gelesen werden.
>
> Näheres zu Markdown gibt es in einem [Wiki](https://github.com/adam-p/markdown-here/wiki/Markdown-Cheatsheet)
## Server starten/ schließen
Mit 'strg'  F5 kann der Server gestartet werden (, allerdings muss der Reiter auf die Webapp verweisen).
Alternativ kann auch die graphische Oberfläche genutzt werden (der grüne Pfeil).
Der Server kann geschlossen werden, indem man im oder neben dem System Tray (bei Windows standardmäßig unten rechts auf den Bildschirm) 
auf das ISS Express Icon rechts klickt und dann Exit wählt. Alternativ kann auch einfach die Projektmappe geschlossen werden.

```
Hinweis: Da das Zertifikat, welches ISS Express verwendet, selbstsigniert ist, kommt im Browser eine Warnung. Man kann entweder dieses Zertifikat importieren 
		oder aber auch im Browser auf weiter klicken, wenn die Wranung kommt. Zum Testen der Requests benutzt man am Besten Postman, da auch die Besipiel Requests 
		als postman_collection.json-Datei exportiert wurden. 
```

## Features
Die REST-Schnittstelle realisiert folgende Funktionalitäten mit Hilfe des asp.net core Frameworks:
```
	▪ GET (ohne If-None-Match Header)
	▪ condtional GET (mit If-None-Match Header)
	▪ PUT (mit If-Match Header)
```
Diese HTTP Methoden realisieren folgende Use-Cases:
```
	▪ LIST(Suppliers) findAllPreferredSuppliers
	▪ Supplier findPreferredSupplier(Product p)
	▪ void setPreferredSupplierForProduct(Supplier s, Product c)
	throws UnknownSupplierException, UnknownProductException
```  
Wir nutzen dabei das EF Core Framework mit MSSQL für die Verwaltung der Daten.
Die Datenbank (LocalDb, mehr Indos unter https://docs.microsoft.com/de-de/aspnet/core/tutorials/razor-pages/sql?view=aspnetcore-2.2&tabs=visual-studio) 
ist im Projekt integriert und wurde so konfiguriert, dass beim (neu-)starten diese in einem konsistenten Zustand vorliegt.
(D.h die initialen Daten erhält man wieder durch Neustart des Servers.) 

Im Quellcode vorhanden (aber nicht mehr relevant für die Abgabe und deshalb noch nicht getestet): 
```
	▪ POST
	▪ DELETE
```
## Testdaten
Bezüglich der Test-Requests, die sich unter "AvG Abgabe 1 - Webapp" in der JSON-Datei "AVG-Abgabe 1.postman_collection.json" finden lassen, 
wurden folgende Anwendungsfälle getestet:
```
	1. Ein erfolgreicher Put-Request 	(Implementierung von void setPreferredSupplierForProduct(Supplier s, Product c) throws UnknownSupplierException, UnknownProductException)
	2. Ein erfolgreicher, bedingter Get-Request mit If-None-Match-Header	(Implementierung von Supplier findPreferredSupplier(Product p))
	3. Ein erfolgreicher Get-Request	(Implementierung von Supplier findPreferredSupplier(Product p))
	4. Ein erfolgreicher Get-Request (Implementierung von LIST(Suppliers) findAllPreferredSuppliers)
	5. Ein erfolgsloser Get-Request zu einem nicht vorhandenen Supplier		(Implementierung von Supplier findPreferredSupplier(Product p))
	6. Ein erfolgsloser Put-Request zu einem nicht vorhandenen Supplier 	(Implementierung von void setPreferredSupplierForProduct(Supplier s, Product c) throws UnknownSupplierException, UnknownProductException)
	7. Ein erfolgsloser Put-Request ohne If-Match-Header	(Implementierung von void setPreferredSupplierForProduct(Supplier s, Product c) throws UnknownSupplierException, UnknownProductException)
	8. Ein erfolgsloser Put-Request zu einem nicht vorhandenen Product	(Implementierung von void setPreferredSupplierForProduct(Supplier s, Product c) throws UnknownSupplierException, UnknownProductException)
	9. Ein erfolgsloser Get-Request zu einem nicht vorhandenen Product	(Implementierung von Supplier findPreferredSupplier(Product p))
```

## grpc
Um den Server zu starten:
```CMD
cd .\SupplierClientGRPC\
dotnet run -f netcoreapp2.1
```
Um den Client zu starten:
```CMD
cd .\SupplierServerGRPC\
dotnet run -f netcoreapp2.1
```
Diese können auch in Visual Studio über strg F5 gestartet werden, jedoch kann nur dann jeweils eine Instanz offen sein.
> Hinweis: Da mit asynchrone Vearbeitung gearbeitet wurde, funktionieren try und catch (gehen nur im selben Thread) nicht! 
>
> Deswegen wurde hier auf Fehlerfälle testen verzichtet.
>
> Die 'setPreferredSupplierForProduct' funktioniert noch nicht richtig, da nicht im richtigen Thread geupdated wird.
>
> TODO: Letzteres beheben!

Einfaches Tutorial: https://grpc.io/docs/tutorials/basic/csharp.html

### Für Entwickler

Um die .proto Datei erkennen zu können, erweitere die .csproj Datai um Folgendes:
```XML
  <ItemGroup>
    <Protobuf Include="**/*.proto" />
  </ItemGroup>
```
Referenz: https://github.com/grpc/grpc/blob/v1.20.0/src/csharp/BUILD-INTEGRATION.md

>Hinweis: Seit proto3 sind Felder, wenn nicht genauer spezifiziert, defaultmäßig auf optioanl gesetzt.
>
>Mehr dazu: https://developers.google.com/protocol-buffers/docs/proto3
