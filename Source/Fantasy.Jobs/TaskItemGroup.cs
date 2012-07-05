using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Fantasy.XSerialization;
using System.Xml.Linq;
namespace Fantasy.Jobs
{
    [XSerializable("items",NamespaceUri=Consts.XNamespaceURI)] 
    [Serializable] 
    public class TaskItemGroup : IConditionalObject, IEnumerable<TaskItem>, ICloneable 
    {
        [XArray(Serializer=typeof(ItemsSerializer))] 
        private List<TaskItem> _list = new List<TaskItem>();

        public TaskItem[] ToArray()
        {
            return this._list.ToArray();
        }

        public int Count
        {
            get { return this._list.Count; }
        }

        public TaskItem AddNewItem(string name, string category)
        {
            TaskItem rs = new TaskItem() { Name = name, Category = category };
            this._list.Add(rs);
            return rs;
        }

        public void Clear()
        {
            this._list.Clear();
        }

        public object Clone()
        {
            TaskItemGroup rs = new TaskItemGroup();
            rs.Condition = this.Condition;
            foreach (TaskItem item in this._list)
            {
                rs._list.Add((TaskItem)item.Clone());
            }
            return rs;
        }

        public void RemoveItem(TaskItem itemToRemove)
        {
            this._list.Remove(itemToRemove);
        }

        public void RemoveAt(int index)
        {
            this._list.RemoveAt(index); 
        }

        public int IndexOf(TaskItem item)
        {
            return _list.IndexOf(item); 
        }

        [XAttribute("condition")] 
        public string Condition { get; set; }

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion


        class ItemsSerializer : IXCollectionSerializer
        {
            #region IXCollectionSerializer Members

            public void Save(IServiceProvider context, XElement element, IEnumerable collection)
            {

                foreach (TaskItem item in collection)
                {
                    XElement childElement = new XElement(element.Name.Namespace + item.Category);
                    element.Add(childElement);
                    item.Save(context, childElement);
                }
            }

            public IEnumerable Load(IServiceProvider context, XElement element)
            {
                ArrayList rs = new ArrayList();
                foreach (XElement childElement in element.Elements())
                {
                    TaskItem item = new TaskItem();
                    item.Load(context, childElement);
                    rs.Add(item);
                }

                return rs;
            }

            #endregion
        }


        #region IEnumerable<TaskItem> Members

        public IEnumerator<TaskItem> GetEnumerator()
        {
            return this._list.GetEnumerator();
        }

        #endregion
    }

    

    
}
