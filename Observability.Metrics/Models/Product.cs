namespace Observability.Metrics.Models;

public class Product
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public ProductType Type { get; set; }

    public Product(int id, string name, ProductType type)
    {
        Id = id;
        Name = name;
        Type = type;
    }
}

public enum ProductType
{
    Pizza = 0,
    Desert = 1,
    Beverage = 2
}