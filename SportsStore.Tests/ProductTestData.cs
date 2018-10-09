using SportsStore.Models;
using System.Collections;
using System.Collections.Generic;

namespace SportsStore.Tests
{
    public class ProductTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { GetProducts };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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

    }
}
