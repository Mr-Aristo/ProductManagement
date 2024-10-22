namespace ProductAPI.DTOs
{
    public class ProductDTO
    {
        public  Guid Id { get; set; }   
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Guid CategoryID { get; set; }      
    }
}
