using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.Identity.Web.Resource;
using System.Security.Claims;

namespace CC.Functions
{
    public static class GetOrders
    {
        [FunctionName("GetOrders")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ClaimsPrincipal principal, 
            ILogger log)
        {

            log.LogInformation("GetOrders function processed a request");

            /* foreach (Claim claim in principal.Claims)
            {
                log.LogInformation($"{claim.Type} - {claim.Value}");
            } */

            req.HttpContext.ValidateAppRole(new string[] { "Orders.Read" });

            return (ActionResult)new OkObjectResult(new List<object> {
                new {
                    Id = 1,
                    OrderDate = new DateTime(2020, 1, 6),
                    Region = "east",
                    Rep = "Jones",
                    Item = "Pencil",
                    Units = 95,
                    UnitCost = 1.99,
                    Total = 189.05
                },
                new {
                    Id = 2,
                    OrderDate = new DateTime(2020, 1, 23),
                    Region = "central",
                    Rep = "Kivell",
                    Item = "Binder",
                    Units = 50,
                    UnitCost = 19.99,
                    Total = 999.50
                },
                new {
                    Id = 3,
                    OrderDate = new DateTime(2020, 2, 9),
                    Region = "central",
                    Rep = "Jardine",
                    Item = "Pencil",
                    Units = 36,
                    UnitCost = 4.99,
                    Total = 179.64
                },
                new {
                    Id = 4,
                    OrderDate = new DateTime(2020, 2, 26),
                    Region = "central",
                    Rep = "Gill",
                    Item = "Pen",
                    Units = 27,
                    UnitCost = 19.99,
                    Total = 539.73
                },
                new {
                    Id = 5,
                    OrderDate = new DateTime(2020, 3, 15),
                    Region = "west",
                    Rep = "Sorvino",
                    Item = "Pencil",
                    Units = 56,
                    UnitCost = 2.99,
                    Total = 167.44
                }
            });
        }
    }
}
