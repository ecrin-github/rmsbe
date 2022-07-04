using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.WebUtilities;
using rmsbe.Services.Interfaces;
using rmsbe.DataLayer.Interfaces;
using rmsbe.SysModels;

namespace rmsbe.Services;

[SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Local")]
public class TestService : ITestService
{
    private readonly ITestRepository _testRepository;
    
    public TestService(ITestRepository testRepository)
    {
        _testRepository = testRepository ?? throw new ArgumentNullException(nameof(testRepository));
    }
    
    public async Task<Statistic> GetMaxId(string tableName)
    {
        int res = await _testRepository.GetMaxId(tableName);
        return new Statistic("maxValue", res);
    }
    
    public async Task<Statistic> StoreNewIds(string tableName, int oldMaxId)
    {
        int res = await _testRepository.StoreNewIds(tableName, oldMaxId);
        return new Statistic("numNewRecs", res);
    }
    
    public async Task<Statistic> DeleteTestData(string tableName, int oldMaxId)
    {
        int res = await _testRepository.DeleteTestData(tableName, oldMaxId);
        return new Statistic("numDeletedRecs", res);
    }
    
    public async Task<Statistic>SetNextId(string tableName)
    {
        int res = await _testRepository.SetNextId(tableName);
        return new Statistic("newIdentityValue", res);
    }
    
}

