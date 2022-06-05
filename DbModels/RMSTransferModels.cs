using Dapper.Contrib.Extensions;
using rmsbe.SysModels;

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
    [Computed]
    public DateTime? created_on { get; set; }
    
    public DtpInDb() { }

    public DtpInDb(Dtp d)
    {
        this.id = d.Id;
        org_id = d.OrgId;
        display_name = d.DisplayName;
        status_id = d.StatusId;
        initial_contact_date = d.InitialContactDate;
        set_up_completed = d.SetUpCompleted;
        md_access_granted = d.MdAccessGranted;
        md_complete_date = d.MdCompleteDate;
        dta_agreed_date = d.DtaAgreedDate;
        upload_access_requested = d.UploadAccessRequested;
        upload_access_confirmed = d.UploadAccessConfirmed;
        uploads_complete = d.UploadsComplete;
        qc_checks_completed = d.QcChecksCompleted;
        md_integrated_with_mdr = d.MdIntegratedWithMdr;
        availability_requested = d.AvailabilityRequested;
        availability_confirmed = d.AvailabilityConfirmed;
    }
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
    [Computed]
    public DateTime created_on { get; set; }
    
    public DtaInDb() { }

    public DtaInDb(Dta d)
    {
        id = d.Id;
        dtp_id = d.DtpId;
        conforms_to_default = d.ConformsToDefault;
        variations = d.Variations;
        repo_signatory_1 = d.RepoSignatory1;
        repo_signatory_2 = d.RepoSignatory2;
        provider_signatory_1 = d.ProviderSignatory1;
        provider_signatory_2 = d.ProviderSignatory2;
        notes = d.Notes;
    }
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
    [Computed]
    public DateOnly? created_on { get; set; }
    
    public DtpDatasetInDb() { }

    public DtpDatasetInDb(DtpDataset d)
    {
        this.id = d.Id;
        object_id = d.ObjectId;
        legal_status_id = d.LegalStatusId;
        legal_status_text = d.LegalStatusText;
        legal_status_path = d.LegalStatusPath;
        desc_md_check_status_id = d.DescmdCheckStatusId;
        desc_md_check_date = d.DescmdCheckDate;
        desc_md_check_by = d.DescmdCheckBy;
        deident_check_status_id = d.DeidentCheckStatusId;
        deident_check_date = d.DeidentCheckDate;
        deident_check_by = d.DeidentCheckBy;
        notes = d.Notes;
    }
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
    [Computed]
    public DateTime? created_on { get; set; }
    
    public DtpStudyInDb() { }

    public DtpStudyInDb(DtpStudy d)
    {
        this.id = d.Id;
        dtp_id = d.DtpId;
        study_id = d.StudyId;
        md_check_status_id = d.MdCheckStatusId;
        md_check_date = d.MdCheckDate;
        md_check_by = d.MdCheckBy;
    }
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
    [Computed]
    public DateTime? created_on { get; set; }
    
    public DtpObjectInDb() { }

    public DtpObjectInDb(DtpObject d)
    {
        id = d.Id;
        dtp_id = d.DtpId;
        object_id = d.ObjectId;
        is_dataset = d.IsDataset;
        access_type_id = d.AccessTypeId;
        download_allowed = d.DownloadAllowed;
        access_details = d.AccessDetails;
        requires_embargo_period = d.RequiresEmbargoPeriod;
        embargo_end_date = d.EmbargoEndDate;
        embargo_still_applies = d.EmbargoStillApplies;
        access_check_status_id = d.AccessCheckStatusId;
        access_check_date = d.AccessCheckDate;
        access_check_by = d.AccessCheckBy;
        md_check_status_id = d.MdCheckStatusId;
        md_check_date = d.MdCheckDate;
        md_check_by = d.MdCheckBy;
        notes = d.Notes;
    }
}

