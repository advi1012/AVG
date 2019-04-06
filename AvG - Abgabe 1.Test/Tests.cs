using System;
using NUnit.Framework;
using Entity;

namespace AvG___Abgabe_1.Test
{
    [TestFixture]
    public class Supplier_Construct
    {
        private readonly Supplier _supplier;

        public Supplier_Construct()
        {
            _supplier = 
                new Supplier("1", 
                    "Test AG", 
                    "test@test.de", 
                    "015639812831", 
                    "TestStrasse 3");
        }
        [Test]
        public void ConstructObject()
        {
            var result = _supplier;
            Assert.True(result is Supplier);
        }
    }
    [TestFixture]
    public class Product_Construct
    {
        private readonly Product _product;

        public Product_Construct()
        {
            _product = new Product();
        }

        [Test]
        public void ConstructObject()
        {
            var result = _product;
            Assert.True(result is Product);
        }
    }
}