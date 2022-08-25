using rmsbe.DbModels;

namespace rmsbe.SysModels;

public class FullDtp
{
    public Dtp? CoreDTP { get; set; }
    public List<Dta>? Dtas { get; set; }
    public List<DtpStudy>? DtpStudies { get; set; }
    public List<DtpObject>? DtpObjects { get; set; }
    public List<DtpPrereq>? DtpPrereqs { get; set; }
    public List<DtpDataset>? DtpDatasets { get; set; }    
    public List<DtpNote>? DtpNotes { get; set; }
    public List<DtpPerson>? DtpPeople { get; set; }

    public FullDtp() { }

    public FullDtp(FullDtpInDb d)
    {
        CoreDTP = d.core_dtp == null ? null : new Dtp(d.core_dtp);
        Dtas = d.dtas_in_db?.Select(r => new Dta(r)).ToList();
        DtpStudies = d.dtp_studies_in_db?.Select(r => new DtpStudy(r)).ToList();;
        DtpObjects = d.dtp_objects_in_db?.Select(r => new DtpObject(r)).ToList();;
        DtpPrereqs = d.dtp_prereqs_in_db?.Select(r => new DtpPrereq(r)).ToList();;
        DtpDatasets = d.dtp_datasets_in_db?.Select(r => new DtpDataset(r)).ToList();;
        DtpNotes = d.dtp_notes_in_db?.Select(r => new DtpNote(r)).ToList();;
        DtpPeople = d.dtp_people_in_db?.Select(r => new DtpPerson(r)).ToList();;
    }
}


public class Dtp
{
    public int Id { get; set; }
    public int? OrgId { get; set; }
    public string? DisplayName { get; set; }
    public int? StatusId { get; set; }
    public DateTime? InitialContactDate { get; set; }
    public DateTime? SetUpCompleted { get; set; }
    public DateTime? MdAccessGranted { get; set; }
    public DateTime? MdCompleteDate { get; set; }
    public DateTime? DtaAgreedDate { get; set; }
    public DateTime? UploadAccessRequested { get; set; }
    public DateTime? UploadAccessConfirmed { get; set; }
    public DateTime? UploadsComplete { get; set; }
    public DateTime? QcChecksCompleted { get; set; }
    public DateTime? MdIntegratedWithMdr { get; set; }
    public DateTime? AvailabilityRequested { get; set; }
    public DateTime? AvailabilityConfirmed { get; set; }

    public Dtp() { }

    public Dtp(DtpInDb d)
    {
        Id = d.id;
        OrgId = d.org_id;
        DisplayName = d.display_name;
        StatusId = d.status_id;
        InitialContactDate = d.initial_contact_date?.ToDateTime(TimeOnly.MinValue);
        SetUpCompleted = d.set_up_completed?.ToDateTime(TimeOnly.MinValue);
        MdAccessGranted = d.md_access_granted?.ToDateTime(TimeOnly.MinValue);
        MdCompleteDate = d.md_complete_date?.ToDateTime(TimeOnly.MinValue);
        DtaAgreedDate = d.dta_agreed_date?.ToDateTime(TimeOnly.MinValue);
        UploadAccessRequested = d.upload_access_requested?.ToDateTime(TimeOnly.MinValue);
        UploadAccessConfirmed = d.upload_access_confirmed?.ToDateTime(TimeOnly.MinValue);
        UploadsComplete = d.uploads_complete?.ToDateTime(TimeOnly.MinValue);
        QcChecksCompleted = d.qc_checks_completed?.ToDateTime(TimeOnly.MinValue);
        MdIntegratedWithMdr = d.md_integrated_with_mdr?.ToDateTime(TimeOnly.MinValue);
        AvailabilityRequested = d.availability_requested?.ToDateTime(TimeOnly.MinValue);
        AvailabilityConfirmed = d.availability_confirmed?.ToDateTime(TimeOnly.MinValue);
    }
}

public class DtpEntry
{
    public int Id { get; set; }
    public int? OrgId { get; set; }
    public string? DisplayName { get; set; }
    
    public DtpEntry() { }

    public DtpEntry(DtpEntryInDb d)
    {
        Id = d.id;
        OrgId = d.org_id;
        DisplayName = d.display_name;
    }
}

public class Dta
{
    public int Id { get; set; }
    public int? DtpId { get; set; }
    public bool? ConformsToDefault { get; set; }
    public string? Variations { get; set; }
    public string? DtaFilePath { get; set; }
    public int? RepoSignatory1 { get; set; }
    public int? RepoSignatory2 { get; set; }
    public int? ProviderSignatory1 { get; set; }
    public int? ProviderSignatory2 { get; set; }  
    public string? Notes { get; set; }

    public Dta() { }
    
    public Dta(DtaInDb d)
    {
        Id = d.id;
        DtpId = d.dtp_id;
        ConformsToDefault = d.conforms_to_default;
        Variations = d.variations;
        DtaFilePath = d.dta_file_path;
        RepoSignatory1 = d.repo_signatory_1;
        RepoSignatory2 = d.repo_signatory_2;
        ProviderSignatory1 = d.provider_signatory_1;
        ProviderSignatory2 = d.provider_signatory_2;
        Notes = d.notes;
    }
}

public class DtpDataset
{
    public int Id { get; set; }
    public int? DtpId { get; set; }
    public string? SdOid { get; set; }
    public int? LegalStatusId { get; set; }
    public string? LegalStatusText { get; set; }
    public string? LegalStatusPath { get; set; }
    public int? DescMdCheckStatusId { get; set; }
    public DateTime? DescMdCheckDate { get; set; }
    public int? DescMdCheckBy { get; set; }
    public int? DeidentCheckStatusId { get; set; }
    public DateTime? DeidentCheckDate { get; set; }
    public int? DeidentCheckBy { get; set; }
    public string? Notes { get; set; }
    
    public DtpDataset() { }

    public DtpDataset(DtpDatasetInDb d)
    {
        Id = d.id;
        DtpId = d.dtp_id;
        SdOid = d.sd_oid;
        LegalStatusId = d.legal_status_id;
        LegalStatusText = d.legal_status_text;
        LegalStatusPath = d.legal_status_path;
        DescMdCheckStatusId = d.desc_md_check_status_id;
        DescMdCheckDate = d.desc_md_check_date?.ToDateTime(TimeOnly.MinValue);
        DescMdCheckBy = d.desc_md_check_by;
        DeidentCheckStatusId = d.deident_check_status_id;
        DeidentCheckDate = d.deident_check_date?.ToDateTime(TimeOnly.MinValue);
        DeidentCheckBy = d.deident_check_by;
        Notes = d.notes;
    }
}

public class DtpStudy
{
    public int Id { get; set; }
    public int DtpId { get; set; }
    public string? SdSid { get; set; }
    public int? MdCheckStatusId { get; set; }
    public DateTime? MdCheckDate { get; set; }
    public int? MdCheckBy { get; set; }
    
    public DtpStudy() { }

    public DtpStudy(DtpStudyInDb d)
    {
        Id = d.id;
        DtpId = d.dtp_id;
        SdSid = d.sd_sid;
        MdCheckStatusId = d.md_check_status_id;
        MdCheckDate = d.md_check_date?.ToDateTime(TimeOnly.MinValue);
        MdCheckBy = d.md_check_by;
    }
}

public class DtpObject
{
    public int Id { get; set; }
    public int DtpId { get; set; }
    public string? SdOid { get; set; }
    public bool? IsDataset { get; set; }
    public int? AccessTypeId { get; set; }
    public bool? DownloadAllowed { get; set; }
    public string? AccessDetails { get; set; }
    public bool? EmbargoRequested { get; set; }
    public string? EmbargoRegime { get; set; }
    public bool? EmbargoStillApplies { get; set; }
    public int? AccessCheckStatusId { get; set; }
    public DateTime? AccessCheckDate { get; set; }
    public int? AccessCheckBy { get; set; }
    public int? MdCheckStatusId { get; set; }
    public DateTime? MdCheckDate { get; set; }
    public int? MdCheckBy { get; set; }
    public string? Notes { get; set; }
    
    public DtpObject() { }

    public DtpObject(DtpObjectInDb d)
    {
        Id = d.id;
        DtpId = d.dtp_id;
        SdOid = d.sd_oid;
        IsDataset = d.is_dataset;
        AccessTypeId = d.access_type_id;
        DownloadAllowed = d.download_allowed;
        AccessDetails = d.access_details;
        EmbargoRequested = d.embargo_requested;
        EmbargoRegime = d.embargo_regime;
        EmbargoStillApplies = d.embargo_still_applies;
        AccessCheckStatusId = d.access_check_status_id;
        AccessCheckDate = d.access_check_date?.ToDateTime(TimeOnly.MinValue);
        AccessCheckBy = d.access_check_by;
        MdCheckStatusId = d.md_check_status_id;
        MdCheckDate = d.md_check_date?.ToDateTime(TimeOnly.MinValue);
        MdCheckBy = d.md_check_by;
        Notes = d.notes;
    }
}

public class DtpPrereq
{
    public int Id { get; set; }
    public int? DtpId { get; set; }
    public string? SdOid { get; set; }
    public int? PreRequisiteTypeId { get; set; }
    public string? PreRequisiteNotes { get; set; }

    public DtpPrereq() { }

    public DtpPrereq(DtpPrereqInDb d)
    {
        Id = d.id;
        DtpId = d.dtp_id;
        SdOid = d.sd_oid;
        PreRequisiteTypeId = d.pre_requisite_type_id;
        PreRequisiteNotes = d.pre_requisite_notes;
    }
}


public class DtpNote
{
    public int Id { get; set; }
    public int? DtpId { get; set; }
    public string? Text { get; set; }
    public int? Author { get; set; }
    public DateTime? CreatedOn { get; set; }
    
    public DtpNote() { }

    public DtpNote(DtpNoteInDb d)
    {
        Id = d.id;
        DtpId = d.dtp_id;
        Text = d.text;
        Author = d.author;
        CreatedOn = d.created_on;
    }
}


public class DtpPerson
{
    public int Id { get; set; }
    public int? DtpId { get; set; }
    public int? PersonId { get; set; }
    public string? Notes { get; set; }
    
    public DtpPerson() { }

    public DtpPerson(DtpPersonInDb d)
    {
        Id = d.id;
        DtpId = d.dtp_id;
        PersonId = d.person_id;
        Notes = d.notes;
    }
}
