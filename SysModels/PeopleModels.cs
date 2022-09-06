using rmsbe.DbModels;

namespace rmsbe.SysModels;

public class FullPerson
{
    public Person? corePerson { get; set; }
    public PersonRole? personRole { get; set; }

    public FullPerson () { }

    public FullPerson(FullPersonInDb d)
    {
        corePerson = d.core_person == null ? null : new Person(d.core_person);
        personRole = d.person_role == null ? null : new PersonRole(d.person_role);
    }
}


public class Person
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? GivenName { get; set; }
    public string? FamilyName { get; set; }
    public string? Name { get; set; }
    public string? FullName { get; set; }
    public string? Designation { get; set; }
    public int? OrgId { get; set; }
    public string? OrgName { get; set; }
    public string? Email { get; set; }
    public string? Comments { get; set; }
    
    public Person () { }

    public Person(PersonInDb d)
    {
        Id = d.id;
        Title = d.title;
        GivenName = d.given_name;
        FamilyName = d.family_name;
        Name = (d.given_name + " " + d.family_name).Trim();
        FullName = (d.title + " " + (d.given_name + " " + d.family_name).Trim()).Trim();
        Designation = d.designation;
        OrgId = d.org_id;
        OrgName = d.org_name;
        Email = d.email;
        Comments = d.comments;
    }
}


public class PersonEntry
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? OrgName { get; set; }
    public string? RoleName { get; set; }
    public string? Email { get; set; }
    
    public PersonEntry () { }

    public PersonEntry(PersonEntryInDb d)
    {
        Id = d.id;
        Name = (d.given_name + " " + d.family_name).Trim();
        OrgName = d.org_name;
        RoleName = d.role_name ?? "(no user role)";
        Email = d.email;
    }
}


public class PersonRole
{
    public int Id { get; set; }
    public int? PersonId { get; set; }
    public int? RoleId { get; set; }
    public string? RoleName { get; set; }
    public bool? IsCurrent { get; set; }
    public DateTime? Granted { get; set; }
    public DateTime? Revoked{ get; set; }
    
    public PersonRole () { }

    public PersonRole(PersonRoleInDb d)
    {
        Id = d.id;
        PersonId = d.person_id;
        RoleId = d.role_id;
        RoleName = d.role_name;
        IsCurrent = d.is_current;
        Granted = d.granted;
        Revoked = d.revoked;
    }
}
