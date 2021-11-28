using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Lab7_3
{
    class StoreCreation
    {
        MainWindow Window { get; set; }
        public Store Store { get; private set; }
        int numberOfpages;

        Label StoreLabel;
        Label pageLabel;
        int pageNumber;

        List<List<ValuePair<Button,Article>>> articlePages;

        int baseArticleSize = 20;
        Thickness firstArticleMargin = new Thickness(20, 115, 0, 0);

        Button prevPageButton;
        Button nextPageButton;
        Button findButton;
        Button AddArticleButton;

        TextBox findTextBox;

        bool helper = false;
        bool messegeHelper = true;
        bool addHelper = true;
        private string savedPrice;

        public StoreCreation(Store store, MainWindow window)
        {
            Store = store;
            Window = window;
        }
        public void Initialize()
        {
            Window.Width = 246;
            Window.Height = 500;
            Window.grid.Width = 246;
            Window.grid.Height = 460;
            helper = true;

            StoreLabel = new Label 
            { 
                Margin = new Thickness(0,23,0,0),
                Content = "Store",
                FontSize = 20,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            Window.grid.Children.Add(StoreLabel);

            pageLabel = new Label 
            {
                Margin = new Thickness(0, 431, 0, 0),
                Content = "page 1",
                FontSize = 10,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            Window.grid.Children.Add(pageLabel);

            nextPageButton = new Button 
            { 
                Width = 65,
                Height = 20,
                Content = ">>",
                Margin = new Thickness(Window.Width - 93,Window.Height - 65, 0,0),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            prevPageButton = new Button 
            {
                Width = 65,
                Height = 20,
                Content = "<<",
                Visibility = Visibility.Hidden,
                Margin = new Thickness(10, Window.Height - 65, 0, 0),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            Window.grid.Children.Add(nextPageButton);
            Window.grid.Children.Add(prevPageButton);

            findButton = new Button
            {
                Width = 25,
                Height = baseArticleSize,
                Content = "find",
                Margin = new Thickness(Window.Width - 105, firstArticleMargin.Top - 40, 0,0),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            Window.grid.Children.Add(findButton);

            findTextBox = new TextBox 
            {
                Width = 115,
                Height = baseArticleSize,
                Margin = new Thickness(20, firstArticleMargin.Top - 40, 0, 0),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            Window.grid.Children.Add(findTextBox);

            pageNumber = 1;
            decimal pagesHelper = (Convert.ToDecimal(Store.NumberOfArticles) + 1) / 8;
           
            if (pagesHelper > decimal.Ceiling(pagesHelper))
            {
                numberOfpages = Convert.ToInt32(decimal.Ceiling(pagesHelper) + 1);
            }
            else
            {
                numberOfpages = Convert.ToInt32(decimal.Ceiling(pagesHelper));
            }
            
            articlePages = new List<List<ValuePair<Button, Article>>>();

            double topLocation = firstArticleMargin.Top - 10 + (Store.NumberOfArticles - numberOfpages * 8 + 8) * 40;
            AddArticleButton = new Button
            {
                Width = 40,
                Height = 40,
                Content = "+",
                VerticalContentAlignment = VerticalAlignment.Top,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                FontSize = 25,
                BorderBrush = Brushes.White,
                Background = Brushes.White,
                Margin = new Thickness(20, topLocation, 0, 0),
                Visibility = Visibility.Hidden,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            Window.grid.Children.Add(AddArticleButton);

            if (numberOfpages == 1)
            {
                AddArticleButton.Visibility = Visibility.Visible;
                nextPageButton.Visibility = Visibility.Hidden;
            }

            for (int i = 0; i < numberOfpages; i++)
            {
                articlePages.Add(new List<ValuePair<Button, Article>>());
                int articlesCount;
                if (i == numberOfpages - 1)
                {
                    articlesCount = Store.NumberOfArticles - i * 8;
                }
                else 
                {
                    articlesCount = 8;
                }
                for (int j = 0; j < articlesCount; j++)
                {
                    Visibility enabled = Visibility.Hidden;
                    if (i == 0)
                    {
                        enabled = Visibility.Visible;
                    }
                    articlePages[i].Add(new ValuePair<Button, Article>());
                    articlePages[i][j].Second = Store.GetArticle(j + i * 8);

                    articlePages[i][j].First = new Button 
                    {
                        Height = baseArticleSize,
                        Content = articlePages[i][j].Second.ProductName,
                        Visibility = enabled,
                        Margin = new Thickness(firstArticleMargin.Left,firstArticleMargin.Top + j * 40,0,0),
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Left
                    };
                    Window.grid.Children.Add(articlePages[i][j].First);
                }
            }

        }
        public void EventSetter()
        {
            if (!helper)
            {
                return;
            }
            nextPageButton.Click += (o, e) => 
            {
                pageNumber++;
                pageLabel.Content = "page " + pageNumber;
                if (pageNumber == numberOfpages)
                {
                    nextPageButton.Visibility = Visibility.Hidden;
                    prevPageButton.Visibility = Visibility.Visible;
                    AddArticleButton.Visibility = Visibility.Visible;
                }
                else
                {
                    nextPageButton.Visibility = Visibility.Visible;
                    prevPageButton.Visibility = Visibility.Visible;
                }

                foreach (var page in articlePages.ElementAt(pageNumber - 2))
                {
                    page.First.Visibility = Visibility.Hidden;
                }
                foreach (var article in articlePages.ElementAt(pageNumber - 1))
                {
                    article.First.Visibility = Visibility.Visible;
                }
            };

            prevPageButton.Click += (o, e) =>
            {
                pageNumber--;
                pageLabel.Content = "page " + pageNumber;
                if (pageNumber == 1)
                {
                    nextPageButton.Visibility = Visibility.Visible;
                    prevPageButton.Visibility = Visibility.Hidden;
                    AddArticleButton.Visibility = Visibility.Hidden;
                }
                else
                {
                    nextPageButton.Visibility = Visibility.Visible;
                    prevPageButton.Visibility = Visibility.Visible;
                    AddArticleButton.Visibility = Visibility.Hidden;
                }
                foreach (var page in articlePages.ElementAt(pageNumber))
                {
                    page.First.Visibility = Visibility.Hidden;
                }
                foreach (var article in articlePages.ElementAt(pageNumber - 1))
                {
                    article.First.Visibility = Visibility.Visible;
                }
            };

            foreach (var page in articlePages)
            {
                foreach (var article in page)
                {
                    article.First.Click += (o, e) =>
                    {
                        if (!messegeHelper)
                        {
                            return;
                        }
                        ArticleClick(article);
                    };
                }
            }

            AddArticleButton.Click += (o, e) =>
            {
                if (!addHelper)
                {
                    return;
                }
                addHelper = false;
                var additionalWindow = new MainWindow(false);
                additionalWindow.Height = 200;
                additionalWindow.Width = 170;
                additionalWindow.Show();

                Label name = new Label
                {
                    Margin = new Thickness(0, 50, 0, 0),
                    Content = "Название - ",
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left
                };
                Label price = new Label
                {
                    Margin = new Thickness(0, 88, 0, 0),
                    Content = "Стоимость - ",
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left
                };
                Label add = new Label
                {
                    Margin = new Thickness(0, 2, 0, 0),
                    Content = "Добавление продукта",
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                additionalWindow.grid.Children.Add(name);
                additionalWindow.grid.Children.Add(price);
                additionalWindow.grid.Children.Add(add);

                TextBox nameTB = new TextBox
                {
                    Height = 23,
                    Width = 60,
                    Margin = new Thickness(70, 53, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left
                };
                TextBox priceTB = new TextBox
                {
                    Height = 23,
                    Width = 60,
                    Margin = new Thickness(75, 90, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left
                };
                additionalWindow.grid.Children.Add(nameTB);
                additionalWindow.grid.Children.Add(priceTB);

                Button ok = new Button
                {
                    Width = 55,
                    Content = "ok",
                    Background = Brushes.White,
                    Margin = new Thickness(0, 135, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                additionalWindow.grid.Children.Add(ok);

                priceTB.TextChanged += (obj, args) =>
                {
                    if (ErrorHelper(priceTB, false))
                    {
                        savedPrice = priceTB.Text;
                    }
                    else
                    {
                        priceTB.Text = savedPrice;
                    }
                };

                additionalWindow.Closed += (obj, args) =>
                {
                    addHelper = true;
                };

                ok.Click += (obj, args) =>
                {
                    addHelper = true;
                    additionalWindow.Close();

                    Store.AddArticle(nameTB.Text,int.Parse(priceTB.Text));                    

                    double topButtonLocation = firstArticleMargin.Top + (Store.NumberOfArticles - numberOfpages * 8 + 7) * 40;
                    if (articlePages.Last().Count == 7)
                    {
                        articlePages.Add(new List<ValuePair<Button, Article>>());

                        numberOfpages++;
                        double topAddLocation = firstArticleMargin.Top - 10 + (Store.NumberOfArticles - numberOfpages * 8 + 8) * 40;
                        AddArticleButton.Margin = new Thickness(20, topAddLocation, 0, 0);

                        AddArticleButton.Visibility = Visibility.Hidden;
                        nextPageButton.Visibility = Visibility.Visible;

                        articlePages.ElementAt(articlePages.Count - 2).Add(new ValuePair<Button, Article>());

                        articlePages.ElementAt(articlePages.Count - 2).Last().First = new Button
                        {
                            Height = baseArticleSize,
                            Content = nameTB.Text,
                            Visibility = Visibility.Visible,
                            Margin = new Thickness(20, topButtonLocation, 0, 0),
                            VerticalAlignment = VerticalAlignment.Top,
                            HorizontalAlignment = HorizontalAlignment.Left
                        };
                        Window.grid.Children.Add(articlePages.ElementAt(articlePages.Count - 2).Last().First);

                        int firInd = articlePages.Count - 2;
                        int secInd = articlePages[firInd].Count - 1;
                        articlePages.ElementAt(articlePages.Count - 2).Last().First.Click += (ob, arg) =>
                        {
                            if (!messegeHelper)
                            {
                                return;
                            }
                            ArticleClick(articlePages[firInd][secInd]);
                        };

                        articlePages.ElementAt(articlePages.Count - 2).Last().Second = Store.GetArticle(Store.NumberOfArticles - 1);
                    }
                    else
                    {
                        articlePages.Last().Add(new ValuePair<Button, Article>());

                        double topAddLocation = firstArticleMargin.Top - 10 + (Store.NumberOfArticles - numberOfpages * 8 + 8) * 40;
                        AddArticleButton.Margin = new Thickness(20, topAddLocation, 0, 0);

                        articlePages.Last().Last().First = new Button 
                        {
                            Height = baseArticleSize,
                            Content = nameTB.Text,
                            Visibility = Visibility.Visible,
                            Margin = new Thickness(firstArticleMargin.Left, topButtonLocation, 0, 0),
                            VerticalAlignment = VerticalAlignment.Top,
                            HorizontalAlignment = HorizontalAlignment.Left
                        };
                        Window.grid.Children.Add(articlePages.Last().Last().First);

                        int firInd = articlePages.Count - 1;
                        int secInd = articlePages[firInd].Count - 1;
                        articlePages.Last().Last().First.Click += (ob, arg) =>
                        {
                            if (!messegeHelper)
                            {
                                return;
                            }
                            ArticleClick(articlePages[firInd][secInd]);
                        };
                        articlePages.Last().Last().Second = Store.GetArticle(Store.NumberOfArticles - 1);                        
                    }
                };                           
            };

            findButton.Click += (o, e) =>
            {
                bool localHelper = false;
                foreach (var page in articlePages)
                {
                    var article = page.Find(product => { return product.Second.ProductName == findTextBox.Text; });
                    if (article != null)
                    {
                        localHelper = true;
                        if (!messegeHelper)
                        {
                            return;
                        }
                        ArticleClick(article);
                        return;
                    }
                }
                if (!localHelper)
                {
                    MessageBox.Show("Продукт не найден.");
                }
            };
        }

        protected void ArticleClick(ValuePair<Button,Article> article)
        {
            messegeHelper = false;
            var additionalWindow = new MainWindow(false);
            additionalWindow.Height = 130;
            additionalWindow.Width = 160 + article.Second.ProductName.Length * 5;
            additionalWindow.Show();
            Label name = new Label
            {
                Margin = new Thickness(19, 12, 0, 0),
                Content = "Наименование - " + article.Second.ProductName,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            Label price = new Label
            {
                Margin = new Thickness(19, 51, 0, 0),
                Content = "Стоимость - " + article.Second.Price + " грн",
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };

            additionalWindow.grid.Children.Add(name);
            additionalWindow.grid.Children.Add(price);

            additionalWindow.Closed += (obj, args) => { messegeHelper = true; };
        }
        protected bool ErrorHelper(TextBox textBox, bool choose)
        {
            if (choose)
            {
                try
                {
                    int.Parse(textBox.Text);
                }
                catch
                {
                    if (textBox.Text == "" || textBox.Text == "-")
                    {
                        return true;
                    }
                    return false;
                }
                return true;
            }
            else
            {
                try
                {
                    int.Parse(textBox.Text);
                }
                catch
                {
                    if (textBox.Text == "")
                    {
                        return true;
                    }
                    return false;
                }
                return true;
            }

        }
    }
}
