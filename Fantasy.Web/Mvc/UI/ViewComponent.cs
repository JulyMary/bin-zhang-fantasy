using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;
using Fantasy.Web.Properties;
using System.Globalization;
using System.IO;

namespace Fantasy.Web.Mvc.UI
{
    public class ViewComponent : IHtmlNode
    {

        // Fields
        private string _idAttributeDotReplacement;
      

        // Methods
        public ViewComponent(string tagName)
        {
            if (string.IsNullOrEmpty(tagName))
            {
                throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, "tagName");
            }
            this.TagName = tagName;
            this.Attributes = new SortedDictionary<string, string>(StringComparer.Ordinal);
        }

        public void AddCssClass(string value)
        {
            string str;
            if (this.Attributes.TryGetValue("class", out str))
            {
                this.Attributes["class"] = value + " " + str;
            }
            else
            {
                this.Attributes["class"] = value;
            }
        }

        public void RemoveCssClass(string value)
        {
            string @class = this.Attributes.GetValueOrDefault("class", string.Empty);
            List<string> values = @class.Split(' ').ToList();
            int index = values.IndexOfBy(value, comparer: StringComparer.OrdinalIgnoreCase);
            if (index >= 0)
            {
                values.RemoveAt(index);
            }

            this.Attributes["class"] = String.Join(" ", values);
        }


        

        private void WriteAttributes(TextWriter output)
        {
            foreach (KeyValuePair<string, string> pair in this.Attributes)
            {
                string key = pair.Key;
                if (/*!string.Equals(key, "id", StringComparison.Ordinal) ||*/ !string.IsNullOrEmpty(pair.Value))
                {
                    string str2 = HttpUtility.HtmlAttributeEncode(pair.Value);
                    output.Write(" ");
                    output.Write(key);
                    output.Write("=\"");
                    output.Write(str2);
                    output.Write('"');
                }
            }
        }

        public static string CreateSanitizedId(string originalId)
        {
            return CreateSanitizedId(originalId, HtmlHelper.IdAttributeDotReplacement);
        }

        public static string CreateSanitizedId(string originalId, string invalidCharReplacement)
        {
            if (string.IsNullOrEmpty(originalId))
            {
                return null;
            }
            if (invalidCharReplacement == null)
            {
                throw new ArgumentNullException("invalidCharReplacement");
            }
            char c = originalId[0];
            if (!Html401IdUtil.IsLetter(c))
            {
                return null;
            }
            StringBuilder builder = new StringBuilder(originalId.Length);
            builder.Append(c);
            for (int i = 1; i < originalId.Length; i++)
            {
                char ch2 = originalId[i];
                if (Html401IdUtil.IsValidIdCharacter(ch2))
                {
                    builder.Append(ch2);
                }
                else
                {
                    builder.Append(invalidCharReplacement);
                }
            }
            return builder.ToString();
        }

        public void GenerateId(string name)
        {
            if (!this.Attributes.ContainsKey("id"))
            {
                string str = CreateSanitizedId(name, this.IdAttributeDotReplacement);
                if (!string.IsNullOrEmpty(str))
                {
                    this.Attributes["id"] = str;
                }
            }
        }

        public void MergeAttribute(string key, string value)
        {
            bool replaceExisting = false;
            this.MergeAttribute(key, value, replaceExisting);
        }

        public void MergeAttribute(string key, string value, bool replaceExisting)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException(Resources.ArgumentCannotBeNullOrEmpty, "key");
            }
            if (replaceExisting || !this.Attributes.ContainsKey(key))
            {
                this.Attributes[key] = value;
            }
        }

        public void MergeAttributes<TKey, TValue>(IDictionary<TKey, TValue> attributes)
        {
            bool replaceExisting = false;
            this.MergeAttributes<TKey, TValue>(attributes, replaceExisting);
        }

        public void MergeAttributes<TKey, TValue>(IDictionary<TKey, TValue> attributes, bool replaceExisting)
        {
            if (attributes != null)
            {
                foreach (KeyValuePair<TKey, TValue> pair in attributes)
                {
                    string key = Convert.ToString(pair.Key, CultureInfo.InvariantCulture);
                    string str2 = Convert.ToString(pair.Value, CultureInfo.InvariantCulture);
                    this.MergeAttribute(key, str2, replaceExisting);
                }
            }
        }

        
       

        public override string ToString()
        {
            StringWriter writer = new StringWriter();
            this.Write(writer);
            return writer.ToString();
        }


        public bool IsSelfClosing{ get; set; }

        public void Write(System.IO.TextWriter output)
        {
            if (!IsSelfClosing)
            {

                output.Write('<');
                output.Write(this.TagName);
                this.WriteAttributes(output);
                output.Write('>');
                foreach (IHtmlNode child in this.ChildNodes)
                {
                    child.Write(output);
                }

                output.Write("</");
                output.Write(this.TagName);
                output.Write('>');
            }
            else
            {
                output.Write('<');
                output.Write(this.TagName);
                this.WriteAttributes(output);
                output.Write(" />");

            }
        }

       

        // Properties
        public IDictionary<string, string> Attributes { get; private set; }

        public string IdAttributeDotReplacement
        {
            get
            {
                if (string.IsNullOrEmpty(this._idAttributeDotReplacement))
                {
                    this._idAttributeDotReplacement = HtmlHelper.IdAttributeDotReplacement;
                }
                return this._idAttributeDotReplacement;
            }
            set
            {
                this._idAttributeDotReplacement = value;
            }
        }

        private List<IHtmlNode> _childNodes = new List<IHtmlNode>();
        public List<IHtmlNode> ChildNodes
        {
            get
            {
                return _childNodes;
            }
        }

        public string TagName { get; private set; }

        // Nested Types
        private static class Html401IdUtil
        {
            // Methods
            private static bool IsAllowableSpecialCharacter(char c)
            {
                char ch = c;
                if (((ch != '-') && (ch != ':')) && (ch != '_'))
                {
                    return false;
                }
                return true;
            }

            private static bool IsDigit(char c)
            {
                return (('0' <= c) && (c <= '9'));
            }

            public static bool IsLetter(char c)
            {
                return ((('A' <= c) && (c <= 'Z')) || (('a' <= c) && (c <= 'z')));
            }

            public static bool IsValidIdCharacter(char c)
            {
                if (!IsLetter(c) && !IsDigit(c))
                {
                    return IsAllowableSpecialCharacter(c);
                }
                return true;
            }
        }

        #region IHtmlNode Members

       

        #endregion
    }
}