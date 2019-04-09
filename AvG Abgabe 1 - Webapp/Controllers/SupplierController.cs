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
    [Route("/Supplier")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierservice;

        // Constructor Injection
        public SupplierController(ISupplierService supplierservice)
        {
            _supplierservice = supplierservice;
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
        [HttpGet(Name = "findAllPreferredSuppliers")]
        public async Task<ActionResult<IEnumerable<Supplier>>> Get()
        {
            var queryParam = Request.Query["product_id"].ToString();
            // falls keine oder falsche Queryparameter angegeben werden: Gib alle zurück
            if (String.IsNullOrEmpty(queryParam))
            {
                var list = await _supplierservice.findAllPreferredSuppliers();
                if (list != null)
                {
                    if (list.Count() > 0)
                    {
                        return Ok(list);
                    }
                }
                return NotFound();
            }

            var product = _supplierservice.findProductById(queryParam);
            if (product != null)
            {
                var result = await _supplierservice.findPreferredSupplier(product);
                return Ok(result);
            }

            return NotFound();
        }

        /// <summary>
        /// implementiert die Httpmethode PUT: https://localhost:44337/Supplier/00000000-0000-0000-0000-000000000001
        /// </summary>
        /// <param name="id"> Id des PreferredSuppliers, um diesen in das Product einzutragen </param>
        /// <param name="c"> Im Body des PUT-Requests, zu aktualisierendes Product </param>
        /// <returns> Status Code 204 Created, andernfalls 400 BadRequest </returns>
        [HttpPut("{id}")]

        public async Task<ActionResult<NoContentResult>> Put(string id, [FromBody] Product c)
        {
            // Exceptionhandling: Im Fehlerfall soll der Client nur den Statuscode 400 BadRequest sehen.
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
                return BadRequest(sup.Message);
            } catch (UnknownProductException pro)
            {
                return BadRequest(pro.Message);
            } catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}
