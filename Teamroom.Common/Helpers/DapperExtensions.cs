using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using HobbyClue.Common.Attributes;
using HobbyClue.Common.Models;

namespace HobbyClue.Common.Helpers
{
    public static class DapperExtensions
    {
        public static void AddParams<T>(this IDbCommand cmd, params object[] args)
        {
            foreach (var item in args)
            {
                AddParam<T>(cmd, item);
            }
        }

        public static void AddParam<T>(this IDbCommand cmd, object item)
        {
            var p = cmd.CreateParameter();
            p.ParameterName = string.Format("@{0}", cmd.Parameters.Count);
            if (item == null)
            {
                p.Value = DBNull.Value;
            }
            else
            {
                if (item is Guid)
                {
                    p.Value = item.ToString();
                    p.DbType = DbType.Guid;
                    p.Size = 4000;
                }
                if (item is Enum)
                {
                    p.Value = Convert.ToInt32(item);
                }
                else if (item.GetType() == typeof(IDictionary<string, T>))
                {
                    var d = (IDictionary<string, T>)item;
                    p.Value = d.Values.FirstOrDefault();
                }
                else
                {
                    p.Value = item;
                }

                if (item is string)
                    p.Size = ((string)item).Length > 4000 ? -1 : 4000;
            }
            cmd.Parameters.Add(p);
        }

        public static dynamic ToDynamic(this object value, bool isInsert = false)
        {
            IDictionary<string, object> expando = new ExpandoObject();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(value.GetType()))
            {
                var isAutoSupplied = property.Attributes[typeof(AutoSuppliedFromDatabaseAttribute)] != null;
                var ignore = property.Attributes[typeof(DapperIgnoreOnSaveOrUpdateAttribute)] != null;

                if (!ignore && (isInsert && !isAutoSupplied || !isInsert))
                {
                    var propValue = property.GetValue(value);
                    if (propValue != null && propValue.GetType().FullName == "System.DateTime")
                    {
                        var dateValue = (DateTime) propValue;
                        if (dateValue == DateTime.MinValue)
                        {
                            dateValue = SqlDateTime.MinValue.Value;
                            expando.Add(property.Name, dateValue);
                        }
                        else
                            expando.Add(property.Name, property.GetValue(value));
                    }
                    else
                        expando.Add(property.Name, property.GetValue(value));
                }
            }
            return expando as ExpandoObject;
        }

        public static void SetPrimaryKey(this object obj, object value)
        {
            var keyField = obj.GetType().GetProperties().FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(PrimaryKeyAttribute)) && !Attribute.IsDefined(prop, typeof(DapperIgnoreOnSaveOrUpdateAttribute)));
            if (keyField != null)
            {
                keyField.SetValue(obj, ChangeType(value, keyField.PropertyType));
            }
        }

        public static object ChangeType(object sourceValue, Type targetType)
        {
            ///FIX: Invalid cast for new-style GUIDs
            if (sourceValue is string && targetType == typeof(Guid))
            {
                return Guid.Parse((string)sourceValue);
            }

            if (sourceValue is byte[] && targetType == typeof (Guid))
                return new Guid((byte[]) sourceValue);

            if (sourceValue is DateTime && targetType == typeof(DateTimeOffset))
            {
                return new DateTimeOffset((DateTime)sourceValue);
            }

            return Convert.ChangeType(sourceValue, targetType, CultureInfo.InvariantCulture);
        }

        public static PropertyAttribute GetPrimaryKeyField(this object obj)
        {
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(obj.GetType()))
            {
                var isPrimaryKeyField = property.Attributes[typeof(PrimaryKeyAttribute)] != null && property.Attributes[typeof(DapperIgnoreOnSaveOrUpdateAttribute)] == null;
                if (isPrimaryKeyField)
                {
                    return new PropertyAttribute
                    {
                        Name = property.Name,
                        Type = property.PropertyType,
                        Value = property.GetValue(obj)
                    };
                }
            }
            return null;
        }
    }
}