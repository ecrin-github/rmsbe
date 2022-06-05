using rmsbe.DbModels;
namespace rmsbe.SysModels;

public class AccessPrereq
{
    public int Id { get; set; }
    public int? ObjectId { get; set; }
    public int? PreRequisiteId { get; set; }
    public string? PreRequisiteNotes { get; set; }

    public AccessPrereq() { }

    public AccessPrereq(AccessPrereqInDb d)
    {
        Id = d.id;
        ObjectId = d.object_id;
        PreRequisiteId = d.pre_requisite_id;
        PreRequisiteNotes = d.pre_requisite_notes;
    }
}

public class ProcessNote
{
    public int Id { get; set; }
    public int? ProcessType { get; set; }
    public int? ProcessId { get; set; }
    public string? Text { get; set; }
    public int? Author { get; set; }
    
    public ProcessNote() { }

    public ProcessNote(ProcessNoteInDb d)
    {
        Id = d.id;
        ProcessType = d.process_type;
        ProcessId = d.process_id;
        Text = d.text;
        Author = d.author;
    }
}

public class ProcessPeople
{
    public int Id { get; set; }
    public int? ProcessType { get; set; }
    public int? ProcessId { get; set; }
    public int? PersonId { get; set; }
    public bool? IsAUser { get; set; }
    public string? Notes { get; set; }
    
    public ProcessPeople() { }

    public ProcessPeople(ProcessPeopleInDb d)
    {
        Id =d.id;
        ProcessType = d.process_type;
        ProcessId = d.process_id;
        PersonId = d.person_id;
        IsAUser = d.is_a_user;
        Notes = d.notes;
    }
}

