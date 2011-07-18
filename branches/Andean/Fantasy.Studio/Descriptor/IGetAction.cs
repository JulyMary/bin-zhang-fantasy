using System;
using System.Collections.Generic;
using System.Text;

namespace Fantasy.Studio.Descriptor
{
	public interface IGetAction
	{
		object Run(object component, string property);
	}
}
