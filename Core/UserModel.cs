namespace App.Core;

#region Model
public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string Email { get; set; } = "";
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

#endregion

#region Actions ENUM and LOGSAction
public enum ActionType { LOGIN, LOGOUT, CREATE, UPDATE, DELETE, VIEW }

public class ActionLog
{
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public ActionType Type { get; set; }
    public string Metadata { get; set; } = "";
}
#endregion

#region  FacotryUser 
public static class UserFactory
{
    public static User Create(string username, string email)
    {
        var ctx = AppContext.Instance;
        return new User
        {
            Id = ctx.GetNextId(),
            Username = username,
            Email = email
        };
    }
}
#endregion