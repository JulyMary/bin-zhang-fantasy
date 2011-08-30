﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.EntityClient;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Runtime.Serialization;

[assembly: EdmSchemaAttribute()]
#region EDM Relationship Metadata

[assembly: EdmRelationshipAttribute("Model1", "Entity1Entity2", "Entity1", System.Data.Metadata.Edm.RelationshipMultiplicity.One, typeof(ConsoleTest.Entity1), "Entity2", System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(ConsoleTest.Entity2))]

#endregion

namespace ConsoleTest
{
    #region Contexts
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    public partial class Model1Container : ObjectContext
    {
        #region Constructors
    
        /// <summary>
        /// Initializes a new Model1Container object using the connection string found in the 'Model1Container' section of the application configuration file.
        /// </summary>
        public Model1Container() : base("name=Model1Container", "Model1Container")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new Model1Container object.
        /// </summary>
        public Model1Container(string connectionString) : base(connectionString, "Model1Container")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new Model1Container object.
        /// </summary>
        public Model1Container(EntityConnection connection) : base(connection, "Model1Container")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        #endregion
    
        #region Partial Methods
    
        partial void OnContextCreated();
    
        #endregion
    
        #region ObjectSet Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<Entity1> Entity1Set
        {
            get
            {
                if ((_Entity1Set == null))
                {
                    _Entity1Set = base.CreateObjectSet<Entity1>("Entity1Set");
                }
                return _Entity1Set;
            }
        }
        private ObjectSet<Entity1> _Entity1Set;

        #endregion
        #region AddTo Methods
    
        /// <summary>
        /// Deprecated Method for adding a new object to the Entity1Set EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToEntity1Set(Entity1 entity1)
        {
            base.AddObject("Entity1Set", entity1);
        }

        #endregion
    }
    

    #endregion
    
    #region Entities
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="Model1", Name="Entity1")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    [KnownTypeAttribute(typeof(Entity2))]
    public partial class Entity1 : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new Entity1 object.
        /// </summary>
        /// <param name="id">Initial value of the Id property.</param>
        public static Entity1 CreateEntity1(global::System.Int32 id)
        {
            Entity1 entity1 = new Entity1();
            entity1.Id = id;
            return entity1;
        }

        #endregion
        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (_Id != value)
                {
                    OnIdChanging(value);
                    ReportPropertyChanging("Id");
                    _Id = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("Id");
                    OnIdChanged();
                }
            }
        }
        private global::System.Int32 _Id;
        partial void OnIdChanging(global::System.Int32 value);
        partial void OnIdChanged();

        #endregion
    
    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="Model1", Name="Entity2")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Entity2 : Entity1
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new Entity2 object.
        /// </summary>
        /// <param name="id">Initial value of the Id property.</param>
        public static Entity2 CreateEntity2(global::System.Int32 id)
        {
            Entity2 entity2 = new Entity2();
            entity2.Id = id;
            return entity2;
        }

        #endregion
    
        #region Navigation Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("Model1", "Entity1Entity2", "Entity1")]
        public Entity1 Entity1_1
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<Entity1>("Model1.Entity1Entity2", "Entity1").Value;
            }
            set
            {
                ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<Entity1>("Model1.Entity1Entity2", "Entity1").Value = value;
            }
        }
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [BrowsableAttribute(false)]
        [DataMemberAttribute()]
        public EntityReference<Entity1> Entity1_1Reference
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<Entity1>("Model1.Entity1Entity2", "Entity1");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedReference<Entity1>("Model1.Entity1Entity2", "Entity1", value);
                }
            }
        }

        #endregion
    }

    #endregion
    
}
