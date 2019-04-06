using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AvG_Abgabe_1___Webapp.Model
{
    public class Product
    {
        private string _id;
        private Supplier _prefferedsupplier;
        private string _name;
        private double _price;
        private Color _color;
        private string _description;
        private int _current_stock;

        public string id { get { return this._id;  } private set { this._id = value;  } }

        public Supplier prefferedSupplier { get { return this._prefferedsupplier; } private set { this._prefferedsupplier = value;  } }

        [JsonConverter(typeof(StringEnumConverter))]
        public Color color { get { return this._color;  } private set { this._color = value; } }

        public double price { get { return this._price;  } private set { this._price = value; } }

        public string name { get { return this._name;  } private set { this._name = value;  } }

        public string description { get { return this._description; } private set { this._description = value; } }

        public int currentStock { get { return this._current_stock; } private set { this._current_stock = value; } }

        public Product(string id, Supplier prefferedSupplier, Color color,
            double price, string name, string description, int currentStock)
        {
            _id = id;
            _name = name;
            _description = description;
            _prefferedsupplier = prefferedSupplier;
            _current_stock = currentStock;
            _color = color;
            _price = price;
        }

        public override string ToString()
        {
            string result = $"Product[ id = {this.id}, name = {this.name}, description = {this.description}, " +
                $"preffered_Supplier = Supplier[ name = {this.prefferedSupplier.name}, id = {this.prefferedSupplier.id} ], " +
                $"current_Stock = {this._current_stock}, color = {this.color},  price = {this.color}]";
            return result;
        }
    }
}
