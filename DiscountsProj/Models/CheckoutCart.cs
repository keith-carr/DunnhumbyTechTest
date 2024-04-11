namespace DunnhumbyTechTest.Models;

// Checkout cart contains all products to be purchased with discounts applied
public class CheckoutCart
{
    public List<Product> Products {get; set;} = new ();
    public decimal TotalCost {get; set;}
    public decimal TotalCostDiscounted {get; set;}
    public decimal TotalDiscount => TotalCost - TotalCostDiscounted;
}
