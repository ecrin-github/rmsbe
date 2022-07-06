using rmsbe.SysModels;

namespace rmsbe.Services.Interfaces;

public interface ITestService
{
    Task<Statistic> GetTotal(string tableName);
    Task<Statistic> GetMaxId(string tableName);
    Task<Statistic> StoreNewIds(string tableName);
    Task<Statistic> DeleteTestData(string tableName);
    Task<Statistic>ResetIdentitySequence(string tableName);
}




