using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MockAcquiringBank.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AcquiringBankController : ControllerBase
    {

        [HttpPost]
        public ActionResult Post([FromBody] AcquiringBankPostModel model)
        {
            if (int.TryParse(model?.CardNumber?.First().ToString(), out var firstDigit))
            {
                if (firstDigit == 3)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }

                if (firstDigit == 4 || firstDigit == 5 || firstDigit == 6)
                {
                    return Created("", Guid.NewGuid());
                }

            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    ////http://www.getcreditcardnumbers.com/
    //"5147004213414803", // Mastercard
    //"6011491706918120", // Discover
    //"379616680189541", // American Express
    //"4916111026621797", // Visa
}
