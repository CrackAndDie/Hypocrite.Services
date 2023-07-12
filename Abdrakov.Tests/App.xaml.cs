using Abdrakov.Engine.MVVM;
using Abdrakov.Styles;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Abdrakov.Tests
{
    public partial class App : AbdrakovApplication
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Application.Current.Resources.MergedDictionaries.Add(new AbdrakovBundledTheme() 
            {
                // main app themes here
            });
            base.OnStartup(e);
        }
    }
}
