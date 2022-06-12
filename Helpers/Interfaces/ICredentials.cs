namespace rmsbe.Helpers.Interfaces;

public interface ICredentials
{
    string GetConnectionString(string service_name);
}