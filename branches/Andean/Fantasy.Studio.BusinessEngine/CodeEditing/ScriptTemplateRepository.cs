﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using Fantasy.Studio.BusinessEngine.Properties;
using Fantasy.IO;

namespace Fantasy.Studio.BusinessEngine.CodeEditing
{
    public class ScriptTemplateRepository : ServiceBase
    {

        public ScriptTemplateRepository()
        {
            this.Templates = new List<ScriptTemplate>();
        }

        public IList<ScriptTemplate> Templates { get; private set; }


        public override void InitializeService()
        {
            string dir = Settings.ExtractToFullPath(Settings.Default.ItemTemplatesPath);
            foreach (string file in LongPathDirectory.EnumerateAllFiles(dir, "*.zip"))
            {
                ScriptTemplate template = ScriptTemplate.LoadFrom(file);
                this.Templates.Add(template);
            }

            base.InitializeService();
        }


        
    }
}