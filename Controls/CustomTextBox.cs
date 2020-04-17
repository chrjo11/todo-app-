using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TodoApp.MVVM.Controls
{
    class CustomTextBox : TextBox
    {
        public CustomTextBox()
        {
            Loaded += CustomTextBoxLoaded;
            TextChanged += CustomTextChanged;
            GotFocus += CustomGotFocus;
            LostFocus += CustomLostFocus;
        }

        private void CustomLostFocus(object sender, RoutedEventArgs e)
        {
            if(this.Text.Length == 0) 
            { 
                VisualStateManager.GoToState(this, "Hint", false);
            }
        }

        private void CustomGotFocus(object sender, RoutedEventArgs e)
        {
            if (this.Text.Length != 0)
            {
                VisualStateManager.GoToState(this, "HintHidden", false);
            }
            else
            {
                VisualStateManager.GoToState(this, "Hint", false);
            }
        }


        private void CustomTextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.Text.Length != 0 && IsFocused == true)
            {
                VisualStateManager.GoToState(this, "HintHidden", false);
            }
        }

        private void CustomTextBoxLoaded(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Hint", false);
        }
    }
}
