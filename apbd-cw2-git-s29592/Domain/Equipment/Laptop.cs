namespace apbd_cw2_git_s29592.Domain.Equipment;

public class Laptop : EquipmentA
{
    public int RamGb { get; set; }
    public int StorageGb { get; set; }

    public Laptop(string name, int ramGb, int storageGb) : base(name)
    {
        RamGb = ramGb;
        StorageGb = storageGb;
    }

    public override string GetTypeName() => "Laptop";

    public override string ToString() =>
        $"{base.ToString()} | RAM: {RamGb} GB, SSD: {StorageGb} GB";
}