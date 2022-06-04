using Dapper.Contrib.Extensions;

namespace rmsbe.DbModels;

[Table("mdr.record_changes")]
public class RecordChange
{
    public int id {get; set;}
    public string? table_name {get; set;}
    public int? table_id {get; set;}
    public int? change_type {get; set;}
    public DateTime change_time {get; set;}
    public string? user_name {get; set;}
    public string? prior {get; set;}
    public string? post {get; set;}
}
