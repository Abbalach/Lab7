using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab7_3
{
    class Store
    {

        List<Article> Articles { get; set; }
        public int NumberOfArticles
        {
            get
            {
                return Articles.Count;
            }
        }
        public Store()
        {
            Articles = new List<Article>();
        }
        public void AddArticle(Article article)
        {
            if (article.Store != this)
            {
                return;
            }
            Articles.Add(article);
        }
        public void AddArticle(string productName, int productPrice)
        {
            Articles.Add(new Article(productName, productPrice, this));
        }
        public Article GetArticle(int index)
        {
            if (index >= Articles.Count)
            {
                return null;
            }
            return Articles[index];
        }
        public Article GetArticle(int index, Action action)
        {
            if (index >= Articles.Count)
            {
                action.Invoke();
                return null;
            }
            return Articles[index];
        }
        public Article GetArticle(string productName)
        {
            var FindedArticle = Articles.Find(article => article.ProductName == productName);
            
            return FindedArticle;
        }
        public Article GetArticle(string productName, Action action)
        {
            var FindedArticle = Articles.Find(article => article.ProductName == productName);
            if (FindedArticle == null)
            {
                action.Invoke();
            }
            return FindedArticle;
        }
    }
}
