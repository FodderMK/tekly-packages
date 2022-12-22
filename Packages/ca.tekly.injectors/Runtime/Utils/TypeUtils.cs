using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Tekly.Injectors.Utils
{
    public static class TypeUtils
    {
        public static ConstructorInfo Constructor(this Type type)
        {
            var constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            if (constructors.Length == 0) {
                return null;
            }

            return constructors[0];
        }

        public static bool IsInjectable(MonoBehaviour obj)
        {
            return IsInjectable((object)obj);
        }
        
        public static bool IsInjectable(object obj)
        {
            if (obj == null) {
                return false;
            }

            return IsInjectable(obj.GetType());
        }
        
        public static bool IsInjectable(Type type)
        {
            return Injector.GetFields(type).Any(IsInjectable) || Injector.GetMethods(type).Any(IsInjectable);
        }

        public static bool IsInjectable(FieldInfo fieldInfo)
        {
            return fieldInfo.GetCustomAttribute<InjectAttribute>() != null;
        }
        
        public static bool IsInjectable(MethodInfo fieldInfo)
        {
            return fieldInfo.GetCustomAttribute<InjectAttribute>() != null;
        }
    }
}