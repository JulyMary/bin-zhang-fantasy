using System;
using System.Collections.Generic;
using System.Text;

using System.Collections;

using System.Linq;
using System.ComponentModel;
using Fantasy.AddIns;

namespace Fantasy.Studio.Codons
{
	public class Enum : CodonBase
	{
        public Type TargetType
        {
            get;
            set;
        }

		public override object BuildItem(object owner, IEnumerable subItems, ConditionCollection conditions)
		{

			_fileds.Clear();
			_fileds.AddRange(subItems.Cast<EnumField>());
			
			return this;
		}

		public string ToString(System.Enum value)
		{
			return Attribute.IsDefined(value.GetType(), typeof(FlagsAttribute)) ? FlagsToString(value)
				: FieldToString(value);
		}

		private string FlagsToString(System.Enum value)
		{

			string rs = String.Empty;
			foreach (System.Enum e in System.Enum.GetValues(value.GetType()))
			{
				if (EnumToInt(e) == 0 && value.Equals(e))
				{
					rs = FieldToString(e);
					break;
				}

				if ((EnumToInt(value) & EnumToInt(e)) > 0)
				{
					if (rs != string.Empty)
					{
						rs += ", ";
					}
					rs += FieldToString(e);
				}
			}

			return rs;
		}

		private int EnumToInt(System.Enum value)
		{
			return Int32.Parse(value.ToString("D"));
		}

		private string FieldToString(System.Enum value)
		{
			foreach (EnumField fcodon in this._fileds)
			{
				if (CaseInsensitiveComparer.Default.Compare(value.ToString(), fcodon.Name) == 0)
				{
					return fcodon.Caption;
				}
			}

			return value.ToString();
		}

		private System.Enum StringToFlags(string value)
		{
			string rs = string.Empty;
			foreach (string s in value.Split(new char[] { ',' }))
			{
				if (rs != String.Empty)
				{
					rs += ", ";
				}
				rs += GetFeildString(s);
			}

			return (System.Enum)System.Enum.Parse(TargetType, rs, true);
		}

		private string GetFeildString(string value)
		{
			foreach (EnumField fcodon in this._fileds)
			{
				if (CaseInsensitiveComparer.Default.Compare(value.ToString(), fcodon.Caption) == 0)
				{
					return fcodon.Name;
				}
			}

			foreach (EnumField fcodon in this._fileds)
			{
                if (CaseInsensitiveComparer.Default.Compare(value.ToString(), fcodon.Name) == 0)
				{
                    return fcodon.Name;
				}
			}

			throw new EnumFieldCodonException(TargetType, value);
		}

		private System.Enum StringToField(string value)
		{
			return (System.Enum)System.Enum.Parse(TargetType, GetFeildString(value)); 
		}

		public System.Enum FromString(string value)
		{
			return Attribute.IsDefined(value.GetType(), typeof(FlagsAttribute)) ? StringToFlags(value)
				: StringToField(value);
		}

		private List<EnumField> _fileds = new List<EnumField>();

	}

	public class EnumCodonTargetException : Exception
	{
		public EnumCodonTargetException(Type t)
			: base(t.ToString() + " is not enum type.")
		{

		}
	}

	


}
