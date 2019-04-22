using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AvG_Abgabe_1___Webapp.Model;
using AvG_Abgabe_1___Webapp.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AvG_Abgabe_1___Webapp.Controllers
{
    /// <summary>
    /// Die Klasse SupplierController beinhaltet sowohl Router (Abbildung von URIs auf Funktionen) 
    /// als auch Handlerfunktionalität (HTTP-Statuscode, Verarbeitung des Requests, Response zurückliefern...)
    /// </summary>
    [Route("/Supplier")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierservice;
        private readonly IUrlHelper _urlHelper;

        // Constructor Injection
        public SupplierController(ISupplierService supplierservice, IUrlHelper urlHelper)
        {
            _supplierservice = supplierservice;
            _urlHelper = urlHelper;
        }

        /// <summary>
        /// implementiert die Httpmethode GET: https://localhost:44337/Supplier
        /// Routing für LIST(Suppliers) findAllPreferredSuppliers()
        /// 
        /// Bis jetzt mögliche Queryparameter << ?product_id=... >>, << ?id=... >>
        /// Routing für Supplier findPreferredSupplier(Product p)
        /// Beispiel: https://localhost:44337/Supplier?product_id=00000000-0000-0000-0000-000000000000
        /// </summary>
        /// <returns> Statuscode 200 OK, im Fehelerfall Statuscode 404 Not Found </returns>
        [HttpGet(Name = nameof(GetSupplier))]
        public ActionResult<IEnumerable<Supplier>> GetSupplier()
        {
            var queryParam1 = Request.Query[Constants.product_idPath].ToString();
            var queryParam2 = Request.Query[Constants.idPath].ToString();
            var requestedEtag = Request.Headers[Constants.IF_NONE_MATCH];
            // var lastModified = Request.Headers[Constants.IF_MODIFIED_SINCE];
            try
            {
                // falls ?product_id eingegeben wurde: Suche nach Supplier über das Produkt mit dieser Id
                if (!String.IsNullOrEmpty(queryParam1) && Request.Query.Count() == 1)
                {
                    var product = _supplierservice.findProductById(queryParam1);
                    var result = _supplierservice.findPreferredSupplier(product);
                    var success = SupplierToOk(requestedEtag, result) as ActionResult;
                    return success;
                }

                // falls ?id eingegeben wurde: Suche nach Supplier mit dieser Id
                if (!String.IsNullOrEmpty(queryParam2) && Request.Query.Count() == 1)
                {
                    var supplier = _supplierservice.findById(queryParam2);
                    var success = SupplierToOk(requestedEtag, supplier) as ActionResult;
                    return success;
                }

                // falls keine oder falsche Queryparameter angegeben werden: Gib alle preferred suppliers zurück 
                var list = _supplierservice.findAllPreferredSuppliers();
                if (list.Count() > 0)
                {
                    return Ok(list.Select(supplier => CreateItemLinksForSupplier(supplier)));
                }

                return NotFound(Constants.UnknownSupplierMessage);
            } catch (UnknownSupplierException sup)
            {
                return NotFound(sup.Message);
            }
            catch (UnknownProductException pro)
            {
                return NotFound(pro.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, Constants.INTERNAL_SERVER_ERROR + ": " + e.Message);
            }
        }

        /// <summary>
        /// implementiert die Httpmethode PUT: void setPreferredSupplierForProduct(Supplier s, Product c)
        /// throws UnknownSupplierException, UnknownProductException
        /// Beispiel: https://localhost:44337/Supplier?product_id=00000000-0000-0000-0000-000000000001
        /// </summary>
        /// <param name="id"> Id des PreferredSuppliers, um diesen in das Product einzutragen </param>
        /// <param name="c"> Im Body des PUT-Requests, zu aktualisierendes Product </param>
        /// <returns> Status Code 204 Created, andernfalls 400 BadRequest </returns>
        [HttpPut(Name = nameof(PutSupplier))]
        public ActionResult<NoContentResult> PutSupplier([FromBody] Product c)
        {
            // Exceptionhandling: Im Fehlerfall soll der Client nur den Statuscode 400 BadRequest bzw  Not Found 404 sehen.
            try
            {
                var productId = Request.Query[Constants.product_idPath].ToString();
                if (String.IsNullOrEmpty(productId))
                {
                    throw new UnknownProductException(Constants.ProductNotSpecified);
                }
                Supplier s = _supplierservice.findById(c.preferredSupplier);
                // nur die Versionszahl interessiert uns, der Rest soll entfernt werden
                var version = Request.Headers[Constants.IF_MATCH].ToString().Replace("\"", "").Replace("\\", "");
                // optimistische Synchronisation benötigt "If-Match" im Request-Header
                if (String.IsNullOrEmpty(version) || version != s.version.ToString())
                {
                    return StatusCode(412, Constants.PRECONDTION_FAILED);
                }
                if (c == null)
                {
                    return BadRequest();
                }
                _supplierservice.setPreferredSupplierForProduct(s, c, productId);
                ETagBuilder(s);
                return NoContent();
            } catch (UnknownSupplierException sup)
            {
                return NotFound(sup.Message);
            } catch (UnknownProductException pro)
            {
                return NotFound(pro.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, Constants.INTERNAL_SERVER_ERROR + ": " + e.Message);
            }
        }

        // Standardmäßige HTTP-Methoden (nicht verlangt)
        [HttpPost(Name = nameof(PostSupplier))]
        public async Task<ActionResult<CreatedResult>> PostSupplier([FromBody] Supplier supplier)
        {
            Supplier result;
            try
            {
              result =  _supplierservice.Create(supplier);
            } catch(Exception e) {
                return BadRequest();
            }
            Supplier updatedResult = result.Clone(0, DateTime.Now, DateTime.Now);
            return CreatedAtAction(nameof(PostSupplier), new { id = updatedResult.id }, updatedResult);
        }

        [HttpDelete("{id}", Name = nameof(DeleteSupplier))]
        public async Task<ActionResult<NoContentResult>> DeleteSupplier(string id)
        {
            try
            {
                Supplier s = _supplierservice.findById(id);
                _supplierservice.Delete(id);
            } catch (UnknownSupplierException sup) {
                return NotFound(sup.Message);
            } 

            return NoContent();
        }

        private Supplier CreateItemLinksForSupplier(Supplier supplier)
        {
            var idObj = new { id = supplier.id };
            supplier.links.Add(
                new LinkDto(this._urlHelper.Link(nameof(this.GetSupplier), null),
                Constants.LIST,
                Constants.GET));

            supplier.links.Add(
                new LinkDto(this._urlHelper.Link(nameof(this.GetSupplier), idObj),
                Constants.SELF,
                Constants.GET));

            return supplier;
        }

        private Supplier CreateSingleLinksForSupplier(Supplier supplier)
        {
            var idObj = new { id = supplier.id };
            supplier.links.Add(
               new LinkDto(this._urlHelper.Link(nameof(this.GetSupplier), idObj),
               Constants.SELF,
               Constants.GET));

            supplier.links.Add(
               new LinkDto(this._urlHelper.Link(nameof(this.GetSupplier), null),
               Constants.LIST,
               Constants.GET));


            supplier.links.Add(
                new LinkDto(this._urlHelper.Link(nameof(this.PutSupplier), idObj),
                Constants.UPDATE,
                Constants.PUT));

            supplier.links.Add(
                new LinkDto(this._urlHelper.Link(nameof(this.PostSupplier), idObj),
                Constants.ADD,
                Constants.POST));

            supplier.links.Add(
                new LinkDto(this._urlHelper.Link(nameof(this.DeleteSupplier), idObj),
                Constants.REMOVE,
                Constants.DELETE));

            return supplier;
        }

        // Aktualisiere Etag des Clients, um ggfs Änderungen an den Daten zu erkennen
        private Supplier ETagHelper(Supplier supplier)
        {
            this.HttpContext.Response.Headers["If-None-Match"] = "{";
            this.HttpContext.Response.Headers["If-None-Match"] += $"\"\\\"{supplier.version}\\\"\"";
            this.HttpContext.Response.Headers["If-None-Match"] += "}";
            return supplier;
        }

        // Aktualisiere Version in der Datenbank
        private Supplier ETagBuilder(Supplier supplier)
        {
            // Nach erfolgreichen Update Etag um 1 erhöhen, modifiedSince updaten
            Supplier result = supplier.Clone(supplier.version + 1, DateTime.Now);
            return result;
        }

        // handelt den Fall << Statuscode 304 oder 200 >> ab
        private object SupplierToOk(string requestedEtag, Supplier supplier)
        {
            // Wenn im Header diese Parameter nicht vorhanden sind, dann Ok 200 zurückgeben
            if(!String.IsNullOrEmpty(requestedEtag))
            {
                // Primitive Implementierung, Verbesserung willkommen!!!
                if (requestedEtag.Replace("\"", "").Replace("\\", "") == supplier.version.ToString())
                {
                    return StatusCode(304, Constants.NOT_MODIFIED);
                }
            }
            return Ok(CreateSingleLinksForSupplier(ETagHelper(supplier)));
        } 
    }
}
