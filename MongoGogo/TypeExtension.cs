using System;

namespace MongoGogo
{
    internal static class TypeExtension
    {
        internal static string GetFriendlyName(this Type type)
        {
            string friendlyName = type.Name;
            if (type.IsGenericType)
            {
                int ibacktick = friendlyName.IndexOf('`');
                if (ibacktick > 0)
                {
                    friendlyName = friendlyName.Remove(ibacktick);
                }
                friendlyName += "<";
                Type[] typeParameters = type.GetGenericArguments();
                for (int i = 0; i < typeParameters.Length; ++i)
                {
                    string typeParamName = GetFriendlyName(typeParameters[i]);
                    friendlyName += (i == 0 ? typeParamName : "," + typeParamName);
                }
                friendlyName += ">";
            }

            return friendlyName;
        }

        internal static bool GenericEquals(this Type type, Type genericType)
        {
            var isFormerGeneric = type != null && type.IsGenericType;
            var isLatter = genericType != null && genericType.IsGenericType;
            if (!isFormerGeneric || !isLatter) return false;

            return type.GetGenericTypeDefinition() == genericType;
        }
    }
}
