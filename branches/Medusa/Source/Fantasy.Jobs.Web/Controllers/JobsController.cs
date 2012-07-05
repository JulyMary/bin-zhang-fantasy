using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using Fantasy.Jobs.Management;
using Fantasy.Jobs.Web.Models;
using System.Text;
using System.ComponentModel;
using System.Collections;
using Telerik.Web.Mvc.Infrastructure;
using System.Threading.Tasks;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs.Web.Controllers
{

    public class JobsController : Controller
    {
        //
        // GET: /TerminatedJobs/
        [GridAction(GridName = "JobList")]
        public ActionResult Terminated(GridCommand command)
        {

            int totalCount;
            IEnumerable data;
            data = GetData(true, command, out totalCount);

            JobsModel model = new JobsModel()
            {
                Data = data,
                TotalCount = totalCount,
                Command = command
            };
            return View(model);
        }

        [GridAction(GridName = "JobList")]
        public ActionResult Unterminated(GridCommand command)
        {
            int totalCount;
            IEnumerable data;
            data = GetData(false, command, out totalCount);

            JobsModel model = new JobsModel()
            {
                Data = data,
                TotalCount = totalCount,
                Command = command
            };
            return View(model);
        }

        private IEnumerable<JobStateModel> ToStateModel(IEnumerable<int> states)
        {
            foreach (int state in states)
            {
                yield return new JobStateModel() { State = state, Caption = JobState.ToString(state) };
            }
        }


        private object GetFilterValue(GridCommand command, string member, FilterOperator? op = null)
        {
            var query = from fd in JobsGridHelper.GetFilterDescriptors(command.FilterDescriptors) where fd.Member == member select fd;
            if (op != null)
            {
                query = from fd in query where fd.Operator == op select fd;
            }

            return query.Select(fd => ((FilterDescriptor)fd).Value).SingleOrDefault();
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return base.Json(data, contentType, contentEncoding, JsonRequestBehavior.AllowGet);
        }


        private Guid[] ToGuids(string ids)
        {
            if (ids != null)
            {
                List<Guid> rs = new List<Guid>();
                foreach (string id in ids.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    rs.Add(new Guid(id));
                }
                return rs.ToArray();
            }
            else
            {
                return new Guid[0];
            }

        }

        public JsonResult ResumeSelected(string ids)
        {
            using (ClientRef<IJobService> svc = ClientFactory.Create<IJobService>())
            {
                svc.Client.Resume(ToGuids(ids));
                return Json(true);
            }
        }

        public JsonResult StopSelected(string ids)
        {
            using (ClientRef<IJobService> svc = ClientFactory.Create<IJobService>())
            {
                svc.Client.Cancel(ToGuids(ids));
                return Json(true);
            }
        }

        public JsonResult PauseSelected(string ids)
        {
            using (ClientRef<IJobService> svc = ClientFactory.Create<IJobService>())
            {
                svc.Client.Pause(ToGuids(ids));
                return Json(true);
            }
        }


        private void GetLinqFilter(string gridfilter, out string filter, out string[] args)
        {
            IList<IFilterDescriptor> filters = FilterDescriptorFactory.Create(gridfilter ?? string.Empty);
            JobsGridHelper.GetLinqFilter(filters, out filter, out args);
        }

        public JsonResult ResumeAll(string filter)
        {
            string linqFilter;
            string[] args;
            GetLinqFilter(filter, out linqFilter, out args);
            Task task = Task.Factory.StartNew(() =>
            {

                using (ClientRef<IJobService> svc = ClientFactory.Create<IJobService>())
                {
                    svc.Client.ResumeByFilter(linqFilter, args);
                }
            });

            bool rs = task.Wait(30000);
            return Json(rs);

        }
        private const int _taskWaitTime = 30000;
        public JsonResult StopAll(string filter)
        {
            string linqFilter;
            string[] args;
            GetLinqFilter(filter, out linqFilter, out args);
            Task task = Task.Factory.StartNew(() =>
            {
                using (ClientRef<IJobService> svc = ClientFactory.Create<IJobService>())
                {
                    svc.Client.CancelByFilter(linqFilter, args);
                }
            });
            bool rs = task.Wait(30000);
            return Json(rs);
        }

        public JsonResult PauseAll(string filter)
        {
            string linqFilter;
            string[] args;
            GetLinqFilter(filter, out linqFilter, out args);
            GetLinqFilter(filter, out linqFilter, out args);
            Task task = Task.Factory.StartNew(() =>
            {
                using (ClientRef<IJobService> svc = ClientFactory.Create<IJobService>())
                {
                    svc.Client.PauseByFilter(linqFilter, args);
                }
            });
            bool rs = task.Wait(30000);
            return Json(rs);
        }

        [ChildActionOnly]
        public PartialViewResult Filter(GridCommand command, bool isTerminated)
        {
            using (ClientRef<IJobService> svc = ClientFactory.Create<IJobService>())
            {
                JobFilterModel model = new JobFilterModel();

                model.AdvancedOptionsVisible = StringComparer.OrdinalIgnoreCase.Compare(Request["ao"], "true") == 0;

                model.Name = (string)GetFilterValue(command, "Name");


                int[] states;

                if (isTerminated)
                {
                    states = new int[] 
                { 
                   JobState.All, JobState.Succeed, JobState.Failed, JobState.Cancelled 
                };
                }
                else
                {
                    states = new int[] 
                {
                   JobState.All, JobState.Unstarted, JobState.Running, JobState.Suspended,  JobState.UserPaused  
                };
                }

                model.States = ToStateModel(states).ToArray();



                double state = (double?)GetFilterValue(command, "State") ?? JobState.All;

                model.StateIndex = Array.IndexOf(states, (int)state);
                JobTemplate[] templates;

                templates = svc.Client.GetJobTemplates();

                var query = from t in templates where !string.IsNullOrEmpty(t.Name) orderby t.Name select t.Name;

                model.Templates = InsertString(query, "All");

                string template = (string)GetFilterValue(command, "Template");
                model.TemplateIndex = template != null ? Array.IndexOf(model.Templates, template) : 0;

                model.Applications = InsertString(svc.Client.GetAllApplications().OrderBy(s => s), "All");
                string application = (string)GetFilterValue(command, "Application");
                model.ApplicationIndex = application != null ? Array.IndexOf(model.Applications, application) : 0;

                model.Users = InsertString(svc.Client.GetAllUsers().OrderBy(s => s), "All");

                string user = (string)GetFilterValue(command, "User");
                model.UserIndex = user != null ? Array.IndexOf(model.Users, user) : 0;

                model.CreationTimeLow = (DateTime?)GetFilterValue(command, "CreationTime", FilterOperator.IsGreaterThanOrEqualTo);

                model.CreationTimeHigh = (DateTime?)GetFilterValue(command, "CreationTime", FilterOperator.IsLessThanOrEqualTo);

                return PartialView(model);
            }
        }

        private string[] InsertString(IEnumerable<string> source, string newValue)
        {
            List<string> rs = new List<string>(source);
            rs.Insert(0, newValue);
            return rs.ToArray();
        }

        private static IEnumerable GetData(bool isTerminated, GridCommand command, out int totalCount)
        {
            string filter;
            string[] args;
            JobsGridHelper.GetLinqFilter(command.FilterDescriptors, out filter, out args);

            string order = JobsGridHelper.GetOrder(command);
            int skip = command.PageSize > 0 ? (command.Page - 1) * command.PageSize : 0;
            int take = command.PageSize > 0 ? command.PageSize : 100;
            
            JobMetaData[] data;
            using (ClientRef<IJobService> svc = ClientFactory.Create<IJobService>())
            {
                if (isTerminated)
                {
                    data = svc.Client.FindTerminatedJob(out totalCount, filter, args, order, skip, take);
                }
                else
                {
                    data = svc.Client.FindUnterminatedJob(out totalCount, filter, args, order, skip, take);
                }
            }
            if (command.GroupDescriptors.Count > 0)
            {
                return JobsGridHelper.ApplyGrouping(data.AsQueryable(), command.GroupDescriptors);
            }
            else
            {
                return data;
            }

        }


    }
}
