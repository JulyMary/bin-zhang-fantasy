using System;
using System.Collections.Generic;

using System.Text;
using System.ComponentModel;

using Fantasy.Studio.Codons;
using Fantasy.Adaption;
using Fantasy.AddIns;

namespace Fantasy.Studio.Descriptor
{
	public class TypeDescriptorAdapterFactory  : ObjectWithSite, IAdapterFactory
	{
		private static readonly string plugInTreePath = "fantasy/studio/typedescriptors";


		public object GetAdapter(object adaptee, Type targetType)
		{

			if (adaptee == null)
			{
				return null;
			}

            Fantasy.Studio.Codons.TypeDescriptor codon = null;
			_typeCodons.TryGetValue(adaptee.GetType(), out codon);

			if (codon == null)
			{
				if (_codons == null)
				{

                    _codons = new List<Fantasy.Studio.Codons.TypeDescriptor>(
                        AddInTree.Tree.GetTreeNode(plugInTreePath).BuildChildItems<Fantasy.Studio.Codons.TypeDescriptor>(null));
				}

                foreach (Fantasy.Studio.Codons.TypeDescriptor c in _codons)
				{
					if (c.TargetType.IsInstanceOfType(adaptee))
					{
						codon = c;
						_typeCodons.Add(adaptee.GetType(), codon);
						break;
					}
				}
			}

			return codon != null ? codon.CreateDescriptor(adaptee) : adaptee;

		}

        private Dictionary<Type, Fantasy.Studio.Codons.TypeDescriptor> _typeCodons = new Dictionary<Type, Fantasy.Studio.Codons.TypeDescriptor>();
        private List<Fantasy.Studio.Codons.TypeDescriptor> _codons = null;
		
		public Type[] GetTargetTypes()
		{
			return new Type[] { typeof(ICustomTypeDescriptor) };
		}
	}
}
