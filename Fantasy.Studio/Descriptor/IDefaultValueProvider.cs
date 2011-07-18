using System;
using System.Collections.Generic;
using System.Text;

namespace Fantasy.Studio.Descriptor
{
	public interface IDefaultValueProvider
	{
		object GetValue(object component, string propertyName, Type propertyType);
	}


	public class DefaultValueProvider : IDefaultValueProvider 
	{
		public object GetValue(object component, string propertyName, Type propertyType)
		{
			if (!propertyType.IsValueType)
			{
				return null;
			}
			else
			{
				return Activator.CreateInstance(propertyType);
			}
		}
	}
}
