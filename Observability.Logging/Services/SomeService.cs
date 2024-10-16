namespace Observability.Logging.Services;

public interface ISomeService
{
    void SomeMethod();

    #region log levels

    void SomeMethodWithLogLevels();

    #endregion

    #region structured logs

    void SomeMethodWithParams();

    #endregion

    #region log scope

    void SomeMethodWithScope();

    #endregion
}

public class SomeService(
    ILogger<SomeService> logger
    ): ISomeService
{
    public void SomeMethod()
    {
        logger.LogInformation("SomeMethod do something");
    }

    #region log levels

    public void SomeMethodWithLogLevels()
    {
        logger.LogTrace("SomeMethodWithLogLevels trace");
        logger.LogDebug("SomeMethodWithLogLevels debug");
        logger.LogInformation("SomeMethodWithLogLevels info");
        logger.LogWarning("SomeMethodWithLogLevels warning");
        logger.LogError("SomeMethodWithLogLevels error");
        logger.LogCritical("SomeMethodWithLogLevels critical");
    }

    #endregion

    #region structured logs
    
    public void SomeMethodWithParams()
    {
        var userId = GetUserId();
        var userLogin = GetUserLogin();
        
        //https://www.youtube.com/watch?v=d1ODcHi5AI4
        
        logger.LogInformation($"Use some method for user with login {userLogin} and id {userId}");
        logger.LogInformation("Use some method for user with login {userLogin} and id {userId}", userLogin, userId);
        try
        {
            throw new ApplicationException("Some exception");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while processing user with login {userLogin} and id {userId}", userLogin, userId);
            logger.LogError(e, $"Use some method for user with login {userLogin} and id {userId}");
        }
    }

    #endregion

    #region log scope

    
    public void SomeMethodWithScope()
    {
        var userId = GetUserId();
        var userLogin = GetUserLogin();
        using (logger.BeginScope(new Dictionary<string, object>()
               {
                   ["userLogin"] = userLogin,
                   ["userId"] = userId,
                   ["requestId"] = Guid.NewGuid()
               }))
        {
            logger.LogInformation("Use some method");
            DoSomething();
            try
            {
                throw new ApplicationException("Some exception");
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error while processing user");
            }
        }
        
        logger.LogInformation("Done some method");
    }

    private void DoSomething()
    {
        logger.LogInformation("Do something");
    }

    #endregion

    private int GetUserId()
    {
        return new Random().Next();
    }

    private string GetUserLogin()
    {
        return $"UserLogin_{new Random().Next()}";
    }
}