using System;
using System.Reflection;

namespace Droppy.Tests.PlayMode
{
    public static class ReflectionUtils
    {
        public static void SetPrivateField(object target, string fieldName, object value)
        {
            Type targetType = target.GetType();
            FieldInfo field = targetType.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            field?.SetValue(target, value);
        }
        
        public static void InvokePrivateMethod(object target, string methodName)
        {
            Type targetType = target.GetType();
            MethodInfo methodInformation = targetType.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            methodInformation?.Invoke(target, null);
        }
    }
}