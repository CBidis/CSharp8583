using CSharp8583.Attributes;
using CSharp8583.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CSharp8583.Messages
{
    /// <summary>
    /// Contains the Fields for Custom Fields. eg Field63 etc
    /// </summary>
    public class CustomField
    {
        /// <summary>
        /// Properties of Custom Field with Tag Attributes
        /// </summary>
        public IOrderedEnumerable<KeyValuePair<PropertyInfo, TagAttribute>> TagPropsIsoFieldAttributes { get; }

        /// <summary>
        /// Common Contructor
        /// </summary>
        public CustomField()
        {
            TagPropsIsoFieldAttributes = this.GetType().PropsAndTagsdAttributes<CustomField>().OrderBy(p => p.Value.Position);

            IEnumerable<int> duplicateTagsPos = TagPropsIsoFieldAttributes.Select(isoTagAttr => isoTagAttr.Value.Position).GroupBy(pos => pos)
                                .Where(group => group.Count() > 1)
                                .Select(group => group.Key);

            if (duplicateTagsPos.Any())
                throw new ArgumentException($"Tags {string.Join(",", duplicateTagsPos)} are defined more than once in the same position");
        }

        /// <summary>
        /// Retrieves Instance Prop by Name
        /// </summary>
        /// <param name="propName">prop Name</param>
        /// <returns>property</returns>
        internal PropertyInfo GetPropByName(string propName)
        {
            if (string.IsNullOrEmpty(propName))
                return null;

            (PropertyInfo prop, TagAttribute propTag) = TagPropsIsoFieldAttributes.FirstOrDefault(p => p.Value.TagName == propName);
            return prop ?? null;
        }

        /// <summary>
        /// Builds Custom Field from Bytes Of Field
        /// </summary>
        /// <param name="fieldBytes">field bytes</param>
        /// <returns>custom field Instance</returns>
        public virtual CustomField ParseCustomField(byte[] fieldBytes) => throw new NotImplementedException();

        /// <summary>
        /// Builds Field Bytes from Instance of Custom Fields
        /// </summary>
        /// <returns>bytes of Custom Field</returns>
        public virtual byte[] BuildCustomField() => throw new NotImplementedException();

        /// <summary>
        /// Prints the Value/Tags of Of the SubField
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sBuilder = new StringBuilder();
            sBuilder.Append($"{Environment.NewLine}");

            foreach ((PropertyInfo prop, TagAttribute attr) in TagPropsIsoFieldAttributes)
            {
                var valueToAppend = prop.GetValue(this, null);

                if (valueToAppend != null)
                {
                    sBuilder.Append($"       [Tag {attr.TagName.PadRight(4, char.MinValue)} " +
                                    $"Len {valueToAppend.ToString().Length.ToString().PadRight(4, ' ')} :  {valueToAppend}]{Environment.NewLine}");
                }
            }

            return sBuilder.ToString();
        }
    }
}
