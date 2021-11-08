using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

namespace Common.Shared
{


    public static class CheckIf
    {
        public static void IsNotNull(object assertion, string msg)
        {
            if (assertion != null)
                throw new InvalidOperationException(msg);
        }
        public static void IsNull(object assertion, string msg)
        {
            if (assertion == null)
            {
                throw new InvalidOperationException(msg);
            }

        }
        public static void IsRequire(bool assertion, string msg)
        {
            if (!assertion)
            {
                throw new PreException(msg);
            }
        }
        public static void IsNotEquals(object obj1, object obj2, string msg)
        {
            if (obj1.Equals(obj2))
            {
                throw new InvalidOperationException(msg);
            }

        }
        public static void IsEquals(object obj1, object obj2, string msg)
        {
            if (!obj1.Equals(obj2))
            {
                throw new InvalidOperationException(msg);
            }
        }
        public static void IsNotEmpty(string stringValue, string msg)
        {
            if (String.IsNullOrWhiteSpace(stringValue))
            {
                throw new InvalidOperationException(msg);
            }
        }
        public static void IsEmpty(IEnumerable collection, string msg)
        {
            if (collection.GetEnumerator().MoveNext())
            {
                throw new InvalidOperationException(msg);
            }
        }
        public static void IsNotEmpty(IEnumerable collection, string msg)
        {
            if (!collection.GetEnumerator().MoveNext())
            {
                throw new InvalidOperationException(msg);
            }
        }
        public static void IsNullOrEmpty(string stringValue, string msg)
        {
            if (String.IsNullOrEmpty(stringValue))
            {
                throw new InvalidOperationException(msg);
            }
        }

        public static void IsError(dynamic value)
        {
            if (value.error == -1)
                throw new Exception(value.message);
        }
    }
}