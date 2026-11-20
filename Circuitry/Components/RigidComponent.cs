using CircuitSim.Numerics;

namespace CircuitSim.Circuitry;

public partial class RigidComponent : Component
{
    public Vector2Int Position = Vector2Int.Zero;

    private ComponentOrientation _orientiation = ComponentOrientation.Right;
    public ComponentOrientation Orientation
    {
        get => _orientiation;
        set
        {
            _orientiation = value;
            CircuitSymbol.Orientation = value;
        }
    }

    public bool _isPreview = false;
    public override bool IsPreview {
        get => _isPreview;
        set
        {
            _isPreview = value;
            CircuitSymbol.IsPreview = value;
        }
    }

    public RigidComponent() {
        InitializeComponent();
    }
}
