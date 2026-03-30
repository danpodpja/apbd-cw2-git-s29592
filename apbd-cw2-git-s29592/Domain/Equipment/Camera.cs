namespace apbd_cw2_git_s29592.Domain.Equipment;

public class Camera : Equipment
{
    public double Megapixels { get; set; }
    public bool   HasTripod  { get; set; }

    public Camera(string name, double megapixels, bool hasTripod) : base(name)
    {
        Megapixels = megapixels;
        HasTripod  = hasTripod;
    }

    public override string GetTypeName() => "Camera";

    public override string ToString() =>
        $"{base.ToString()} | {Megapixels} MP, Statyw: {(HasTripod ? "tak" : "nie")}";
}