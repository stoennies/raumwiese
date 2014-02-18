using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Raumwiese
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>       
    public partial class App : Application
    {
        public static String[] mArgs;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Count() > 0)
            {
                mArgs = e.Args;
            }
        }
    }
}
