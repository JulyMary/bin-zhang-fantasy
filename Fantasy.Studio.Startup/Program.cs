﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Fantasy.IO;
using Fantasy.AddIns;
using Fantasy.ServiceModel;
using Fantasy;

namespace FantasyDeveloper
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            string dir = LongPath.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().CodeBase);

            DefaultAddInTree.Initialize(LongPathDirectory.EnumerateAllFiles(dir, "*.addin.xaml"));

            IEnumerable<object> services = AddInTree.Tree.GetTreeNode("fantasy/services").BuildChildItems<object>(null, ServiceManager.Services);
            ServiceManager.Services.InitializeServices(services.ToArray());

            foreach (ICommand command in AddInTree.Tree.GetTreeNode("fantasy/startup").BuildChildItems<ICommand>(null, ServiceManager.Services))
            {
                command.Execute(null);
            }

            ServiceManager.Services.UninitializeServices();

        }
    }
}