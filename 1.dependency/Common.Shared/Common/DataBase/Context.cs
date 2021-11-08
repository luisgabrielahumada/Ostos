using Common.Infrastructure.Utils;
using System.Text;

namespace Common.Shared
{
    public class Context
    {
        public string Token { get; set; }
        public bool IsLocal { get; set; } = false;

        public AppDataBase appDataBase
        {
            get
            {
                if (string.IsNullOrEmpty(this.Token))
                    return null;

                var token = this.Token.DecryptData();
                return new AppDataBase(token);
            }
        }
    }
}
