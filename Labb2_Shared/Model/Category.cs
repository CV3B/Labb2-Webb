namespace Labb2_Shared.Model;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    public override string ToString()
    {
        return Name;
    }
}