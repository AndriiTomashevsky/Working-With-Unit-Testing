using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using System;
using Xunit;

namespace SportsStore.Tests
{
    public class OrderControllerTests
    {
        //[Theory]
        //[ClassData(typeof(ProductTestData))]
        [Fact]
        public void _Can_Paginate()
        {
            //Arrange
            var mock = new Mock<IOrderRepository>();
            //var mockCart = new Mock<Cart>();

            //mockRepository.Setup(m => m.SaveOrder(It.IsAny<Order>()));
            //mockCart.SetupGet(m => m.Lines).Returns(new CartLine[] { });

            Order order = new Order();

            OrderController target = new OrderController(mock.Object, new Cart());

            //Act
            ViewResult result = target.Checkout(order) as ViewResult;

            //Assert
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
            Assert.True(String.IsNullOrEmpty(result.ViewName));
            Assert.False(target.ModelState.IsValid);
        }

        [Fact]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            //Arrange
            var mock = new Mock<IOrderRepository>();

            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);

            OrderController target = new OrderController(mock.Object, cart);
            target.ModelState.AddModelError("error", "");

            //Act
            ViewResult result = target.Checkout(new Order()) as ViewResult;

            //Assert
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
            Assert.True(String.IsNullOrEmpty(result.ViewName));
            Assert.False(target.ModelState.IsValid);
        }

        [Fact]
        public void Can_Checkout_And_Submit_Order()
        {
            //Arrange
            var mock = new Mock<IOrderRepository>();

            Order order = new Order();
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);

            OrderController target = new OrderController(mock.Object, cart);

            //Act
            RedirectToActionResult result = target.Checkout(new Order()) as RedirectToActionResult;

            //Assert
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);
            Assert.Equal(nameof(target.Completed), result.ActionName);
            Assert.True(target.ModelState.IsValid);
        }


    }
}
