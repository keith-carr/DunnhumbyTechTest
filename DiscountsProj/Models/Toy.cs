using DunnhumbyTechTest.Models;

namespace DunnhumbyTechTest;
public class Toy : Product
{
    public Toy(int productId, string name, decimal price, decimal discount = 0) 
        : base(productId, name, price, discount) {}
}
