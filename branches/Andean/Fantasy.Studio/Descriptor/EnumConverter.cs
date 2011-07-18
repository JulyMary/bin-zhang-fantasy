using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using Codons = Fantasy.Studio.Codons;
using Fantasy.AddIns;

namespace Fantasy.Studio.Descriptor
{
	public class EnumConverter : TypeConverter
	{

		public EnumConverter()
		{
		}

		public EnumConverter(Type enumType)
		{
			_enumType = enumType;
		}

		private Type _enumType; 

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == _enumType;
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			foreach (Codons.Enum codon in AddInTree.Tree.GetTreeNode("fantasy/studio/enums").BuildChildItems(value))
			{
				if (codon.TargetType == _enumType)
				{
					return codon.FromString((string)value);
				}
			}

			return new System.ComponentModel.EnumConverter(_enumType).ConvertFromString((string)value);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof(string);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
            foreach (Codons.Enum codon in AddInTree.Tree.GetTreeNode("fantasy/studio/enums").BuildChildItems(value))
			{
				if (codon.TargetType == value.GetType())
				{
					return codon.ToString((Enum)value);
				}
			}

			return new System.ComponentModel.EnumConverter(value.GetType()).ConvertToString(value);
		}
	}
}
