using rmsbe.DbModels;

namespace rmsbe.DataLayer.Interfaces;

public interface ITestRepository
{
    Task<int> GetTotal(string tableName);
    Task<int> GetMaxId(string tableName);
    Task<int> StoreNewIds(string tableName);
    Task<int> DeleteTestData(string tableName);
    Task<int> ResetIdentitySequence(string tableName);
}