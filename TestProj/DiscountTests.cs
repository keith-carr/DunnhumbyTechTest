using DunnhumbyTechTest;
using DunnhumbyTechTest.Models;
using DunnhumbyTechTest.Services.Implementations;

namespace TestsProj;

public class DiscountTests
{
    IOrderDiscountService service = new OrderDiscountService();

    [Fact]
    public void SingleBook_NoDiscount()
    {
        var products = new List<Product>() 
        {
            new Book(setId: 1, bookId: 1, title: "Book1", price: 5.00m)
        };

        var result = service.ApplyDiscounts(products);

        Assert.True(!result.Products.Any(p => p.IsDiscounted), "No books should be discounted");
        Assert.True(result.TotalDiscount == 0, "No discount should be applied");
    }

    [Fact]
    public void TwoBooks_TwoInSeries_Discount5PercOnBooks()
    {
        var products = new List<Product>() 
        {
            new Book(setId: 1, bookId: 1, title: "Book1", price: 5.00m),
            new Book(setId: 1, bookId: 2, title: "Book2", price: 5.00m)
        };

        var result = service.ApplyDiscounts(products);
        
        Assert.True(result.Products.Where(b => b.IsDiscounted).Count() == 2, "Both books should be discounted");
        Assert.True(result.TotalCostDiscounted == 9.5m, "Discount should be 5% of 10.00 = 9.50");
    }

    [Fact]
    public void TwoBooksAndOneToy_TwoBooksInSeries_Discount5PercOnBooks_NoDiscountToy()
    {
        var products = new List<Product>() 
        {
            new Book(setId: 1, bookId: 1, title: "Book1", price: 5.00m),
            new Book(setId: 1, bookId: 2, title: "Book2", price: 5.00m),
            new Toy(productId: 1, name: "Figure", price: 10m)
        };

        var result = service.ApplyDiscounts(products);
        
        Assert.True(result.Products.Where(p => !p.IsDiscounted && p is Toy).Count() == 1, "Toy should not be discounted");
        Assert.True(result.Products.Where(p => p.IsDiscounted && p is Book).Count() == 2, "Both books should be discounted");
        Assert.True(result.TotalCostDiscounted == 19.5m, "Discount should be 5% of 10.00 + 10.00 = 19.50");
    }

    [Fact]
    public void TwoBooksAndTwoToys_TwoBooksInSeries_OneToyDiscounted_Discount5PercOnBooks_DiscountOnOneToy()
    {
        var products = new List<Product>() 
        {
            new Book(setId: 1, bookId: 1, title: "Book1", price: 5.00m),
            new Book(setId: 1, bookId: 2, title: "Book2", price: 5.00m),
            new Toy(productId: 1, name: "Figure", price: 10m, discount: 0.2m),
            new Toy(productId: 2, name: "Figure", price: 2.5m),
        };

        var result = service.ApplyDiscounts(products);
        
        Assert.True(result.Products.Where(p => !p.IsDiscounted && p is Toy).Count() == 1, "One toy should not be discounted");
        Assert.True(result.Products.Where(p => p.IsDiscounted && p is Toy).Count() == 1, "One toy should be discounted");
        Assert.True(result.Products.Where(p => p.IsDiscounted && p is Book).Count() == 2, "Both books should be discounted");
        Assert.True(result.TotalCostDiscounted == 20m, "Discount should be 5% of 10.00 + 20% of 10.00 + 2.50 = 20.00");
    }

    [Fact]
    public void TwoBooks_TwoInSeriesOneDuplicate_Discount5PercOnBooks()
    {
        var products = new List<Product>() 
        {
            new Book(setId: 1, bookId: 1, title: "Book1", price: 5.00m),
            new Book(setId: 1, bookId: 2, title: "Book2", price: 5.00m),
            new Book(setId: 1, bookId: 2, title: "Book2", price: 5.00m),
        };

        var result = service.ApplyDiscounts(products);
        
        Assert.True(result.Products.Where(b => !b.IsDiscounted).Count() == 1, "Duplicate book should not be discounted");
        Assert.True(result.Products.Where(b => b.IsDiscounted).Count() == 2, "Two books should be discounted");
        Assert.True(result.TotalCostDiscounted == 14.5m, "Discount should be 5% of 10.00 + 5.00 = 14.50");
    }

    [Fact]
    public void ThreeBooks_ThreeInSeries_Discount10PercOnBooks()
    {
        var products = new List<Product>() 
        {
            new Book(setId: 1, bookId: 1, title: "Book1", price: 5.00m),
            new Book(setId: 1, bookId: 2, title: "Book2", price: 5.00m),
            new Book(setId: 1, bookId: 3, title: "Book3", price: 5.00m)
        };

        var result = service.ApplyDiscounts(products);
        
        Assert.True(result.Products.Where(b => b.IsDiscounted).Count() == 3, "All books should be discounted");
        Assert.True(result.TotalCostDiscounted == 13.5m, "Discount should be 10% of 15.00 = 13.50");
    }

    [Fact]
    public void FourBooks_FourInSeries_Discount20PercOnBooks()
    {
        var products = new List<Product>() 
        {
            new Book(setId: 1, bookId: 1, title: "Book1", price: 5.00m),
            new Book(setId: 1, bookId: 2, title: "Book2", price: 5.00m),
            new Book(setId: 1, bookId: 3, title: "Book3", price: 5.00m),
            new Book(setId: 1, bookId: 4, title: "Book4", price: 5.00m)
        };

        var result = service.ApplyDiscounts(products);
        
        Assert.True(result.Products.Where(b => b.IsDiscounted).Count() == 4, "All books should be discounted");
        Assert.True(result.TotalCostDiscounted == 16m, "Discount should be 20% of 20.00 = 16.00");
    }

    [Fact]
    public void FiveBooks_FiveInSeries_Discount25PercOnBooks()
    {
        var products = new List<Product>() 
        {
            new Book(setId: 1, bookId: 1, title: "Book1", price: 5.00m),
            new Book(setId: 1, bookId: 2, title: "Book2", price: 5.00m),
            new Book(setId: 1, bookId: 3, title: "Book3", price: 5.00m),
            new Book(setId: 1, bookId: 4, title: "Book4", price: 5.00m),
            new Book(setId: 1, bookId: 5, title: "Book5", price: 5.00m)
        };

        var result = service.ApplyDiscounts(products);
        
        Assert.True(result.Products.Where(b => b.IsDiscounted).Count() == 5, "All books should be discounted");
        Assert.True(result.TotalCostDiscounted == 18.75m, "Discount should be 25% of 25.00 = 18.75");
    }

    [Fact]
    public void FiveBooks_3InSeries_2Duplicates_Discount10PercOnBooks_NoDiscountOnTwoBooks()
    {
        var products = new List<Product>() 
        {
            new Book(setId: 1, bookId: 1, title: "Book1", price: 5.00m),
            new Book(setId: 1, bookId: 2, title: "Book2", price: 5.00m),
            new Book(setId: 1, bookId: 3, title: "Book3", price: 5.00m),
            new Book(setId: 1, bookId: 2, title: "Book2", price: 5.00m),
            new Book(setId: 1, bookId: 3, title: "Book3", price: 5.00m)
        };

        var result = service.ApplyDiscounts(products);
        
        Assert.True(result.Products.Where(b => !b.IsDiscounted).Count() == 2, "2 duplicate books should not be discounted");
        Assert.True(result.Products.Where(b => b.IsDiscounted).Count() == 3, "All books should be discounted");
        Assert.True(result.TotalCostDiscounted == 23.5m, "Discount should be 10% of 15.00 + 10.00 = 23.5");
    }

    [Fact]
    public void SixBooks_SixInSeries_Discount25PercOnBooks()
    {
        var products = new List<Product>() 
        {
            new Book(setId: 1, bookId: 1, title: "Book1", price: 5.00m),
            new Book(setId: 1, bookId: 2, title: "Book2", price: 5.00m),
            new Book(setId: 1, bookId: 3, title: "Book3", price: 5.00m),
            new Book(setId: 1, bookId: 4, title: "Book4", price: 5.00m),
            new Book(setId: 1, bookId: 5, title: "Book5", price: 5.00m),
            new Book(setId: 1, bookId: 6, title: "Book6", price: 5.00m)
        };

        var result = service.ApplyDiscounts(products);
        
        Assert.True(result.Products.Where(b => b.IsDiscounted).Count() == 6, "All books should be discounted");
        Assert.True(result.TotalCostDiscounted == 22.5m, "Discount should be 25% of 30.00 = 22.50");
    }

    [Fact]
    public void SevenBooks_ThreeInSeriesOne_ThreeInSeriesTwoWithOneDuplicate_Discount10PercOnSeries1Books_Discount20PercOnSeries2Books_NoDiscountOnOneBook()
    {
        var products = new List<Product>() 
        {
            new Book(setId: 1, bookId: 1, title: "Book1 S1", price: 5.00m),
            new Book(setId: 1, bookId: 2, title: "Book2 S1", price: 5.00m),
            new Book(setId: 1, bookId: 3, title: "Book3 S1", price: 5.00m),
            new Book(setId: 2, bookId: 1, title: "Book1 S2", price: 5.00m),
            new Book(setId: 2, bookId: 2, title: "Book2 S2", price: 5.00m),
            new Book(setId: 2, bookId: 3, title: "Book3 S2", price: 5.00m),
            new Book(setId: 2, bookId: 4, title: "Book4 S3", price: 5.00m)
        };

        var result = service.ApplyDiscounts(products);
        
        Assert.True(result.Products.Where(b => b.IsDiscounted).Count() == 7, "All books should be discounted");
        Assert.True(result.TotalCostDiscounted == 29.5m, "Discount should be 10% of 15.00 + 20% of 20.00 = 29.50");
    }

    [Fact]
    public void TenBooks_FourInSeriesOneWithOneDuplicate_FourInSeriesTwoWithTwoDuplicate_Discount10PercOnSeries1Books_Discount20PercOnSeries2Books_ThreeBooksNoDiscount()
    {
        var products = new List<Product>() 
        {
            new Book(setId: 1, bookId: 1, title: "Book1 S1", price: 5.00m),
            new Book(setId: 1, bookId: 2, title: "Book2 S1", price: 5.00m),
            new Book(setId: 1, bookId: 2, title: "Book2 S1", price: 5.00m),
            new Book(setId: 1, bookId: 3, title: "Book3 S1", price: 5.00m),
            new Book(setId: 2, bookId: 1, title: "Book1 S2", price: 5.00m),
            new Book(setId: 2, bookId: 2, title: "Book2 S2", price: 5.00m),
            new Book(setId: 2, bookId: 3, title: "Book3 S2", price: 5.00m),
            new Book(setId: 2, bookId: 3, title: "Book3 S2", price: 5.00m),
            new Book(setId: 2, bookId: 4, title: "Book4 S3", price: 5.00m),
            new Book(setId: 2, bookId: 4, title: "Book4 S3", price: 5.00m)
        };

        var result = service.ApplyDiscounts(products);
        
        Assert.True(result.Products.Where(b => !b.IsDiscounted).Count() == 3, "Three duplicate books should not be discounted");
        Assert.True(result.Products.Where(b => b.IsDiscounted).Count() == 7, "All books should be discounted");
        Assert.True(result.TotalCostDiscounted == 44.5m, "Discount should be 10% of 15.00 + 20% of 20.00 + 15.00 = 44.50");
    }
}