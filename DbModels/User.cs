namespace rmsbe.DbModels;

public class UserInDb
{
    public string? sub { get; set; }
    public string? name { get; set; }
    public string? preferred_user_name { get; set; }
    public string? given_name { get; set; }
    public string? family_name { get; set; }
    public string? email { get; set; }
    public bool? email_verified { get; set; }
}