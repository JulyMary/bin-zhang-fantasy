using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Fantasy.Jobs.Management
{
    [ServiceContract(Namespace = Consts.JobServiceNamespaceURI)]
    public interface IJobService
    {

        [OperationContract]
        [WebGet]
        string Version();

        [OperationContract]
        [WebGet]
        JobMetaData StartJob(string startInfo);

        [OperationContract(Name="ResumeById")]
        
        void Resume(Guid id);

        [OperationContract(Name = "CancelById")]
        
        void Cancel(Guid id);

        [OperationContract(Name = "PauseById")]
        
        void Pause(Guid id);

        [OperationContract(Name="ResumeByIds")]
       
        void Resume(Guid[] ids);

        [OperationContract(Name="CancelByIds")]
       
        void Cancel(Guid[] ids);

        [OperationContract(Name="PauseByIds")]
        void Pause(Guid[] ids);

        [OperationContract]
        [WebInvoke(RequestFormat=WebMessageFormat.Xml, BodyStyle=WebMessageBodyStyle.WrappedRequest)] 
        void ResumeByFilter(string filter, string[] args = null);

        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.WrappedRequest)] 
        void CancelByFilter(string filter, string[] args = null);

        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.WrappedRequest)] 
        void PauseByFilter(string filter, string[] args = null);

        [OperationContract] 
        JobMetaData FindJobById(Guid id);

        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Xml)] 
        JobMetaData[] FindUnterminatedJob(out int totalCount, string filter = "", string[] args = null, string order = "", int skip = 0, int take = Int32.MaxValue);

        [OperationContract]
        [WebInvoke(RequestFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat=WebMessageFormat.Xml)] 
        JobMetaData[] FindTerminatedJob(out int totalCount, string filter = "", string[] args = null, string order = "", int skip = 0, int take = Int32.MaxValue);

        [OperationContract] 
        int GetTerminatedCount();

        [OperationContract]
        int GetUnterminatedCount();

        [OperationContract] 
        string GetJobLog(Guid id);

        [OperationContract]
        string GetManagerLog(DateTime date);

        [OperationContract]
        DateTime[] GetManagerLogAvaiableDates();


        [OperationContract]
        JobTemplate[] GetJobTemplates();

        [OperationContract]
        string GetJobScript(Guid id);


        [OperationContract]
        string[] GetAllApplications();

        [OperationContract]
        string[] GetAllUsers();


        [OperationContract]
        string GetSettings(string typeName);

        [OperationContract]
        [WebInvoke(BodyStyle=WebMessageBodyStyle.WrappedRequest)] 
        void SetSettings(string typeName, string xml);

        [OperationContract]
        string GetLocation();
 
    }


    public static class IJobServiceExtension
    {

        private static string NormalizeTypeName(Type t)
        {
            string rs = string.Format("{0}, {1}", t.FullName, t.Assembly.GetName().Name);
            return rs;
        }

        public static string GetSettings<T>(this IJobService service)
        {
            return service.GetSettings(NormalizeTypeName(typeof(T)));
        }

        public static void SetSettings<T>(this IJobService service, string xml)
        {
            service.SetSettings(NormalizeTypeName(typeof(T)), xml); 
        }
    }

}
