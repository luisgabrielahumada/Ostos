using Core;
using Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

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
        public HttpMessage<dynamic> List(int pageIndex, int pageSize)
        {

            //TODO: llamar al metodo que valida el usuario en la base de datos.
            var data = _process.List(new PaginationModel
            {
                PageIndex = pageIndex,
                PageSize = pageSize
            });

            return new HttpMessage<dynamic>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Data = data
            };
        }

        [HttpGet]
        [AllowAnonymous]
        public HttpMessage<dynamic> Get(int id)
        {

            //TODO: llamar al metodo que valida el usuario en la base de datos.
            var data = _process.Get(new Dictionary<string, object> { {"id", id } });

            return new HttpMessage<dynamic>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Data =  data
            };
        }

        [HttpPost]
        [AllowAnonymous]
        public HttpMessage<dynamic> Delete(Dictionary<string, object> req)
        {

            //TODO: llamar al metodo que valida el usuario en la base de datos.
            _process.Delete(req);

            return new HttpMessage<dynamic>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true
            };
        }

        [HttpPost]
        [AllowAnonymous]
        public HttpMessage<dynamic> Save(Dictionary<string, object> req)
        {

            //TODO: llamar al metodo que valida el usuario en la base de datos.
            _process.Save(req);

            return new HttpMessage<dynamic>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true
            };
        }
    }
}
