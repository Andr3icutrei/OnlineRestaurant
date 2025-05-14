using OnlineRestaurant.Database.Entities;
using OnlineRestaurant.UI.View;
using OnlineRestaurant.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OnlineRestaurant.UI.Services
{
    public class WindowService : IWindowService
    {
        private Dictionary<Window, WindowContext> _windowContexts = new Dictionary<Window, WindowContext>();

        private class WindowContext
        {
            public object Owner { get; set; }
            public object ViewModel { get; set; }
        }

        public void ShowWindow<T>(object dataContext = null, object owner = null) where T : Window, new()
        {
            var window = new T();

            if (dataContext != null)
            {
                window.DataContext = dataContext;
            }

            _windowContexts[window] = new WindowContext
            {
                Owner = owner,
                ViewModel = dataContext ?? window.DataContext
            };

            WireUpCloseRequest(window);

            window.Closed += Window_Closed;

            window.Show();
        }

        public bool? ShowDialog<T>(object dataContext = null, object owner = null) where T : Window, new()
        {
            var window = new T();

            if (dataContext != null)
            {
                window.DataContext = dataContext;
            }

            _windowContexts[window] = new WindowContext
            {
                Owner = owner,
                ViewModel = dataContext ?? window.DataContext
            };

            WireUpCloseRequest(window);

            window.Closed += Window_Closed;

            if (owner is Window ownerWindow)
            {
                window.Owner = ownerWindow;
            }

            return window.ShowDialog();
        }

        public void ShowVariableTextBoxesWindow(
            List<Item> selectedItems,
            object owner,
            Action<VariableTextBoxesVM, bool> resultCallback)
        {
            var window = new VariableTextBoxesWindow(selectedItems);

            var viewModel = window.DataContext as VariableTextBoxesVM;

            if (viewModel == null)
            {
                throw new InvalidOperationException("The VariableTextBoxesWindow did not create a VariableTextBoxesVM.");
            }

            viewModel.RequestClose = () => CloseWindow(window);

            _windowContexts[window] = new WindowContext
            {
                Owner = owner,
                ViewModel = viewModel
            };

            void closedHandler(object sender, EventArgs e)
            { 
                resultCallback(viewModel, viewModel.WasConfirmed);

                window.Closed -= closedHandler;
                _windowContexts.Remove(window);
            }

            window.Closed += closedHandler;

            if (owner is Window ownerWindow)
            {
                window.Owner = ownerWindow;
            }

            window.Show();
        }

        public void CloseWindow(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }
        private void WireUpCloseRequest(Window window)
        {
            var viewModel = window.DataContext;

            if (viewModel is ICloseable closeable)
            {
                closeable.RequestClose = () => CloseWindow(window);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (sender is Window window && _windowContexts.ContainsKey(window))
            {
                _windowContexts.Remove(window);

                window.Closed -= Window_Closed;
            }
        }
    }
}
