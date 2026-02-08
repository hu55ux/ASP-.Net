using ASP_.Net_06_HW.Interface;
using Microsoft.AspNetCore.Mvc;
namespace ASP_.Net_06_HW.Controllers;

public class UserController : Controller
{
    private readonly IEmailService _emailService;

    public UserController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public IActionResult Index()
    {
        return View(Models.User.Instance);
    }

    public IActionResult Skills()
    {
        return View(Models.User.Instance);
    }

    public IActionResult Projects()
    {
        return View(Models.User.Instance);
    }

    public IActionResult AboutMe()
    {
        return View(Models.User.Instance);
    }

    public IActionResult Contact()
    {
        return View(Models.User.Instance);
    }


    [HttpPost]
    public async Task<IActionResult> Contact(string visitorEmail, string subject, string message)
    {
        if (!ModelState.IsValid) return View(Models.User.Instance);

        await _emailService.SendEmailAsync(Models.User.Instance.Email, $"New Message: {subject}", message, visitorEmail);

        string thanks = $"Hello, I'm {Models.User.Instance.FirstName}. Thanks for reaching out! I'll get back to you soon.";
        await _emailService.SendEmailAsync(visitorEmail, "Auto-Reply: Thank you!", thanks);

        TempData["MessageSent"] = "Your message has been sent successfully!";
        return RedirectToAction("Contact");
    }


}
