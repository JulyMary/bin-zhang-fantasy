using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using NHibernate.Linq;

namespace Fantasy.BusinessEngine.Services
{
    public class DefaultBusinessDataTypeRepository : ServiceBase, IBusinessDataTypeRepository
    {


        public override void InitializeService()
        {

            IEntityService es = this.Site.GetRequiredService<IEntityService>();
            var query = from t in es.DefaultSession.Query<BusinessDataType>() orderby t.Name select t;
            _all = query.ToArray();

            this.Binary = _all.Single(t => t.Id == BusinessDataType.WellknownIds.Binary);
            this.Boolean = _all.Single(t => t.Id == BusinessDataType.WellknownIds.Boolean);
            this.Byte = _all.Single(t => t.Id == BusinessDataType.WellknownIds.Byte);
            this.DateTime = _all.Single(t => t.Id == BusinessDataType.WellknownIds.DateTime); 
            this.DateTimeOffset = _all.Single(t => t.Id == BusinessDataType.WellknownIds.DateTimeOffset);
            this.Decimal = _all.Single(t => t.Id == BusinessDataType.WellknownIds.Decimal);
            this.Double = _all.Single(t => t.Id == BusinessDataType.WellknownIds.Double);
            this.Float = _all.Single(t => t.Id == BusinessDataType.WellknownIds.Float);
            this.Guid = _all.Single(t => t.Id == BusinessDataType.WellknownIds.Guid);
            this.Int16 = _all.Single(t => t.Id == BusinessDataType.WellknownIds.Int16);
            this.Int32 = _all.Single(t => t.Id == BusinessDataType.WellknownIds.Int32);
            this.Int64 = _all.Single(t => t.Id == BusinessDataType.WellknownIds.Int64);
            this.UInt16 = _all.Single(t => t.Id == BusinessDataType.WellknownIds.UInt16);
            this.UInt32 = _all.Single(t => t.Id == BusinessDataType.WellknownIds.UInt32);
            this.UInt64 = _all.Single(t => t.Id == BusinessDataType.WellknownIds.UInt64);
            this.String = _all.Single(t => t.Id == BusinessDataType.WellknownIds.String);
            this.Text = _all.Single(t => t.Id == BusinessDataType.WellknownIds.Text);
            this.Bitmap = _all.Single(t => t.Id == BusinessDataType.WellknownIds.Bitmap);
            this.Enum = _all.Single(t => t.Id == BusinessDataType.WellknownIds.Enum);
            this.Class = _all.Single(t => t.Id == BusinessDataType.WellknownIds.Class);
            base.InitializeService();
            
        }
       
        BusinessDataType[] _all;

        public BusinessDataType[] All
        {
            get
            {
                return _all;
            }
        }



        #region IBusinessDataTypeRepository Members


        public BusinessDataType Binary
        {
            get;
            private set;
        }

        public BusinessDataType Boolean
        {
            get;
            private set;
        }

        public BusinessDataType Byte
        {
            get;
            private set;
        }

        public BusinessDataType DateTime
        {
            get;
            private set;
        }

        public BusinessDataType DateTimeOffset
        {
            get;
            private set;
        }

        public BusinessDataType Decimal
        {
            get;
            private set;
        }

        public BusinessDataType Double
        {
            get;
            private set;
        }

        public BusinessDataType Float
        {
            get;
            private set;
        }

        public BusinessDataType Guid
        {
            get;
            private set;
        }

        public BusinessDataType Int16
        {
            get;
            private set;
        }

        public BusinessDataType Int32
        {
            get;
            private set;
        }

        public BusinessDataType Int64
        {
            get;
            private set;
        }

        public BusinessDataType UInt16
        {
            get;
            private set;
        }

        public BusinessDataType UInt32
        {
            get;
            private set;
        }

        public BusinessDataType UInt64
        {
            get;
            private set;
        }

        public BusinessDataType String
        {
            get;
            private set;
        }

        public BusinessDataType Text
        {
            get;
            private set;
        }

        public BusinessDataType Bitmap
        {
            get;
            private set;
        }

        public BusinessDataType Enum
        {
            get;
            private set;
        }

        public BusinessDataType Class
        {
            get;
            private set;
        }

        #endregion
    }
}
