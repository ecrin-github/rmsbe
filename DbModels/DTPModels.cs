using Dapper.Contrib.Extensions;
using rmsbe.SysModels;

namespace rmsbe.DbModels;

public class FullDtpInDb
{
    public DtpInDb? core_dtp { get; set; }
    public List<DtaInDb>? dtas_in_db { get; set; }
    public List<DtpStudyInDb>? dtp_studies_in_db { get; set; }
    public List<DtpObjectInDb>? dtp_objects_in_db { get; set; }
    public List<DtpPrereqInDb>? dtp_prereqs_in_db { get; set; }
    public List<DtpDatasetInDb>? dtp_datasets_in_db { get; set; }    
    public List<DtpNoteInDb>? dtp_notes_in_db { get; set; }
    public List<DtpPersonInDb>? dtp_people_in_db { get; set; }

    public FullDtpInDb() { }

    public FullDtpInDb(DtpInDb? coreDtp, List<DtaInDb>? dtasInDb, List<DtpStudyInDb>? dtpStudiesInDb, 
        List<DtpObjectInDb>? dtpObjectsInDb, List<DtpPrereqInDb>? dtpPrereqsInDb, 
        List<DtpDatasetInDb>? dtpDatasetsInDb, List<DtpNoteInDb>? dtpNotesInDb, 
        List<DtpPersonInDb>? dtpPeopleInDb)
    {
        core_dtp = coreDtp;
        dtas_in_db = dtasInDb;
        dtp_studies_in_db = dtpStudiesInDb;
        dtp_objects_in_db = dtpObjectsInDb;
        dtp_prereqs_in_db = dtpPrereqsInDb;
        dtp_datasets_in_db = dtpDatasetsInDb;
        dtp_notes_in_db = dtpNotesInDb;
        dtp_people_in_db = dtpPeopleInDb;
    }
}


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
        initial_contact_date = d.InitialContactDate != null ? DateOnly.FromDateTime((DateTime)d.InitialContactDate) : null;
        set_up_completed = d.SetUpCompleted != null ? DateOnly.FromDateTime((DateTime)d.SetUpCompleted) : null;
        md_access_granted = d.MdAccessGranted != null ? DateOnly.FromDateTime((DateTime)d.MdAccessGranted) : null;
        md_complete_date = d.MdCompleteDate != null ? DateOnly.FromDateTime((DateTime)d.MdCompleteDate) : null;
        dta_agreed_date = d.DtaAgreedDate != null ? DateOnly.FromDateTime((DateTime)d.DtaAgreedDate) : null;
        upload_access_requested = d.UploadAccessRequested != null ? DateOnly.FromDateTime((DateTime)d.UploadAccessRequested) : null;
        upload_access_confirmed = d.UploadAccessConfirmed != null ? DateOnly.FromDateTime((DateTime)d.UploadAccessConfirmed) : null;
        uploads_complete = d.UploadsComplete != null ? DateOnly.FromDateTime((DateTime)d.UploadsComplete) : null;
        qc_checks_completed = d.QcChecksCompleted != null ? DateOnly.FromDateTime((DateTime)d.QcChecksCompleted) : null;
        md_integrated_with_mdr = d.MdIntegratedWithMdr != null ? DateOnly.FromDateTime((DateTime)d.MdIntegratedWithMdr) : null;
        availability_requested = d.AvailabilityRequested != null ? DateOnly.FromDateTime((DateTime)d.AvailabilityRequested) : null;
        availability_confirmed = d.AvailabilityConfirmed != null ? DateOnly.FromDateTime((DateTime)d.AvailabilityConfirmed) : null;
    }
}

public class DtpEntryInDb
{
    public int id { get; set; }
    public string? display_name { get; set; }
    public string? org_name { get; set; }
    public string? status_name { get; set; }
    
    public DtpEntryInDb() { }

    public DtpEntryInDb(DtpEntry d)
    {
        id = d.Id;
        display_name = d.DisplayName;
        org_name = d.OrgName;
        status_name = d.statusName;
    }
}


[Table("rms.dtas")]
public class DtaInDb
{
    public int id { get; set; }
    public int? dtp_id { get; set; }
    public bool? conforms_to_default { get; set; }
    public string? variations { get; set; }
    public string? dta_file_path { get; set; }
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
        dta_file_path = d.DtaFilePath;
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
    public int? dtp_id  { get; set; }
    public string? sd_oid { get; set; }
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
    public DateTime? created_on { get; set; }
    
    public DtpDatasetInDb() { }

    public DtpDatasetInDb(DtpDataset d)
    {
        this.id = d.Id;
        dtp_id = d.DtpId;
        sd_oid = d.SdOid;
        legal_status_id = d.LegalStatusId;
        legal_status_text = d.LegalStatusText;
        legal_status_path = d.LegalStatusPath;
        desc_md_check_status_id = d.DescMdCheckStatusId;
        desc_md_check_date = d.DescMdCheckDate != null ? DateOnly.FromDateTime((DateTime)d.DescMdCheckDate) : null;
        desc_md_check_by = d.DescMdCheckBy;
        deident_check_status_id = d.DeidentCheckStatusId;
        deident_check_date = d.DeidentCheckDate != null ? DateOnly.FromDateTime((DateTime)d.DeidentCheckDate) : null;
        deident_check_by = d.DeidentCheckBy;
        notes = d.Notes;
    }
}


[Table("rms.dtp_studies")]
public class DtpStudyInDb
{
    public int id { get; set; }
    public int dtp_id { get; set; }
    public string? sd_sid { get; set; }
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
        sd_sid = d.SdSid;
        md_check_status_id = d.MdCheckStatusId;
        md_check_date = d.MdCheckDate != null ? DateOnly.FromDateTime((DateTime)d.MdCheckDate) : null;
        md_check_by = d.MdCheckBy;
    }
}


[Table("rms.dtp_objects")]
public class DtpObjectInDb
{
    public int id { get; set; }
    public int dtp_id { get; set; }
    public string? sd_oid { get; set; }
    public bool? is_dataset { get; set; }
    public int? access_type_id { get; set; }
    public bool? download_allowed { get; set; }
    public string? access_details { get; set; }
    public bool? embargo_requested { get; set; }
    public string? embargo_regime { get; set; }
    public bool? embargo_still_applies { get; set; }
    public int? access_check_status_id { get; set; }
    public DateOnly? access_check_date { get; set; }
    public int? access_check_by { get; set; }
    public int? md_check_status_id { get; set; }
    public DateOnly? md_check_date { get; set; }
    public int? md_check_by { get; set; }
    public string? notes { get; set; }
    [Computed]
    public DateTime? created_on { get; set; }
    
    public DtpObjectInDb() { }

    public DtpObjectInDb(DtpObject d)
    {
        id = d.Id;
        dtp_id = d.DtpId;
        sd_oid = d.SdOid;
        is_dataset = d.IsDataset;
        access_type_id = d.AccessTypeId;
        download_allowed = d.DownloadAllowed;
        access_details = d.AccessDetails;
        embargo_requested = d.EmbargoRequested;
        embargo_regime = d.EmbargoRegime;
        embargo_still_applies = d.EmbargoStillApplies;
        access_check_status_id = d.AccessCheckStatusId;
        access_check_date = d.AccessCheckDate != null ? DateOnly.FromDateTime((DateTime)d.AccessCheckDate) : null;
        access_check_by = d.AccessCheckBy;
        md_check_status_id = d.MdCheckStatusId;
        md_check_date = d.MdCheckDate != null ? DateOnly.FromDateTime((DateTime)d.MdCheckDate) : null;
        md_check_by = d.MdCheckBy;
        notes = d.Notes;
    }
}


[Table("rms.dtp_prereqs")]
public class DtpPrereqInDb
{
    public int id { get; set; }
    public int? dtp_id  { get; set; }
    public string? sd_oid { get; set; }
    public int? pre_requisite_type_id { get; set; }
    public string? pre_requisite_notes { get; set; }
    [Computed] 
    public DateTime? created_on { get; set; }

    public DtpPrereqInDb() { }

    public DtpPrereqInDb(DtpPrereq d)
    {
        id = d.Id;
        dtp_id = d.DtpId;
        sd_oid = d.SdOid;
        pre_requisite_type_id = d.PreRequisiteTypeId;
        pre_requisite_notes = d.PreRequisiteNotes;
    }
}


[Table("rms.dtp_notes")]
public class DtpNoteInDb
{
    public int id { get; set; }
    public int? dtp_id { get; set; }
    public string? text { get; set; }
    public int? author { get; set; }
    [Computed]
    public DateTime? created_on { get; set; }
    
    public DtpNoteInDb() { }

    public DtpNoteInDb(DtpNote d)
    {
        id = d.Id;
        dtp_id = d.DtpId;
        text = d.Text;
        author = d.Author;
    }
}


[Table("rms.dtp_people")]
public class DtpPersonInDb
{
    public int id { get; set; }
    public int? dtp_id { get; set; }
    public int? person_id { get; set; }
    public string? notes { get; set; }
    [Computed]
    public DateTime created_on { get; set; }
    
    public DtpPersonInDb() { }

    public DtpPersonInDb(DtpPerson d)
    {
        id = d.Id;
        dtp_id = d.DtpId;
        person_id = d.PersonId;
        notes = d.Notes;
    }
}


