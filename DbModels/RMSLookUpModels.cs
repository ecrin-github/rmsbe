using Dapper.Contrib.Extensions;

namespace rmsbe.DbModels;

public class BaseRmsLookUp : BaseLup 
{
    public DateOnly date_added { get; set; }
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


