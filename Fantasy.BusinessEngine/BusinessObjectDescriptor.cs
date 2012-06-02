using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Windows;
using Fantasy.BusinessEngine.Services;
using Fantasy.BusinessEngine.Security;

namespace Fantasy.BusinessEngine
{
    public class BusinessObjectDescriptor
    {

        public BusinessPropertyDescriptorCollection Properties { get; private set; }

        public Type EntityType { get; private set; }

        public BusinessObjectDescriptor(BusinessClass @class)
        {
            Init();
            this.Class = @class;
            LoadClassMetaData();
            BusinessObjectSecurity security = BusinessEngineContext.Current.Application.GetClassSecurity(@class);
            
            this.LoadSecurity(security);
        }


        public BusinessObjectDescriptor(BusinessObject obj)
        {
            Init();
            this.Object = obj;
            this.Class = BusinessEngineContext.Current.GetRequiredService<IObjectModelService>().FindBusinessClass(obj.ClassId);
            LoadClassMetaData();
            BusinessObjectSecurity security = BusinessEngineContext.Current.Application.GetObjectSecurity(obj);
            this.LoadSecurity(security);
        }


        private void Init()
        {
            this.Properties = new BusinessPropertyDescriptorCollection(this);
        }

        private void LoadSecurity(BusinessObjectSecurity security)
        {
            this.CanCreate = (bool)security.CanCreate;
            this.CanDelete = (bool)security.CanDelete;
            foreach (BusinessPropertyDescriptor prop in this.Properties)
            {
                BusinessObjectMemberSecurity member = security.Properties[prop.CodeName];
                prop.CanRead = (bool)member.CanRead;
                prop.CanWrite = (bool)member.CanWrite;
                
            }
        }

        private void LoadClassMetaData()
        {

            this.EntityType = this.Class.EntityType();

            IBusinessDataTypeRepository dataTypes = BusinessEngineContext.Current.GetRequiredService<IBusinessDataTypeRepository>();
            foreach (BusinessProperty property in this.Class.AllProperties())
            {
                BusinessPropertyDescriptor desc = new BusinessPropertyDescriptor()
                {
                    Owner =this,
                    Name = property.Name,
                    CodeName = property.CodeName,
                    IsScalar = true,
                    MemberType = BusinessObjectMemberTypes.Property,
                    PropertyType = EntityType.GetProperty(property.CodeName).PropertyType,
                    DisplayOrder = property.DisplayOrder,
                    Property = property,
                    Entensions = property.Extensions
                };

                if (property.DataType == dataTypes.Class)
                {
                    desc.ReferencedClass = property.DataClassType; 
                }
                else if (property.DataType == dataTypes.Enum)
                {
                    desc.ReferencedEnum = property.DataEnumType;  
                }

                this.Properties.Add(desc);

               
            }


            foreach (BusinessAssociation assn in this.Class.AllLeftAssociations().Where(r=>r.RightNavigatable) )
            {
                BusinessPropertyDescriptor desc = new BusinessPropertyDescriptor()
                {
                    Owner = this,
                    Name = assn.RightRoleName,
                    CodeName = assn.RightRoleCode,
                    IsScalar = new Cardinality(assn.RightCardinality).IsScalar,
                    MemberType = BusinessObjectMemberTypes.LeftAssociation,
                    PropertyType = EntityType.GetProperty(assn.RightRoleCode).PropertyType,
                    DisplayOrder = assn.RightRoleDisplayOrder,
                    ReferencedClass = assn.RightClass,
                    Association = assn,
                    Entensions = assn.RightExtensions

                };
                this.Properties.Add(desc);
            }


            foreach (BusinessAssociation assn in this.Class.AllRightAssociations().Where(a=>a.LeftNavigatable)  )
            {
                BusinessPropertyDescriptor desc = new BusinessPropertyDescriptor()
                {
                    Owner = this,
                    Name = assn.LeftRoleName,
                    CodeName = assn.LeftRoleCode,
                    IsScalar = new Cardinality(assn.LeftCardinality).IsScalar,
                    MemberType = BusinessObjectMemberTypes.RightAssociation,
                    PropertyType = EntityType.GetProperty(assn.LeftRoleCode).PropertyType,
                    DisplayOrder = assn.LeftRoleDisplayOrder,
                    ReferencedClass = assn.LeftClass,
                    Association = assn,
                    Entensions = assn.LeftExtensions
                };
                this.Properties.Add(desc);
            }

            this.Properties.SortBy(p => p.DisplayOrder);
        }
        
        public BusinessObject Object { get; private set; }

        public BusinessClass Class { get; private set; }


        public bool CanCreate { get; private set; }

        public bool CanDelete { get; private set; }

    }
}
