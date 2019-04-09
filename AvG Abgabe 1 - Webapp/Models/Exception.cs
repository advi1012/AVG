using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvG_Abgabe_1___Webapp.Model
{
    // Eigene Exception sind hier definiert

        /// <summary>
        /// Wird geworfen, wenn Supplier nicht bekannt ist
        /// </summary>
    public class UnknownSupplierException : Exception
    {
        public UnknownSupplierException() : base() { }
        public UnknownSupplierException(string message) : base(message) { }
        public UnknownSupplierException(string message, System.Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// Wird geworfen, wenn Product nicht bekannt ist
    /// </summary>
    public class UnknownProductException : Exception
    {
        public UnknownProductException() : base() { }
        public UnknownProductException(string message) : base(message) { }
        public UnknownProductException(string message, System.Exception inner) : base(message, inner) { }
    }
}
