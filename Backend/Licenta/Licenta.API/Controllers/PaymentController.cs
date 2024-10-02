using Licenta.API.Models;
using Licenta.Application.Persistence;
using Licenta.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Licenta.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IFineRepository _fineRepository;
    private readonly IConfiguration _configuration;
    private readonly IMembershipRepository _membershipRepository;
    private readonly IUserRepository _userRepository;

    public PaymentController(IFineRepository fineRepository, IConfiguration configuration, IMembershipRepository membershipRepository, IUserRepository userRepository)
    {
        _fineRepository = fineRepository;
        _configuration = configuration;
        _membershipRepository = membershipRepository;
        _userRepository = userRepository;
        StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
    }

    [HttpPost("Pay-Fine")]
    public async Task<IActionResult> PayFine([FromBody] FinePaymentRequest request)
    {
        if (request == null || request.FineId == Guid.Empty)
        {
            return BadRequest("Invalid request.");
        }

        var fineResult = await _fineRepository.FindByIdAsync(request.FineId);
        if (!fineResult.IsSuccess || fineResult.Value == null)
            return NotFound("Amenda nu a fost găsită.");

        var fine = fineResult.Value;
        if (fine.IsPaid)
            return BadRequest("Amenda a fost deja plătită.");

        var domain = "http://localhost:5173"; 
        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = new List<SessionLineItemOptions>
        {
            new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = fine.Amount * 100,
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Plată amendă",
                        Description = fine.Description
                    }
                },
                Quantity = 1
            }
        },
            Mode = "payment",
            SuccessUrl = domain + $"/succes/succes/?fineId={request.FineId}",


            CancelUrl = domain + $"/succes/cancel/?fineId={request.FineId}"
        };

        var service = new SessionService();
        Session session = service.Create(options);

        return new JsonResult(new { url = session.Url });
    }

        [HttpPost("PurchaseMembership")]
        public async Task<IActionResult> PurchaseMembership([FromBody] MembershipRequest request)
        {
            if (request == null || request.UserId == Guid.Empty)
            {
                return BadRequest("Invalid request.");
            }

            var user = await _userRepository.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var domain = "http://localhost:5173";
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = 1000, 
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Membership",
                                Description = "Access to all audiobooks"
                            }
                        },
                        Quantity = 1
                    }
                },
                Mode = "payment",
               SuccessUrl = domain + $"/succes/succes/?userId={request.UserId}",


            CancelUrl = domain + $"/succes/cancel/?userId={request.UserId}"
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return new JsonResult(new { url = session.Url });
        }

  



}



