using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        // API sees them like identical thats why we are gonna make a different name on each of them on the httpget request
        [HttpGet("not-found")]
        public ActionResult GetNotFound()
        {
            return NotFound();
        }
        [HttpGet("bad-request")]
        // we return this bad request if we attempt to save something into our database and for whatever reasons there is no changes to be saved in there
        public ActionResult GetBadRequest(){
            return BadRequest(new ProblemDetails{Title="This is a bad request!"});
        }
        [HttpGet("unauthorised")]
        // return this if a user is not authenticated
        public ActionResult GetUnauthorised(){
            return Unauthorized();
        }
        // return this if the user forgets to fill the required fields in a form or something ...
        [HttpGet("validation-error")]
        public ActionResult GetValidationError(){
            // returns a 400 bad request and an array of errors that will show up.
            ModelState.AddModelError("Problem1","This is the first error");
            ModelState.AddModelError("Problem2","This is the first error");
            // validation problem e kthen nje pergjigje nja modelstate errors
            return ValidationProblem();
        }
        [HttpGet("server-error")]
        public ActionResult GetServerError(){
            throw new Exception("This is a server error");
        }
    }
}