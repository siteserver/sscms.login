using System.Collections.Generic;
using System.Threading.Tasks;
using SSCMS.Login.Models;

namespace SSCMS.Login.Abstractions
{
    public interface ILoginManager
    {
        Task<WeixinSettings> GetWeixinSettingsAsync();

        Task<QqSettings> GetQqSettingsAsync();

        Task<WeiboSettings> GetWeiboSettingsAsync();

        Task SetWeixinSettingsAsync(WeixinSettings settings);

        Task SetQqSettingsAsync(QqSettings settings);

        Task SetWeiboSettingsAsync(WeiboSettings settings);

        List<TemplateInfo> GetTemplateInfoList(string type);

        TemplateInfo GetTemplateInfo(string name);

        void Clone(string nameToClone, TemplateInfo templateInfo, string templateHtml = null);

        void Edit(TemplateInfo templateInfo);

        Task<string> GetTemplateHtmlAsync(TemplateInfo templateInfo);

        void SetTemplateHtml(TemplateInfo templateInfo, string html);

        void DeleteTemplate(string name);
    }
}
