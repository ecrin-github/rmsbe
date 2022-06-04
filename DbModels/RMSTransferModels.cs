using Dapper.Contrib.Extensions;

namespace rmsbe.DbModels;

[Table("rms.dtps")]
public class DtpInDb
{
    public int id { get; set; }
    public int? org_id { get; set; }
    public string? display_name { get; set; }
    public int? status_id { get; set; }
    public DateOnly? initial_contact_date { get; set; }
    public DateOnly? set_up_completed { get; set; }
    public DateOnly? md_access_granted { get; set; }
    public DateOnly? md_complete_date { get; set; }
    public DateOnly? dta_agreed_date { get; set; }
    public DateOnly? upload_access_requested { get; set; }
    public DateOnly? upload_access_confirmed { get; set; }
    public DateOnly? uploads_complete { get; set; }
    public DateOnly? qc_checks_completed { get; set; }
    public DateOnly? md_integrated_with_mdr { get; set; }
    public DateOnly? availability_requested { get; set; }
    public DateOnly? availability_confirmed { get; set; }
    public DateTime? created_on { get; set; }
}


[Table("rms.dtas")]
public class DtaInDb
{
    public int id { get; set; }
    public int? dtp_id { get; set; }
    public int? conforms_to_default { get; set; }
    public string? variations { get; set; }
    public int? repo_signatory_1 { get; set; }
    public int? repo_signatory_2 { get; set; }
    public int? provider_signatory_1 { get; set; }
    public int? provider_signatory_2 { get; set; }
    public string? notes { get; set; }
    public DateTime created_on { get; set; }
}


[Table("rms.dtp_datasets")]
public class DtpDatasetInDb
{
    public int id { get; set; }
    public string? object_id { get; set; }
    public int? legal_status_id { get; set; }
    public string? legal_status_text { get; set; }
    public string? legal_status_path { get; set; }
    public int? desc_md_check_status_id { get; set; }
    public DateOnly? desc_md_check_date { get; set; }
    public int? desc_md_check_by { get; set; }
    public int? deident_check_status_id { get; set; }
    public DateOnly? deident_check_date { get; set; }
    public int? deident_check_by { get; set; }
    public string? notes { get; set; }
    public DateOnly? created_on { get; set; }
}


[Table("rms.dtp_studies")]
public class DtpStudyInDb
{
    public int id { get; set; }
    public int dtp_id { get; set; }
    public string? study_id { get; set; }
    public int? md_check_status_id { get; set; }
    public DateOnly? md_check_date { get; set; }
    public int? md_check_by { get; set; }
    public DateTime? created_on { get; set; }
}


[Table("rms.dtp_objects")]
public class DtpObjectInDb
{
    public int id { get; set; }
    public int dtp_id { get; set; }
    public string? object_id { get; set; }
    public bool? is_dataset { get; set; }
    public int? access_type_id { get; set; }
    public bool? download_allowed { get; set; }
    public string? access_details { get; set; }
    public bool? requires_embargo_period { get; set; }
    public DateOnly? embargo_end_date { get; set; }
    public bool? embargo_still_applies { get; set; }
    public int? access_check_status_id { get; set; }
    public DateOnly? access_check_date { get; set; }
    public string? access_check_by { get; set; }
    public int? md_check_status_id { get; set; }
    public DateOnly? md_check_date { get; set; }
    public string? md_check_by { get; set; }
    public string? notes { get; set; }
    public DateTime? created_on { get; set; }
}

