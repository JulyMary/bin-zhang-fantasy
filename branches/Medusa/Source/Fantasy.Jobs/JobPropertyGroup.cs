using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Fantasy.XSerialization;
using System.Xml;

namespace Fantasy.Jobs
{
    [XSerializable("properties", NamespaceUri=Consts.XNamespaceURI)] 
    [Serializable]
    public class JobPropertyGroup :  IConditionalObject, IEnumerable, ICloneable
    {
        [XArray(Serializer = typeof(JobPropertiesSerializer))]
        private List<JobProperty> _list = new List<JobProperty>();

        public JobProperty[] ToArray()
        {
            return this._list.ToArray();
        }

        public int Count
        {
            get { return this._list.Count; }
        }

        public JobProperty AddNewItem(string name)
        {
            JobProperty rs = new JobProperty() { Name = name };
            this._list.Add(rs);
            return rs;
        }

        public void Clear()
        {
            this._list.Clear();
        }

        public object Clone()
        {
            JobPropertyGroup rs = new JobPropertyGroup();
            rs.Condition = this.Condition;
            foreach (JobProperty prop in this._list)
            {
                this._list.Add((JobProperty)prop.Clone());
            }

            return rs;
        }

        public void RemoveItem(JobProperty itemToRemove)
        {
            this._list.Remove(itemToRemove);
        }

        public void RemoveAt(int index)
        {
            this._list.RemoveAt(index);
        }

        [XAttribute("condition")] 
        public string Condition { get; set; }

        public IEnumerator GetEnumerator()
        {
            return this._list.GetEnumerator();
        }
       
    }
}
