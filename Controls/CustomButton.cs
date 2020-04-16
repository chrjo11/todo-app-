using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TodoApp.MVVM.Controls
{
    class CustomButton : Button
    {
        public CustomButton()
        {
            Loaded += CustomButtonLoaded;
            Click += CustomButtonClick;
        }

        private void CustomButtonLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "NeverClicked", false);
        }

        int counter = 0;

        private void CustomButtonClick(object sender, RoutedEventArgs e)
        {
            counter++;

            if (counter == 0)
            {
                VisualStateManager.GoToState(this, "NeverClicked", false);
            }
            else
            {
                VisualStateManager.GoToState(this, "Clicked", false);
            }
        }
    }
}
