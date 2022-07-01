using Npgsql;
using rmsbe.Helpers.Interfaces;

namespace rmsbe.Helpers;

public class Creds : ICreds
{
    private readonly Dictionary<string, Cred> _creds = new Dictionary<string, Cred>();

    public Creds(IConfiguration settings)
    {
        _creds.Add("rms", new Cred{  
            host_name = settings["rms_host"], 
            user_name = settings["rms_user"],
            pass_word = settings["rms_password"], 
            db_name = settings["rms_db"]});
        
        _creds.Add("mdm", new Cred{ 
            host_name = settings["mdm_host"], 
            user_name = settings["mdm_user"],
            pass_word = settings["mdm_password"], 
            db_name = settings["mdm_db"]});
        
        _creds.Add("context", new Cred{ 
            host_name = settings["context_host"], 
            user_name = settings["context_user"],
            pass_word = settings["context_password"], 
            db_name = settings["context_db"]});
        
        _creds.Add("people", new Cred{ 
            host_name = settings["people_host"], 
            user_name = settings["people_user"],
            pass_word = settings["people_password"], 
            db_name = settings["people_db"]});
    }
    
    public string GetConnectionString(string service_name)
    {
        Cred c = _creds[service_name];
        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = c.host_name,
            Username = c.user_name,
            Password = c.pass_word,
            Database = c.db_name
        };
        return builder.ConnectionString;
    }

    private class Cred
    {
        public string? host_name { get; init; }
        public string? user_name { get; init; }
        public string? pass_word { get; init; }
        public string? db_name { get; init; }
    }
}

