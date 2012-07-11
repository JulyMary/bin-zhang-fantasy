using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.XSerialization;
using System.Text.RegularExpressions;

namespace Fantasy.Jobs
{
    [Instruction]
    [XSerializable("foreach", NamespaceUri = Consts.XNamespaceURI)]
    internal class Foreach : Sequence 
    {
        public override void Execute()
        {
            IJob job = this.Site.GetRequiredService<IJob>();
            IStringParser parser = this.Site.GetRequiredService<IStringParser>();
            Dictionary<string, object> context = new Dictionary<string, object>() 
            { 
              { "EnableTaskItemReader", false }
            };
            string s = parser.Parse(this.In, context);
            string[] strings = s != null ? s.Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries) : new string[0];
            List<object> values = new List<object>();
            Regex reg = new Regex(@"^@\((?<cate>[\w]+)\)$");

            foreach (string str in strings)
            {
                Match match = reg.Match(str);
                if (match.Success)
                {
                    string cat = match.Groups["cate"].Value;
                    values.AddRange(job.GetEvaluatedItemsByCatetory(cat)); 
                }
                else
                {
                    values.Add(str);
                }
            }

            int index = job.RuntimeStatus.Local.GetValue("foreach.index", 0);
            while (index < values.Count)
            {
                object value = values[index];
                if ( ! (value is string) || (!string.IsNullOrWhiteSpace((string)value) || !_skipEmptyString))
                {
                    job.RuntimeStatus.Local[Var] = value;

                    this.ExecuteSequence(); 
                    this.ResetSequenceIndex();
                }
                index++;
                job.RuntimeStatus.Local["foreach.index"] = index;
            }
           

        }


        [XAttribute("var")] 
        public string Var { get; set; }

        [XAttribute("in")]
        public string In { get; set; }

        private bool _skipEmptyString = true;

        [XAttribute("skipEmptyString")]
        public bool SkipEmptyString
        {
            get { return _skipEmptyString; }
            set { _skipEmptyString = value; }
        }

        
    }
}
