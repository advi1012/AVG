﻿using System;
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
    }

    // Mocking Service Klasse
    // Implementierung fehlt noch !!
    public class SupplierServiceMock : ISupplierService
    {
        public SupplierServiceMock()
        {


        }

        public Product findProductById(string id)
        {
            return new Product(id, "00000000-0000-0000-0000-000000000001", Color.green, 12.0, "Mocking", "", 1);
        }

        public Supplier findById(string id)
        {
            return new Supplier(id, "Beta", "MusterMail", "+49 89 123456 789", "Muster-Adresse");
        }
        
        public async Task<List<Supplier>> findAllPreferredSuppliers()
        {
            List<Supplier> resultMock = new List<Supplier>();
            resultMock.Add(new Supplier(System.Guid.NewGuid().ToString(),"Muster", "MusterMail", " 07231 4252567", "Muster-Adresse"));
            resultMock.Add(new Supplier(System.Guid.NewGuid().ToString(), "Beta", "MusterMail", " 07231 4252567", "Muster-Adresse"));
            resultMock.Add(new Supplier(System.Guid.NewGuid().ToString(), "Gamma", "MusterMail", " 07231 4252567", "Muster-Adresse"));
            return resultMock;
        }

        public async Task<Supplier> findPreferredSupplier(Product p)
        {
            var result = new Supplier(System.Guid.NewGuid().ToString(), "ABC", "MusterMail", " 07231 4252567", "Muster-Adresse");
            return result;
        }

        public async void setPreferredSupplierForProduct(Supplier s, Product c)
        {

        }

    }
}