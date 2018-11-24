using CSharp8583.Attributes;
using CSharp8583.Common;
using CSharp8583.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CSharp8583.Messages
{
    /// <summary>
    /// Used As Base Class Message
    /// </summary>
    public class BaseMessage
    {
        /// <summary>
        /// Properties with ISO Fields Attributes
        /// </summary>
        public IOrderedEnumerable<KeyValuePair<PropertyInfo, IsoFieldAttribute>> MessagePropsIsoFieldAttributes { get; }

        /// <summary>
        /// Common Constructor
        /// </summary>
        public BaseMessage()
        {
            MessagePropsIsoFieldAttributes = this.GetType().PropsAndIsoFieldAttributes<BaseMessage>().OrderBy(p => p.Value.Position);

            IEnumerable<IsoFields> duplicateFields = MessagePropsIsoFieldAttributes.Select(isoFieldAttrs => isoFieldAttrs.Value.Position).GroupBy(isoField => isoField)
                        .Where(group => group.Count() > 1)
                        .Select(group => group.Key);

            if (duplicateFields.Any())
                throw new ArgumentException($"Fields {string.Join(",", duplicateFields)} are defined more than once");
        }

        /// <summary>
        /// All Messages contain an MTI
        /// </summary>
        [IsoField(position: IsoFields.MTI, maxLen: 4, lengthType: LengthType.FIXED, contentType: ContentType.B, dataType: DataType.ASCII)]
        public virtual string MTI { get; set; }

        /// <summary>
        /// All Messages contain a BitMap, Binary Representation
        /// </summary>
        [IsoField(position: IsoFields.BitMap, maxLen: 8, lengthType: LengthType.FIXED, contentType: ContentType.B, dataType: DataType.HEX)]
        public virtual string BitMap { get; set; }

        /// <summary>
        /// Binary representation of BitMap
        /// </summary>
        public virtual string BinaryBitMap => BitMap.ToBinaryStringFromHex();

        /// <summary>
        /// Creates a Formatted Dump of the Generated Message
        /// </summary>
        /// <param name="isoFieldFormatters">(optional) A function that will format the specified ISO Field Values</param>
        /// <returns>string formatted dump</returns>
        public string GetDumpedMessage(Func<IsoFields,string, string> isoFieldFormatters = null)
        {
            Type typeOfInstance = this.GetType();
            var sBuilder = new StringBuilder();

            IOrderedEnumerable<KeyValuePair<PropertyInfo, IsoFieldAttribute>> attrsPropsOfT = typeOfInstance.
                                                            PropsAndIsoFieldAttributes<BaseMessage>().OrderBy(attr => attr.Value.Position);

            foreach((PropertyInfo prop, IsoFieldAttribute attr) in attrsPropsOfT)
            {
                var valueToAppend = prop.GetValue(this, null);

                if(valueToAppend != null)
                {
                    valueToAppend = isoFieldFormatters == null ? valueToAppend : isoFieldFormatters(attr.Position, valueToAppend.ToString());
                    sBuilder.Append($"[{attr.Position.ToString().PadRight(4, ' ')} :  {valueToAppend}]{Environment.NewLine}");
                }
            }

            return sBuilder.ToString();
        }

        /// <summary>
        /// Retrieve KeyValue Pair of Prop and Each ISO Field Attr by Field Position
        /// </summary>
        /// <param name="bitPosition">field position</param>
        /// <returns>KeyValue Pair of Prop and Each ISO Field</returns>
        internal KeyValuePair<PropertyInfo, IsoFieldAttribute> GetPropAndFieldAttrByPos(int bitPosition) => this.MessagePropsIsoFieldAttributes.
                                                                         FirstOrDefault(isoFieldAttr => isoFieldAttr.Value.Position == (IsoFields)bitPosition);

        /// <summary>
        /// Retrieve the Iso Field Attribute of Property
        /// </summary>
        /// <param name="propName">property Name</param>
        /// <returns>Iso Field Attribute of Property</returns>
        internal IsoFieldAttribute GetIsoFieldByPropName(string propName) => this.MessagePropsIsoFieldAttributes.First(p => p.Key.Name == propName).Value;
    }
}
