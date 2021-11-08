using Core;
using Core.Component.Library.PagerRecord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Helper;
using Models;

namespace Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/Customer/[action]")]
    public class CustomerController : ControllerBase
    {
        private ICustomer _process;
        public CustomerController(ICustomer process)
        {
            _process = process;
        }

        [HttpGet]
        [AllowAnonymous]
        public HttpMessage<WebPagerRecord<Customer>> List(int pageIndex, int pageSize)
        {

            //TODO: llamar al metodo que valida el usuario en la base de datos.
            var data = _process.List(new PaginationModel
            {
                PageIndex = pageIndex,
                PageSize = pageSize
            });

            return new HttpMessage<WebPagerRecord<Customer>>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Data = data
            };
        }

        [HttpGet]
        [AllowAnonymous]
        public HttpMessage<Customer> Get(int id)
        {

            //TODO: llamar al metodo que valida el usuario en la base de datos.
            var data = _process.Get(id);

            return new HttpMessage<Customer>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Data =  data
            };
        }

        [HttpPost]
        [AllowAnonymous]
        public HttpMessage<Customer> Delete(int data)
        {

            //TODO: llamar al metodo que valida el usuario en la base de datos.
            _process.Delete(data);

            return new HttpMessage<Customer>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true
            };
        }

        [HttpPost]
        [AllowAnonymous]
        public HttpMessage<Customer> Save(Customer data)
        {

            //TODO: llamar al metodo que valida el usuario en la base de datos.
            _process.Save(data);

            return new HttpMessage<Customer>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true
            };
        }
    }
}
