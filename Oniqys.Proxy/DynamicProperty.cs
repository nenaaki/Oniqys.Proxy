using System;
using System.Reflection;

namespace Oniqys.Proxy
{
    public interface IDynamicPropertyGenerator
    {
        Type GetGetterType();

        Type GetSetterType();

        Delegate CreateGetter(PropertyInfo propertyInfo);

        Delegate CreateSetter(PropertyInfo propertyInfo);
    }

    public class DynamicPropertyGenerator<TClass, TValue> : IDynamicPropertyGenerator
    {
        public Type GetGetterType() => typeof(Func<TClass, TValue>);

        public Type GetSetterType() => typeof(Action<TClass, TValue>);


        public Func<TClass, TValue> CreateGetter(PropertyInfo propertyInfo)
        {
            var getter = propertyInfo.GetGetMethod();
            if (getter == null)
                return null;

            return (Func<TClass, TValue>)Delegate.CreateDelegate(GetGetterType(), getter);
        }

        public Action<TClass, TValue> CreateSetter(PropertyInfo propertyInfo)
        {
            var setter = propertyInfo.GetSetMethod();
            if (setter == null)
                return null;

            return (Action<TClass, TValue>)Delegate.CreateDelegate(GetSetterType(), setter);
        }

        Delegate IDynamicPropertyGenerator.CreateGetter(PropertyInfo propertyInfo) => CreateGetter(propertyInfo);

        Delegate IDynamicPropertyGenerator.CreateSetter(PropertyInfo propertyInfo) => CreateSetter(propertyInfo);
    }
}
