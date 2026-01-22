namespace API.Core.User.exceptions;

public class UsernameTooShortException(string err) : Exception(err,null)
{
}