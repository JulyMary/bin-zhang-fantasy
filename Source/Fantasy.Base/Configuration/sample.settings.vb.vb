'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.1
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Namespace Fantasy.Configuration
    Partial Friend NotInheritable Class sample
        Inherits Fantasy.Configuration.SettingsBase

        Private Shared _default As sample = CType(Fantasy.Configuration.SettingStorage.Load(New sample()), sample)
        Public Shared ReadOnly Property [Default]
            Get
                Return _default
            End Get
        End Property

        <System.Configuration.DefaultSettingValueAttribute("%ALLUSERSPROFILE%\Application Data\Fantasy\JobService\Template")> _
        Public Property JobTemplateDirectory() As System.String
            Get

                Return CType(this.GetValue("JobTemplateDirectory"), System.String)
            End Get
            Set(value As System.String)

                this.SetValue("JobTemplateDirectory", value)
            End Set
        End Property

        <System.Configuration.DefaultSettingValueAttribute("%ALLUSERSPROFILE%\Application Data\Fantasy\JobService\Job")> _
        Public Property JobDirectory() As System.String
            Get

                Return CType(this.GetValue("JobDirectory"), System.String)
            End Get
            Set(value As System.String)

                this.SetValue("JobDirectory", value)
            End Set
        End Property

        <System.Configuration.DefaultSettingValueAttribute("%ALLUSERSPROFILE%\Application Data\Fantasy\JobService\Log")> _
        Public Property LogDirectory() As System.String
            Get

                Return CType(this.GetValue("LogDirectory"), System.String)
            End Get
            Set(value As System.String)

                this.SetValue("LogDirectory", value)
            End Set
        End Property

        <System.Configuration.DefaultSettingValueAttribute("6")> _
        Public Property JobProcessCount() As System.Int32
            Get

                Return CType(this.GetValue("JobProcessCount"), System.Int32)
            End Get
            Set(value As System.Int32)

                this.SetValue("JobProcessCount", value)
            End Set
        End Property

        <System.Configuration.DefaultSettingValueAttribute(".\Fantasy.Jobs.JobHost.exe")> _
        Public Property JobHostPath() As System.String
            Get

                Return CType(this.GetValue("JobHostPath"), System.String)
            End Get
            Set(value As System.String)

                this.SetValue("JobHostPath", value)
            End Set
        End Property

        Public Property StartupFolders() As System.String
            Get

                Return CType(this.GetValue("StartupFolders"), System.String)
            End Get
            Set(value As System.String)

                this.SetValue("StartupFolders", value)
            End Set
        End Property

        <System.Configuration.DefaultSettingValueAttribute("%ALLUSERSPROFILE%\Application Data\Schedule")> _
        Public Property ScheduleDirectory() As System.String
            Get

                Return CType(this.GetValue("ScheduleDirectory"), System.String)
            End Get
            Set(value As System.String)

                this.SetValue("ScheduleDirectory", value)
            End Set
        End Property

        <System.Configuration.DefaultSettingValueAttribute("%ALLUSERSPROFILE%\Application Data\ScheduleTemplate")> _
        Public Property ScheduleTemplateDirectory() As System.String
            Get

                Return CType(this.GetValue("ScheduleTemplateDirectory"), System.String)
            End Get
            Set(value As System.String)

                this.SetValue("ScheduleTemplateDirectory", value)
            End Set
        End Property

        <System.Configuration.DefaultSettingValueAttribute("<?xml version=""1.0"" encoding=""utf-16""?>" _
      & vbCrLf & "<ArrayOfString xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">" _
      & vbCrLf & "  <string>asdf</string>" _
      & vbCrLf & "  <string>asdf</string>" _
      & vbCrLf & "  <string>asdf</string>" _
      & vbCrLf & "  <string>asdf</string>" _
      & vbCrLf & "  <string>asdf</string>" _
      & vbCrLf & "</ArrayOfString>")> _
        Public ReadOnly Property strings() As System.Collections.Specialized.StringCollection
            Get

                Return CType(this.GetValue("strings"), System.Collections.Specialized.StringCollection)
            End Get
        End Property


    End Class
End Namespace


