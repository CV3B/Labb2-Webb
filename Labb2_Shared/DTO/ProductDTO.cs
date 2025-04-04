using Labb2_Shared.Model;

namespace Labb2_Shared.DTO;

public class ProductDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public Guid CategoryId { get; set; }
    public Status Status {get; set;}
    public string ImageLink { get; set; }
}