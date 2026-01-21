namespace ASP_.Net_06_HW.Models;

public class User
{
    private static User? _instance;
    private static readonly object _lock = new object();

    public string FirstName { get; set; } = "Huseyn";
    public string LastName { get; set; } = "Sabziyev";
    public string Email { get; set; } = "hsbziyev@gmail.com";
    public List<string> Skills { get; set; } = new List<string>
    { "C#", "C++", "Python", ".NET", "EF Core", "Microsoft SQL", "Tailwind", "HTML", "CSS", "System Programming", "Network Programming", "Javascript", "React", "ASP .NET Web API" };

    public List<string> ProjectLinks { get; set; } = new List<string>
    {
        "https://github.com/hu55ux/Blogerz.git",
        "https://github.com/hu55ux/E-Commerce.git",
        "https://github.com/hu55ux/Quiz-App.git"
    };
    public string AboutMe { get; set; } = "FullStack Developer & Madridista";
    public string PhotoUrl { get; set; } = "https://avatars.githubusercontent.com/u/192879789";

    private User() { }

    public static User Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new User();
                }
            }
            return _instance;
        }
    }
}