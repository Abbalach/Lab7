using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace Lab7_12
{
    class MyMatrix : BaseMatrixClass
    {
        protected virtual int Height { get; set; }
        protected virtual int Width { get; set; }
        protected int prevHeight { get; set; }
        protected int prevWidth { get; set; }

        protected ValuePair<TextBox, int>[,] matrixElements { get; set; }
        List<ValuePair<TextBox, int>[,]> baseMatrices { get; set; }

        protected string savedHeightdisplay { get; set; }
        protected string savedWidthdisplay { get; set; }

        protected ValuePair<int,int> StableBoxSize { get; }
        protected ValuePair<int, int> FirstBoxLocation { get; }

        protected TextBox matrixWidthTextBox;
        protected TextBox matrixHeightTextBox;

        protected Label matrixWidthLabel;
        protected Label matrixHeightLabel;

        protected Button createButton;
        protected Button prevButton;
        protected Button baseMatrix;

        MainWindow Window { get; }

        MyArray lastApp;
        public MyArray LastApp
        {
            get
            {
                return lastApp;
            }
            set
            {
                lastApp = value;
                if (lastApp.NextApp == null)
                    lastApp.NextApp = this;
            }
        }


        public MyMatrix(MainWindow window)
        {
            Window = window;
            Height = 0;
            Width = 0;
            StableBoxSize = new ValuePair<int, int>(46, 23);
            FirstBoxLocation = new ValuePair<int, int>(12, 120);
        }
        public override void Initialize()
        {
            Window.Width = 360;
            Window.Height = 190;
            matrixWidthTextBox = new TextBox
            {
                Margin = new Thickness(76,38,0,0),
                Width = 100,
                Height = 23,             
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            matrixHeightTextBox = new TextBox
            {
                Margin = new Thickness(76, 80, 0, 0),
                Width = 100,
                Height = 23,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            Window.grid.Children.Add(matrixHeightTextBox);
            Window.grid.Children.Add(matrixWidthTextBox);

            matrixWidthLabel = new Label
            {
                Margin = new Thickness(33, 35, 0, 0),
                Width = 50,
                Height = 40,
                Content = "width",
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            matrixHeightLabel = new Label
            {
                Margin = new Thickness(29, 77, 0, 0),
                Width = 50,
                Height = 40,
                Content = "height",
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            Window.grid.Children.Add(matrixHeightLabel);
            Window.grid.Children.Add(matrixWidthLabel);

            createButton = new Button 
            {
                Margin = new Thickness(219, 41, 0, 0),
                Width = 109,
                Height = 57,
                Content = "Create",
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            prevButton = new Button
            {
                Margin = new Thickness(5, Window.Height-67, 0, 0),
                Width = 70,
                Height = 20,
                Content = "<<",
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            Window.grid.Children.Add(prevButton);
            Window.grid.Children.Add(createButton);
        }
        public override void EventSetter()
        {
            matrixHeightTextBox.TextChanged += (o, e) =>
            {
                if (ErrorHelper(matrixHeightTextBox,false))
                {
                    if (matrixHeightTextBox.Text == "")
                    {
                        Height = 0;
                    }
                    else
                    {
                        Height = int.Parse(matrixHeightTextBox.Text);
                        savedHeightdisplay = matrixHeightTextBox.Text;
                    }
                }
                else
                {
                    matrixHeightTextBox.Text = savedHeightdisplay;
                }
            };
            matrixWidthTextBox.TextChanged += (o, e) =>
            {
                if (ErrorHelper(matrixWidthTextBox,false))
                {
                    if (matrixWidthTextBox.Text == "")
                    {
                        Width = 0;
                    }
                    else
                    {
                        Width = int.Parse(matrixWidthTextBox.Text);
                        savedWidthdisplay = matrixWidthTextBox.Text;
                    }
                }
                else
                {
                    matrixWidthTextBox.Text = savedWidthdisplay;
                }
            };
            createButton.Click += MatrixCreation;
            prevButton.Click += AppSwitcher;
        }
        public override void Terminate()
        {
            Window.grid.Children.Remove(matrixWidthTextBox);
            Window.grid.Children.Remove(matrixHeightTextBox);

            Window.grid.Children.Remove(createButton);
            Window.grid.Children.Remove(prevButton);
            Window.grid.Children.Remove(baseMatrix);

            Window.grid.Children.Remove(matrixWidthLabel);
            Window.grid.Children.Remove(matrixHeightLabel);

            if (matrixElements != null)
            {
                foreach (var element in matrixElements)
                {
                    Window.grid.Children.Remove(element.First);
                }
            }
            if (baseMatrices != null)
            {
                foreach (var matrix in baseMatrices)
                {
                    foreach (var element in matrix)
                    {
                        Window.grid.Children.Remove(element.First);
                    }
                }
            }
            
        }
        protected virtual void MatrixCreation(object o, EventArgs e)
        {
            bool localhelper = true;

            if (Height == 0)
            {
                matrixHeightTextBox.Text = "0";
                localhelper = false;
            }

            if (Width == 0)
            {
                matrixWidthTextBox.Text = "0";
                localhelper = false;
            }
            if (!localhelper)
            {
                return;
            }
            if (matrixElements != null)
            {
                for (int i = 0; i < prevWidth; i++)
                {
                    for (int j = 0; j < prevHeight; j++)
                    {
                        Window.grid.Children.Remove(matrixElements[i, j].First);
                    }
                }
            }
            if (baseMatrices != null)
            {
                foreach (var matrix in baseMatrices)
                {
                    for (int i = 0; i < matrix.GetLength(0); i++)
                    {
                        for (int j = 0; j < matrix.GetLength(1); j++)
                        {
                            Window.grid.Children.Remove(matrix[i, j].First);
                        }
                    }
                }
            }
            Window.grid.Children.Remove(baseMatrix);

            prevHeight = Height;
            prevWidth = Width;
            matrixElements = new ValuePair<TextBox, int>[Width, Height];


            if (Width * 52 < 360)
            {
                Window.Width = 360;
                Window.Height = 260 + 29 * Height;
            }            
            else
            {
                Window.Width = 90 + 52 * Width;
                Window.Height = 260 + 29 * Height;
            }             

            prevButton.Margin = new Thickness(5, Window.Height - 67, 0, 0);
            baseMatrix = new Button 
            {
                Width = 100,
                Height = 20,
                Margin = new Thickness(12, FirstBoxLocation.Second + Height * (StableBoxSize.Second + 6) + 20, 0, 0),
                Content = "base matrices",
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            Window.grid.Children.Add(baseMatrix);
            baseMatrix.Click += BaseMatricesCreation;


            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    matrixElements[j, i] = new ValuePair<TextBox, int>(new TextBox
                    {
                        Width = StableBoxSize.First,
                        Height = StableBoxSize.Second,
                        Margin = new Thickness(FirstBoxLocation.First + j * (StableBoxSize.First + 6),
                                             FirstBoxLocation.Second + i * (StableBoxSize.Second + 6),0,0),
                        TextAlignment = TextAlignment.Right,
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Left
                    }, 0);
                    Window.grid.Children.Add(matrixElements[j, i].First);
                }               
            }
            foreach (var element in matrixElements)
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
                };
            }
        }

        protected virtual void BaseMatricesCreation(object o, EventArgs e)
        {
            int step = matrixElements.GetLength(0);
            if (step > matrixElements.GetLength(1))
            {
                step = matrixElements.GetLength(1);
            }

            if (baseMatrices != null)
            {
                foreach (var matrix in baseMatrices)
                {
                    foreach (var element in matrix)
                    {
                        Window.grid.Children.Remove(element.First);
                    }
                }
            }

            baseMatrices = new List<ValuePair<TextBox, int>[,]>();

            for (int i = 0; i < step; i++)
            {
                baseMatrices.Add(new ValuePair<TextBox, int>[i, i]);
            }

            foreach (var matrix in baseMatrices)
            {
                for (int i = 0; i < matrix.GetLength(1); i++)
                {
                    for (int j = 0; j < matrix.GetLength(0); j++)
                    {
                        matrix[j, i] = new ValuePair<TextBox, int>();
                        matrix[j, i].Second = matrixElements[j, i].Second;
                    }
                }
            }

            for (int i = 0; i < baseMatrices.Count; i++)
            {
                for (int j = 0; j < baseMatrices[i].GetLength(1); j++)
                {
                    for (int L = 0; L < baseMatrices[i].GetLength(0); L++)
                    {
                        double baseMatrixTop = 20 + FirstBoxLocation.Second + baseMatrix.Height + Height * (StableBoxSize.Second + 6) +
                            j * (StableBoxSize.Second + 6) + i * i * (StableBoxSize.Second + 10) / 2;
                        baseMatrices[i][L, j].First = new TextBox
                        {
                            Width = StableBoxSize.First,
                            Height = StableBoxSize.Second,
                            Margin = new Thickness(FirstBoxLocation.First + L * (StableBoxSize.First + 6), baseMatrixTop, 0, 0),
                            Text = baseMatrices[i][L, j].Second.ToString(),
                            TextAlignment = TextAlignment.Right,
                            VerticalAlignment = VerticalAlignment.Top,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            IsReadOnly = true
                        };
                        Window.grid.Children.Add(baseMatrices[i][L, j].First);
                    }
                }
            }
            int HeightHelper = baseMatrices.Count - 1;
            if (Width * 52 < 360)
            {
                Window.Width = 360;
                if (step == 1)
                {
                    Window.Height = 260 + 29 * Height;
                }
                else
                {
                    Window.Height = 90 + FirstBoxLocation.Second + baseMatrix.Height + Height * (StableBoxSize.Second + 6) +
                            baseMatrices[baseMatrices.Count - 1].GetLength(1) * (StableBoxSize.Second + 6) +
                            HeightHelper * HeightHelper * (StableBoxSize.Second + 10) / 2;
                }
                
            }
            else
            {
                Window.Width = 90 + 52 * Width;
                if (step == 1)
                {
                    Window.Height = 260 + 29 * Height;
                }
                else
                {
                    Window.Height = 90 + FirstBoxLocation.Second + baseMatrix.Height + Height * (StableBoxSize.Second + 6) +
                            baseMatrices[baseMatrices.Count - 1].GetLength(1) * (StableBoxSize.Second + 6) +
                            HeightHelper * HeightHelper * (StableBoxSize.Second + 10) / 2;
                }
            }
            prevButton.Margin = new Thickness(5, Window.Height - 67, 0, 0);
        }
        protected virtual void AppSwitcher(object o, EventArgs e)
        {
            Terminate();
            lastApp.Initialize();
            lastApp.EventSetter();
        }
    }
}
