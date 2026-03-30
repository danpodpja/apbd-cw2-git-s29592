namespace apbd_cw2_git_s29592.Domain.Equipment;

public class Projector : Equipment
{
    public string Resolution { get; set; }
    public int    Lumens     { get; set; }

    public Projector(string name, string resolution, int lumens) : base(name)
    {
        Resolution = resolution;
        Lumens     = lumens;
    }

    public override string GetTypeName() => "Projector";

    public override string ToString() =>
        $"{base.ToString()} | {Resolution}, {Lumens} lm";
}