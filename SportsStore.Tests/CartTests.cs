using SportsStore.Models;
using System.Linq;
using Xunit;

namespace SportsStore.Tests
{
    public class CartTests
    {
        public Product GetProduct(int productId = 0, string category = null, decimal price = 0) => new Product()
        {
            ProductID = productId,
            Category = category,
            Price = price
        };

        [Fact]
        public void Can_Add_New_Lines()
        {
            //Arrange
            Product p1 = GetProduct(productId: 1, category: "A");
            Product p2 = GetProduct(productId: 2, category: "B");

            //Act
            Cart target = new Cart { };
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);

            int result1 = target.Lines.Where(cartLine
                => cartLine.Product.ProductID == p1.ProductID).Count();
            int result2 = target.Lines.Count();

            //Assert
            Assert.Equal(1, result1);
            Assert.Equal(2, result2);
        }

        [Fact]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            //Arrange
            Product p1 = GetProduct(productId: 1, category: "A");

            //Act
            Cart target = new Cart { };
            target.AddItem(p1, 1);
            target.AddItem(p1, 2);

            decimal result = target.Lines.ToArray()[0].Quantity;

            //Assert
            Assert.Equal(3, result);
        }

        [Fact]
        public void Can_Remove_Line()
        {
            //Arrange
            Product p1 = GetProduct(productId: 1, category: "A");

            //Act
            Cart target = new Cart { };
            target.AddItem(p1, 1);
            target.AddItem(p1, 3);
            target.RemoveLine(p1);

            int result = target.Lines.Count();

            //Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void Calculate_Cart_Total()
        {
            //Arrange
            Product p1 = GetProduct(productId: 1, price: 200);
            Product p2 = GetProduct(productId: 2, price: 400);

            //Act
            Cart target = new Cart { };
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);

            decimal result = target.ComputeTotalValue();

            //Assert
            Assert.Equal(600, result);
        }

        [Fact]
        public void Can_Clear_Contents()
        {
            //Arrange
            Product p1 = GetProduct(productId: 1, price: 200);
            Product p2 = GetProduct(productId: 2, price: 400);

            //Act
            Cart target = new Cart { };
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.Clear();

            int result = target.Lines.Count();

            //Assert
            Assert.Equal(0, result);
        }
    }
}
