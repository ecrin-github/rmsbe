using rmsbe.DbModels;

namespace rmsbe.DataLayer.Interfaces;

public interface ILookupRepository
{
    Task<IEnumerable<BaseLup>> GetLupData(string typeName);
}