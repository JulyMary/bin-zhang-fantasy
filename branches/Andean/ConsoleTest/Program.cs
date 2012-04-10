using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;
using System.Xml;
using System.Xaml;
using System.Diagnostics;
using System.IO;
using Fantasy.IO;
using Fantasy.ServiceModel;
using Fantasy.BusinessEngine.Services;
using Fantasy;
using Fantasy.Testing;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Prepare();
            Do();
            Cleanup();

        }

        private static void Cleanup()
        {
            ServiceManager.Services.UninitializeServices();
        }

        private static void Do()
        {
            IEntityService es = ServiceManager.Services.GetRequiredService<IEntityService>();
            es.BeginUpdate();
            try
            {
                Department department = es.CreateEntity<Department>();
                
               
               
                
                department.Name = "Development";

                Person leader = es.CreateEntity<Person>();
               
                leader.Name = "Bush";
                leader.Age = 30;
                department.Staffs.Add(leader);
                leader.Department = department;
                department.Leader = leader;
               
                
                es.SaveOrUpdate(department);
                es.SaveOrUpdate(leader);
                
                
            }
            finally
            {
                es.EndUpdate(true);
            }
        }

        private static void Prepare()
        {
            string dir = LongPath.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().CodeBase);

            DefaultAddInTree.Initialize(LongPathDirectory.EnumerateAllFiles(dir, "*.addin.xaml"));

            IEnumerable<object> services = AddInTree.Tree.GetTreeNode("fantasy/services").BuildChildItems<object>(null, ServiceManager.Services);
            ServiceManager.Services.InitializeServices(services.ToArray());

            foreach (ICommand command in AddInTree.Tree.GetTreeNode("fantasy/startup").BuildChildItems<ICommand>(null, ServiceManager.Services))
            {
                command.Execute(null);
            }
        }
    }
}
