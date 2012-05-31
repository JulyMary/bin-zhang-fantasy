using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using NHibernate;
using NHibernate.Linq;

namespace Fantasy.BusinessEngine.Services
{
    public class ObjectModelService : ServiceBase, IObjectModelService
    {

        public override void InitializeService()
        {

            IEntityService es = this.Site.GetRequiredService<IEntityService>();

            this._allClasses = es.Query<BusinessClass>().ToList();
            _allClasses.SortBy(c => c.Id);
            this._typeClasses = this._allClasses.ToDictionary(c => c.EntityType());
            this.RootClass = this.FindBusinessClass(BusinessClass.RootClassId);


            IImageListService imageList = this.Site.GetRequiredService<IImageListService>();
            foreach(BusinessClass @class in this._allClasses.Where(c=>c.Icon != null))
            {
                imageList.Register(this.GetImageKeyInternal(@class), @class.Icon);
            }

            base.InitializeService();
        }

        private IList<BusinessClass> _allClasses;
        private Dictionary<Type, BusinessClass> _typeClasses;
        public IEnumerable<BusinessClass> AllClasses
        {
            get { return this._allClasses; }
        }

        #region IObjectModelService Members


        public BusinessClass FindBusinessClass(Guid id)
        {
            int pos = _allClasses.BinarySearchBy(id, c => c.Id);
            if (pos >= 0)
            {
                return _allClasses[pos];
            }
            else
            {
                throw new EntityNotFoundException(typeof(BusinessClass), id);
            }
            
        }

        public BusinessClass FindBusinessClassForType(Type type)
        {
            BusinessClass rs = null;
            while (rs == null && type != null)
            {
                rs = this._typeClasses.GetValueOrDefault(type);
                if (rs == null)
                {
                    type = type.BaseType;
                }

            }
            return rs;
        }

        public BusinessClass RootClass { get; private set; }
    

        #endregion

        #region IObjectModelService Members

        public string GetImageKey(BusinessClass @class)
        {
            while (@class != null && @class.Icon == null)
            {
                @class = @class.ParentClass;
            }

            return @class != null ? GetImageKeyInternal(@class) : string.Empty;
        }


        public string GetImageKey(BusinessClass @class, Enum state)
        {
            throw new NotImplementedException();
        }


        private string GetImageKeyInternal(BusinessClass @class)
        {
            return @class.FullCodeName;
        }

        private string GetImageKeyInternal(BusinessClass @class, Enum state)
        {
            return @class.FullCodeName + "." + state;
        }

        #endregion

       
    }
}
