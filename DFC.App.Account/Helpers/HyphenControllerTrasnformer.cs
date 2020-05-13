using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Routing;

namespace DFC.App.Account.Helpers
{
    public class HyphenControllerTransformer : IOutboundParameterTransformer
    {
        public string TransformOutbound(object value)
        {
            if(value == null)
            {
                return null;
            }
            return Regex.Replace(value.ToString(), "([a-z])([A-Z])", "$1-$2").ToLower();
        }
    }
}
