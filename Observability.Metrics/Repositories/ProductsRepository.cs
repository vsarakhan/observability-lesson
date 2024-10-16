using Observability.Metrics.Models;

namespace Observability.Metrics.Repositories;

public interface IProductsRepository
{
    Product GetProduct(int id);
}

public class ProductsRepository: IProductsRepository
{
    private readonly List<Product> _products =
    [
        new Product(1, "Margherita", ProductType.Pizza),
        new Product(2, "Pepperoni", ProductType.Pizza),
        new Product(3, "4 cheese", ProductType.Pizza),
        new Product(4, "Cheesecake", ProductType.Desert),
        new Product(5, "Chocolate fondant", ProductType.Desert),
        new Product(6, "Americano", ProductType.Beverage),
        new Product(7, "Still water", ProductType.Beverage)
    ];

    public Product GetProduct(int id)
    {
        return _products.First(p => p.Id == id);
    }
}