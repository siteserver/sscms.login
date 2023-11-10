﻿using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Web;
using SSCMS.Configuration;
using SSCMS.Parse;
using SSCMS.Plugins;
using SSCMS.Repositories;
using SSCMS.Services;
using SSCMS.Utils;

namespace SSCMS.Login.Core
{
    public class StlLogin : IPluginParseAsync
    {
        public string ElementName => "stl:login";

        public const string AttributeType = "type";
        public const string AttributeUrl = "url";
        public const string AttributeRedirectUrl = "redirectUrl";

        private readonly IPathManager _pathManager;
        private readonly ISiteRepository _siteRepository;

        public StlLogin(IPathManager pathManager, ISiteRepository siteRepository)
        {
            _pathManager = pathManager;
            _siteRepository = siteRepository;
        }

        public async Task<string> ParseAsync(IParseStlContext context)
        {
            var type = string.Empty;
            var url = string.Empty;
            var redirectUrl = await context.GetCurrentUrlAsync();
            var attributes = new NameValueCollection();

            foreach (var name in context.StlAttributes.AllKeys)
            {
                var value = context.StlAttributes[name];
                if (StringUtils.EqualsIgnoreCase(name, AttributeType))
                {
                    type = await context.ParseAsync(value);
                }
                else if (StringUtils.EqualsIgnoreCase(name, AttributeUrl))
                {
                    url = await context.ParseAsync(value);
                }
                else if (StringUtils.EqualsIgnoreCase(name, AttributeRedirectUrl))
                {
                    redirectUrl = await context.ParseAsync(value);
                }
                else
                {
                    attributes.Add(name, await context.ParseAsync(value));
                }
            }

            var site = await _siteRepository.GetAsync(context.SiteId);
            var apiUrl = _pathManager.GetApiHostUrl(site, Constants.ApiPrefix);

            if (!string.IsNullOrEmpty(url))
            {
                var parsedUrl = string.Empty;

                if (StringUtils.EqualsIgnoreCase(url, OAuthType.Weibo.Value))
                {
                    parsedUrl = $"{ApiUtils.GetAuthUrl(OAuthType.Weibo)}?redirectUrl={HttpUtility.UrlEncode(redirectUrl)}";
                }
                else if (StringUtils.EqualsIgnoreCase(url, OAuthType.Weixin.Value))
                {
                    parsedUrl = $"{ApiUtils.GetAuthUrl(OAuthType.Weixin)}?redirectUrl={HttpUtility.UrlEncode(redirectUrl)}";
                }
                else if (StringUtils.EqualsIgnoreCase(url, OAuthType.Qq.Value))
                {
                    parsedUrl = $"{ApiUtils.GetAuthUrl(OAuthType.Qq)}?redirectUrl={HttpUtility.UrlEncode(redirectUrl)}";
                }
                else if (StringUtils.EqualsIgnoreCase(url, "logout"))
                {
                    parsedUrl = _pathManager.GetApiHostUrl(site, $"assets/login/templates/logout/index.html?apiUrl={HttpUtility.UrlEncode(apiUrl)}&redirectUrl={HttpUtility.UrlEncode(redirectUrl)}");
                }

                if (!string.IsNullOrEmpty(parsedUrl))
                {
                    if (context.IsStlEntity)
                    {
                        return parsedUrl;
                    }

                    attributes["href"] = parsedUrl;

                    return $@"<a {TranslateUtils.ToAttributesString(attributes)}>{context.StlInnerHtml}</a>";
                }
            }

            if (string.IsNullOrEmpty(type))
            {
                type = "login-account";
            }

            var elementId = $"iframe_{StringUtils.GetShortGuid(false)}";
            var libUrl = _pathManager.GetApiHostUrl(site, "assets/login/lib/iframe-resizer-3.6.3/iframeResizer.min.js");
            var pageUrl = await _pathManager.GetSiteUrlAsync(site, $"assets/login/templates/{type}/index.html?apiUrl={HttpUtility.UrlEncode(apiUrl)}&redirectUrl={HttpUtility.UrlEncode(redirectUrl)}",false);
            return $@"
<iframe id=""{elementId}"" frameborder=""0"" scrolling=""no"" src=""{pageUrl}"" style=""width: 1px;min-width: 100%;""></iframe>
<script type=""text/javascript"" src=""{libUrl}""></script>
<script type=""text/javascript"">iFrameResize({{log: false}}, '#{elementId}')</script>
";
        }
    }
}
