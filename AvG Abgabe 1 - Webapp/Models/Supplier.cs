using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using AvG_Abgabe_1___Webapp.Controllers;
using Newtonsoft.Json;

namespace AvG_Abgabe_1___Webapp.Model
{

    /// <summary>
    /// Entity-Class Supplier. Data class that describes a supplier for products.
    /// </summary>
    public class Supplier
    {
        private string _id;
        private string _name;
        private string _email;
        private string _phone;
        private string _address;
        // technische Daten
        private List<LinkDto> _links = new List<LinkDto>();
        private int _version;

        [Required]
        [RegularExpression(Constants.ID_REGEX)]
        public string id { get { return this._id; } private set { this._id = value; } }

        [Required]
        [StringLength(maximumLength: 40, MinimumLength = 4)]
        [RegularExpression(Constants.NAME_REGEX)]
        public string name { get { return this._name; } private set { this._name = value; } }


        [EmailAddress]
        [Required]
        public string email { get { return this._email; } private set { this._email = value; } }

        [Phone]
        public string phone { get { return this._phone;  } private set { this._phone = value; } }

        [StringLength(maximumLength: 40, MinimumLength = 4)]
        public string address { get { return this._address;  } private set { this._address = value; } }

        // technische Getter/ Setter

        public List<LinkDto> links { get { return this._links; } private set { this._links = value; } }

        [ConcurrencyCheck]
        [JsonIgnore]
        public int version { get { return this._version; } private set { this._version = value; } }

        public Supplier(string id, string name, string email, string phone, string address)
        {
            _id = id;
            _name = name;
            _email = email;
            _phone = phone;
            _address = address;
        }

        public override string ToString()
        {
            string result = $"Supplier[ id = {this.id}, name = {this.name}, phone = {this.phone}, address = {this.address}, " +
                $"mail = {this.email} ]";  
            return result;
        }
    }
}
