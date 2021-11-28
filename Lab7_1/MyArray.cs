using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Lab7_12
{
    class MyArray : BaseMatrixClass
    {

        protected virtual int Size { get; set; }

        protected int prevSize { get; set; }

        protected ValuePair<TextBox, int>[] arrayElements { get; set; }

        protected string savedSizedisplay { get; set; }

        protected ValuePair<int, int> StableBoxSize { get; }
        protected ValuePair<int, int> FirstBoxLocation { get; }

        protected TextBox arraySizeTextBox;
        protected TextBox MaxTextBox;

        protected Label matrixSizeLabel;
        protected Label MaxElementLabel;

        protected Button createButton;
        protected Button nextButton;

        MainWindow Window { get; }

        MyMatrix nextApp;
        public MyMatrix NextApp
        {
            get
            {
                return nextApp;
            }
            set
            {
                nextApp = value;
                if (nextApp.LastApp == null)
                    nextApp.LastApp = this;
            }
        }

        public MyArray(MainWindow window)
        {
            Window = window;
            Size = 0;
            StableBoxSize = new ValuePair<int, int>(46, 23);
            FirstBoxLocation = new ValuePair<int, int>(12, 120);
        }
        public override void Initialize()
        {
            Window.Width = 320;
            Window.Height = 190;
            arraySizeTextBox = new TextBox
            {
                Margin = new Thickness(50, 57, 0, 0),
                Width = 100,
                Height = 23,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            Window.grid.Children.Add(arraySizeTextBox);

            matrixSizeLabel = new Label
            {
                Margin = new Thickness(12, 54, 0, 0),
                Width = 50,
                Height = 40,
                Content = "size",
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            Window.grid.Children.Add(matrixSizeLabel);

            createButton = new Button
            {
                Margin = new Thickness(180, 41, 0, 0),
                Width = 109,
                Height = 57,
                Content = "Create",
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            nextButton = new Button
            {
                Margin = new Thickness(Window.Width - 100, Window.Height - 67, 0, 0),
                Width = 70,
                Height = 20,
                Content = ">>",
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            Window.grid.Children.Add(nextButton);
            Window.grid.Children.Add(createButton);
        }
        public override void EventSetter()
        {
            arraySizeTextBox.TextChanged += (o, e) =>
            {
                if (ErrorHelper(arraySizeTextBox, false))
                {
                    if (arraySizeTextBox.Text == "")
                    {
                        Size = 0;
                    }
                    else
                    {
                        Size = int.Parse(arraySizeTextBox.Text);
                        savedSizedisplay = arraySizeTextBox.Text;
                    }
                }
                else
                {
                    arraySizeTextBox.Text = savedSizedisplay;
                }
            };
            createButton.Click += MatrixCreation;
            nextButton.Click += AppSwitcher;
        }
        public override void Terminate()
        {
            Window.grid.Children.Remove(arraySizeTextBox);
            Window.grid.Children.Remove(arraySizeTextBox);
            Window.grid.Children.Remove(MaxTextBox);

            Window.grid.Children.Remove(createButton);
            Window.grid.Children.Remove(nextButton);
            

            Window.grid.Children.Remove(matrixSizeLabel);
            Window.grid.Children.Remove(MaxElementLabel);

            if (arrayElements != null)
            {
                foreach (var element in arrayElements)
                {
                    Window.grid.Children.Remove(element.First);
                }
            }          
        }
        protected virtual void MatrixCreation(object o, EventArgs e)
        {
            bool localhelper = true;

            if (Size == 0)
            {
                arraySizeTextBox.Text = "0";
                localhelper = false;
            }
            if (!localhelper)
            {
                return;
            }
            if (arrayElements != null)
            {
                for (int i = 0; i < prevSize; i++)
                {
                    Window.grid.Children.Remove(arrayElements[i].First);
                }
            }
            Window.grid.Children.Remove(MaxTextBox);

            prevSize = Size;
            arrayElements = new ValuePair<TextBox, int>[Size];


            if (Size * 52 < 360)
            {
                Window.Width = 360;
                Window.Height = 240;
            }
            else
            {
                Window.Width = 90 + 52 * Size;
                Window.Height = 240;
            }

            nextButton.Margin = new Thickness(Window.Width - 100, Window.Height - 67, 0, 0);

            MaxTextBox = new TextBox
            {
                Margin = new Thickness(50, 152, 0, 0),
                Width = 100,
                Height = 23,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            Window.grid.Children.Add(MaxTextBox);

            MaxElementLabel = new Label 
            {
                Margin = new Thickness(6, 150, 0, 0),
                Width = 50,
                Height = 40,
                Content = "max=",
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            Window.grid.Children.Add(MaxElementLabel);

           
            for (int i = 0; i < Size; i++)
            {
                arrayElements[i] = new ValuePair<TextBox, int>(new TextBox
                {
                    Width = StableBoxSize.First,
                    Height = StableBoxSize.Second,
                    Margin = new Thickness(FirstBoxLocation.First + i * (StableBoxSize.First + 6), FirstBoxLocation.Second, 0, 0),
                    TextAlignment = TextAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left
                }, 0);
                Window.grid.Children.Add(arrayElements[i].First);
            }
            foreach (var element in arrayElements)
            {
                element.First.TextChanged += (obj, args) =>
                {
                    if (ErrorHelper(element.First, true))
                    {
                        if (element.First.Text == "")
                        {
                            element.Second = 0;
                        }
                        else
                        {
                            if (element.First.Text == "-")
                            {
                                element.Second = -1;
                            }
                            else
                            {
                                element.Second = int.Parse(element.First.Text);
                            }
                        }
                    }
                    else
                    {
                        element.First.Text = element.Second.ToString();
                    }
                    MaxElementFinding(arrayElements);
                };
            }
        }
        protected virtual void AppSwitcher(object o, EventArgs e)
        {
            Terminate();
            nextApp.Initialize();
            nextApp.EventSetter();
        }

        protected virtual void MaxElementFinding(ValuePair<TextBox,int>[] elements)
        {
            int maxElement = int.MinValue;
            foreach (var element in elements)
            {
                if (element.Second > maxElement)
                {
                    maxElement = element.Second;
                }
            }
            MaxTextBox.Text = maxElement.ToString();
        }
        
    }
}
