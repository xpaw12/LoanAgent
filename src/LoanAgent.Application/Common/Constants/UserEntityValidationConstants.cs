namespace LoanAgent.Application.Common.Constants;

public static class UserEntityValidationConstants
{
    public const int UsernameMinLength = 8;
    public const int UsernameMaxLength = 20;
    public const int FirstNameMaxLength = 50;
    public const int LastNameMaxLength = 50;
    public const int IdNumberMinLength = 8;
    public const int IdNumberMaxLength = 20;
    public const int PasswordMinLength = 8;

    public const string PasswordRegex = "^(?=.[A-Za-z])(?=.[^A-Za-z0-9])(?=.[a-z])(?=.[A-Z]).{8,}$";
}
