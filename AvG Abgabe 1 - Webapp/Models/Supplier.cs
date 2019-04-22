using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using AvG_Abgabe_1___Webapp.Controllers;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using SupplierServiceGRPC;

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
        private DateTime _created;
        private DateTime _modifiedSince;
        private int _version;

        [Key]
        [RegularExpression(Constants.ID_REGEX)]
        [JsonIgnore]
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

        [NotMapped]
        public List<LinkDto> links { get { return this._links; } private set { this._links = value; } }

        [NotMapped]
        [ConcurrencyCheck]
        [JsonIgnore]
        public int version { get { return this._version; } private set { this._version = value; } }

        [NotMapped]
        [Timestamp]
        [JsonIgnore]
        public DateTime created { get { return this._created; } set { this._created = value; } }

        [NotMapped]
        [Timestamp]
        [JsonIgnore]
        public DateTime modifiedSince { get { return this._modifiedSince; } set { this._modifiedSince = value; } }

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

        // Klonen für fachliche Updates
        public Supplier Clone(string id = null, string name = null, string email = null, string phone = null, string address = null)
        {
            Supplier Klon = (Supplier) this.MemberwiseClone();

            if(id != null)
            {
                Klon._id = id;
            }

            if (name != null)
            {
                Klon._name = name;
            }

            if (email != null)
            {
                Klon._email = email;
            }

            if (phone != null)
            {
                Klon._phone = phone;
            }

            if (address != null)
            {
                Klon._address = address;
            }
            return Klon;
        }

        // Klonen für technische Updates
        public Supplier Clone(int version, DateTime created, DateTime modifiedSince)
        {
            Supplier Klon = (Supplier)this.MemberwiseClone();
            Klon._version = version;
            Klon._created = created;
            Klon._modifiedSince = modifiedSince;
            
            return Klon;
        }

        // Klonen für technische Updates
        public Supplier Clone(int version, DateTime modifiedSince)
        {
            Supplier Klon = (Supplier)this.MemberwiseClone();
            Klon._version = version;
            Klon._modifiedSince = modifiedSince;

            return Klon;
        }

        // Für gRPC-generierten Code
        public PreferredSupplier ToPrefferedSupplier()
        {
            PreferredSupplier result = new PreferredSupplier();
            result.Id = this.id;
            result.Name = this.name; 
            result.Email = this.email;
            result.Phone = this.phone;
            result.Address = this.address;

            return result;
    }
    }
}
