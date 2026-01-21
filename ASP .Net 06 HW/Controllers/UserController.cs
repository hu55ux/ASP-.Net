using ASP_.Net_06_HW.Interface;
using ASP_.Net_06_HW.Models;
using Microsoft.AspNetCore.Mvc;
namespace ASP_.Net_06_HW.Controllers;

public class UserController : Controller
{
    private readonly IEmailService _emailService;
    private readonly User _user;

    public UserController(IEmailService emailService, User user)
    {
        _emailService = emailService;
        _user = user;
    }

    public IActionResult Index()
    {
        return View(_user);
    }

    public IActionResult Skills()
    {
        return View(_user);
    }

    public IActionResult Projects()
    {
        return View(_user);
    }

    public IActionResult AboutMe()
    {
        return View(_user);
    }
    public IActionResult Contact()
    {
        return View(_user);
    }

    [HttpPost]
    public async Task<IActionResult> Contact(string visitorEmail, string subject, string message)
    {
        if (!ModelState.IsValid) return View(_user);

        await _emailService.SendEmailAsync(_user.Email, $"New Message: {subject}", message, visitorEmail);

        string thanks = $"Hello, I'm {_user.FirstName}. Thanks for reaching out! I'll get back to you soon.";
        await _emailService.SendEmailAsync(visitorEmail, "Auto-Reply: Thank you!", thanks);

        TempData["MessageSent"] = "Your message has been sent successfully!";
        return RedirectToAction("Contact");
    }


}
