using Core.Component.Library.PagerRecord;
using Models;

namespace Core
{
    public interface ICustomer
    {
        WebPagerRecord<Customer> List(PaginationModel pag);
        Customer Get(int id);
        void Delete(int id );
        void Save(Customer data);
    }
}
