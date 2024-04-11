namespace DunnhumbyTechTest.Models
{
    public class Book : Product
    {
        public Book(int setId, int bookId, string title, decimal price) 
            : base(bookId, title, price)
        {
            SetId = setId;
        }

        public int SetId {get; set;}
    }
}