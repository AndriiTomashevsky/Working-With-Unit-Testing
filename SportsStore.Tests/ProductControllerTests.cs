using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SportsStore.Tests
{
    public class ProductControllerTests
    {
        [Theory]
        [MemberData(nameof(ProductTestData))]
        public void Can_Paginate(Product[] products)
        {
            //Arrange
            Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();

            mockRepository.SetupGet(m => m.Products).Returns(products.AsQueryable());

            ProductController productController = new ProductController(mockRepository.Object);
            productController.PageSize = 2;

            //Act
            ViewResult viewResult = productController.List(null, 1);
            ProductsListViewModel productsListViewModel = (ProductsListViewModel)viewResult.Model;
            Product[] result = productsListViewModel.Products.ToArray();

            //Assert
            Assert.True(result.Length == 2);
            Assert.Equal(1, result[0].ProductID);
            Assert.Equal(2, result[1].ProductID);
        }

        [Theory]
        [MemberData(nameof(ProductTestData))]
        public void Can_Send_Pagination_View_Model(Product[] products)
        {
            //Arrange
            Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();

            mockRepository.SetupGet(m => m.Products).Returns(products.AsQueryable());

            ProductController productController = new ProductController(mockRepository.Object);
            productController.PageSize = 2;

            //Act
            ViewResult viewResult = productController.List(null, 2);
            ProductsListViewModel productsListViewModel = (ProductsListViewModel)viewResult.Model;
            PagingInfo pagingInfo = productsListViewModel.PagingInfo;

            //Assert
            Assert.Equal(5, pagingInfo.TotalItems);
            Assert.Equal(2, pagingInfo.ItemsPerPage);
            Assert.Equal(2, pagingInfo.CurrentPage);
            Assert.Equal(3, pagingInfo.TotalPages);
        }

        [Theory]
        [MemberData(nameof(ProductTestData))]
        public void Can_Filter_Product(Product[] products)
        {
            //Arrange
            Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();

            mockRepository.SetupGet(m => m.Products).Returns(products.AsQueryable());

            ProductController productController = new ProductController(mockRepository.Object);
            productController.PageSize = 3;

            //Act
            ViewResult viewResult = productController.List("B", 1);
            ProductsListViewModel productsListViewModel = (ProductsListViewModel)viewResult.Model;
            Product[] result = productsListViewModel.Products.ToArray();

            //Assert
            Assert.True(result.Length == 2);
            Assert.Equal(2, result[0].ProductID);
            Assert.Equal(4, result[1].ProductID);
        }

        [Theory]
        [MemberData(nameof(ProductTestData))]
        public void Generate_Category_Specific_Product_Count(Product[] products)
        {
            //Arrange
            Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();

            mockRepository.SetupGet(m => m.Products).Returns(products.AsQueryable());

            ProductController target = new ProductController(mockRepository.Object);
            target.PageSize = 5;

            //Act
            //ViewResult viewResult1 = target.List("A");
            //ViewResult viewResult2 = target.List("B");
            //ProductsListViewModel productsListViewModel1 = (ProductsListViewModel)viewResult1.Model;
            //ProductsListViewModel productsListViewModel2 = (ProductsListViewModel)viewResult2.Model;
            Func<ViewResult, ProductsListViewModel> GetModel = viewResult => viewResult.Model as ProductsListViewModel;

            ProductsListViewModel productsListViewModel1 = GetModel(target.List("A"));
            ProductsListViewModel productsListViewModel2 = GetModel(target.List("B"));
            ProductsListViewModel productsListViewModel3 = GetModel(target.List(null));

            Product[] products1 = productsListViewModel1.Products.ToArray();
            Product[] products2 = productsListViewModel2.Products.ToArray();
            Product[] products3 = productsListViewModel3.Products.ToArray();

            int result1 = products1.Count();
            int result2 = products2.Count();
            int result3 = products3.Count();

            //Assert
            Assert.Equal(2, result1);
            Assert.Equal(2, result2);
            Assert.Equal(5, result3);
        }

        private static Product[] GetProducts
        {
            get
            {
                return new Product[]
                {
                    new Product { ProductID = 1, Category="A" },
                    new Product { ProductID = 2, Category="B" },
                    new Product { ProductID = 3, Category="A" },
                    new Product { ProductID = 4, Category="B" },
                    new Product { ProductID = 5, Category="C" }
                };
            }
        }

        public static IEnumerable<object> ProductTestData()
        {
            yield return new object[] { GetProducts };
        }
    }
}
