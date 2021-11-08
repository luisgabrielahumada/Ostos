using System.Collections.Generic;

namespace Core
{
    public interface ICustomer
    {
        dynamic List(PaginationModel pag);
        dynamic Get(Dictionary<string, object> req);
        void Delete(Dictionary<string, object> req);
        void Save(Dictionary<string, object> req);
    }
}
