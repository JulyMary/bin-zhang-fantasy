using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Linq;
using Fantasy.ServiceModel;
using System.Xml.Linq;
using Fantasy.BusinessEngine.Collections;
using NHibernate;
using System.Xml;

namespace Fantasy.BusinessEngine.Services
{
    public class MapBusinessClassService : ServiceBase
    {
        #region IMetaDataService Members

        private object _syncRoot = new object();
   
        private XNamespace _nhns = "urn:nhibernate-mapping-2.2";

      

        public override void InitializeService()
        {

            INHConfigurationService nhConfigSvc = this.Site.GetRequiredService<INHConfigurationService>();
            ISessionFactory sessionFactory = nhConfigSvc.Configuration.BuildSessionFactory();
            ISession session = sessionFactory.OpenSession();
            try
            {
                BusinessClass[] classes = session.Query<BusinessClass>().ToArray();
                XElement mappingElement = new XElement(_nhns + "hibernate-mapping");

                foreach (BusinessClass @class in classes)
                {
                    if (@class.Id == BusinessClass.RootClassId)
                    {
                        mappingElement.Add(this.CreateRootClassMap(@class));
                    }
                    else if (!@class.IsSimple)
                    {
                        mappingElement.Add(this.CreateClassMap(@class));
                    }
                    else
                    {
                        mappingElement.Add(this.CreateSimpleClassMap(@class));
                    }
                }
                XmlReader reader = mappingElement.CreateReader();
                nhConfigSvc.Configuration.AddXmlReader(reader);
                reader.Close();
            }
            finally
            {
                session.Close();
            }
           

            
            base.InitializeService();
        }

        private XElement CreateSimpleClassMap(BusinessClass @class)
        {
            throw new NotImplementedException();
        }

        private XElement CreateClassMap(BusinessClass @class)
        {
           
            XElement rs = new XElement(_nhns + "subclass",
                new XAttribute("name", @class.EntityType().AssemblyQualifiedName),
                new XAttribute("extends", @class.ParentClass.EntityType().AssemblyQualifiedName),
               
                new XAttribute("discriminator-value", @class.Id),
                new XElement(_nhns + "join",
                     new XAttribute("table", @class.TableName),
                     new XAttribute("schema", @class.TableSchema),
                     new XElement(_nhns + "key",
                         new XAttribute("column", "ID"))
                     )
                );

            XElement join = rs.Element(_nhns + "join");

            foreach (BusinessProperty property in @class.Properties)
            {
                if (property.DataType.Id != BusinessDataType.WellknownIds.Class)
                {
                    join.Add(this.CreatePropertyMap(property));
                }
                else
                {
                    join.Add(this.CreateClassPropertyMap(property));
                }
            }

            foreach (BusinessAssociation assn in @class.LeftAssociations.Where(w=>w.RightNavigatable))
            {
                join.Add(this.CreateAssnMap(assn.RightRoleCode, assn.TableName, assn.TableSchema, assn.RightClass, new Cardinality(assn.RightCardinality).IsSingleton, "LEFTID", "RIGHTID", assn.LeftNavigatable )); 

            }

            foreach (BusinessAssociation assn in @class.RightAssociations.Where(w => w.LeftNavigatable))
            {
                join.Add(this.CreateAssnMap(assn.LeftRoleCode, assn.TableName, assn.TableSchema, assn.LeftClass, new Cardinality(assn.LeftCardinality).IsSingleton, "RIGHTID", "LEFTID", false));

            }

            return rs;

        }

        private XElement CreateAssnMap(string name, string table, string schema, BusinessClass @class, bool isManyToOne, string column, string refcolumn, bool inverse)
        {
            XElement rs = new XElement(_nhns + "bag",
                new XAttribute("name", name),
                new XAttribute("table", table),
                new XAttribute("schema", schema),
                new XAttribute("lazy", true),
                new XAttribute("inverse", inverse),
                new XAttribute("cascade", "none"),
                new XAttribute("collection-type", typeof(ObservableList<>).MakeGenericType(@class.EntityType()).AssemblyQualifiedName),
                new XAttribute("access", isManyToOne ? typeof(ManyToOneAccessor).AssemblyQualifiedName : typeof(ManyToManyAccessor).AssemblyQualifiedName),
               
                new XElement(_nhns + "key", new XAttribute("column", column)),
                new XElement(_nhns + "many-to-many", 
                    new XAttribute("class", @class.EntityType().AssemblyQualifiedName),
                    new XAttribute("column", refcolumn))
                );

            return rs;

        }

        private XElement CreateClassPropertyMap(BusinessProperty property)
        {
            XElement rs = new XElement(_nhns + "many-to-one",
                new XAttribute("name", property.CodeName),
                new XAttribute("class", property.DataClassType.EntityType().AssemblyQualifiedName),
                new XAttribute("column", property.FieldName),
                new XAttribute("cascade", "none"),
                new XAttribute("fetch", "select"));
            return rs;
        }

        private XElement CreateRootClassMap(BusinessClass @class)
        {
            XElement classElement = new XElement(_nhns + "class",
                new XAttribute("name", @class.ExternalType),
                new XAttribute("table", @class.TableName),
                new XAttribute("discriminator-value", @class.Id),
                new XAttribute("schema", @class.TableSchema),
                new XElement(_nhns + "id",
                    new XAttribute("name", "Id"),
                    new XAttribute("column", "ID"),
                    new XElement(_nhns + "generator",
                        new XAttribute("class", "assigned"))),
                new XElement(_nhns + "discriminator", 
                    new XAttribute("column", "CLASSID"),
                    new XAttribute("type", "Guid"),
                    new XAttribute("insert", "false")));

            
            foreach (BusinessProperty property in @class.Properties)
            {
                if (property.Id != BusinessProperty.WellKnownIds.Id)
                {
                    XElement propEle = CreatePropertyMap(property);
                    if (property.Id == BusinessProperty.WellKnownIds.ClassId)
                    {
                        propEle.SetAttributeValue("update", false);
                    }
                    classElement.Add(propEle );
                }
            }

            return classElement; 

        }

        private XElement CreatePropertyMap(BusinessProperty property)
        {
            
            XElement rs = new XElement(_nhns + "property",
                new XAttribute("name", property.CodeName),
                new XAttribute("column", property.FieldName),
                new XAttribute("type", property.DataType.Id != BusinessDataType.WellknownIds.Enum ? property.DataType.NHType : property.DataEnumType.RuntimeType().AssemblyQualifiedName));
            return rs;

        }

        
        #endregion
    }
}
