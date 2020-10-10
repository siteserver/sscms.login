using Datory;
using Datory.Annotations;

namespace SSCMS.Login.Models
{
    [DataTable("sscms_login_oauth")]
	public class OAuth : Entity
	{
        [DataColumn]
        public string UserName { get; set; }

        [DataColumn]
        public string Source { get; set; }

        [DataColumn]
        public string UniqueId { get; set; }
    }
}
