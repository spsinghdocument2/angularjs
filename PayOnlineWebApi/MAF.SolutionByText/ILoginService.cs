namespace MAF.SolutionByText
{
    using MAF.ENTITY.SolutionByText;

    /// <summary>
    /// Members for login to SBT
    /// </summary>
    public interface ILoginService
    {
        ResponseMessage Authenticate();
        string SaveSecurityToken(string token, string tokenExpireInMinutes);
        string GetSecurityToken();
    }
}
