using MyProject.Domain.Enums;
using MyProject.Helpers.Attributes;
using System.Reflection;

namespace MyProject.Helpers
{
    public static class EnumHelper
    {
        public static string GetDescription(this object obj, Language language = Language.Arabic)
        {
            return DescriptionAnnotation.GetDescription(obj, language);
        }
    }
}
