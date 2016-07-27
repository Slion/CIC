using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Globalization;

namespace SharpLib.Utils
{
    /// <summary>
    /// Allow serialization into JSON.
    /// Most useful to be able to save complex settings for instance.
    /// Usage:
    /// ...
    /// [TypeConverter(typeof(TypeConverterJson<PersistantObject>))]
    /// [DataContract]
    /// public class PersistantObject
    /// ...
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TypeConverterJson<T> : TypeConverter where T : class
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            else
                return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Internalize
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string stringValue = value as string;
            if (stringValue != null)
            {
                //try
                //{
                    //Load object form JSON string
                    byte[] byteArray = Encoding.UTF8.GetBytes(stringValue);
                    MemoryStream stream = new MemoryStream(byteArray);
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T),
                        new DataContractJsonSerializerSettings()
                        {
                            UseSimpleDictionaryFormat = true
                        });
                    T settings = (T)ser.ReadObject(stream);
                    return settings;
                //}
                //catch (Exception ex)
                //{
                    //That's not helping with partial loading
                //    Console.WriteLine("WARNING: Internalize exception -" + ex.ToString() );
                //    return null;
                //}
            }
            else
            {
                return base.ConvertFrom(context, culture, value);
            }
                
        }

        /// <summary>
        /// Externalize
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                //Save settings into JSON string
                MemoryStream stream = new MemoryStream();
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T), new DataContractJsonSerializerSettings()
                {
                    UseSimpleDictionaryFormat = true
                });
                ser.WriteObject(stream, value);
                // convert stream to string
                stream.Position = 0;
                StreamReader reader = new StreamReader(stream);
                string text = reader.ReadToEnd();
                return text;
            }
            else
            {
                return base.ConvertTo(context,culture,value,destinationType);
            }
        }
    }
}