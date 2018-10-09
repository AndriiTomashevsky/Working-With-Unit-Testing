using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using SportsStore.Components;
using SportsStore.Models;
using Xunit;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace SportsStore.Tests
{
    public class NavigationMenuViewComponentTests
    {
        [Fact]
        public void Can_Select_Categories()
        {
            //Arrange
            Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();

            mockRepository.SetupGet(m => m.Products)
                .Returns((new[] {
                    new Product { ProductID = 1, Category="B" },
                    new Product { ProductID = 2, Category="B" },
                    new Product { ProductID = 3, Category="A" },
                    new Product { ProductID = 4, Category="A" },
                    new Product { ProductID = 5, Category="B" } }
                ).AsQueryable());

            NavigationMenuViewComponent component = new NavigationMenuViewComponent(mockRepository.Object);

            //Act
            ViewViewComponentResult viewComponentResult = (ViewViewComponentResult)component.Invoke();
            string[] result = ((IEnumerable<string>)viewComponentResult.ViewData.Model).ToArray();

            //Assert
            Assert.True(Enumerable.SequenceEqual(new string[] { "A", "B" }, result));
        }

        [Fact]
        public void Indicate_Selected_Category()
        {
            //Arrange
            Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();

            mockRepository.SetupGet(m => m.Products)
                .Returns((new[] {
                    new Product { ProductID = 1, Category="B" },
                    new Product { ProductID = 2, Category="B" },
                    new Product { ProductID = 3, Category="A" },
                    new Product { ProductID = 4, Category="A" },
                    new Product { ProductID = 5, Category="B" } }
                ).AsQueryable());

            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mockRepository.Object);

            target.ViewComponentContext = new ViewComponentContext()
            {
                ViewContext = new ViewContext()
                {
                    RouteData = new RouteData()
                }
            };

            target.RouteData.Values["categoryy"] = "B";

            //Act
            ViewViewComponentResult componentResult = (ViewViewComponentResult)target.Invoke();
            string result = (string)componentResult.ViewData["categoryToSelectt"];

            //Assert
            Assert.Equal("B", result);
        }
    }
}
