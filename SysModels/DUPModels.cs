using rmsbe.DbModels;

namespace rmsbe.SysModels;

public class Dup
{
    public int Id { get; set; }
    public int OrgId { get; set; }
    public string? DisplayName { get; set; }
    public int? StatusId { get; set; }
    public DateOnly? InitialContactDate { get; set; }
    public DateOnly? SetUpCompleted { get; set; }
    public DateOnly? PrereqsMet { get; set; }
    public DateOnly? DuaAgreedDate { get; set; }
    public DateOnly? AvailabilityRequested { get; set; }
    public DateOnly? AvailabilityConfirmed { get; set; }
    public DateOnly? AccessConfirmed { get; set; }
    
    public Dup() { }

    public Dup(DupInDb d)
    {
        Id = d.id;
        OrgId = d.org_id;
        DisplayName = d.display_name;
        StatusId = d.status_id;
        InitialContactDate = d.initial_contact_date;
        SetUpCompleted = d.set_up_completed;
        PrereqsMet = d.prereqs_met;
        DuaAgreedDate = d.dua_agreed_date;
        AvailabilityRequested = d.availability_requested;
        AvailabilityConfirmed = d.availability_confirmed;
        AccessConfirmed = d.access_confirmed;
    }
}


public class Dua
{
    public int Id { get; set; }
    public int DupId { get; set; }
    public int? ConformsToDefault { get; set; }
    public string? Variations { get; set; }
    public bool? RepoAsProxy { get; set; }
    public int? RepoSignatory1 { get; set; }
    public int? RepoSignatory2 { get; set; }
    public int? ProviderSignatory1 { get; set; }
    public int? ProviderSignatory2 { get; set; }
    public int? RequesterSignatory1 { get; set; }
    public int? RequesterSignatory2 { get; set; }
    public string? Notes { get; set; }
    
    public Dua() { }

    public Dua(DuaInDb d)
    {
        Id = d.id;
        DupId = d.dup_id;
        ConformsToDefault = d.conforms_to_default;
        Variations = d.variations;
        RepoAsProxy = d.repo_as_proxy;
        RepoSignatory1 = d.repo_signatory_1;
        RepoSignatory2 = d.repo_signatory_2;
        ProviderSignatory1 = d.provider_signatory_1;
        ProviderSignatory2 = d.provider_signatory_2;
        RequesterSignatory1 = d.requester_signatory_1;
        RequesterSignatory2 = d.requester_signatory_2;
        Notes = d.notes;
    }
}


public class DupStudy
{
    public int Id { get; set; }
    public int DupId { get; set; }
    public string? StudyId { get; set; }
    
    public DupStudy() { }

    public DupStudy(DupStudyInDb d)
    {
        Id = d.id;
        DupId = d.dup_id;
        StudyId = d.study_id;
    }
}


public class DupObject
{
    public int Id { get; set; }
    public int DupId { get; set; }
    public string? ObjectId { get; set; }
    public int? AccessTypeId { get; set; }
    public string? AccessDetails { get; set; }
    public string? Notes { get; set; }
    
    public DupObject() { }

    public DupObject(DupObjectInDb d)
    {
        Id = d.id;
        DupId = d.dup_id;
        ObjectId = d.object_id;
        AccessTypeId = d.access_type_id;
        AccessDetails = d.access_details;
        Notes = d.notes;
    }
}


public class DupPrereq
{
    public int Id { get; set; }
    public int DupId { get; set; }
    public string? ObjectId { get; set; }
    public int? PreRequisiteId { get; set; }
    public DateOnly? PrerequisiteMet { get; set; }
    public string? MetNotes { get; set; }
    
    public DupPrereq() { }

    public DupPrereq(DupPrereqInDb d)
    {
        Id = d.id;
        DupId = d.dup_id;
        ObjectId = d.object_id;
        PreRequisiteId = d.pre_requisite_id;
        PrerequisiteMet = d.prerequisite_met;
        MetNotes = d.met_notes;
    }
}


public class SecondaryUse
{
    public int Id { get; set; }
    public int DupId { get; set; }
    public string? SecondaryUseType { get; set; }
    public string? Publication { get; set; }
    public string? Doi { get; set; }
    public bool? AttributionPresent { get; set; }
    public string? Notes { get; set; }

    public SecondaryUse() { }

    public SecondaryUse(SecondaryUseInDb d)
    {
        Id = d.id;
        DupId = d.dup_id;
        SecondaryUseType = d.secondary_use_type;
        Publication = d.publication;
        Doi = d.doi;
        AttributionPresent = d.attribution_present;
        Notes = d.notes;
    }
}


public class DupNote
{
    public int Id { get; set; }
    public int? DupId { get; set; }
    public string? Text { get; set; }
    public int? Author { get; set; }
    
    public DupNote() { }

    public DupNote(DupNoteInDb d)
    {
        Id = d.id;
        DupId = d.dup_id;
        Text = d.text;
        Author = d.author;
    }
}


public class DupPerson
{
    public int Id { get; set; }
    public int? DupId { get; set; }
    public int? PersonId { get; set; }
    public bool? IsAUser { get; set; }
    public string? Notes { get; set; }
    
    public DupPerson() { }

    public DupPerson(DupPersonInDb d)
    {
        Id = d.id;
        DupId = d.dup_id;
        PersonId = d.person_id;
        IsAUser = d.is_a_user;
        Notes = d.notes;
    }
}