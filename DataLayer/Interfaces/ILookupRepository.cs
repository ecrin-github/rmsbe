using rmsbe.DbModels;

namespace rmsbe.DataLayer.Interfaces;

public interface ILookupRepository
{
    Task<IEnumerable<BaseLup>> GetLupDataAsync(string typeName);
}