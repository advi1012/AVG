﻿using System;
using System.Collections.Generic;
using System.Linq;
using AvG_Abgabe_1___Webapp.Model;

namespace AvG_Abgabe_1___Webapp.Service
{
    /// <summary>
    /// Realisierung von ISupplierService, eigentliche Serviceklasse
    /// </summary>
    public class SupplierService : ISupplierService
    {
        private readonly SupplierContext _supplierContext;

        // Constructor injection
        public SupplierService(SupplierContext supplierContext)
        {
            _supplierContext = supplierContext;
        }

        /// <summary>
        /// implementiert die Anforderung LIST(Suppliers) findAllPreferredSuppliers()
        /// </summary>
        /// <returns> eine Liste von Supplier, die mindestens einen Eintrag in preferredSupplier haben </returns>
        public List<Supplier> findAllPreferredSuppliers()
        {
            List<Supplier> result = new List<Supplier>();
            var allProducts = _supplierContext.Product;
            foreach(Product p in allProducts)
            {
                Supplier s = _supplierContext.Find<Supplier>(p.preferredSupplier);
                if (!result.Contains(s))
                {
                    result.Add(s);
                }
            }
            return result;
        }

        /// <summary>
        /// implementiert die Anforderung Supplier findPreferredSupplier(Product p)
        /// </summary>
        /// <param name="p"> Das Produkt, welches als Suchkriterium benutzt wird </param>
        /// <returns> Den Supplier, der von jenem Produkt bevorzugt wird </returns>
        public Supplier findPreferredSupplier(Product p)
        {

            var isProductThere = _supplierContext.FindAsync<Product>(p.id);
            if (isProductThere == null)
            {
                throw new UnknownProductException(Constants.UnknownProductMessage);
            }
            // Erinnerung: "preferredSupplier" ist Fremdschlüssel und bezieht sich auf "id" der Tabelle "Supplier"
            var result = _supplierContext.Supplier.Find(p.preferredSupplier);
            if (result == null)
            {
                throw new UnknownSupplierException(Constants.UnknownSupplierMessage);
            }
            return result;
            
        }

        /// <summary>
        /// implementiert die Anforderung void setPreferredSupplierForProduct(Supplier s, Product c)
        /// aus technischen Gründen wurde ein weitere Parameter hinzugefügt (für sinnvolles Routing)
        /// </summary>
        /// <param name="s"> Der Supplier, dessen ID in preferredSupplier eingetragen werden soll </param>
        /// <param name="c"> Das Produkt, das aktualisert werden soll </param>
        /// <param name="productId"> Die ID des zu aktualisierenden Produkts </param>
        public void setPreferredSupplierForProduct(Supplier s, Product c, string productId)
        {
            var isSupplierThere = _supplierContext.Find<Supplier>(s.id);
            if (isSupplierThere == null)
            {
                throw new UnknownSupplierException(Constants.UnknownSupplierMessage);
            }
            var isProductThere = _supplierContext.Find<Product>(productId);
            if (isProductThere == null)
            {
                throw new UnknownProductException(Constants.UnknownProductMessage);
            }

            // TODO: update benutzen
            // Work-around
            // Update wirft Exception...
            _supplierContext.Remove<Product>(isProductThere);
            isProductThere = isProductThere.Clone(isSupplierThere.id);
            // _supplierContext.Update<Product>(isProductThere);
            _supplierContext.Add<Product>(isProductThere);
            _supplierContext.SaveChanges();

        }
        //Hilfsfunktionen
        public Product findProductById(string Id)
        {
            var result = _supplierContext.Find<Product>(Id);
            if (result == null)
            {
                throw new UnknownProductException(Constants.UnknownSupplierMessage);
            }
            return result;
        }

        public Supplier findById(string Id)
        {
            var result = _supplierContext.Find<Supplier>(Id);
            if (result == null)
            {
                throw new UnknownSupplierException(Constants.UnknownSupplierMessage);
            }
            return result;
        }

        public Supplier Create(Supplier supplier)
        {
            // zufällige UUID hinzufügen
            Supplier result = supplier.Clone(id: Guid.NewGuid().ToString());
            _supplierContext.Add<Supplier>(result);
            _supplierContext.SaveChanges();
            return result;
        }

        public void Delete(string id)
        {
            Supplier isThere = _supplierContext.Find<Supplier>(id);
            if (isThere == null)
            {
                throw new UnknownSupplierException(Constants.UnknownSupplierMessage);
            }

            _supplierContext.Remove<Supplier>(isThere);
            _supplierContext.SaveChanges();
        }
    }
}
