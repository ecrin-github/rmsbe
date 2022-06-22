using Dapper.Contrib.Extensions;
using rmsbe.SysModels;

namespace rmsbe.DbModels;

[Table("ctx.organisations")]
public class OrganisationInDb
{
    public int id { get; set; }
    public string? default_name { get; set; }
    public string? ror_id { get; set; }
    public string? display_suffix { get; set; }
    public int? scope_id { get; set; }
    public string? scope_notes { get; set; }
    public bool? is_current { get; set; }
    public int? year_established { get; set; }
    public int? year_ceased { get; set; }
    [Computed]
    public DateTime? created_on { get; set; }
    
    public OrganisationInDb() { }

    public OrganisationInDb(Organisation d)
    {
        id = d.Id;
        default_name = d.DefaultName;
        ror_id = d.RorId;
        display_suffix = d.DisplaySuffix;
        scope_id = d.ScopeId;
        scope_notes = d.ScopeNotes;
        is_current = d.IsCurrent;
        year_established = d.YearEstablished;
        year_ceased = d.YearCeased;
    }
}

public class OrgSimpleInDb
{
    public int id { get; set; }
    public string? name { get; set; }
    
    public OrgSimpleInDb() { }

    public OrgSimpleInDb(OrgSimple d)
    {
        id = d.Id;
        name = d.Name;
    }
}

public class OrgWithNamesInDb
{
    public int id { get; set; }
    public string? name { get; set; }
    public int? org_id { get; set; }
    public string? default_name { get; set; }
    
    public OrgWithNamesInDb() { }

    public OrgWithNamesInDb(OrgWithNames d)
    {
        id = d.Id;
        name = d.Name;
        org_id = d.OrgId;
        default_name = d.DefaultName;
    }
}


public class LangCodeInDb
{
    [Key]
    public string? code { get; set; }
    public string? name { get; set; }
    
    public LangCodeInDb() { }

    public LangCodeInDb(LangCode d)
    {
        code = d.Code; 
        name = d.Name;
    }
}

[Table("lup.language_codes")]
public class LangDetailsInDb
{
    [Key]
    public string? code { get; set; }
    public string? marc_code { get; set; }
    public string? lang_name_en { get; set; }
    public string? lang_name_fr { get; set; }
    public string? lang_name_de { get; set; }
    
    public LangDetailsInDb() { }

    public LangDetailsInDb(LangDetails d)
    {
        code = d.Code; 
        marc_code = d.MarcCode;
        lang_name_en = d.LangNameEn;
        lang_name_fr = d.LangNameFr;
        lang_name_de = d.LangNameDe;
    }
}