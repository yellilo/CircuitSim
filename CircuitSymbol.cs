using CircuitSim.Circuitry;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Numerics;
using Windows.UI;

namespace CircuitSim;

public sealed partial class CircuitSymbol : UserControl
{
    public static readonly int BaseSize = 20;
    public static readonly string Font = "ms-appx:///Assets/Fonts/CircuitSimCircuitSymbols.ttf#CircuitSimCircuitSymbols";

    public static Color DefaultColor => ((SolidColorBrush)Application.Current.Resources["SystemControlForegroundBaseHighBrush"]).Color;
    public static Color PreviewColor => ((SolidColorBrush)Application.Current.Resources["SystemControlForegroundBaseMediumBrush"]).Color;

    public CanvasAntialiasing Antialiasing { get; set; } = CanvasAntialiasing.Aliased;

    private bool _isPreview = false;
    public bool IsPreview
    {
        get => _isPreview;
        set
        {
            _isPreview = value;
            Canvas.Invalidate();
        }
    }

    private CircuitSymbolType _type = CircuitSymbolType.Ammeter;
    public CircuitSymbolType SymbolType {
        get => _type;
        set {
            _type = value;
            Canvas.Invalidate();
        }
    }
    private char Glyph => (char)SymbolType;

    private int _scale = 1;
    public int SymbolScale {
        get => _scale;
        set
        {
            _scale = value;
            float size = value * BaseSize;
            Canvas.Width = size;
            Canvas.Height = size;
            Canvas.Invalidate();
        }
    }

    private ComponentOrientation _orientiation = ComponentOrientation.Right;
    public ComponentOrientation Orientation
    {
        get => _orientiation;
        set
        {
            _orientiation = value;
            Canvas.Invalidate();
        }
    }

    public CircuitSymbol()
    {
        InitializeComponent();
        Canvas.Draw += OnDraw;
        ActualThemeChanged += (_, __) => Canvas.Invalidate();
    }

    private void OnDraw(CanvasControl sender, CanvasDrawEventArgs args)
    {
        var ds = args.DrawingSession;

        if (Antialiasing == CanvasAntialiasing.Aliased)
        {
            ds.TextAntialiasing = CanvasTextAntialiasing.Aliased;
            ds.Antialiasing = CanvasAntialiasing.Aliased;
        }

        float size = SymbolScale * BaseSize;
        float halfSize = size / 2;

        var format = new CanvasTextFormat() {
            VerticalAlignment = CanvasVerticalAlignment.Bottom,
            FontFamily = Font,
            FontSize = size
        };

        var layout = new CanvasTextLayout(ds, Glyph.ToString(), format, size, size);

        float glyphWidth = (float)layout.DrawBounds.Width;
        float glyphLeft = halfSize - glyphWidth / 2;
        float glyphRight = glyphLeft + glyphWidth;

        var color = IsPreview ? PreviewColor : DefaultColor;

        ds.Transform = Matrix3x2.CreateRotation(MathF.PI * (float)Orientation / 2, new(halfSize));

        // Draws short sections of wire coming out either side of the circuit symbol
        ds.DrawLine(
            new Vector2(0, halfSize),
            new Vector2(glyphLeft, halfSize),
            color,
            SymbolScale
        );

        ds.DrawLine(
            new Vector2(glyphRight, halfSize),
            new Vector2(size, halfSize),
            color,
            SymbolScale
        );

        if (SymbolType == CircuitSymbolType.Ammeter || SymbolType == CircuitSymbolType.Ohmmeter || SymbolType == CircuitSymbolType.Voltmeter)
        {
            ds.Transform = Matrix3x2.Identity;
        }

        // Draws the main part of the circuit symbol as a custom font character
        ds.DrawTextLayout(layout, new Vector2(glyphLeft, 0), color);
    }
}
