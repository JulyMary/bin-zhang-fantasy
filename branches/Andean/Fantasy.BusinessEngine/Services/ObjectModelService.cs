using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Linq;
using Fantasy.ServiceModel;
using System.Xml.Linq;
using Fantasy.BusinessEngine.Collections;

namespace Fantasy.BusinessEngine.Services
{
    public class ObjectModelService : ServiceBase, IObjectModelService
    {
        #region IMetaDataService Members

        private object _syncRoot = new object();
        private BusinessClass[] _allClasses = null;
        private XNamespace _nhns = "urn:nhibernate-mapping-2.2";
        private Dictionary<Type, BusinessClass> _typeBusinessClasses;

        public override void InitializeService()
        {
            LoadAllClasses();
            this._typeBusinessClasses = this.AllClasses.ToDictionary(c => c.EntityType());

            XElement mappingElement = new XElement(_nhns + "hibernate-mapping");

            foreach(BusinessClass @class in this.AllClasses)
            {
                if (@class == this.BusinessObjectClass)
                {
                    mappingElement.Add(this.CreateBusinessObjectMap());
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

            
            base.InitializeService();
        }

        private XElement CreateSimpleClassMap(BusinessClass @class)
        {
            throw new NotImplementedException();
        }

        private XElement CreateClassMap(BusinessClass @class)
        {
            IBusinessDataTypeRepository dataTypes = this.Site.GetRequiredService<IBusinessDataTypeRepository>();
            XElement rs = new XElement(_nhns + "joined-subclass",
                new XAttribute("name", @class.EntityType().AssemblyQualifiedName),
                new XAttribute("extends", @class.ParentClass.EntityType().AssemblyQualifiedName),
                new XElement("table", @class.TableName),
                new XElement("owner", @class.TableSchema),
                new XAttribute("discriminator-value", @class.Id),
                new XElement(_nhns + "key", "ID"));

            foreach (BusinessProperty property in @class.Properties)
            {
                if (property.DataType != dataTypes.Class)
                {
                    rs.Add(this.CreatePropertyMap(property));
                }
                else
                {
                    rs.Add(this.CreateClassPropertyMap(property));
                }
            }

            foreach (BusinessAssociation assn in @class.LeftAssociations.Where(w=>w.RightNavigatable))
            {
                rs.Add(this.CreateAssnMap(assn.RightRoleCode, assn.TableName, assn.TableSchema, assn.RightClass, new Cardinality(assn.RightCardinality).IsSingleton, "LEFTID", "RIGHTID", true,)); 

            }

            foreach (BusinessAssociation assn in @class.RightAssociations.Where(w => w.LeftNavigatable))
            {
                rs.Add(this.CreateAssnMap(assn.LeftRoleCode, assn.TableName, assn.TableSchema, assn.LeftClass, new Cardinality(assn.LeftCardinality).IsSingleton, "RIGHTID", "LEFTID", false));

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
                new XAttribute("inverse", inverse),
                new XElement(_nhns + "key", column),
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

        private XElement CreateBusinessObjectMap()
        {
            BusinessClass @class = this.BusinessObjectClass;


            XElement classElement = new XElement(_nhns + "class",
                new XAttribute("name", @class.ExternalType),
                new XAttribute("table", @class.TableName),
                new XAttribute("discriminator-value", @class.Id),
                new XAttribute("owner", @class.TableSchema),
                new XElement(_nhns + "id",
                    new XAttribute("name", "Id"),
                    new XAttribute("column", "ID"),
                    new XElement(_nhns + "generator",
                        new XAttribute("class", "assigned"))),
                new XElement(_nhns + "discriminator", 
                    new XAttribute("column", "CLASSID"),
                    new XAttribute("type", "Guid"),
                    new XAttribute("insert", "false")));

            Guid idPropertyId = new Guid("c9b092be-fce4-4793-9bba-9f3300ac9427");
            Guid classIdPropertyId = new Guid("57b4a057-35b6-4e09-89f4-9f3300ac942f");
            foreach (BusinessProperty property in @class.Properties)
            {
                if (property.Id != idPropertyId)
                {
                    XElement propEle = CreatePropertyMap(property);
                    if (property.Id == classIdPropertyId)
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
            IBusinessDataTypeRepository dataTypes = this.Site.GetRequiredService<IBusinessDataTypeRepository>();
            XElement rs = new XElement(_nhns + "property",
                new XAttribute("name", property.CodeName),
                new XAttribute("column", property.FieldName),
                new XAttribute("type", property.DataType != dataTypes.Enum ? property.DataType.NHType : property.DataEnumType.RuntimeType().AssemblyQualifiedName));
            return rs;

        }

        

        private Guid _businessObjectClassId = new Guid("bf0aa7f4-588f-4556-963d-33242e649d57");




        public IEnumerable<BusinessClass> AllClasses
        {
            get 
            {
               
                return _allClasses;
            }
        }

        private void LoadAllClasses()
        {
            lock (_syncRoot)
            {
                _allClasses = this.Site.GetRequiredService<IEntityService>().Query<BusinessClass>().OrderBy(c => c.Id).ToArray();
                this.BusinessObjectClass = this.FindClass(this._businessObjectClassId);
            }
        }

        public BusinessClass FindClass(Guid id)
        {
           
            int n = _allClasses.BinarySearchBy(id, c => c.Id);
            return n >= 0 ? _allClasses[n] : null;
        }


        public BusinessClass BusinessObjectClass { get; private set; }


        public BusinessClass BusinessClassForEntityType(Type type)
        {
            BusinessClass rs = null;
            while (type != null && rs == null)
            {
                rs = this._typeBusinessClasses.GetValueOrDefault(type);
                if (rs != null)
                {
                    type = type.BaseType;
                }
            }
            return rs;
        }

       
        
        #endregion
    }
}
