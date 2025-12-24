namespace LostAndFound.Configuration;

public class SeedDataSettings
{
    public SuperadminSettings Superadmin { get; set; } = new();
    public BuildingSettings DefaultBuilding { get; set; } = new();
    public RoomSettings DefaultRoom { get; set; } = new();
}

public class SuperadminSettings
{
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FullName { get; set; } = "Super Administrator";
}

public class BuildingSettings
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}

public class RoomSettings
{
    public string Name { get; set; } = string.Empty;
    public int Floor { get; set; } = 1;
}
