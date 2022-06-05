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
}

public class LupWithDescription
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}

public class LupWithListOrder
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int? ListOrder { get; set; }
}

public class LupWithDescAndListOrder
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? ListOrder { get; set; }
}

