using DunnhumbyTechTest.Models;

namespace DunnhumbyTechTest;
public interface IOrderDiscountService
{
     CheckoutCart ApplyDiscounts(List<Product> basket);
}
