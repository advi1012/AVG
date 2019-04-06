namespace AvG_Abgabe_1.Model
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

        public string id { get { return this._id; } private set { this._id = value; } }

        public string name { get { return this._name; } private set { this._name = value; } }

        public string email { get { return this._email; } private set { this._email = value; } }

        public string phone { get { return this._phone;  } private set { this._phone = value; } }

        public string address { get { return this._address;  } private set { this._address = value; } }

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
