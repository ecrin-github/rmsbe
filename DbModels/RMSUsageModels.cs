using Dapper.Contrib.Extensions;

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
    public DateTime? created_on { get; set; }
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
    public DateTime? created_on { get; set; }
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
    public DateTime? created_on { get; set; }
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
    public DateTime? created_on { get; set; }
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
    public DateTime? created_on { get; set; }
}