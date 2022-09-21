using rmsbe.DbModels;

namespace rmsbe.SysModels;


public class FullDup
{
    public Dup? CoreDup { get; set; }
    public List<Dua>? Duas { get; set; }
    public List<DupStudy>? DupStudies { get; set; }
    public List<DupObject>? DupObjects { get; set; }
    public List<DupPrereq>? DupPrereqs { get; set; }
    public List<DupSecondaryUse>? DupSecUses { get; set; }    
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
        DupSecUses = d.dup_sec_use_in_db?.Select(r => new DupSecondaryUse(r)).ToList();;
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

public class DupOut
{
    public int Id { get; set; }
    public int OrgId { get; set; }
    public string? OrgName { get; set; }
    public string? DisplayName { get; set; }
    public int? StatusId { get; set; }
    public string? StatusName { get; set; }
    public DateTime? InitialContactDate { get; set; }
    public DateTime? SetUpCompleted { get; set; }
    public DateTime? PrereqsMet { get; set; }
    public DateTime? DuaAgreedDate { get; set; }
    public DateTime? AvailabilityRequested { get; set; }
    public DateTime? AvailabilityConfirmed { get; set; }
    public DateTime? AccessConfirmed { get; set; }
    
    public DupOut(DupOutInDb d)
    {
        Id = d.id;
        OrgId = d.org_id;
        OrgName = d.org_name;
        DisplayName = d.display_name;
        StatusId = d.status_id;
        StatusName = d.status_name;
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
    public string? OrgName {get; set; }
    public string? DisplayName { get; set; }
    public string? statusName { get; set; }
    
    public DupEntry() { }

    public DupEntry(DupEntryInDb d)
    {
        Id = d.id;
        OrgName = d.org_name;
        DisplayName = d.display_name;
        statusName = d.status_name;
    }
}


public class Dua
{
    public int Id { get; set; }
    public int DupId { get; set; }
    public bool? ConformsToDefault { get; set; }
    public string? Variations { get; set; }
    public string? DuaFilePath { get; set; }
    public bool? RepoIsProxyProvider { get; set; }
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
        RepoIsProxyProvider = d.repo_is_proxy_provider;
        DuaFilePath = d.dua_file_path;
        RepoSignatory1 = d.repo_signatory_1;
        RepoSignatory2 = d.repo_signatory_2;
        ProviderSignatory1 = d.provider_signatory_1;
        ProviderSignatory2 = d.provider_signatory_2;
        RequesterSignatory1 = d.requester_signatory_1;
        RequesterSignatory2 = d.requester_signatory_2;
        Notes = d.notes;
    }
}

public class DuaOut
{
    public int Id { get; set; }
    public int DupId { get; set; }
    public bool? ConformsToDefault { get; set; }
    public string? Variations { get; set; }
    public bool? RepoIsProxyProvider { get; set; }
    public string? DuaFilePath { get; set; }
    public int? RepoSignatory1 { get; set; }
    public string? RepoSignatory1Name { get; set; }
    public int? RepoSignatory2 { get; set; }
    public string? RepoSignatory2Name { get; set; }
    public int? ProviderSignatory1 { get; set; }
    public string? ProviderSignatory1Name { get; set; }
    public int? ProviderSignatory2 { get; set; }
    public string? ProviderSignatory2Name { get; set; }
    public int? RequesterSignatory1 { get; set; }
    public string? RequesterSignatory1Name { get; set; }
    public int? RequesterSignatory2 { get; set; }
    public string? RequesterSignatory2Name { get; set; }
    public string? Notes { get; set; }
    
    public DuaOut(DuaOutInDb d)
    {
        Id = d.id;
        DupId = d.dup_id;
        ConformsToDefault = d.conforms_to_default;
        Variations = d.variations;
        RepoIsProxyProvider = d.repo_is_proxy_provider;
        DuaFilePath = d.dua_file_path;
        RepoSignatory1 = d.repo_signatory_1;
        RepoSignatory1Name = d.repo_signatory_1_name;
        RepoSignatory2 = d.repo_signatory_2;
        RepoSignatory2Name = d.repo_signatory_2_name;
        ProviderSignatory1 = d.provider_signatory_1;
        ProviderSignatory1Name = d.provider_signatory_1_name;
        ProviderSignatory2 = d.provider_signatory_2;
        ProviderSignatory2Name = d.provider_signatory_2_name;
        RequesterSignatory1 = d.requester_signatory_1;
        RequesterSignatory1Name = d.requester_signatory_1_name;
        RequesterSignatory2 = d.requester_signatory_2;
        RequesterSignatory2Name = d.requester_signatory_2_name;
        Notes = d.notes;
    }
}



public class DupStudy
{
    public int Id { get; set; }
    public int DupId { get; set; }
    public string? SdSid { get; set; }
    
    public DupStudy() { }

    public DupStudy(DupStudyInDb d)
    {
        Id = d.id;
        DupId = d.dup_id;
        SdSid = d.sd_sid;
    }
}

public class DupStudyOut
{
    public int Id { get; set; }
    public int DupId { get; set; }
    public string? SdSid { get; set; }
    public string? StudyName { get; set; }
    
    public DupStudyOut(DupStudyOutInDb d)
    {
        Id = d.id;
        DupId = d.dup_id;
        SdSid = d.sd_sid;
        StudyName = d.study_name;
    }
}

public class DupObject
{
    public int Id { get; set; }
    public int DupId { get; set; }
    public string? SdOid { get; set; }
    public int? AccessTypeId { get; set; }
    public string? AccessDetails { get; set; }
    public string? Notes { get; set; }
    
    public DupObject() { }

    public DupObject(DupObjectInDb d)
    {
        Id = d.id;
        DupId = d.dup_id;
        SdOid = d.sd_oid;
        AccessTypeId = d.access_type_id;
        AccessDetails = d.access_details;
        Notes = d.notes;
    }
}

public class DupObjectOut
{
    public int Id { get; set; }
    public int DupId { get; set; }
    public string? SdOid { get; set; }
    public string? ObjectName { get; set; }
    public int? AccessTypeId { get; set; }
    public string? AccessTypeName { get; set; }
    public string? AccessDetails { get; set; }
    public string? Notes { get; set; }
    
    public DupObjectOut(DupObjectOutInDb d)
    {
        Id = d.id;
        DupId = d.dup_id;
        SdOid = d.sd_oid;
        ObjectName = d.object_name;
        AccessTypeId = d.access_type_id;
        AccessTypeName = d.access_type_name;
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
    public string? PreRequisiteNotes { get; set; }
    public DateTime? PreRequisiteMet { get; set; }
    public string? MetNotes { get; set; }
    
    public DupPrereq() { }

    public DupPrereq(DupPrereqInDb d)
    {
        Id = d.id;
        DupId = d.dup_id;
        SdOid = d.sd_oid;
        PreRequisiteId = d.pre_requisite_id;
        PreRequisiteNotes = d.pre_requisite_notes;
        PreRequisiteMet = d.pre_requisite_met?.ToDateTime(TimeOnly.MinValue);
        MetNotes = d.met_notes;
    }
}

public class DupPrereqOut
{
    public int Id { get; set; }
    public int DupId { get; set; }
    public string? SdOid { get; set; }
    public string? ObjectName { get; set; }
    public int? PreRequisiteId { get; set; }
    public string? PreRequisiteName  { get; set; }
    public string? PreRequisiteNotes { get; set; }
    public DateTime? PreRequisiteMet { get; set; }
    public string? MetNotes { get; set; }
    
    public DupPrereqOut(DupPrereqOutInDb d)
    {
        Id = d.id;
        DupId = d.dup_id;
        SdOid = d.sd_oid;
        ObjectName = d.object_name;
        PreRequisiteId = d.pre_requisite_id;
        PreRequisiteName = d.pre_requisite_name;
        PreRequisiteNotes = d.pre_requisite_notes;
        PreRequisiteMet = d.pre_requisite_met?.ToDateTime(TimeOnly.MinValue);
        MetNotes = d.met_notes;
    }
}



public class DupSecondaryUse
{
    public int Id { get; set; }
    public int DupId { get; set; }
    public string? SecondaryUseSummary { get; set; }
    public string? Publication { get; set; }
    public string? Doi { get; set; }
    public bool? AttributionPresent { get; set; }
    public string? Notes { get; set; }

    public DupSecondaryUse() { }

    public DupSecondaryUse(DupSecondaryUseInDb d)
    {
        Id = d.id;
        DupId = d.dup_id;
        SecondaryUseSummary = d.secondary_use_summary;
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
    public DateTime? CreatedOn { get; set; }
    
    public DupNote() { }

    public DupNote(DupNoteInDb d)
    {
        Id = d.id;
        DupId = d.dup_id;
        Text = d.text;
        Author = d.author;
        CreatedOn = d.created_on;
    }
}

public class DupNoteOut
{
    public int Id { get; set; }
    public int? DupId { get; set; }
    public string? Text { get; set; }
    public int? Author { get; set; }
    public string? AuthorName { get; set; }
    public DateTime? CreatedOn { get; set; }
    
    public DupNoteOut(DupNoteOutInDb d)
    {
        Id = d.id;
        DupId = d.dup_id;
        Text = d.text;
        Author = d.author;
        AuthorName = d.author_name;
        CreatedOn = d.created_on;
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

public class DupPersonOut
{
    public int Id { get; set; }
    public int? DupId { get; set; }
    public int? PersonId { get; set; }
    public string? PersonName { get; set; }
    public string? Notes { get; set; }
    
    public DupPersonOut(DupPersonOutInDb d)
    {
        Id = d.id;
        DupId = d.dup_id;
        PersonId = d.person_id;
        PersonName = d.person_name;
        Notes = d.notes;
    }
}