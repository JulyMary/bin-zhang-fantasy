using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Fantasy.XSerialization;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml;

namespace Fantasy.Jobs.Scheduling
{
    [DataContract,
     KnownType(typeof(TimeTrigger)),
     KnownType(typeof(DailyTrigger)),
     KnownType(typeof(WeeklyTrigger)),
     KnownType(typeof(MonthlyTrigger)),
     KnownType(typeof(MonthlyDOWTrigger)),
     KnownType(typeof(TemplateAction)),
     KnownType(typeof(InlineAction)),
     KnownType(typeof(CustomAction))
    ]
    [XSerializable("schedule", NamespaceUri = Consts.ScheduleNamespaceURI)]
    public class ScheduleItem : IXSerializable
    {
        public ScheduleItem()
        {
            
        }

        [DataMember]
        [XAttribute("name")]
        public string Name { get; set; }

        [DataMember]
        [XAttribute("author")]
        public string Author { get; set; }

        [DataMember]
        [XAttribute("description")]
        public string Description { get; set; }

        [DataMember]
        [XAttribute("enabled")]
        public bool Enabled { get; set; }

        [DataMember]
        public Trigger Trigger { get; set; }

        [DataMember]
        public Action Action { get; set; }

        [DataMember]
        [XAttribute("priority")]
        public int Priority { get; set; }

        [DataMember]
        [XAttribute("restartOnFailure")]
        public Restart RestartOnFailure { get; set; }

        [DataMember]
        [XArray(Name = "requiredUncPath"),
        XArrayItem(Name = "unc", Type = typeof(UncPath))]
        public UncPath[] RequiredUncPths { get; set; }

        [DataMember]
        [XAttribute("runOnlyIfIdle")]
        public bool RunOnlyIfIdle { get; set; }

        [DataMember]
        [XAttribute("startWhenAvailable")]
        public bool StartWhenAvailable { get; set; }

        [DataMember]
        public string CustomParams { get; set; }


        [DataMember]
        [XAttribute("multipleInstances")]
        public InstancesPolicy MultipleInstance { get; set; }


        [DataMember]
        [XAttribute("expired")]
        public bool Expired { get; set; }

        [DataMember]
        [XAttribute("deleteAfterExpired")]
        public bool DeleteAfterExpired { get; set; }

        #region IXSerializable Members

        void IXSerializable.Load(IServiceProvider context, XElement element)
        {
            XHelper.Default.LoadByXAttributes(context, element, this);
           
            XmlNamespaceManager nm = new XmlNamespaceManager(new NameTable());
            nm.AddNamespace("s", Consts.ScheduleNamespaceURI);

            XElement customParams = element.XPathSelectElement("s:custom", nm);
            if (customParams != null)
            {
                this.CustomParams = customParams.ToString();
            }


            XElement triggerElement = element.XPathSelectElement("s:trigger", nm);
            if (triggerElement != null)
            {
                Type type;
                TriggerType triggerType = (TriggerType)Enum.Parse(typeof(TriggerType), (string)triggerElement.Attribute("type"));
                switch (triggerType)
                {
                    case TriggerType.Time:
                        type = typeof(TimeTrigger);
                        break;
                    case TriggerType.Daily:
                        type = typeof(DailyTrigger);
                        break;
                    case TriggerType.Weekly:
                        type = typeof(WeeklyTrigger);
                        break;
                    case TriggerType.Monthly:
                        type = typeof(MonthlyTrigger);
                        break;
                    case TriggerType.MonthlyDayOfWeek:
                        type = typeof(MonthlyDOWTrigger);
                        break;
                    default:
                        throw new Exception("Absurd!");
                }
                XSerializer ser = new XSerializer(type);
                ser.Context = context;
                this.Trigger = (Trigger)ser.Deserialize(triggerElement);

            }

            XElement actionElement = element.XPathSelectElement("s:action", nm);
            if (actionElement != null)
            {
                Type type;
                ActionType actionType = (ActionType)Enum.Parse(typeof(ActionType), (string)actionElement.Attribute("type"));
                switch (actionType)
                {
                    case ActionType.Template:
                        type = typeof(TemplateAction);
                        break;
                    case ActionType.Inline:
                        type = typeof(InlineAction);
                        break;
                    case ActionType.Custom:
                        type = typeof(CustomAction);
                        break;
                    default:
                        throw new Exception("absurd!");
                }
                XSerializer ser = new XSerializer(type) { Context = context };
                this.Action = (Action)ser.Deserialize(actionElement);
            }
        }

        void IXSerializable.Save(IServiceProvider context, XElement element)
        {
            XHelper.Default.SaveByXAttributes(context, element, this);
            XNamespace ns = Consts.ScheduleNamespaceURI;

            if (this.Trigger != null)
            {
                XElement triggerElement = new XElement(ns + "trigger");
                element.Add(triggerElement);
                XSerializer ser = new XSerializer(this.Trigger.GetType()) { Context = context };
                triggerElement.SetAttributeValue("type", this.Trigger.Type.ToString());
                ser.Serialize(triggerElement, this.Trigger);
            }

            if (this.Action != null)
            {
                XElement actionElement = new XElement(ns + "action");
                element.Add(actionElement);
                XSerializer ser = new XSerializer(this.Action.GetType()) { Context = context };
                actionElement.SetAttributeValue("type", this.Action.Type.ToString());
                ser.Serialize(actionElement, this.Action);
            }

            if (this.CustomParams != null)
            {
                element.Add(XElement.Parse(this.CustomParams));
            }

        }

        #endregion
    }
}
