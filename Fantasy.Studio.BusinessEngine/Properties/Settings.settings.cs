 
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
namespace Fantasy.Studio.BusinessEngine.Properties
{
    public partial class Settings : Fantasy.Configuration.SettingsBase
	{
	    private static Settings _default = (Settings)Fantasy.Configuration.SettingStorage.Load(new Settings());
		
		public static Settings Default
		{
		    get
			{
			    return _default;
			}
		}

	    [System.Configuration.UserScopedSetting]
		        
        [System.Configuration.DefaultSettingValueAttribute(@"<GridViewLayoutSetting/>")]
        public Fantasy.Studio.Controls.GridViewLayoutSetting PropertyEditorGridViewLayout
		{
		    get
			{
			    return (Fantasy.Studio.Controls.GridViewLayoutSetting)this.GetValue("PropertyEditorGridViewLayout");
			}
			set
			{
			    this.SetValue("PropertyEditorGridViewLayout", value);
			}
		}

	    [System.Configuration.UserScopedSetting]
		        
        [System.Configuration.DefaultSettingValueAttribute(@"<GridViewLayoutSetting/>")]
        public Fantasy.Studio.Controls.GridViewLayoutSetting EnumValuesPanelGridViewLayout
		{
		    get
			{
			    return (Fantasy.Studio.Controls.GridViewLayoutSetting)this.GetValue("EnumValuesPanelGridViewLayout");
			}
			set
			{
			    this.SetValue("EnumValuesPanelGridViewLayout", value);
			}
		}

	    [System.Configuration.UserScopedSetting]
		        
        [System.Configuration.DefaultSettingValueAttribute(@"<GridViewLayoutSetting/>")]
        public Fantasy.Studio.Controls.GridViewLayoutSetting AddGACReferenceDialogGridViewLayout
		{
		    get
			{
			    return (Fantasy.Studio.Controls.GridViewLayoutSetting)this.GetValue("AddGACReferenceDialogGridViewLayout");
			}
			set
			{
			    this.SetValue("AddGACReferenceDialogGridViewLayout", value);
			}
		}

	    [System.Configuration.UserScopedSetting]
		        
        [System.Configuration.DefaultSettingValueAttribute(@"<GridViewLayoutSetting/>")]
        public Fantasy.Studio.Controls.GridViewLayoutSetting ObjectSecurityPropertyGridViewLayout
		{
		    get
			{
			    return (Fantasy.Studio.Controls.GridViewLayoutSetting)this.GetValue("ObjectSecurityPropertyGridViewLayout");
			}
			set
			{
			    this.SetValue("ObjectSecurityPropertyGridViewLayout", value);
			}
		}

		[System.Configuration.ApplicationScopedSetting]
            
        [System.Configuration.DefaultSettingValueAttribute(@"dbo")]
        public System.String DefaultTableSchema
		{
		    get
			{
			    return (System.String)this.GetValue("DefaultTableSchema");
			}
		}

		[System.Configuration.ApplicationScopedSetting]
            
        [System.Configuration.DefaultSettingValueAttribute(@"CLS")]
        public System.String DefaultClassTablePrefix
		{
		    get
			{
			    return (System.String)this.GetValue("DefaultClassTablePrefix");
			}
		}

		[System.Configuration.ApplicationScopedSetting]
            
        [System.Configuration.DefaultSettingValueAttribute(@"ASSN")]
        public System.String DefaultAssociationTablePrefix
		{
		    get
			{
			    return (System.String)this.GetValue("DefaultAssociationTablePrefix");
			}
		}

		[System.Configuration.ApplicationScopedSetting]
            public System.String DefaultTableSpace
		{
		    get
			{
			    return (System.String)this.GetValue("DefaultTableSpace");
			}
		}

	    [System.Configuration.UserScopedSetting]
		        
        [System.Configuration.DefaultSettingValueAttribute(@"300")]
        public System.Double DefaultParticipantACLPanelLeftColumnWidth
		{
		    get
			{
			    return (System.Double)this.GetValue("DefaultParticipantACLPanelLeftColumnWidth");
			}
			set
			{
			    this.SetValue("DefaultParticipantACLPanelLeftColumnWidth", value);
			}
		}

		[System.Configuration.ApplicationScopedSetting]
            
        [System.Configuration.DefaultSettingValueAttribute(@".\Templates\BusinessClass.Designer.tt")]
        public System.String BusinessClassAutoScriptT4Path
		{
		    get
			{
			    return (System.String)this.GetValue("BusinessClassAutoScriptT4Path");
			}
		}

		[System.Configuration.ApplicationScopedSetting]
            
        [System.Configuration.DefaultSettingValueAttribute(@".\Templates\NewBusinessClass.tt")]
        public System.String NewBusinessClassT4Path
		{
		    get
			{
			    return (System.String)this.GetValue("NewBusinessClassT4Path");
			}
		}

	    [System.Configuration.UserScopedSetting]
		        
        [System.Configuration.DefaultSettingValueAttribute(@"Consolas")]
        public System.String CodeEditorFontFamily
		{
		    get
			{
			    return (System.String)this.GetValue("CodeEditorFontFamily");
			}
			set
			{
			    this.SetValue("CodeEditorFontFamily", value);
			}
		}

	    [System.Configuration.UserScopedSetting]
		        
        [System.Configuration.DefaultSettingValueAttribute(@"11")]
        public System.Double CodeEditorFontSize
		{
		    get
			{
			    return (System.Double)this.GetValue("CodeEditorFontSize");
			}
			set
			{
			    this.SetValue("CodeEditorFontSize", value);
			}
		}

	    [System.Configuration.UserScopedSetting]
		        public System.String SolutionPath
		{
		    get
			{
			    return (System.String)this.GetValue("SolutionPath");
			}
			set
			{
			    this.SetValue("SolutionPath", value);
			}
		}

	    [System.Configuration.UserScopedSetting]
		        
        [System.Configuration.DefaultSettingValueAttribute(@"Fantasy.BusinessData")]
        public System.String BusinessDataAssemblyName
		{
		    get
			{
			    return (System.String)this.GetValue("BusinessDataAssemblyName");
			}
			set
			{
			    this.SetValue("BusinessDataAssemblyName", value);
			}
		}

	    [System.Configuration.UserScopedSetting]
		        
        [System.Configuration.DefaultSettingValueAttribute(@"Fantasy.Web")]
        public System.String WebAssemblyName
		{
		    get
			{
			    return (System.String)this.GetValue("WebAssemblyName");
			}
			set
			{
			    this.SetValue("WebAssemblyName", value);
			}
		}

		[System.Configuration.ApplicationScopedSetting]
            
        [System.Configuration.DefaultSettingValueAttribute(@".\Templates\ClassLibraryTemplate.csproj")]
        public System.String ClassLibraryTemplatePath
		{
		    get
			{
			    return (System.String)this.GetValue("ClassLibraryTemplatePath");
			}
		}

		[System.Configuration.ApplicationScopedSetting]
            
        [System.Configuration.DefaultSettingValueAttribute(@".\Templates\SolutionTemplate.tt")]
        public System.String SolutionTemplatePath
		{
		    get
			{
			    return (System.String)this.GetValue("SolutionTemplatePath");
			}
		}

		[System.Configuration.ApplicationScopedSetting]
            
        [System.Configuration.DefaultSettingValueAttribute(@"{4CDABEB6-2016-476B-8692-4ED84EBC606D}")]
        public System.String BusinessDataProjectId
		{
		    get
			{
			    return (System.String)this.GetValue("BusinessDataProjectId");
			}
		}

		[System.Configuration.ApplicationScopedSetting]
            
        [System.Configuration.DefaultSettingValueAttribute(@".\Templates\BusinessEnum.tt")]
        public System.String BusinessEnumT4Path
		{
		    get
			{
			    return (System.String)this.GetValue("BusinessEnumT4Path");
			}
		}

		[System.Configuration.ApplicationScopedSetting]
            
        [System.Configuration.DefaultSettingValueAttribute(@".\ItemTemplates")]
        public System.String ItemTemplatesPath
		{
		    get
			{
			    return (System.String)this.GetValue("ItemTemplatesPath");
			}
		}

		[System.Configuration.ApplicationScopedSetting]
            
        [System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfString xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <string>None</string>
  <string>Content</string>
  <string>EmbeddedResource</string>
  <string>CodeAnalysisDictionary</string>
  <string>ApplicationDefinition</string>
  <string>Page</string>
  <string>Resource</string>
  <string>SplashScreen</string>
  <string>DesignData</string>
  <string>DesignDataWithDesignTimeCreatableTypes</string>
  <string>EntityDeploy</string>
</ArrayOfString>")]
        public System.Collections.Specialized.StringCollection ScriptBuildActions
		{
		    get
			{
			    return (System.Collections.Specialized.StringCollection)this.GetValue("ScriptBuildActions");
			}
		}

		[System.Configuration.ApplicationScopedSetting]
            
        [System.Configuration.DefaultSettingValueAttribute(@".\Templates\NewBusinessApplication.tt")]
        public System.String NewBusinessApplicationT4Path
		{
		    get
			{
			    return (System.String)this.GetValue("NewBusinessApplicationT4Path");
			}
		}

		[System.Configuration.ApplicationScopedSetting]
            
        [System.Configuration.DefaultSettingValueAttribute(@".\Templates\NewBusinessUser.tt")]
        public System.String NewBusinessUserT4Path
		{
		    get
			{
			    return (System.String)this.GetValue("NewBusinessUserT4Path");
			}
		}

		[System.Configuration.ApplicationScopedSetting]
            
        [System.Configuration.DefaultSettingValueAttribute(@".\Templates\NewBusinessRole.tt")]
        public System.String NewBusinessRoleT4Path
		{
		    get
			{
			    return (System.String)this.GetValue("NewBusinessRoleT4Path");
			}
		}

	}
}
