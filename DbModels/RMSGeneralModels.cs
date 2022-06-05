using Dapper.Contrib.Extensions;
using rmsbe.SysModels;

namespace rmsbe.DbModels;

[Table("rms.access_prereqs")]
public class AccessPrereqInDb
{
    public int id { get; set; }
    public int? object_id { get; set; }
    public int? pre_requisite_id { get; set; }
    public string? pre_requisite_notes { get; set; }
    [Computed] 
    public DateTime? created_on { get; set; }

    public AccessPrereqInDb() { }

    public AccessPrereqInDb(AccessPrereq d)
    {
        id = d.Id;
        object_id = d.ObjectId;
        pre_requisite_id = d.PreRequisiteId;
        pre_requisite_notes = d.PreRequisiteNotes;
    }
}


[Table("rms.process_notes")]
public class ProcessNoteInDb
{
    public int id { get; set; }
    public int? process_type { get; set; }
    public int? process_id { get; set; }
    public string? text { get; set; }
    public int? author { get; set; }
    [Computed]
    public DateTime? created_on { get; set; }
    
    public ProcessNoteInDb() { }

    public ProcessNoteInDb(ProcessNote d)
    {
        id = d.Id;
        process_type = d.ProcessType;
        process_id = d.ProcessId;
        text = d.Text;
        author = d.Author;
    }
}


[Table("rms.process_people")]
public class ProcessPeopleInDb
{
    public int id { get; set; }
    public int? process_type { get; set; }
    public int? process_id { get; set; }
    public int? person_id { get; set; }
    public bool? is_a_user { get; set; }
    public string? notes { get; set; }
    [Computed]
    public DateTime created_on { get; set; }
    
    public ProcessPeopleInDb() { }

    public ProcessPeopleInDb(ProcessPeople d)
    {
        id = d.Id;
        process_type = d.ProcessType;
        process_id = d.ProcessId;
        person_id = d.PersonId;
        is_a_user = d.IsAUser;
        notes = d.Notes;
    }
}

