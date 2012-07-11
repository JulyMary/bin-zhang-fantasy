using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.XSerialization;
using System.Xml.Linq;
using System.Collections.Specialized;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs
{
    [Instruction]
    [XSerializable("items", NamespaceUri = Consts.XNamespaceURI)]  
    internal class CreateItems : AbstractInstruction, IConditionalObject, IXSerializable
    {

        private List<CreateItemsItem> _list = new List<CreateItemsItem>();

        public override void Execute()
        {
            IStringParser parser = this.Site.GetRequiredService<IStringParser>();
            IItemParser itemParser = this.Site.GetRequiredService<IItemParser>();
            ILogger logger = this.Site.GetService<ILogger>();
            IJob job = this.Site.GetRequiredService<IJob>();
            IConditionService conditionSvc = this.Site.GetRequiredService<IConditionService>();
            if (conditionSvc.Evaluate(this))
            {
                int index = job.RuntimeStatus.Local.GetValue("createitems.index", 0);
                TaskItemGroup group = null;
                while (index < _list.Count)
                {
                    CreateItemsItem item = _list[index];
                    TaskItem[] parsedItems = itemParser.ParseItem(item.Name);
                    if (parsedItems.Length > 0)
                    {
                        NameValueCollection meta = new NameValueCollection();
                        foreach (string name in item.MetaData.AllKeys)
                        {
                            meta.Add(name, parser.Parse(item.MetaData[name]));
                        }
                        if (group == null)
                        {
                            group = job.AddTaskItemGroup();
                        }
                        foreach (TaskItem parsedItem in parsedItems)
                        {
                            TaskItem newItem = group.AddNewItem(parsedItem.Name, item.Category);
                            parsedItem.CopyMetaDataTo(newItem);
                            foreach (string name in meta)
                            {
                                newItem[name] = meta[name];
                            }
                        }

                    }
                    index++;
                    job.RuntimeStatus.Local["setproperties.index"] = index;

                }
            }
        }

        public string Condition { get; set; }

        #region IXSerializable Members

        public void Load(IServiceProvider context, XElement element)
        {
            this.Condition = (string)element.Attribute("condition");
            foreach (XElement itemElement in element.Elements())
            {
                CreateItemsItem item = new CreateItemsItem();
                item.Load(context, itemElement);
                this._list.Add(item);
            }
        }

        public void Save(IServiceProvider context, XElement element)
        {
            if (!string.IsNullOrEmpty(this.Condition))
            {
                element.SetAttributeValue("condition", this.Condition);
            }

            foreach (CreateItemsItem item in this._list)
            {
                XElement itemElement = new XElement(element.Name.Namespace + item.Category);
                element.Add(itemElement);
                item.Save(context, itemElement);
            }
        }

        #endregion
    }
}
