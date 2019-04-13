# AVG

REST-Schnittstelle Funktionalitäten:
▪ GET (ohne If-None-Match Header)
▪ condtional GET (mit If-None-Match Header)
▪ PUT (mit If-Match Header)

Diese HTTP Methoden realisieren folgende Use-Cases:
▪ LIST(Suppliers) findAllPreferredSuppliers
▪ Supplier findPreferredSupplier(Product p)
▪ void setPreferredSupplierForProduct(Supplier s, Product c)
  throws UnknownSupplierException, UnknownProductException
  
Beispielhafte Requests finden sich unter AvG Abgabe 1 - Webapp in der postman_collection.json-Datei
Ggfs kann dies in das eigene Postman importiert werden

Im Quellcode vorhanden (aber nicht mehr relevant für die Abgabe und deshalb noch nicht getestet): 
▪ POST
▪ DELETE
