using Common.Shared;
using Core.Component.Library.PagerRecord;
using Core.Component.Library.SQL;
using Models;
using System.Linq;

namespace Core
{
    public class CustomerService : ICustomer
    {

        public AppDataBase db;
        public readonly Settings _settings;
        public CustomerService(Settings settings)
        {
            _settings = settings;
            db = new AppDataBase(_settings.ConnectionStrings.Cnn);
        }

        public WebPagerRecord<Customer> List(PaginationModel pag)
        {
            var data = new Execute(db, @"SELECT *, COUNT(1) OVER() AS TotalRecords 
                                         FROM Customers 
                                         ORDER BY CURRENT_TIMESTAMP OFFSET (@pageIndex-1)*@pageSize ROWS FETCH FIRST @pageSize ROWS ONLY",
                                 new {
                                     pageIndex = pag.PageIndex,
                                     pageSize =  pag.PageSize })
                                 .Query<Customer>()
                                 .ToList();
            return new WebPagerRecord<Customer>(list: data, page: pag.PageIndex, pageSize: pag.PageSize, allItemsCount: pag.TotalRecords);
        }

        public Customer Get(int id)
        {
            return  new Execute(db, @"SELECT *
                                         FROM Customers 
                                         WHERE id =@id",
                                new
                                {
                                    id = id
                                })
                                .Query<Customer>()
                                .FirstOrDefault();
        }

        public void Delete(int id)
        {
             new Execute(db, @"DELETE FROM Customers 
                               WHERE id =@id",
                            new
                            {
                                id = id
                            })
                            .Query<Customer>()
                            .FirstOrDefault();
        }

        public void Save(Customer data)
        {
            if(data.id == 0)
            {
                new Execute(db, @"INSERT INTO Customers(FirstName,LastName,Document) VALUES(@firstName,@lastName,@document)",
                          new
                          {
                              firstName = data.FirstName,
                              lastName = data.LastName,
                              document = data.Document
                          })
                          .Query<Customer>()
                          .FirstOrDefault();
            }
            else
            {
                new Execute(db, @"UPDATE Customers
                                    SET FirstName = @firstName,
                                        LastName = @lastName,
                                        Document =@document
                                  WHERE id=@id",
                        new
                        {
                            id = data.id,
                            firstName = data.FirstName,
                            lastName = data.LastName,
                            document = data.Document
                        })
                        .Query<Customer>()
                        .FirstOrDefault();
            }
        }
    }
}
