using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SportsStore.Tests
{
    public class AdminControllerTests
    {
        private T GetViewModel<T>(ViewResult viewResult) where T : class
        {
            return viewResult.Model as T;
        }

        [Theory]
        [ClassData(typeof(ProductTestData))]
        public void Index_Contains_All_Products(Product[] products)
        {
            //Arrange
            var mock = new Mock<IProductRepository>();
            mock.SetupGet(m => m.Products).Returns(products.AsQueryable);

            AdminController target = new AdminController(mock.Object);

            //Act
            int result = GetViewModel<IEnumerable<Product>>(target.Index()).Count();

            //Assert
            Assert.Equal(5, result);
        }

        [Theory]
        [ClassData(typeof(ProductTestData))]
        public void Can_Edit_Product(Product[] products)
        {
            //Arrange
            var mock = new Mock<IProductRepository>();
            mock.SetupGet(m => m.Products).Returns(products.AsQueryable);

            AdminController target = new AdminController(mock.Object);

            //Act
            Product result = GetViewModel<Product>(target.Edit(1));

            //Assert
            Assert.Equal(1, result.ProductID);
        }

        [Theory]
        [ClassData(typeof(ProductTestData))]
        public void Cannot_Edit_Nonexistent_Product(Product[] products)
        {
            //Arrange
            var mock = new Mock<IProductRepository>();
            mock.SetupGet(m => m.Products).Returns(products.AsQueryable);

            AdminController target = new AdminController(mock.Object);

            //Act
            Product result = GetViewModel<Product>(target.Edit(6));

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [ClassData(typeof(ProductTestData))]
        public void Can_Save_Valid_Changes(Product[] products)
        {
            //Arrange
            var mockRepository = new Mock<IProductRepository>();
            var mockDictionary = new Mock<ITempDataDictionary>();

            AdminController target = new AdminController(mockRepository.Object);
            target.TempData = mockDictionary.Object;
            Product p1 = new Product() { Name = "p6" };

            //Act
            target.Edit(p1);

            //Assert
            mockRepository.Verify(m => m.SaveProduct(p1));
        }

        [Theory]
        [ClassData(typeof(ProductTestData))]
        public void Cannot_Save_Invalid_Changes(Product[] products)
        {
            //Arrange
            var mockRepository = new Mock<IProductRepository>();

            AdminController target = new AdminController(mockRepository.Object);
            target.ModelState.AddModelError("error", "error");
            Product p1 = new Product() { Name = "p6" };

            //Act
            target.Edit(p1);

            //Assert
            mockRepository.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never);
        }

        [Theory]
        [ClassData(typeof(ProductTestData))]
        public void Can_Delete_Valid_Products(Product[] products)
        {
            //Arrange
            var mockRepository = new Mock<IProductRepository>();

            AdminController target = new AdminController(mockRepository.Object);

            //Act
            target.Delete(products[0].ProductID);

            //Assert
            mockRepository.Verify(m => m.DeleteProduct(products[0].ProductID));
        }
    }
}