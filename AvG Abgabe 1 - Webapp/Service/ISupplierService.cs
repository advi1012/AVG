using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvG_Abgabe_1___Webapp.Model;

namespace AvG_Abgabe_1___Webapp.Service
{
    public interface ISupplierService
    {
        List<Supplier> findAllPreferredSuppliers();
        Supplier findPreferredSupplier(Product p);
        void setPreferredSupplierForProduct(Supplier s, Product c, string productId);
        //Hilfsfunktionen
        Product findProductById(string Id);
        Supplier findById(string Id);
        Supplier Create(Supplier supplier);
        void Delete(string id);
    }
}
