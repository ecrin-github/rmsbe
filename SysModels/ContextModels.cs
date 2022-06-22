using Dapper.Contrib.Extensions;
using rmsbe.DbModels;

namespace rmsbe.SysModels;

public class Organisation
{
    public int Id { get; set; }
    public string? DefaultName { get; set; }
    public string? RorId { get; set; }
    public string? DisplaySuffix { get; set; }
    public int? ScopeId { get; set; }
    public string? ScopeNotes { get; set; }
    public bool? IsCurrent { get; set; }
    public int? YearEstablished { get; set; }
    public int? YearCeased { get; set; }
    
    public Organisation() { }

    public Organisation(OrganisationInDb d)
    {
        Id = d.id;
        DefaultName = d.default_name;
        RorId = d.ror_id;
        DisplaySuffix = d.display_suffix;
        ScopeId = d.scope_id;
        ScopeNotes = d.scope_notes;
        IsCurrent = d.is_current;
        YearEstablished = d.year_established;
        YearCeased = d.year_ceased;
    }
}


public class OrgSimple
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public OrgSimple() { }

    public OrgSimple(OrgSimpleInDb d)
    {
        Id = d.id;
        Name = d.name;
    }
}


public class OrgWithNames
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int? OrgId { get; set; }
    public string? DefaultName { get; set; }
    
    public OrgWithNames() { }

    public OrgWithNames(OrgWithNamesInDb d)
    {
        Id = d.id;
        Name = d.name;
        OrgId = d.org_id;
        DefaultName = d.default_name;
    }
}



public class LangCode
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    
    public LangCode() { }

    public LangCode(LangCodeInDb d)
    {
        Code = d.code;
        Name = d.name;
    }
}

public class LangDetails
{
    public string? Code { get; set; }
    public string? MarcCode { get; set; }
    public string? LangNameEn { get; set; }
    public string? LangNameFr { get; set; }
    public string? LangNameDe { get; set; }
    
    public LangDetails() { }

    public LangDetails(LangDetailsInDb d)
    {
        Code = d.code;
        MarcCode = d.marc_code;
        LangNameEn = d.lang_name_en;
        LangNameFr = d.lang_name_fr;
        LangNameDe = d.lang_name_de;
    }
}
