namespace apbd_cw2_git_s29592.Domain.Equipment;

public abstract class EquipmentA
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; set; }
    public EquipmentStatus Status { get; private set; } = EquipmentStatus.Available;

    protected EquipmentA(string name)
    {
        Name = name;
    }

    public bool IsAvailable => Status == EquipmentStatus.Available;

    public void MarkAsRented() => Status = EquipmentStatus.Rented;
    public void MarkAsAvailable() => Status = EquipmentStatus.Available;
    public void MarkAsUnavailable() => Status = EquipmentStatus.Unavailable;

    public abstract string GetTypeName();

    public override string ToString() =>
        $"[{Id.ToString()[..8]}] {GetTypeName()}: {Name} ({Status})";
}