namespace rmsbe.DbModels;

public class StatisticInDb
{
    public int stat_type {get; set;}
    public int stat_value {get; set;}
    
    public StatisticInDb() { }
    
    public StatisticInDb(int statType, int statValue)
    {
        stat_type = statType;
        stat_value = statValue;
    }
}