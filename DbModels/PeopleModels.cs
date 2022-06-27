using Dapper.Contrib.Extensions;
using rmsbe.SysModels;

namespace rmsbe.DbModels;

[Table("rms.people")]
public class PersonInDb
{
    public int id { get; set; }
    public string? title { get; set; }
    public string? given_name { get; set; }
    public string? family_name { get; set; }
    public string? designation { get; set; }
    public int? org_id { get; set; }
    public string? org_name { get; set; }
    public string? email { get; set; }
    public string? comments { get; set; }
    [Computed]
    public DateTime? created_on { get; set; }
    
    public PersonInDb () { }

    public PersonInDb(Person d)
    {
        id = d.Id;
        title = d.Title;
        given_name = d.GivenName;
        family_name = d.FamilyName;
        designation = d.Designation;
        org_id = d.OrgId;
        org_name = d.OrgName;
        email = d.Email;
        comments = d.Comments;
    }
}

public class PersonEntryInDb
{
    public int id { get; set; }
    public string? title { get; set; }
    public string? given_name { get; set; }
    public string? family_name { get; set; }
    public int? org_id { get; set; }
    public string? org_name { get; set; }
    
    public PersonEntryInDb () { }
    
}

[Table("rms.people_roles")]
public class PersonRoleInDb
{
    public int id { get; set; }
    public int? person_id { get; set; }
    public int? role_id { get; set; }
    public bool? is_current { get; set; }
    public DateTime? granted { get; set; }
    public DateTime? revoked{ get; set; }
    [Computed]
    public DateTime? created_on { get; set; }
    
    public PersonRoleInDb () { }

    public PersonRoleInDb(PersonRole d)
    {
        id = d.Id;
        person_id = d.PersonId;
        role_id = d.RoleId;
        is_current = d.IsCurrent;
        granted = d.Granted;
        revoked = d.Revoked;
    }
}