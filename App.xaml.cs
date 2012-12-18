using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using UnifiToEM.ViewModels;

namespace UnifiToEM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow app = new MainWindow();
            CategoryListViewModel context = new CategoryListViewModel();
            app.DataContext = context;
            app.Show();
        }

    }
}
