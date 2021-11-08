using Core.Component.Library.SQL;

namespace Common.Shared
{
    public class AppDataBase : IDatabase
    {
        public AppDataBase(string cnn)
        {
            this.ConnectionString = cnn;
        }
    }
}
