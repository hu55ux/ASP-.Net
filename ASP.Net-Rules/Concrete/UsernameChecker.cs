
class UsernameChecker : BaseChecker
{
    public override bool Check(object request)
    {
        if (request is User user)
        {
            if (!string.IsNullOrWhiteSpace(user.Username))
            {
                return Next.Check(request);
            }
        }
        return false;
    }
}