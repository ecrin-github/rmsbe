using Dapper.Contrib.Extensions;
using rmsbe.SysModels;

namespace rmsbe.DbModels;

public class FullDupInDb
{
    public DupInDb? core_dup { get; set; }
    public List<DuaInDb>? duas_in_db { get; set; }
    public List<DupStudyInDb>? dup_studies_in_db { get; set; }
    public List<DupObjectInDb>? dup_objects_in_db { get; set; }
    public List<DupPrereqInDb>? dup_prereqs_in_db { get; set; }
    public List<DupSecondaryUseInDb>? dup_sec_use_in_db { get; set; }
    public List<DupNoteInDb>? dup_notes_in_db { get; set; }
    public List<DupPersonInDb>? dup_people_in_db { get; set; }

    public FullDupInDb() { }

    public FullDupInDb(DupInDb? coreDup, List<DuaInDb>? duasInDb, List<DupStudyInDb>? dupStudiesInDb, 
        List<DupObjectInDb>? dupObjectsInDb, List<DupPrereqInDb>? dupPrereqsInDb, 
        List<DupSecondaryUseInDb>? dupSecUseInDb, List<DupNoteInDb>? dupNotesInDb, 
        List<DupPersonInDb>? dupPeopleInDb)
    {
        core_dup = coreDup;
        duas_in_db = duasInDb;
        dup_studies_in_db = dupStudiesInDb;
        dup_objects_in_db = dupObjectsInDb;
        dup_prereqs_in_db = dupPrereqsInDb;
        dup_sec_use_in_db = dupSecUseInDb;
        dup_notes_in_db = dupNotesInDb;
        dup_people_in_db = dupPeopleInDb;
    }
}



[Table("rms.dups")]
public class DupInDb
{
    public int id { get; set; }
    public int org_id { get; set; }
    public string? display_name { get; set; }
    public int? status_id { get; set; }
    public DateOnly? initial_contact_date { get; set; }
    public DateOnly? set_up_completed { get; set; }
    public DateOnly? prereqs_met { get; set; }
    public DateOnly? dua_agreed_date { get; set; }
    public DateOnly? availability_requested { get; set; }
    public DateOnly? availability_confirmed { get; set; }
    public DateOnly? access_confirmed { get; set; }
    [Computed] 
    public DateTime? created_on { get; set; }

    public DupInDb()
    {
    }

    public DupInDb(Dup d)
    {
        id = d.Id;
        org_id = d.OrgId;
        display_name = d.DisplayName;
        status_id = d.StatusId;
        initial_contact_date =
            d.InitialContactDate != null ? DateOnly.FromDateTime((DateTime)d.InitialContactDate) : null;
        set_up_completed = d.SetUpCompleted != null ? DateOnly.FromDateTime((DateTime)d.SetUpCompleted) : null;
        prereqs_met = d.PrereqsMet != null ? DateOnly.FromDateTime((DateTime)d.PrereqsMet) : null;
        dua_agreed_date = d.DuaAgreedDate != null ? DateOnly.FromDateTime((DateTime)d.DuaAgreedDate) : null;
        availability_requested = d.AvailabilityRequested != null
            ? DateOnly.FromDateTime((DateTime)d.AvailabilityRequested)
            : null;
        availability_confirmed = d.AvailabilityConfirmed != null
            ? DateOnly.FromDateTime((DateTime)d.AvailabilityConfirmed)
            : null;
        access_confirmed = d.AccessConfirmed != null ? DateOnly.FromDateTime((DateTime)d.AccessConfirmed) : null;
    }
}

public class DupEntryInDb
{
    public int id { get; set; }
    public string? org_name { get; set; }
    public string? display_name { get; set; }
    public string? status_name { get; set; }
    
    public DupEntryInDb() { }

    public DupEntryInDb(DupEntry d)
    {
        id = d.Id;
        org_name = d.OrgName;
        display_name = d.DisplayName;
        status_name = d.statusName;
    }
}


[Table("rms.duas")]
public class DuaInDb
{
    public int id { get; set; }
    public int dup_id { get; set; }
    public bool? conforms_to_default { get; set; }
    public string? variations { get; set; }
    public bool? repo_is_proxy_provider { get; set; }
    public string? dua_file_path { get; set; }
    public int? repo_signatory_1 { get; set; }
    public int? repo_signatory_2 { get; set; }
    public int? provider_signatory_1 { get; set; }
    public int? provider_signatory_2 { get; set; }
    public int? requester_signatory_1 { get; set; }
    public int? requester_signatory_2 { get; set; }
    public string? notes { get; set; }
    [Computed] 
    public DateTime? created_on { get; set; }

    public DuaInDb()
    {
    }

    public DuaInDb(Dua d)
    {
        id = d.Id;
        dup_id = d.DupId;
        conforms_to_default = d.ConformsToDefault;
        variations = d.Variations;
        repo_is_proxy_provider = d.RepoIsProxyProvider;
        dua_file_path = d.DuaFilePath;
        repo_signatory_1 = d.RepoSignatory1;
        repo_signatory_2 = d.RepoSignatory2;
        provider_signatory_1 = d.ProviderSignatory1;
        provider_signatory_2 = d.ProviderSignatory2;
        requester_signatory_1 = d.RequesterSignatory1;
        requester_signatory_2 = d.RequesterSignatory2;
        notes = d.Notes;
    }
}


[Table("rms.dup_studies")]
public class DupStudyInDb
{
    public int id { get; set; }
    public int dup_id { get; set; }
    public string? sd_sid { get; set; }
    [Computed] 
    public DateTime? created_on { get; set; }

    public DupStudyInDb()
    {
    }

    public DupStudyInDb(DupStudy d)
    {
        id = d.Id;
        dup_id = d.DupId;
        sd_sid = d.SdSid;
    }
}


[Table("rms.dup_objects")]
public class DupObjectInDb
{
    public int id { get; set; }
    public int dup_id { get; set; }
    public string? sd_oid { get; set; }
    public int? access_type_id { get; set; }
    public string? access_details { get; set; }
    public string? notes { get; set; }
    [Computed] 
    public DateTime? created_on { get; set; }

    public DupObjectInDb()
    {
    }

    public DupObjectInDb(DupObject d)
    {
        id = d.Id;
        dup_id = d.DupId;
        sd_oid = d.SdOid;
        access_type_id = d.AccessTypeId;
        access_details = d.AccessDetails;
        notes = d.Notes;
    }
}


[Table("rms.dup_prereqs")]
public class DupPrereqInDb
{
    public int id { get; set; }
    public int dup_id { get; set; }
    public string? sd_oid { get; set; }
    public int? pre_requisite_id { get; set; }
    public string? pre_requisite_notes { get; set; }
    public DateOnly? pre_requisite_met { get; set; }
    public string? met_notes { get; set; }
    [Computed] 
    public DateTime? created_on { get; set; }

    public DupPrereqInDb()
    {
    } 

    public DupPrereqInDb(DupPrereq d)
    {
        id = d.Id;
        dup_id = d.DupId;
        sd_oid = d.SdOid;
        pre_requisite_id = d.PreRequisiteId;
        pre_requisite_notes = d.PreRequisiteNotes;
        pre_requisite_met = d.PreRequisiteMet != null ? DateOnly.FromDateTime((DateTime)d.PreRequisiteMet) : null;
        met_notes = d.MetNotes;
    }
}


[Table("rms.dup_sec_use")]
public class DupSecondaryUseInDb
{
    public int id { get; set; }
    public int dup_id { get; set; }
    public string? secondary_use_summary { get; set; }
    public string? publication { get; set; }
    public string? doi { get; set; }
    public bool? attribution_present { get; set; }
    public string? notes { get; set; }
    [Computed] 
    public DateTime? created_on { get; set; }

    public DupSecondaryUseInDb()
    {
    }

    public DupSecondaryUseInDb(DupSecondaryUse d)
    {
        id = d.Id;
        dup_id = d.DupId;
        secondary_use_summary = d.SecondaryUseSummary;
        publication = d.Publication;
        doi = d.Doi;
        attribution_present = d.AttributionPresent;
        notes = d.Notes;
    }
}


[Table("rms.dup_notes")]
public class DupNoteInDb
{
    public int id { get; set; }
    public int? dup_id { get; set; }
    public string? text { get; set; }
    public int? author { get; set; }
    [Computed] 
    public DateTime? created_on { get; set; }

    public DupNoteInDb()
    {
    }

    public DupNoteInDb(DupNote d)
    {
        id = d.Id;
        dup_id = d.DupId;
        text = d.Text;
        author = d.Author;
    }
}


[Table("rms.dup_people")]
public class DupPersonInDb
{
    public int id { get; set; }
    public int? dup_id { get; set; }
    public int? person_id { get; set; }
    public string? notes { get; set; }
    [Computed] 
    public DateTime created_on { get; set; }

    public DupPersonInDb()
    {
    }

    public DupPersonInDb(DupPerson d)
    {
        id = d.Id;
        dup_id = d.DupId;
        person_id = d.PersonId;
        notes = d.Notes;
    }
}