using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab7_3
{
    class Article
    {
        public string ProductName { get; private set; }
        public int Price { get; private set; }
        public Store Store { get; private set; }
        public Article()
        {
            ProductName = default;
            Price = default;
            Store = default;
        }
        public Article(string productName, int price, Store store)
        {
            ProductName = productName;
            Price = price;
            Store = store;
        }
    }
}
