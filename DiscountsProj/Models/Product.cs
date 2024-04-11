namespace DunnhumbyTechTest.Models;

// Base product which all other products derive from
public abstract class Product
{
    public Product(int productId, string name, decimal price, decimal discountBy = 0)
    {
        ProductId = productId;
        Name = name;
        Price = price;
        
        if (discountBy > 0) 
        {
            DiscountBy(discountBy);
        }
    }

    public int ProductId {get; set;}
    public string Name {get; set;}
    public decimal Price {get; set;}
    public decimal DiscountedPrice => Price * Discount;
    public decimal Discount { get; set; } = 1;
    public bool IsDiscounted => Discount != 1;

    public void DiscountBy(decimal percentage)
    {
        Discount = 1m - percentage;
    }
}
