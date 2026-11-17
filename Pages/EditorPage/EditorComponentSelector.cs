using CircuitSim.Circuitry;
using CommunityToolkit.WinUI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Diagnostics;

namespace CircuitSim;

public sealed partial class EditorComponentSelector : UserControl
{
    private CircuitBoard Board => this.FindAscendant<EditorPage>()!.CircuitBoard;

    public EditorComponentSelector()
    {
        InitializeComponent();

        List<EditorComponentSelectorItem> items = [];

        foreach (var record in ComponentTypeRegistry.SelectorCircuitSymbolTypes)
        {
            var typeId = record.Key;
            var name = ComponentTypeRegistry.GetName(typeId);
            var symbolType = record.Value;

            var item = new EditorComponentSelectorItem(typeId, name, symbolType);
            items.Add(item);
        }

        ItemsView.ItemsSource = items.ToArray();

        Loaded += OnLoaded;
        ItemsView.SelectionChanged += OnSelectionChanged;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        ItemsView.Select(0);
    }

    private void OnSelectionChanged(ItemsView sender, ItemsViewSelectionChangedEventArgs args)
    {
        if (ItemsView.SelectedItem is not EditorComponentSelectorItem selectedItem) return;
        
        Debug.WriteLine($"Selected component type {selectedItem.TypeId}");
        Board.SelectedComponentTypeId = selectedItem.TypeId;
    }
}

public class EditorComponentSelectorItem(string typeId, string name, CircuitSymbolType symbolType)
{
    public string TypeId = typeId;
    public string Name = name;
    public CircuitSymbolType SymbolType = symbolType;
}