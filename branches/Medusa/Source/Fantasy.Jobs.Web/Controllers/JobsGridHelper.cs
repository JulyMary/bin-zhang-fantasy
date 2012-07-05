using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Web.Mvc.Infrastructure;
using Fantasy.Jobs.Management;
using System.Collections;
using Telerik.Web.Mvc;
using System.Text;
using System.ComponentModel;

namespace Fantasy.Jobs.Web.Controllers
{
    public static class JobsGridHelper
    {
        public static string GetOrder(GridCommand command)
        {
            StringBuilder rs = new StringBuilder();

            foreach (GroupDescriptor gd in command.GroupDescriptors)
            {
                if (rs.Length > 0)
                {
                    rs.Append(", ");
                }
                rs.AppendFormat("job.{0} {1}", gd.Member, gd.SortDirection == ListSortDirection.Descending ? "descending" : String.Empty);
            }

            foreach (SortDescriptor sd in command.SortDescriptors)
            {
                if (rs.Length > 0)
                {
                    rs.Append(", ");
                }
                rs.AppendFormat("job.{0} {1}", sd.Member, sd.SortDirection == ListSortDirection.Descending ? "descending" : String.Empty);
            }

            if (rs.Length == 0)
            {
                rs.Append("job.CreationTime  descending");
            }

            return rs.ToString();

        }


        private static string EncodeCSString(string text)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in text)
            {
                switch (c)
                {
                    case '\a':
                        sb.Append("\\a");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    case '\v':
                        sb.Append("\\v");
                        break;
                    case '\'':
                        sb.Append("\\\'");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
            }
            return sb.ToString();
                       
        }

        public static IEnumerable<FilterDescriptor> GetFilterDescriptors(IEnumerable<IFilterDescriptor> descriptors)
        {
            foreach (object o in descriptors)
            {
                if (o is CompositeFilterDescriptor)
                {
                    CompositeFilterDescriptor cf = (CompositeFilterDescriptor)o;
                    foreach (FilterDescriptor fd in GetFilterDescriptors(cf.FilterDescriptors))
                    {
                        yield return fd;
                    }
                }
                else if (o is FilterDescriptor)
                {
                    yield return (FilterDescriptor)o;
                }
            }
        }

        public static void GetLinqFilter(IList<IFilterDescriptor> filterDescriptors, out string filter, out string[] args)
        {
            StringBuilder rs = new StringBuilder();
            List<string> list = new List<string>();

            foreach (FilterDescriptor fd in GetFilterDescriptors(filterDescriptors))
            {
                if (rs.Length > 0)
                {
                    rs.Append(" && ");
                }
                switch (fd.Member)
                {
                    case "State":
                        rs.AppendFormat("job.State == {0}", fd.Value);
                        break;
                    case "Template":
                        rs.AppendFormat("job.Template.ToLower() == \"{0}\"", EncodeCSString((string)fd.Value).ToLower());
                        break;
                    case "Name":
                        rs.AppendFormat("job.Name.ToLower().Contains(\"{0}\")", EncodeCSString((string)fd.Value).ToLower());
                        break;
                    case "Application":
                        rs.AppendFormat("job.Application.ToLower() == \"{0}\"", EncodeCSString((string)fd.Value).ToLower());
                        break;
                    case "User":
                        rs.AppendFormat("job.User.ToLower() == \"{0}\"", EncodeCSString((string)fd.Value).ToLower());
                        break;
                    case "Priority":
                        rs.AppendFormat("job.Priority == {0}", fd.Value);
                        break;
                    case "CreationTime":
                        if (fd.Operator == FilterOperator.IsGreaterThanOrEqualTo)
                        {
                            rs.AppendFormat("job.CreationTime >= _{0}",list.Count); 
                            list.Add(String.Format("DateTime.Parse(\"{0}\")", fd.Value));
                        }
                        else if (fd.Operator == FilterOperator.IsLessThanOrEqualTo)
                        {
                            DateTime value = ((DateTime)fd.Value).AddDays(1);
                            rs.AppendFormat("job.CreationTime < _{0}", list.Count);
                            list.Add(String.Format("DateTime.Parse(\"{0}\")", value));
                        }
                        break;
                }
            }
            filter = rs.ToString();
            args = list.ToArray();
        }

        public static IEnumerable ApplyGrouping(IQueryable<JobMetaData> data, IList<GroupDescriptor> groupDescriptors)
        {
            Func<IEnumerable<JobMetaData>, IEnumerable<AggregateFunctionsGroup>> selector = null;
            foreach (var group in groupDescriptors.Reverse())
            {
                if (selector == null)
                {
                    if (group.Member == "State")
                    {
                        selector = jobs => BuildInnerGroup(jobs, o => JobState.ToString(o.State));
                    }
                    else if (group.Member == "Template")
                    {
                        selector = jobs => BuildInnerGroup(jobs, o => o.Template);
                    }
                    else if (group.Member == "Application")
                    {
                        selector = jobs => BuildInnerGroup(jobs, o => o.Application);
                    }
                    else if (group.Member == "User")
                    {
                        selector = jobs => BuildInnerGroup(jobs, o => o.User);
                    }
                    else if (group.Member == "Priority")
                    {
                        selector = jobs => BuildInnerGroup(jobs, o => o.Priority);
                    }
                    else if (group.Member == "CreationTime")
                    {
                        selector = jobs => BuildInnerGroup(jobs, o => o.CreationTime.ToShortDateString());
                    }
                    else if (group.Member == "StartTime")
                    {
                        selector = jobs => BuildInnerGroup(jobs, o =>o.StartTime != null ? o.StartTime.Value.ToShortDateString() : "Unstarted");
                    }
                    else if (group.Member == "EndTime")
                    {
                        selector = jobs => BuildInnerGroup(jobs, o => o.EndTime.Value.ToShortDateString());
                    }
                }
                else
                {
                    if (group.Member == "State")
                    {
                        selector = BuildGroup(o => JobState.ToString(o.State), selector);
                    }
                    else if (group.Member == "Template")
                    {
                        selector = BuildGroup(o => o.Template, selector);
                    }
                    else if (group.Member == "Application")
                    {
                        selector = BuildGroup(o => o.Application, selector);
                    }
                    else if (group.Member == "User")
                    {
                        selector = BuildGroup(o => o.User, selector);
                    }
                    else if (group.Member == "Priority")
                    {
                        selector = BuildGroup(o => o.Priority, selector);
                    }
                    else if (group.Member == "CreationTime")
                    {
                        selector = BuildGroup(o => o.CreationTime.ToShortDateString(), selector);
                    }
                    else if (group.Member == "StartTime")
                    {
                        selector = BuildGroup(o=> o.StartTime != null ? o.StartTime.Value.ToShortDateString() : "Unstarted", selector);
                    }
                    else if (group.Member == "EndTime")
                    {
                        selector = BuildGroup(o => o.EndTime.Value.ToShortDateString(), selector);
                    }
                }
            }
            return selector.Invoke(data).ToList();
        }

        private static Func<IEnumerable<JobMetaData>, IEnumerable<AggregateFunctionsGroup>> BuildGroup<T>(Func<JobMetaData, T> groupSelector, Func<IEnumerable<JobMetaData>, IEnumerable<AggregateFunctionsGroup>> selectorBuilder)
        {
            var tempSelector = selectorBuilder;

            return g => g.GroupBy(groupSelector)
                         .Select(c => new AggregateFunctionsGroup
                         {
                             Key = c.Key,
                             HasSubgroups = true,
                             Items = tempSelector.Invoke(c).ToList()
                         });
        }

        private static IEnumerable<AggregateFunctionsGroup> BuildInnerGroup<T>(IEnumerable<JobMetaData> group, Func<JobMetaData, T> groupSelector)
        {
            return group.GroupBy(groupSelector)
                    .Select(i => new AggregateFunctionsGroup
                    {
                        Key = i.Key,
                        Items = i.ToList()
                    });
        }
    }
}