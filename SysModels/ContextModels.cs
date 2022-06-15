using Dapper.Contrib.Extensions;

namespace rmsbe.SysModels;


[Table("rms.access_prereqs")]
public class OrganisationInDb
{
    public int id { get; set; }
    public string? default_name { get; set; }
    public string? ror_id { get; set; }
    public string? display_suffix { get; set; }
    public int? scope_id { get; set; }
    public string? scope_notes { get; set; }
    public bool? is_current { get; set; }
    public int? year_established { get; set; }
    public int? year_ceased { get; set; }
    public DateTime? created_on { get; set; }
}

public class OrganisationName
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

