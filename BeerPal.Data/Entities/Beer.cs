namespace BeerPal.Data.Entities
{
    public class Beer : BaseEntity
    {
        public string Name { get; set; }
        public int Price { get; set; }
    }
}