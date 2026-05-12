// <copyright file="IsAuthenticatedController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoriaStore.API.Controllers;

[ApiController]
[Route("api/auth")]
public class IsAuthenticatedController() : ControllerBase
{
    [Authorize]
    [HttpGet("verify")]
    public ActionResult Handle(CancellationToken cancellationToken)
    {
        return this.NoContent();
    }
}