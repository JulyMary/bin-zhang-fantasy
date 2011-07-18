using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using Fantasy.Studio.Controls;


namespace Fantasy.Studio.Codons
{
    public class ToggleToolBarButtonBuilder : IButtonBuilder
    {

        private Dictionary<DefaultTooBar, ButtonModel> _buttons = new Dictionary<DefaultTooBar, ButtonModel>();

        #region IButtonBuilder Members

        public ButtonModel[] Build(object caller)
        {
            ToolBarTray tray = (ToolBarTray)caller;
            foreach (DefaultTooBar bar in tray.ToolBars)
            {
                ButtonModel button;
                if (! _buttons.ContainsKey(bar))
                {
                    button = new ButtonModel()
                    {
                        Text = bar.Caption,
                        IsCheckable = true,
                        Command = new ToggleToolBarCommand() { Bar = bar }
                    };
                    _buttons.Add(bar, button);
                }

            }

            return _buttons.Values.OrderBy(b => b.Text).ToArray();

        }


        class ToggleToolBarCommand : ICommand
        {
            public DefaultTooBar Bar;

            #region ICommand Members

            public bool CanExecute(object parameter)
            {
                this.CommandData().Value = Bar.Visibility == Visibility.Visible; 
                return true;
            }

            event EventHandler ICommand.CanExecuteChanged { add { } remove { } }

            public void Execute(object parameter)
            {
                if (Bar.Visibility == Visibility.Visible)
                {
                    Bar.Visibility = Visibility.Collapsed;
                }
                else
                {
                    Bar.Visibility = Visibility.Visible; 
                }
            }

            #endregion
        }

        #endregion
    }
}
