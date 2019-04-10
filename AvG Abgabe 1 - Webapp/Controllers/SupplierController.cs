using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AvG_Abgabe_1___Webapp.Model;
using AvG_Abgabe_1___Webapp.Service;
using System.Web;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AvG_Abgabe_1___Webapp.Controllers
{
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
        /// Bis jetzt mögliche Queryparameter << ?product_id=... >>
        /// Routing für Supplier findPreferredSupplier(Product p)
        /// Beispiel: https://localhost:44337/Supplier?product_id=100
        /// </summary>
        /// <returns> Statuscode 200 OK, im Fehelerfall Statuscode 404 Not Found </returns>
        [HttpGet(Name = nameof(GetSupplier))]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetSupplier()
        {
            var queryParam1 = Request.Query["product_id"].ToString();
            var queryParam2 = Request.Query["id"].ToString();

            // falls ?product_id eingegeben wurde: Suche nach Supplier über das Produkt mit dieser Id
            if (!String.IsNullOrEmpty(queryParam1) && Request.Query.Count() == 1) {
                var product = _supplierservice.findProductById(queryParam1);
                if (product != null)
                {
                    var result = await _supplierservice.findPreferredSupplier(product);
                    if (Request.Headers["ifNoneMatch"] == result.version)
                    {
                       return StatusCode(304);
                    } 
                    return Ok(CreateSingleLinksForSupplier(result));
                }

                return NotFound();
            }

            // falls ?id eingegeben wurde: Suche nach Supplier mit dieser Id
            if (!String.IsNullOrEmpty(queryParam2) && Request.Query.Count() == 1)
            {
                var supplier = _supplierservice.findById(queryParam1);
                if (supplier != null)
                {
                    return Ok(CreateSingleLinksForSupplier(supplier));
                }

                return NotFound();
            }

            // falls keine oder falsche Queryparameter angegeben werden: Gib alle zurück (nicht verlangt)
            var list = await _supplierservice.findAllPreferredSuppliers();
            if (list != null)
            {
                if (list.Count() > 0)
                {
                    return Ok(list.Select(supplier => CreateItemLinksForSupplier(supplier)));
                }
            }
            return NotFound();

        }

        /// <summary>
        /// implementiert die Httpmethode PUT: https://localhost:44337/Supplier/00000000-0000-0000-0000-000000000001
        /// </summary>
        /// <param name="id"> Id des PreferredSuppliers, um diesen in das Product einzutragen </param>
        /// <param name="c"> Im Body des PUT-Requests, zu aktualisierendes Product </param>
        /// <returns> Status Code 204 Created, andernfalls 400 BadRequest </returns>
        [HttpPut("{id}", Name = nameof(PutSupplier))]

        public async Task<ActionResult<NoContentResult>> PutSupplier(string id, [FromBody] Product c)
        {
            // Exceptionhandling: Im Fehlerfall soll der Client nur den Statuscode 400 BadRequest bzw  Not Found 404 sehen.
            try
            {
                Supplier s = _supplierservice.findById(id);
                if (c == null)
                {
                    return BadRequest();
                }
                _supplierservice.setPreferredSupplierForProduct(s, c);

                return NoContent();
            } catch (UnknownSupplierException sup)
            {
                return NotFound(sup.Message);
            } catch (UnknownProductException pro)
            {
                return NotFound(pro.Message);
            } catch (Exception e)
            {
                return BadRequest();
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
            return CreatedAtAction(nameof(PostSupplier), new { id = supplier.id }, supplier);
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
            supplier.itemLinks.Add(
                new LinkDto(this._urlHelper.Link(nameof(this.GetSupplier), null),
                Constants.HREF,
                Constants.GET));

            supplier.itemLinks.Add(
                new LinkDto(this._urlHelper.Link(nameof(this.GetSupplier), idObj),
                Constants.SELF,
                Constants.GET));

            return supplier;
        }

        private Supplier CreateSingleLinksForSupplier(Supplier supplier)
        {
            var idObj = new { id = supplier.id };
            supplier.singleLinks.Add(
               new LinkDto(this._urlHelper.Link(nameof(this.GetSupplier), idObj),
               Constants.SELF,
               Constants.GET));

            supplier.singleLinks.Add(
               new LinkDto(this._urlHelper.Link(nameof(this.GetSupplier), null),
               Constants.HREF,
               Constants.GET));


            supplier.singleLinks.Add(
                new LinkDto(this._urlHelper.Link(nameof(this.PutSupplier), idObj),
                Constants.UPDATE,
                Constants.PUT));

            supplier.singleLinks.Add(
                new LinkDto(this._urlHelper.Link(nameof(this.PostSupplier), idObj),
                Constants.ADD,
                Constants.POST));

            supplier.singleLinks.Add(
                new LinkDto(this._urlHelper.Link(nameof(this.DeleteSupplier), idObj),
                Constants.REMOVE,
                Constants.DELETE));

            return supplier;
        }
    }
}
