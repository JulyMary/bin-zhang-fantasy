using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using Fantasy.Jobs.Properties;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs.Management
{
    public interface IJobTemplatesService
    {
        JobTemplate GetJobTemplateByName(string name);
        JobTemplate GetJobTemplateByPath(string path);

        JobTemplate[] GetJobTemplates();
    }

    public class JobTemplatesService : AbstractService, IJobTemplatesService
    {

        private ILogger _logger;
        public ILogger Logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = (ILogger)this.Site.GetService(typeof(ILogger));
                }
                return _logger;
            }


        }

        private List<JobTemplate> _templates = new List<JobTemplate>();
        private List<JobTemplate> _invalidTemplates = new List<JobTemplate>();

        private int _sequence = 0;

        public override void InitializeService()
        {
            DirectoryInfo dir = new DirectoryInfo(JobManagerSettings.Default.JobTemplateDirectoryFullPath);
            if (dir.Exists)
            {
                LoadTemplates(dir);
            }
            base.InitializeService();
        }

        private void LoadTemplates(DirectoryInfo dir)
        {
            foreach(FileInfo fi in dir.GetFiles("*.jt"))
            {
                LoadTemplate(fi, false);
            }
            foreach (DirectoryInfo sub in dir.GetDirectories())
            {
                LoadTemplates(sub);
            }
        }

        private void LoadTemplate(FileInfo fi, bool replace)
        {
            JobTemplate template = new JobTemplate();
            template.id = this._sequence++;
            template.Location = fi.FullName;
            template.Valid = false;
            try
            {
                

                XmlDocument doc = new XmlDocument();

                string text = File.ReadAllText(fi.FullName);
                template.Content = text;
                doc.LoadXml(text);
                template.Name = doc.DocumentElement.GetAttribute("name");
                
                if (!String.IsNullOrEmpty(template.Name))
                {
                    JobTemplate old = (from t in this._templates where (StringComparer.OrdinalIgnoreCase.Compare(t.Name, template.Name) == 0) select t).SingleOrDefault();
                    if(old != null)
                    {
                        if(replace)
                        {
                            this._templates.Remove(old);
                        }
                        else
                        {
                            throw new ApplicationException(String.Format(Properties.Resources.DulplicateTemplateNamesText, old.Location, fi.FullName, old.Name));
                        }
                    }
                    template.Valid = true;
                    this._templates.Add(template);
                    Logger.LogMessage(LogCategories.Manager, Properties.Resources.SuccessToLoadTemplateText, fi.FullName, template.Name);
                }
                else
                {
                    template.Valid = true;
                    this._templates.Add(template);
                    
                    Logger.LogMessage(LogCategories.Manager, Properties.Resources.SuccessToLoadAnonymousTemplateText, fi.FullName);  
                }

            }
            catch(Exception ex)
            {
                Logger.LogWarning(LogCategories.Manager, ex, MessageImportance.High, Properties.Resources.FailToLoadTemplateText, fi.FullName);  
            }
            if (!template.Valid)
            {
                this._invalidTemplates.Add(template);
            }
        }




        #region IJobTemplatesService Members

        public JobTemplate GetJobTemplateByName(string name)
        {
            try
            {

                return (from template in this._templates where StringComparer.OrdinalIgnoreCase.Compare(template.Name, name) == 0 select template).Single();
            }
            catch (InvalidOperationException)
            {
                throw new JobException(String.Format(Properties.Resources.CannotFindTemplateByNameText, name)); 
            }

        }

        public JobTemplate GetJobTemplateByPath(string path)
        {
            Uri uri = new Uri(path);
            try
            {
                return (from template in this._templates where (new Uri(template.Location)) == uri select template).Single(); 
            }
            catch (InvalidOperationException)
            {
                throw new JobException(String.Format(Properties.Resources.CannotFindTemplateByPathText, path));
            }
        }

        #endregion


        public JobTemplate[] GetJobTemplates()
        {
            List<JobTemplate> rs = new List<JobTemplate>();
            rs.AddRange(this._templates);
            rs.AddRange(this._invalidTemplates);
            return rs.ToArray();
        }
    }
}
