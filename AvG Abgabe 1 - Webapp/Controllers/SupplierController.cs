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

        // GET: api/Supplier
        // Routing für LIST(Suppliers) findAllPreferredSuppliers()
        // Routing für Supplier findPreferredSupplier(Product p) über Queryparameter << ?id=... >>
        [HttpGet(Name = "findAllPreferredSuppliers")]
        public async Task<ActionResult<IEnumerable<Supplier>>> Get()
        {
            // hier ist die Id vom Produkt gemeint
            // Verbesserungsvorschläge sehr wünschenswert
            var queryParam = Request.Query["id"].ToString();
            if(String.IsNullOrEmpty(queryParam))
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

        // UPDATE: api/Supplier/{id}
        // Routing für void setPreferredSupplierForProduct(Supplier s, Product c)
        [HttpPut("{id}")]

        public async Task<ActionResult<NoContentResult>> Put(string id, [FromBody] Product c)
        {
            Supplier s = _supplierservice.findById(id);

            if(c == null)
            {
                return BadRequest();
            } 
            _supplierservice.setPreferredSupplierForProduct(s, c);
            // dann noch in DB speichern/ commit falls vorhander
            // ...
            // BadRequest() einbauen, falls das Product c fehlerhaft ist

            return NoContent();
        }
    }
}
