namespace Common.Model
{
    public class UserAddress
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        public string PrimaryAddress { get; set; }

        public string SecondaryAddress { get; set; }

        public string City { get; set; }

        public string StateProvince { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }
    }
}
