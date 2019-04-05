namespace Entity
{

    /// <summary>
    /// Entity-Class Supplier. Data class that describes a supplier for products.
    /// </summary>
    public class Supplier
    {
        private string id;
        private string name;
        private string email;
        private string phone;
        private string address;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        public Supplier(string id, string name, string email, string phone, string address)
        {
            Id = id;
            Name = name;
            Email = email;
            Phone = phone;
            Address = address;
        }

    }


    public class Product
    {
        private string id;
        private string prefferedsupplier;
        private string name;
        private enum color {  };
        private double price;
        private string description;
        private int current_stock;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public string PrefferedSupplier
        {
            get { return prefferedsupplier; }
            set { prefferedsupplier = value; }
        }
        /*
        public enum Color
        {
            get { return color; }
            set { color = value; }
        }

    */
        public double Price
        {
            get { return price; }
            set { price = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public int CurrentStock
        {
            get { return current_stock; }
            set { current_stock = value; }
        }


        public Product()
        {
            // Konstruktor implementieren

        }




    }
}