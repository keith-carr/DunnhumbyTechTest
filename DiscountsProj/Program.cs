using DunnhumbyTechTest.Models;
using DunnhumbyTechTest.Services;
using DunnhumbyTechTest.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace DunnhumbyTechTest;
class Program
{
    static void Main()
    {
        // Wire up dependency injection
        var serviceColleciton = new ServiceCollection();
        serviceColleciton.AddTransient<IOrderDiscountService, OrderDiscountService>();
        var serviceProvider = serviceColleciton.BuildServiceProvider();

        // Create instance of service
        var discountService = serviceProvider.GetRequiredService<IOrderDiscountService>();

        // Create example basket of items
        var basket = new List<Product>()
        {
            new Book(setId: 1, bookId: 1, title: "HarryP 1", price: 5),
            new Book(setId: 1, bookId: 2, title: "HarryP 2", price: 5),
            new Book(setId: 1, bookId: 3, title: "HarryP 3", price: 5),
            new Book(setId: 1, bookId: 3, title: "HarryP 3", price: 5), // Duplicate no discount applied

            new Book(setId: 2, bookId: 1, title: "Lord of the Rings 1", price: 5),
            new Book(setId: 2, bookId: 1, title: "Lord of the Rings 1", price: 5), // Duplicate no discount applied
            new Book(setId: 2, bookId: 2, title: "Lord of the Rings 2", price: 5),
            new Book(setId: 2, bookId: 3, title: "Lord of the Rings 3", price: 5),
            new Book(setId: 2, bookId: 3, title: "Lord of the Rings 3", price: 5), // Duplicate no discount applied
            new Book(setId: 2, bookId: 4, title: "Lord of the Rings 4", price: 5),
            new Book(setId: 2, bookId: 5, title: "Lord of the Rings 5", price: 5),
            new Book(setId: 2, bookId: 6, title: "Lord of the Rings 6", price: 5),

            // Not books, no discounting
            new Toy(productId: 5, name: "Car", price: 10.00m),
            new Toy(productId: 5, name: "Figure", price: 10.00m, 0.3m)
        };

        // Calculate discounts
        var cart = discountService.ApplyDiscounts(basket);

        // Write result to console indicating any discount
        if (cart.TotalDiscount > 0)
        {
            Console.WriteLine("Total cost before discount: {0:.00}", cart.TotalCost);
            Console.WriteLine("Total discount: {0:.00}", cart.TotalDiscount);
            Console.WriteLine("Total cost: {0:.00}", cart.TotalCostDiscounted);
        }
        else
        {
            Console.WriteLine("Total cost: {0:.00}", cart.TotalCost);
        }
        
        Console.Write("\n");

        Console.WriteLine($"{"ProductId", -20}{"Product name", -40}{"Price", -10}");
        foreach(var product in cart.Products)
        {
            Console.WriteLine("{0, -20}{1,-40}{2,-10:.00}", 
                product.ProductId,
                product.Name,
                product.IsDiscounted ? product.DiscountedPrice : product.Price);
        }
    }
}
