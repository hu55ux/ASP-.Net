using System.Text.RegularExpressions;
class PasswordChecker : BaseChecker
{
    public override bool Check(object request)
    {
        if (request is User user)
        {
            if (!string.IsNullOrWhiteSpace(user.Password) &&
                new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$").IsMatch(user.Password))
            {
                return Next.Check(request);
            }
        }
        return false;
    }
}