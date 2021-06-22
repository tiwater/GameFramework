using System;
using System.Collections.Generic;
using System.Reflection;
using GameFramework.GameStructure.GameItems.ObjectModel;
using Newtonsoft.Json;

namespace GameFramework.GameStructure.Util
{
    public class ObjectUtil
    {
        public static T Clone<T>(T o)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(o));
        }

        public static string ExportGameItemMeta(GameItem gameItem)
        {
            return null;
        }

        /// <summary>
        /// Copy the values from the src object to the dest object
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        /// <param name="deepCopy"></param>
        /// <param name="ignoreEmbeddedObj">If true, will not copy the embedded object in shallow copy mode. This parameter is ignored in deep copy mode</param>
        public static void CopyObject(object dest, object src, bool deepCopy = false, bool ignoreEmbeddedObj = true)
        {
            //Fields
            FieldInfo[] fields = dest.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < fields.Length; i++)
            {
                if (deepCopy)
                {
                    FieldInfo field = fields[i];
                    var fieldValue = field.GetValue(src);
                    ///The value type, string, enum has no deep copy
                    if (fieldValue == null || fieldValue.GetType().IsValueType || fieldValue.GetType().Equals(typeof(System.String)) || fieldValue.GetType().IsEnum)
                    {
                        field.SetValue(dest, fieldValue);
                    }
                    else
                    {
                        //Get target embedded object
                        object targetValue = field.GetValue(dest);
                        if (targetValue == null)
                        {
                            //If it's null, then create a new one
                            //If didn't have public constructor with no parameter, exception will be thrown here
                            targetValue = Activator.CreateInstance(fieldValue.GetType());
                            field.SetValue(dest, targetValue);
                        }
                        CopyObject(targetValue, fieldValue, deepCopy);
                    }
                }
                else
                {
                    FieldInfo field = fields[i];
                    if (ignoreEmbeddedObj)
                    {

                        var fieldValue = field.GetValue(src);
                        if (fieldValue != null && (fieldValue.GetType().IsValueType || fieldValue.GetType().Equals(typeof(System.String)) || fieldValue.GetType().IsEnum))
                        {
                            //Normal value, set directly
                            field.SetValue(dest, fieldValue);
                        }
                        else
                        {
                            if (fieldValue == null)
                            {
                                //Get target embedded object to judge the type
                                object targetValue = field.GetValue(dest);
                                if(targetValue != null && (targetValue.GetType().IsValueType || targetValue.GetType().Equals(typeof(System.String)) || targetValue.GetType().IsEnum))
                                {
                                    //Normal value type, set it
                                    field.SetValue(dest, fieldValue);
                                }
                            } else
                            {
                                //fieldValue is not null which means this is an embedded object field, so ignore it
                            }
                        } 
                    }
                    else
                    {
                        field.SetValue(dest, field.GetValue(src));
                    }
                }
            }
            //Properties
            PropertyInfo[] properties = dest.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < properties.Length; i++)
            {
                if (deepCopy)
                {
                    PropertyInfo property = properties[i];
                    var fieldValue = property.GetValue(src);
                    ///The value type, string, enum has no deep copy
                    if (fieldValue == null || fieldValue.GetType().IsValueType || fieldValue.GetType().Equals(typeof(System.String)) || fieldValue.GetType().IsEnum)
                    {
                        property.SetValue(dest, fieldValue);
                    }
                    else
                    {
                        //Get target embedded object
                        object targetValue = property.GetValue(dest);
                        if (targetValue == null)
                        {
                            //If it's null, then create a new one
                            //If didn't have public constructor with no parameter, exception will be thrown here
                            targetValue = Activator.CreateInstance(fieldValue.GetType());
                            property.SetValue(dest, targetValue);
                        }
                        CopyObject(targetValue, fieldValue, deepCopy);
                    }
                }
                else
                {
                    PropertyInfo property = properties[i];
                    if (ignoreEmbeddedObj)
                    {

                        var fieldValue = property.GetValue(src);
                        if (fieldValue != null && (fieldValue.GetType().IsValueType || fieldValue.GetType().Equals(typeof(System.String)) || fieldValue.GetType().IsEnum))
                        {
                            //Normal value, set directly
                            property.SetValue(dest, fieldValue);
                        }
                        else
                        {
                            if (fieldValue == null)
                            {
                                //Get target embedded object to judge the type
                                object targetValue = property.GetValue(dest);
                                if (targetValue != null && (targetValue.GetType().IsValueType || targetValue.GetType().Equals(typeof(System.String)) || targetValue.GetType().IsEnum))
                                {
                                    //Normal value type, set it
                                    property.SetValue(dest, fieldValue);
                                }
                            }
                            else
                            {
                                //fieldValue is not null which means this is an embedded object field, so ignore it
                            }
                        }
                    }
                    else
                    {
                        property.SetValue(dest, property.GetValue(src));
                    }
                }
            }
        }



        /// <summary>
        /// Compare whether two dictionaries are equal. Return the first different key if found
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        /// <returns></returns>
        public static List<T> CompareObjects<T, M>(Dictionary<T, M> dest, Dictionary<T, M> src)
        {
            List<T> differentFields = new List<T>();

            //Check the dictionary
            foreach(var pair in dest)
            {
                if (!src.ContainsKey(pair.Key))
                {
                    //If the dest key existed in src?
                    differentFields.Add(pair.Key);
                    return differentFields;
                } else
                {
                    //Yes
                    if (CompareObjects(src[pair.Key], pair.Value).Count != 0)
                    {
                        //Compare the value
                        differentFields.Add(pair.Key);
                        return differentFields;
                    }
                }
            }

            if (dest.Count < src.Count)
            {
                foreach (var pair in src)
                {
                    if (!dest.ContainsKey(pair.Key))
                    {
                        //If the dest key existed in src?
                        differentFields.Add(pair.Key);
                        return differentFields;
                    }
                    else
                    {
                        //Yes
                        if (CompareObjects(dest[pair.Key], pair.Value).Count != 0)
                        {
                            //Compare the value
                            differentFields.Add(pair.Key);
                            return differentFields;
                        }
                    }
                }
            }

            return differentFields;
        }

        /// <summary>
        /// Compare the direct fields and properties of two objects, return the name list of the fields which are different in the values
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        /// <returns></returns>
        public static List<string> CompareObjects(object dest, object src)
        {
            List<string> differentFields = new List<string>();

            //Fields
            FieldInfo[] fields = dest.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                var fieldValue = field.GetValue(src);
                ///The value type, string, enum can compare directly
                if (fieldValue == null || fieldValue.GetType().IsValueType || fieldValue.GetType().Equals(typeof(System.String)) || fieldValue.GetType().IsEnum)
                {
                    var targetValue = field.GetValue(dest);
                    if (targetValue == fieldValue)
                    {//Do nothing for equal
                    }
                    else
                    {
                        if (targetValue == null || fieldValue == null || fieldValue.ToString() != targetValue.ToString())
                        {
                            differentFields.Add(field.Name);
                        }
                    }
                }
            }
            //Properties
            PropertyInfo[] properties = dest.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo property = properties[i];
                var fieldValue = property.GetValue(src);
                ///The value type, string, enum has no deep copy
                if (fieldValue == null || fieldValue.GetType().IsValueType || fieldValue.GetType().Equals(typeof(System.String)) || fieldValue.GetType().IsEnum)
                {
                    var targetValue = property.GetValue(dest);
                    if (targetValue == fieldValue)
                    {//Do nothing for equal
                    }
                    else
                    {
                        if (targetValue == null || fieldValue == null || targetValue.ToString() != fieldValue.ToString())
                        {
                            differentFields.Add(property.Name);
                        }
                    }
                }
            }
            return differentFields;
        }

        /// <summary>
        /// Try to parse the provided object to a specific type
        /// </summary>
        /// <typeparam name="T">The type we want to conver to</typeparam>
        /// <param name="obj">Object to convert</param>
        /// <param name="defaultValue">The default value if cannot parse</param>
        /// <returns></returns>
        public static T TryParse<T>(object obj, T defaultValue = default(T))
        {
            if (obj == null)
                return defaultValue;

            Type t = typeof(T);
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))   //支持可空类型
                t = t.GetGenericArguments()[0];

            var tryParse = t.GetMethod("TryParse", BindingFlags.Public | BindingFlags.Static, Type.DefaultBinder
                , new Type[] { obj.GetType(), t.MakeByRefType() }
                , new ParameterModifier[] { new ParameterModifier(2) });

            if (tryParse != null)
            {
                var parameters = new object[] { obj, Activator.CreateInstance(t) };
                bool success = (bool)tryParse.Invoke(null, parameters);
                if (success)
                    return (T)parameters[1];
                else
                    return defaultValue;
            }

            return (T)Convert.ChangeType(obj, typeof(T));
        }
    }
}