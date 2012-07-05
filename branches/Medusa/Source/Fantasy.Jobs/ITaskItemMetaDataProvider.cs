using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using Fantasy.IO;

namespace Fantasy.Jobs
{
    public interface ITaskItemMetaDataProvider
    {
        string[] GetNames(TaskItem item);

        string GetData(TaskItem item, string name);
    }


    public class NameAndCategoryMetaDataProvider : ITaskItemMetaDataProvider
    {

        private static readonly string[] Names = new string[] { "Name", "Category" };
        #region ITaskItemMetaDataProvider Members

        public string[] GetNames(TaskItem item)
        {
            return Names;
        }

        public string GetData(TaskItem item, string name)
        {
            switch (name.ToLower())
            {
                case "name":
                    return item.Name;
                case "category":
                    return item.Category;
                default:
                    throw new Exception("Absurd!");
            }
        }

        #endregion
    }

    public class FileInfoMetaDataProvider : ITaskItemMetaDataProvider
    {

        private static readonly string[] Names = new string[] { "Directory", "Exists", "Extension", "FullName", "FileName", "NameWithoutExtension"
            /*"CreationTime", "IsReadOnly", "LastAccessTime", "LastWriteTime", "Length", */ };

        #region ITaskItemMetaDataProvider Members

        public string[] GetNames(TaskItem item)
        {
            return FileInfoMetaDataProvider.Names;
        }

        public string GetData(TaskItem item, string name)
        {
            Uri dir = new Uri(JobEngine.CurrentEngine.JobDirectory + "\\");
            Uri full = new Uri(dir, item.Name);
            string path = full.IsFile ? full.LocalPath : full.ToString();
            switch (name.ToLower())
            {
                case "namewithoutextension" :
                    return LongPath.GetFileNameWithoutExtension(path).ToLower(); 
                case "filename":
                    return LongPath.GetFileName(path); 
                case "exists":
                    return LongPathFile.Exists(path).ToString();
                case "extension":
                    return LongPath.GetExtension(path);
                case "fullname":
                    return path;
                case "directory":
                    return LongPath.GetDirectoryName(path);
                default:
                    throw new NotSupportedException();
            }
        }

        #endregion
    }
}
