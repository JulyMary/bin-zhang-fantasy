using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;
using Fantasy.BusinessEngine.EntityExtensions;
using Fantasy.BusinessEngine.Properties;

namespace Fantasy.BusinessEngine
{
    public class BusinessPropertyDescriptor
    {

        public BusinessObjectDescriptor Owner { get; internal set; }

        public string Name { get; internal set; }

        public string CodeName { get; internal set; }

        public object Value
        {
            get
            {
                return Invoker.Invoke(Owner.Object, CodeName);
            }
            set
            {

                switch (this.MemberType)
                {
                    case BusinessObjectMemberTypes.Property:
                        this.Owner.Object.GetType().GetProperty(CodeName).SetValue(this.Owner.Object, value, null);
                        break;
                    case BusinessObjectMemberTypes.LeftAssociation:

                        if (this.IsScalar)
                        {
                            this.SetAssnValue((BusinessObject)value, this.Association.LeftNavigatable ? this.Association.LeftRoleCode : null,
                                (new Cardinality(this.Association.LeftCardinality)).IsScalar);  
                        }
                        else
                        {
                            throw new NotSupportedException();
                        }


                        break;
                    case BusinessObjectMemberTypes.RightAssociation:
                        if(this.IsScalar)
                        {
                            this.SetAssnValue((BusinessObject)value, this.Association.RightNavigatable ? this.Association.RightRoleCode : null,
                              (new Cardinality(this.Association.RightCardinality)).IsScalar);  
                        }
                        else
                        {
                            throw new NotSupportedException();
                        }
                        break;
                  
                    default:
                        throw new NotSupportedException();
                }
                
            }
        }


        private void SetAssnValue(BusinessObject newValue, string otherSideRole, bool otherSideIsScalar)
        {


            PropertyInfo otherSidePropty = null; 
            if(otherSideRole != null)
            {
                otherSidePropty =  this.ReferencedClass.EntityType().GetProperty(otherSideRole); 
                BusinessObject oldValue = (BusinessObject)this.Value;
                if (oldValue != null)
                {
                    if (otherSideIsScalar)
                    {
                        otherSidePropty.SetValue(oldValue, null, null);
                    }
                    else
                    {
                        IList list = (IList)otherSidePropty.GetValue(oldValue, null);
                        if (list != null)
                        {
                            list.Remove(this.Owner.Object);
                        }
                    }
                }
            }
            this.Owner.Object.GetType().GetProperty(CodeName).SetValue(this.Owner.Object, newValue, null);

            if (newValue != null && otherSideRole != null)
            {
                if (otherSideIsScalar)
                {
                    otherSidePropty.SetValue(newValue, this.Owner.Object, null);
                }
                else
                {
                    IList list = (IList)otherSidePropty.GetValue(newValue, null);
                    if (list != null)
                    {
                        list.Add(this.Owner.Object);
                    }
                }
            }


        }


        public Type PropertyType { get; internal set; }

        public bool CanRead { get; internal set; }

        public bool CanWrite { get; internal set; }

        public BusinessObjectMemberTypes MemberType { get; internal set; }


        public bool IsScalar { get; internal set; }


        public BusinessClass ReferencedClass { get; internal set; }

        public BusinessEnum ReferencedEnum { get; internal set; }


        public BusinessProperty Property { get; internal set; }

        public BusinessAssociation Association { get; internal set; }

        public long DisplayOrder { get; internal set; }

        public IList<IEntityExtension> Entensions { get; internal set; }

        public string Category
        {
            get
            {
                string rs = null;
                Category cate = this.Entensions.OfType<Category>().FirstOrDefault();
                if (cate != null)
                {
                    rs = cate.Value;
                }
                if (String.IsNullOrEmpty(rs))
                {
                    rs = Resources.MiscCategoryName;
                }
                return rs;
            }
        }


    }
}
