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
    
    public async Task<Statistic> GetTotal(string tableName)
    {
        int res = await _testRepository.GetTotal(tableName);
        return new Statistic("Total", res);
    }
    
    public async Task<Statistic> GetMaxId(string tableName)
    {
        int res = await _testRepository.GetMaxId(tableName);
        return new Statistic("maxValue", res);
    }
    
    public async Task<Statistic> StoreNewIds(string tableName)
    {
        int res = await _testRepository.StoreNewIds(tableName);
        return new Statistic("numNewRecs", res);
    }
    
    public async Task<Statistic> DeleteTestData(string tableName)
    {
        int res = await _testRepository.DeleteTestData(tableName);
        return new Statistic("numDeletedRecs", res);
    }
    
    public async Task<Statistic>ResetIdentitySequence(string tableName)
    {
        int res = await _testRepository.ResetIdentitySequence(tableName);
        return new Statistic("nextIdentityValue", res);
    }
}

