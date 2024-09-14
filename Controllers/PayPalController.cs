using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class PayPalController : ControllerBase
{
    private readonly PayPalService _payPalService;

    public PayPalController(PayPalService payPalService)
    {
        _payPalService = payPalService;
    }
    /// <summary>
    /// CreatePlan
    /// </summary>
    /// <returns></returns>
    [HttpPost("create-plan")]
    public IActionResult CreatePlan()
    {
        var plan = _payPalService.CreatePlan();
        return Ok(plan);
    }

    /// <summary>
    /// ActivatePlan
    /// </summary>
    /// <param name="planId"></param>
    /// <returns></returns>
    [HttpPost("activate-plan/{planId}")]
    public IActionResult ActivatePlan(string planId)
    {
        var plan = _payPalService.ActivatePlan(planId);
        return Ok(plan);
    }
}
