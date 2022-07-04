using rmsbe.SysModels;

namespace rmsbe.Services.Interfaces;

public interface ITestService
{
    Task<Statistic> GetMaxId(string tableName);
    Task<Statistic> StoreNewIds(string tableName, int oldMaxId);
    Task<Statistic> DeleteTestData(string tableName, int oldMaxId);
    Task<Statistic>SetNextId(string tableName);
}




