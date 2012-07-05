using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Fantasy.Jobs.Scheduling
{
    [ServiceContract(Namespace=Consts.ScheduleNamespaceURI)]
    public interface IScheduleLibraryService
    {
        [OperationContract] 
        string[] GetGroups(string path);

        [OperationContract] 
        void CreateGroup(string path);

        [OperationContract] 
        void DeleteGroup(string path);

        [OperationContract] 
        void CreateSchedule(string path, ScheduleItem schedule, bool overwrite);

        [OperationContract] 
        void DeleteSchedule(string path);

        [OperationContract] 
        string[] GetScheduleNames(string path);

        [OperationContract] 
        ScheduleItem GetSchedule(string path);

        [OperationContract] 
        ScheduleEvent[] GetScheduleHistory(string path);

        [OperationContract]
        string[] GetTemplateNames();

        [OperationContract]
        string GetTemplate(string name);

    }
}
