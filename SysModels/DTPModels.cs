using rmsbe.DbModels;

namespace rmsbe.SysModels;

public class Dtp
{
    public int Id { get; set; }
    public int? OrgId { get; set; }
    public string? DisplayName { get; set; }
    public int? StatusId { get; set; }
    public DateOnly? InitialContactDate { get; set; }
    public DateOnly? SetUpCompleted { get; set; }
    public DateOnly? MdAccessGranted { get; set; }
    public DateOnly? MdCompleteDate { get; set; }
    public DateOnly? DtaAgreedDate { get; set; }
    public DateOnly? UploadAccessRequested { get; set; }
    public DateOnly? UploadAccessConfirmed { get; set; }
    public DateOnly? UploadsComplete { get; set; }
    public DateOnly? QcChecksCompleted { get; set; }
    public DateOnly? MdIntegratedWithMdr { get; set; }
    public DateOnly? AvailabilityRequested { get; set; }
    public DateOnly? AvailabilityConfirmed { get; set; }

    public Dtp() { }

    public Dtp(DtpInDb d)
    {
        Id = d.id;
        OrgId = d.org_id;
        DisplayName = d.display_name;
        StatusId = d.status_id;
        InitialContactDate = d.initial_contact_date;
        SetUpCompleted = d.set_up_completed;
        MdAccessGranted = d.md_access_granted;
        MdCompleteDate = d.md_complete_date;
        DtaAgreedDate = d.dta_agreed_date;
        UploadAccessRequested = d.upload_access_requested;
        UploadAccessConfirmed = d.upload_access_confirmed;
        UploadsComplete = d.uploads_complete;
        QcChecksCompleted = d.qc_checks_completed;
        MdIntegratedWithMdr = d.md_integrated_with_mdr;
        AvailabilityRequested = d.availability_requested;
        AvailabilityConfirmed = d.availability_confirmed;
    }
}

public class Dta
{
    public int Id { get; set; }
    public int? DtpId { get; set; }
    public int? ConformsToDefault { get; set; }
    public string? Variations { get; set; }
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
    public string? ObjectId { get; set; }
    public int? LegalStatusId { get; set; }
    public string? LegalStatusText { get; set; }
    public string? LegalStatusPath { get; set; }
    public int? DescmdCheckStatusId { get; set; }
    public DateOnly? DescmdCheckDate { get; set; }
    public int? DescmdCheckBy { get; set; }
    public int? DeidentCheckStatusId { get; set; }
    public DateOnly? DeidentCheckDate { get; set; }
    public int? DeidentCheckBy { get; set; }
    public string? Notes { get; set; }
    
    public DtpDataset() { }

    public DtpDataset(DtpDatasetInDb d)
    {
        Id = d.id;
        ObjectId = d.object_id;
        LegalStatusId = d.legal_status_id;
        LegalStatusText = d.legal_status_text;
        LegalStatusPath = d.legal_status_path;
        DescmdCheckStatusId = d.desc_md_check_status_id;
        DescmdCheckDate = d.desc_md_check_date;
        DescmdCheckBy = d.desc_md_check_by;
        DeidentCheckStatusId = d.deident_check_status_id;
        DeidentCheckDate = d.deident_check_date;
        DeidentCheckBy = d.deident_check_by;
        Notes = d.notes;
    }
}

public class DtpStudy
{
    public int Id { get; set; }
    public int DtpId { get; set; }
    public string? StudyId { get; set; }
    public int? MdCheckStatusId { get; set; }
    public DateOnly? MdCheckDate { get; set; }
    public int? MdCheckBy { get; set; }
    
    public DtpStudy() { }

    public DtpStudy(DtpStudyInDb d)
    {
        Id = d.id;
        DtpId = d.dtp_id;
        StudyId = d.study_id;
        MdCheckStatusId = d.md_check_status_id;
        MdCheckDate = d.md_check_date;
        MdCheckBy = d.md_check_by;
    }
}

public class DtpObject
{
    public int Id { get; set; }
    public int DtpId { get; set; }
    public string? ObjectId { get; set; }
    public bool? IsDataset { get; set; }
    public int? AccessTypeId { get; set; }
    public bool? DownloadAllowed { get; set; }
    public string? AccessDetails { get; set; }
    public bool? RequiresEmbargoPeriod { get; set; }
    public DateOnly? EmbargoEndDate { get; set; }
    public bool? EmbargoStillApplies { get; set; }
    public int? AccessCheckStatusId { get; set; }
    public DateOnly? AccessCheckDate { get; set; }
    public string? AccessCheckBy { get; set; }
    public int? MdCheckStatusId { get; set; }
    public DateOnly? MdCheckDate { get; set; }
    public string? MdCheckBy { get; set; }
    public string? Notes { get; set; }
    
    public DtpObject() { }

    public DtpObject(DtpObjectInDb d)
    {
        Id = d.id;
        DtpId = d.dtp_id;
        ObjectId = d.object_id;
        IsDataset = d.is_dataset;
        AccessTypeId = d.access_type_id;
        DownloadAllowed = d.download_allowed;
        AccessDetails = d.access_details;
        RequiresEmbargoPeriod = d.requires_embargo_period;
        EmbargoEndDate = d.embargo_end_date;
        EmbargoStillApplies = d.embargo_still_applies;
        AccessCheckStatusId = d.access_check_status_id;
        AccessCheckDate = d.access_check_date;
        AccessCheckBy = d.access_check_by;
        MdCheckStatusId = d.md_check_status_id;
        MdCheckDate = d.md_check_date;
        MdCheckBy = d.md_check_by;
        Notes = d.notes;
    }
}

public class DtpPrereq
{
    public int Id { get; set; }
    public int? DtpId { get; set; }
    public string? ObjectId { get; set; }
    public int? PreRequisiteTypeId { get; set; }
    public string? PreRequisiteNotes { get; set; }

    public DtpPrereq() { }

    public DtpPrereq(DtpPrereqInDb d)
    {
        Id = d.id;
        DtpId = d.dtp_id;
        ObjectId = d.object_id;
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
    
    public DtpNote() { }

    public DtpNote(DtpNoteInDb d)
    {
        Id = d.id;
        DtpId = d.dtp_id;
        Text = d.text;
        Author = d.author;
    }
}


public class DtpPerson
{
    public int Id { get; set; }
    public int? DtpId { get; set; }
    public int? PersonId { get; set; }
    public bool? IsAUser { get; set; }
    public string? Notes { get; set; }
    
    public DtpPerson() { }

    public DtpPerson(DtpPersonInDb d)
    {
        Id = d.id;
        DtpId = d.dtp_id;
        PersonId = d.person_id;
        IsAUser = d.is_a_user;
        Notes = d.notes;
    }
}
