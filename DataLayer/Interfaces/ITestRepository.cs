using rmsbe.DbModels;

namespace rmsbe.DataLayer.Interfaces;

public interface ITestRepository
{
    Task<int> GetMaxId(string tableName);
    Task<int> StoreNewIds(string tableName, int oldMaxId);
    Task<int> DeleteTestData(string tableName, int oldMaxId);
    Task<int>SetNextId(string tableName);
}