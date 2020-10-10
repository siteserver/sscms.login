using System.Collections.Specialized;
using System.Threading.Tasks;
using SSCMS.Parse;
using SSCMS.Plugins;
using SSCMS.Services;
using SSCMS.Utils;

namespace SSCMS.Login.Parser
{
    public class StlLogout : IPluginParseAsync
    {
        public string ElementName => "stl:logout";

        public const string AttributeRedirectUrl = "redirectUrl";

        private readonly IPluginManager _pluginManager;

        public StlLogout(IPluginManager pluginManager)
        {
            _pluginManager = pluginManager;
        }

        public async Task<string> ParseAsync(IParseStlContext context)
        {
            var redirectUrl = string.Empty;
            var attributes = new NameValueCollection();

            ParseUtils.RegisterBodyHtml(context, _pluginManager);

            foreach (var name in context.StlAttributes.AllKeys)
            {
                var value = context.StlAttributes[name];
                if (StringUtils.EqualsIgnoreCase(name, AttributeRedirectUrl))
                {
                    redirectUrl = await context.ParseAsync(value);
                }
                else
                {
                    attributes.Add(name, await context.ParseAsync(value));
                }
            }

            if (string.IsNullOrEmpty(redirectUrl))
            {
                redirectUrl = "/home";
            }

            var innerHtml = await context.ParseAsync(context.StlInnerHtml);
            attributes["href"] = "javascript:;";
            attributes["onclick"] = ParseUtils.OnClickLogout;

            return $"<a {TranslateUtils.ToAttributesString(attributes)}>{innerHtml}</a>";
        }

        //public static object Logout(IRequest request)
        //{
        //    request.UserLogout();
        //    return new object();
        //}
    }
}
