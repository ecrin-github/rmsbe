using Dapper.Contrib.Extensions;

namespace rmsbe.DbModels;

public class BaseRmsLookUp
{
    public int id { get; set; }
    public string? name { get; set; }
    public string? description { get; set; }
    public int? list_order { get; set; }
    public DateOnly? created_on { get; set; }
}

[Table("rms.access_prereq_types")]
public class AccessPrereqTypeInDb : BaseRmsLookUp { }
[Table("rms.check_status_types")]
public class CheckStatusTypeInDb : BaseRmsLookUp { }
[Table("rms.dtp_status_types")]
public class DtpStatusTypeInDb : BaseRmsLookUp { }
[Table("rms.legal_status_types")]
public class LegalStatusTypeInDb : BaseRmsLookUp { }
[Table("rms.repo_access_types")]
public class RepoAccessTypeInDb : BaseRmsLookUp { }


