using DunnhumbyTechTest.Models;

namespace DunnhumbyTechTest.Services.Implementations;

// Given a basket of products, calculates total cost including discounts
public class OrderDiscountService : IOrderDiscountService
{
    // Check product types in basket and apply discounting logic
    public CheckoutCart ApplyDiscounts(List<Product> basket)
    {
        // Get products types (e.g. book, toys, etc)
        var productsByTypes = basket.GroupBy(p => p.GetType().Name).ToList();

        // Apply discount logic based on product type
        foreach(var productsByType in productsByTypes)
        {
            switch (productsByType.Key)
            {
                case "Book":
                    ApplyDiscount(new List<Book>(productsByType.Cast<Book>()));
                break;
            }
        }

        return GetCheckoutCart(basket);
    }

    // Applies discount logic if products are books
    private void ApplyDiscount(List<Book> books)
    {
        var discountTable = new Dictionary<int, decimal>()
        {
            {2, 0.05m},
            {3, 0.1m},
            {4, 0.2m},
            {5, 0.25m},
        };

        // Apply discount if it belongs to the same set of books
        var sets = books.GroupBy(b => b.SetId);
        foreach(var set in sets)
        {
            // Get discount percentage based on number of unique books being purchased
            var distinctBookIds = set.DistinctBy(b => b.ProductId);

            discountTable.TryGetValue(
                distinctBookIds.Count() > discountTable.Keys.Max() ? discountTable.Keys.Max() : distinctBookIds.Count(), // Cap max discount
                out decimal discountPercentage);
            
            // Apply discount to the first instance of each unique book
            // Book duplicates will not be discounted
            var firstOfEachDistinctBookId = set.GroupBy(b => b.ProductId, 
                (key, g) => g.OrderBy(e => e.ProductId).First())
                .ToList();
                
            foreach(var book in firstOfEachDistinctBookId)
            {
                book.DiscountBy(discountPercentage);
            }
        }
    }

    // Calculates cost and discounted cost totals
    private CheckoutCart GetCheckoutCart(List<Product> products)
    {
        var TotalCost = products.Sum(p => p.Price);
        var TotalCostDiscounted = products.Sum(p => p.IsDiscounted ? p.DiscountedPrice : p.Price);
        return new () 
        { 
            TotalCost = TotalCost, 
            TotalCostDiscounted = TotalCostDiscounted, 
            Products = products 
        };
    }
}
