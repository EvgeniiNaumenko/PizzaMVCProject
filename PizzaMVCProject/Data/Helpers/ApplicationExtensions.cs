using Org.BouncyCastle.Tls.Crypto;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace PizzaMVCProject.Data.Helpers
{
    public static class ApplicationExtensions
    {
        public static string GetDescription(this Enum myEnum)
        {
            return myEnum.GetType().GetMember(myEnum.ToString()).First().GetCustomAttribute<DisplayAttribute>().Name;
        }
    }
}
