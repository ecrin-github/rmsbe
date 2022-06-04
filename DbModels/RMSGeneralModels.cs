using Dapper.Contrib.Extensions;

namespace rmsbe.DbModels;


[Table("rms.access_prereqs")]
public class AccessPrereqInDb
{
    public int id { get; set; }
    public int? object_id { get; set; }
    public int? pre_requisite_id { get; set; }
    public string? pre_requisite_notes { get; set; }
    public DateTime? created_on { get; set; }
}


[Table("rms.process_notes")]
public class ProcessNoteInDb
{
    public int id { get; set; }
    public int? process_type { get; set; }
    public int? process_id { get; set; }
    public string? text { get; set; }
    public int? author { get; set; }
    public DateTime? created_on { get; set; }
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
    public DateTime created_on { get; set; }
}

