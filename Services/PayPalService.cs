using PayPal.Api;

public class PayPalService
{
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _mode;

    /// <summary>
    /// PayPalService
    /// </summary>
    /// <param name="config"></param>
    public PayPalService(IConfiguration config)
    {
        _clientId = config["PayPal:ClientId"] ?? string.Empty;
        _clientSecret = config["PayPal:ClientSecret"] ?? string.Empty;
        _mode = config["PayPal:Mode"] ?? string.Empty;
    }

    private APIContext GetAPIContext()
    {
        var config = new Dictionary<string, string> { { "mode", _mode } };
        var accessToken = new OAuthTokenCredential(_clientId, _clientSecret, config).GetAccessToken();
        return new APIContext(accessToken);
    }

    public Plan CreatePlan()
    {
        var apiContext = GetAPIContext();

        var plan = new Plan
        {
            name = "Monthly Subscription Plan",
            description = "Monthly subscription for our service",
            type = "fixed",
            payment_definitions = new List<PaymentDefinition>
            {
                new PaymentDefinition
                {
                    name = "Regular Payments",
                    type = "REGULAR",
                    frequency = "MONTH",
                    frequency_interval = "1",
                    amount = new Currency { value = "10", currency = "USD" },
                    cycles = "12",
                    charge_models = new List<ChargeModel>
                    {
                        new ChargeModel
                        {
                            type = "SHIPPING",
                            amount = new Currency { value = "1", currency = "USD" }
                        }
                    }
                }
            },
            merchant_preferences = new MerchantPreferences
            {
                setup_fee = new Currency { value = "1", currency = "USD" },
                cancel_url = "https://www.yoursite.com/cancel",
                return_url = "https://www.yoursite.com/success",
                auto_bill_amount = "YES",
                initial_fail_amount_action = "CONTINUE",
                max_fail_attempts = "1"
            }
        };

        return plan.Create(apiContext);
    }

    public Plan ActivatePlan(string planId)
    {
        var apiContext = GetAPIContext();

        var patch = new Patch
        {
            op = "replace",
            path = "/",
            value = new Plan
            {
                state = "ACTIVE"
            }
        };

        var patchRequest = new PatchRequest { patch };

        var plan = Plan.Get(apiContext, planId);
        plan.Update(apiContext, patchRequest);

        return plan;
    }
}
