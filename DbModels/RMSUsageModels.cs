using Dapper.Contrib.Extensions;
using rmsbe.SysModels;

namespace rmsbe.DbModels;

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

    public DupInDb() { }

    public DupInDb(Dup d)
    {
        id = d.Id;
        org_id = d.OrgId;
        display_name = d.DisplayName;
        status_id = d.StatusId;
        initial_contact_date = d.InitialContactDate;
        set_up_completed = d.SetUpCompleted;
        prereqs_met = d.PrereqsMet;
        dua_agreed_date = d.DuaAgreedDate;
        availability_requested = d.AvailabilityRequested;
        availability_confirmed = d.AvailabilityConfirmed;
        access_confirmed = d.AccessConfirmed;
    }
}


[Table("rms.duas")]
public class DuaInDb
{
    public int id { get; set; }
    public int dup_id { get; set; }
    public int? conforms_to_default { get; set; }
    public string? variations { get; set; }
    public bool? repo_as_proxy { get; set; }
    public int? repo_signatory_1 { get; set; }
    public int? repo_signatory_2 { get; set; }
    public int? provider_signatory_1 { get; set; }
    public int? provider_signatory_2 { get; set; }
    public int? requester_signatory_1 { get; set; }
    public int? requester_signatory_2 { get; set; }
    public string? notes { get; set; }
    [Computed]
    public DateTime? created_on { get; set; }
    
    public DuaInDb() { }

    public DuaInDb(Dua d)
    {
        id = d.Id;
        dup_id = d.DupId;
        conforms_to_default = d.ConformsToDefault;
        variations = d.Variations;
        repo_as_proxy = d.RepoAsProxy;
        repo_signatory_1 = d.RepoSignatory1;
        repo_signatory_2 = d.RepoSignatory2;
        provider_signatory_1 = d.ProviderSignatory1;
        provider_signatory_2 = d.ProviderSignatory2;
        requester_signatory_1 = d.RequesterSignatory1;
        requester_signatory_2 = d.RequesterSignatory2;
        notes = d.Notes;
    }
}


[Table("rms.dup_objects")]
public class DupObjectInDb
{
    public int id { get; set; }
    public int dup_id { get; set; }
    public string? object_id { get; set; }
    public int? access_type_id { get; set; }
    public string? access_details { get; set; }
    public string? notes { get; set; }
    [Computed]
    public DateTime? created_on { get; set; }
    
    public DupObjectInDb() { }

    public DupObjectInDb(DupObject d)
    {
        id = d.Id;
        dup_id = d.DupId;
        object_id = d.ObjectId;
        access_type_id = d.AccessTypeId;
        access_details = d.AccessDetails;
        notes = d.Notes;
    }
}


[Table("rms.dup_prereqs")]
public class DupPreReqInDb
{
    public int id { get; set; }
    public int dup_id { get; set; }
    public string? object_id { get; set; }
    public int? pre_requisite_id { get; set; }
    public DateOnly? prerequisite_met { get; set; }
    public string? met_notes { get; set; }
    [Computed]
    public DateTime? created_on { get; set; }
    
    public DupPreReqInDb() { }

    public DupPreReqInDb(DupPrereq d)
    {
        id = d.Id;
        dup_id = d.DupId;
        object_id = d.ObjectId;
        pre_requisite_id = d.PreRequisiteId;
        prerequisite_met = d.PrerequisiteMet;
        met_notes = d.MetNotes;
    }
}


[Table("rms.secondary_use")]
public class SecondaryUseInDb
{
    public int id { get; set; }
    public int dup_id { get; set; }
    public string? secondary_use_type { get; set; }
    public string? publication { get; set; }
    public string? doi { get; set; }
    public bool? attribution_present { get; set; }
    public string? notes { get; set; }
    [Computed]
    public DateTime? created_on { get; set; }
    
    public SecondaryUseInDb() { }

    public SecondaryUseInDb(SecondaryUse d)
    {
        id = d.Id;
        dup_id = d.DupId;
        secondary_use_type = d.SecondaryUseType;
        publication = d.Publication;
        doi = d.Doi;
        attribution_present = d.AttributionPresent;
        notes = d.Notes;
    }
}