using rmsbe.DbModels;

namespace rmsbe.SysModels;


public class FullDup
{
    public Dup? CoreDup { get; set; }
    public List<Dua>? Duas { get; set; }
    public List<DupStudy>? DupStudies { get; set; }
    public List<DupObject>? DupObjects { get; set; }
    public List<DupPrereq>? DupPrereqs { get; set; }
    public List<SecondaryUse>? DupSecUses { get; set; }    
    public List<DupNote>? DupNotes { get; set; }
    public List<DupPerson>? DupPeople { get; set; }

    public FullDup() { }

    public FullDup(FullDupInDb d)
    {
        CoreDup = d.core_dup == null ? null : new Dup(d.core_dup);
        Duas = d.duas_in_db?.Select(r => new Dua(r)).ToList();
        DupStudies = d.dup_studies_in_db?.Select(r => new DupStudy(r)).ToList();;
        DupObjects = d.dup_objects_in_db?.Select(r => new DupObject(r)).ToList();;
        DupPrereqs = d.dup_prereqs_in_db?.Select(r => new DupPrereq(r)).ToList();;
        DupSecUses = d.dup_sec_use_in_db?.Select(r => new SecondaryUse(r)).ToList();;
        DupNotes = d.dup_notes_in_db?.Select(r => new DupNote(r)).ToList();;
        DupPeople = d.dup_people_in_db?.Select(r => new DupPerson(r)).ToList();;
    }
}


public class Dup
{
    public int Id { get; set; }
    public int OrgId { get; set; }
    public string? DisplayName { get; set; }
    public int? StatusId { get; set; }
    public DateTime? InitialContactDate { get; set; }
    public DateTime? SetUpCompleted { get; set; }
    public DateTime? PrereqsMet { get; set; }
    public DateTime? DuaAgreedDate { get; set; }
    public DateTime? AvailabilityRequested { get; set; }
    public DateTime? AvailabilityConfirmed { get; set; }
    public DateTime? AccessConfirmed { get; set; }
    
    public Dup() { }

    public Dup(DupInDb d)
    {
        Id = d.id;
        OrgId = d.org_id;
        DisplayName = d.display_name;
        StatusId = d.status_id;
        InitialContactDate = d.initial_contact_date?.ToDateTime(TimeOnly.MinValue);
        SetUpCompleted = d.set_up_completed?.ToDateTime(TimeOnly.MinValue);
        PrereqsMet = d.prereqs_met?.ToDateTime(TimeOnly.MinValue);
        DuaAgreedDate = d.dua_agreed_date?.ToDateTime(TimeOnly.MinValue);
        AvailabilityRequested = d.availability_requested?.ToDateTime(TimeOnly.MinValue);
        AvailabilityConfirmed = d.availability_confirmed?.ToDateTime(TimeOnly.MinValue);
        AccessConfirmed = d.access_confirmed?.ToDateTime(TimeOnly.MinValue);
    }
}


public class DupEntry
{
    public int Id { get; set; }
    public int OrgId { get; set; }
    public string? DisplayName { get; set; }
    
    public DupEntry() { }

    public DupEntry(DupEntryInDb d)
    {
        Id = d.id;
        OrgId = d.org_id;
        DisplayName = d.display_name;
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
    public string? SdOid { get; set; }
    public int? PreRequisiteId { get; set; }
    public DateTime? PrerequisiteMet { get; set; }
    public string? MetNotes { get; set; }
    
    public DupPrereq() { }

    public DupPrereq(DupPrereqInDb d)
    {
        Id = d.id;
        DupId = d.dup_id;
        SdOid = d.sd_oid;
        PreRequisiteId = d.pre_requisite_id;
        PrerequisiteMet = d.prerequisite_met?.ToDateTime(TimeOnly.MinValue);
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
    public string? Notes { get; set; }
    
    public DupPerson() { }

    public DupPerson(DupPersonInDb d)
    {
        Id = d.id;
        DupId = d.dup_id;
        PersonId = d.person_id;
        Notes = d.notes;
    }
}