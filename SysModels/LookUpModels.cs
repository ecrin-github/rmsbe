using rmsbe.DbModels;

namespace rmsbe.SysModels;

/****************************************************************************
 *
 *
 *
 * v1.0, Steve Canham; 02/06/2022
 ***************************************************************************/

public class Lup
{
    public int Id { get; set; }
    public string? Name { get; set; }
    
    public Lup() { }
    
    public Lup(int id, string? name )
    {
        Id = id;
        Name = name;
    }
}

public class LupWithDescription
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    
    public LupWithDescription() { }
    
    public LupWithDescription(int id, string? name, string? description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
}

public class LupWithListOrder
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int? ListOrder { get; set; }
    
    public LupWithListOrder() { }
    
    public LupWithListOrder(int id, string? name, int? list_order )
    {
        Id = id;
        Name = name;
        ListOrder = list_order;
    }
}

public class LupFull
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? ListOrder { get; set; }
    
    public LupFull() { }
    
    public LupFull(BaseLup d)
    {
        Id = d.id;
        Name = d.name;
        Description = d.description;
        ListOrder = d.list_order;
    }
}

