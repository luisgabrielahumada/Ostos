using Common.Shared;
using Core.Component.Library.PagerRecord;
using Core.Component.Library.SQL;
using Core.Component.Library.WebTools;
using System.Collections.Generic;
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

        public dynamic List(PaginationModel pag)
        {
            var data = new Execute(db, @"SELECT *, COUNT(1) OVER() AS TotalRecords 
                                         FROM Customers 
                                         ORDER BY CURRENT_TIMESTAMP OFFSET (@pageIndex-1)*@pageSize ROWS FETCH FIRST @pageSize ROWS ONLY",
                                 new {
                                     pageIndex = pag.PageIndex,
                                     pageSize =  pag.PageSize })
                                 .Query<dynamic>()
                                 .ToList();
            return new WebPagerRecord<dynamic>(list: data, page: pag.PageIndex, pageSize: pag.PageSize, allItemsCount: pag.TotalRecords);
        }

        public dynamic Get(Dictionary<string, object> req)
        {
            return  new Execute(db, @"SELECT *
                                         FROM Customers 
                                         WHERE id =@id",
                                new
                                {
                                    id = req.GetInteger("id"),
                                })
                                .Query<dynamic>()
                                .FirstOrDefault();
        }

        public void Delete(Dictionary<string, object> req)
        {
             new Execute(db, @"DELETE FROM Customers 
                               WHERE id =@id",
                            new
                            {
                                id = req.GetInteger("id"),
                            })
                            .Query<dynamic>()
                            .FirstOrDefault();
        }

        public void Save(Dictionary<string, object> req)
        {
            if(req.GetInteger("id") == 0)
            {
                new Execute(db, @"INSERT INTO Customers(FirstName,LastName,Document) VALUES(@firstName,@lastName,@document)",
                          new
                          {
                              firstName = req.GetString("firstName"),
                              lastName = req.GetString("lastName"),
                              document = req.GetString("document")
                          })
                          .Query<dynamic>()
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
                            id = req.GetInteger("id"),
                            firstName = req.GetString("firstName"),
                            lastName = req.GetString("lastName"),
                            document = req.GetString("document")
                        })
                        .Query<dynamic>()
                        .FirstOrDefault();
            }
        }
    }
}
