using System.Collections.Generic;
using System.Threading.Tasks;
using Datory;
using SSCMS.Login.Abstractions;
using SSCMS.Login.Models;
using SSCMS.Services;

namespace SSCMS.Login.Core
{
    public class OAuthRepository : IOAuthRepository
    {
        private readonly Repository<OAuth> _repository;

        public OAuthRepository(ISettingsManager settingsManager)
        {
            _repository = new Repository<OAuth>(settingsManager.Database, settingsManager.Redis);
        }

        public string TableName => _repository.TableName;

        public List<TableColumn> TableColumns => _repository.TableColumns;

        private static class Attr
        {
            public const string UserName = nameof(OAuth.UserName);
            public const string Source = nameof(OAuth.Source);
            public const string UniqueId = nameof(OAuth.UniqueId);
        }

        public async Task<int> InsertAsync(OAuth login)
        {
            return await _repository.InsertAsync(login);
        }

        public async Task DeleteAsync(string userName, string source)
        {
            await _repository.DeleteAsync(Q
                .Where(Attr.UserName, userName)
                .Where(Attr.Source, source)
            );
        }

        public async Task<string> GetUserNameAsync(string source, string uniqueId)
        {
            return await _repository.GetAsync<string>(Q
                .Select(Attr.UserName)
                .Where(Attr.Source, source)
                .Where(Attr.UniqueId, uniqueId)
            );
        }
    }
}
