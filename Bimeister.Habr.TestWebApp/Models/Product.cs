namespace Bimeister.Habr.TestWebApp.Models
{
    public class Product
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public int SortOrder { get; set; }

        public bool Enabled { get; set; }

        public DateTime ActivationDate { get; set; }
    }
}
