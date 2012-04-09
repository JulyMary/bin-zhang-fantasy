using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Fantasy.IO;
using System.Text.RegularExpressions;
using Fantasy.Web.Mvc.Properties;
using Fantasy.ServiceModel;

namespace Fantasy.Web.Mvc
{
    public class WatchDependencyPathCommand : ICommand
    {


        static List<FileSystemWatcher> _dependencyWatchers = new List<FileSystemWatcher>();

        static void WatchDependencyPaths()
        {
            string root = LongPath.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            Regex regex = new Regex(@"(?<dir>.*)\\?(?<filter>[^\\]*)$", RegexOptions.RightToLeft);

            foreach (string path in Settings.Default.DependencyPath.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                Match match = regex.Match(path);

                string fullPath = LongPath.Combine(root, match.Groups["dir"].Value);
                string filter = match.Groups["filter"].Value;
                FileSystemWatcher watcher = new FileSystemWatcher(fullPath, filter);
                watcher.Changed += new FileSystemEventHandler(DependencyFileChanged);
                watcher.Created += new FileSystemEventHandler(DependencyFileChanged);
                watcher.Deleted += new FileSystemEventHandler(DependencyFileChanged);
                watcher.EnableRaisingEvents = true;
                _dependencyWatchers.Add(watcher);

            }
        }


        private static bool _unloaded = false;
        static void DependencyFileChanged(object sender, FileSystemEventArgs e)
        {
            if (!_unloaded)
            {
                _unloaded = true;
                HttpRuntime.UnloadAppDomain();
            }
        }

        #region ICommand Members

        public object Execute(object args)
        {
            WatchDependencyPaths();
            return null;
        }

        #endregion
    }
}