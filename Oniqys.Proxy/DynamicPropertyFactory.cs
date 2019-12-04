using System;
using System.Collections.Generic;
using System.Reflection;

namespace Oniqys.Proxy
{
    public static class DynamicPropertyFactory
    {
        private static readonly Dictionary<Type, Dictionary<Type, IDynamicPropertyGenerator>> _dynamicPropertieGenerators = new Dictionary<Type, Dictionary<Type, IDynamicPropertyGenerator>>();

        public static IDynamicPropertyGenerator Create(Type classType, Type propertyType)
        {
            if (_dynamicPropertieGenerators.TryGetValue(classType, out var propDic))
            {
                if (propDic.TryGetValue(propertyType, out var result))
                {
                    return result;
                }
            }
            else
            {
                propDic = new Dictionary<Type, IDynamicPropertyGenerator>();
                _dynamicPropertieGenerators[classType] = propDic;
            }
            return propDic[propertyType] = (IDynamicPropertyGenerator)Activator.CreateInstance(typeof(DynamicPropertyGenerator<,>).MakeGenericType(classType, propertyType));
        }

        public static DynamicPropertyGenerator<TClass, TProperty> Create<TClass, TProperty>()
        {
            return (DynamicPropertyGenerator<TClass, TProperty>)Create(typeof(TClass), typeof(TProperty));
        }

        public static IDynamicPropertyGenerator Create<TClass>(Type propertyType)
        {
            return Create(typeof(TClass), propertyType);
        }

        public static Func<TClass, TProperty> CreateGetter<TClass, TProperty>(PropertyInfo propertyInfo)
        {
            return Create<TClass, TProperty>().CreateGetter(propertyInfo);
        }

        public static Action<TClass, TProperty> CreateSetter<TClass, TProperty>(PropertyInfo propertyInfo)
        {
            return Create<TClass, TProperty>().CreateSetter(propertyInfo);
        }
    }
}
