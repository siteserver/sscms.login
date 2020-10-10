using System.Threading.Tasks;
using SSCMS.Login.Core;
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
    }
}
