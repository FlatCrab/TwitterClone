namespace API.Core.User.exceptions;

public class UsernameTooLongException(string err) : Exception(err,null)
{
    
}