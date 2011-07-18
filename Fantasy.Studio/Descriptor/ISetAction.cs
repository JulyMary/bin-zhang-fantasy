using System;
using System.Collections.Generic;
using System.Text;

namespace Fantasy.Studio.Descriptor
{
	public interface ISetAction
	{
		void Run(object component, string property,object value);
	}
}
