using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Lab7_12
{
    abstract class BaseMatrixClass
    {
        abstract public void Initialize();
        abstract public void EventSetter();
        abstract public void Terminate();
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
