using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PerfumeShop.Models;

namespace PerfumeShop.Controllers.ApiController
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIProductsController : ControllerBase
    {
        private readonly DBContext context;
    }
}
