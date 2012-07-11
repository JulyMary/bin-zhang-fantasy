using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs
{
    public class ItemParseService : AbstractService, IItemParser
    {
        public TaskItem[] GetItemByNames(string names)
        {
            IJob job = (IJob)this.Site.GetService(typeof(IJob));
            List<TaskItem> rs = new List<TaskItem>();
            foreach (string name in names.Split(';'))
            {
                if (!string.IsNullOrEmpty(name))
                {
                    TaskItem item = job.GetEvaluatedItemByName(name);
                    if (item == null)
                    {
                        item = new TaskItem() { Name = name };

                    }
                    rs.Add(item);
                }
            }

            return rs.ToArray();
        }

        public TaskItem[] ParseItem(string text)
        {
            string str = this.ParseStringWithoutItems(text);
            Regex reg = new Regex(@"(?<sym>(#|@|%))\((?<cate>[\w]+)\)");
            List<TaskItem> rs = new List<TaskItem>();
            if (!string.IsNullOrEmpty(str))
            {
                int s = 0;
                while (s < str.Length)
                {
                    Match m = reg.Match(str, s);
                    if (m.Success)
                    {
                        string itemNames = str.Substring(s, m.Index - s);
                        if (!string.IsNullOrEmpty(itemNames))
                        {
                            rs.AddRange(this.GetItemByNames(itemNames));
                        }
                        string sym = m.Groups["sym"].Value;
                        if (sym == "@" || sym == "%")
                        {
                            string[] names = m.Groups["cate"].Value.Split(new char[] { '.' }, 2);
                            if (names.Length == 1)
                            {
                                TaskItem[] items = this.GetItemByCategory(m.Groups["cate"].Value);
                                if (items.Length > 0)
                                {
                                    if (sym == "@")
                                    {
                                        rs.AddRange(items);
                                    }
                                    else
                                    {
                                        rs.Add(items.First());
                                    }
                                }
                            }
                            else
                            {
                                IStringParser parser = this.Site.GetRequiredService<IStringParser>();
                                string meta = parser.Parse(m.Value);
                                rs.AddRange(this.GetItemByNames(meta));
                            }

                        }
                        else
                        {
                            string[] names = m.Groups["cate"].Value.Split(new char[] { '.' }, 2);
                            IJob job = this.Site.GetRequiredService<IJob>();

                            object var;
                            if (job.RuntimeStatus.TryGetValue(names[0], out var))
                            {
                                if (var is TaskItem)
                                {
                                    TaskItem item = (TaskItem)var;
                                    if (item != null)
                                    {
                                        if (names.Length > 1)
                                        {
                                            rs.AddRange(this.GetItemByNames(item[names[1]]));
                                        }
                                        else
                                        {
                                            rs.Add(item);
                                        }
                                    }
                                }
                                else if (var is string)
                                {
                                    rs.AddRange(this.GetItemByNames((string)var));
                                }
                            }
                        }
                        s = m.Index + m.Length;
                    }
                    else
                    {
                        string itemNames = str.Substring(s);
                        if (!string.IsNullOrEmpty(itemNames))
                        {
                            rs.AddRange(this.GetItemByNames(itemNames));
                        }
                        s = str.Length;
                    }
                }
            }

            return rs.ToArray();
        }

        public TaskItem[] GetItemByCategory(string category)
        {
            IJob job = (IJob)this.Site.GetService(typeof(IJob));
            TaskItem[] items = job.GetEvaluatedItemsByCatetory(category);
            return items;
        }

        private string ParseStringWithoutItems(string value)
        {
            Dictionary<string, object> context = new Dictionary<string, object>() 
            { 
              { "EnableTaskItemReader", false }
            };

            IStringParser parser = (IStringParser)this.Site.GetService(typeof(IStringParser));
            return parser.Parse(value, context);
        }

        
    }
}
