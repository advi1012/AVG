using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvG_Abgabe_1___Webapp.Model;

namespace AvG_Abgabe_1___Webapp.Service
{
    public interface ISupplierService
    {
        Task<List<Supplier>> findAllPreferredSuppliers();
        Task<Supplier> findPreferredSupplier(Product p);
        void setPreferredSupplierForProduct(Supplier s, Product c);
        //Hilfsfunktionen
        Product findProductById(string Id);
        Supplier findById(string Id);
        Supplier Create(Supplier supplier);
        void Delete(string id);
    }
}
