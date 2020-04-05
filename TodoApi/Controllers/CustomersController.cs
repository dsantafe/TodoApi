using System.Web.Http;

namespace TodoApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/customers")]
    public class CustomersController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetById(int id)
        {
            var customerFake = "customer-fake";
            return Ok(customerFake);
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var customersFake = new string[] { "customer-1", "customer-2", "customer-3" };
            return Ok(customersFake);
        }
    }
}