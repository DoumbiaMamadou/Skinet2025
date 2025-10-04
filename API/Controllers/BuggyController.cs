using System;
using API.DTOs;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BuggyController : BaseApiController
{
    [HttpGet("unauthorized")]
     public IActionResult GetUnthorized()
    {
        return Unauthorized();
    }

    public IActionResult GetBadRequest()
    {
        return BadRequest("Ce n'est pas une bonne requête ");
    }

    [HttpPost("notfound")]
    public IActionResult GetNotFound()
    {
        return NotFound();
    }

    [HttpGet("internalerror")]
    public IActionResult GetInternalError()
    {
        throw new Exception(" C'est une erreur interne.");
    }

    [HttpGet("validationerror")]
    public IActionResult GetUValidationError(CreateProductDTO product)
    {
        return Ok();
    }

}
