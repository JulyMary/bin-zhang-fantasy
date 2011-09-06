using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;
using Fantasy.Utils;
using Fantasy.Studio.BusinessEngine.Properties;

namespace Fantasy.Studio.BusinessEngine.EnumEditing
{
    public class AddEnumValueCommand : ObjectWithSite, ICommand
    {
        #region ICommand Members

        public object Execute(object args)
        {
            BusinessEnum @enum = (BusinessEnum)args;
            IEntityService svc = this.Site.GetRequiredService<IEntityService>();
            BusinessEnumValue value = svc.CreateEntity<BusinessEnumValue>();
            value.Name = UniqueNameGenerator.GetName(Resources.DefaultNewBusinessEnumValueName, @enum.EnumValues.Select(p => p.Name));
            value.CodeName = UniqueNameGenerator.GetCodeName(value.Name);
            value.Enum = @enum;

            if (@enum.IsFlags)
            {
                value.Value = GetNextFlagsValue(@enum);
            }
            else
            {
                value.Value = GetNextValue(@enum);
            }

            @enum.EnumValues.Add(value);

            return value;
        }

        private long GetNextValue(BusinessEnum @enum)
        {
            long rs = 0;
            while (@enum.EnumValues.Any(v => v.Value == rs))
            {
                rs++;
            }

            return rs;
        }

        private long GetNextFlagsValue(BusinessEnum @enum)
        {
            if (@enum.EnumValues.Count == 0)
            {
                return 0;
            }
            else
            {
                long existed = 0;
                foreach (BusinessEnumValue value in @enum.EnumValues)
                {
                    existed |= value.Value;
                }

                long rs = 1;
                long max = 1L << (sizeof(long) * 8 - 3);
                while ((rs & existed) > 0 && rs <= max)
                {
                    rs <<= 1;
                }

                return rs;

            }
        }



        #endregion
    }
}
