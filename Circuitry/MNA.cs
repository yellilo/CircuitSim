using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CircuitSim.Circuitry;

/// <summary>
/// Allows Modified Nodal Analysis to be performed on a <see cref="CircuitBoard"/>, solving for voltages and currents.
/// </summary>
public class MNA
{
    public Dictionary<Junction, double> ResolvedVoltages = [];
    public Dictionary<Component, double> ResolvedCurrents = [];

    private readonly List<Junction> Nodes = [];
    private readonly List<Component> VoltageSources = [];
    private readonly List<Component> Resistors = [];

    private Junction Ground;

    private readonly Dictionary<CircuitElement, int> ElementIndices = [];
    private int MatrixSize => ElementIndices.Count;

    // Ax = z
    private Matrix<double> A;
    // n current sources, followed by m voltage sources
    private Vector<double> z;
    // n resolved voltages, followed by m resolved currents
    private Vector<double> x => A.Solve(z);

    /// <returns>
    /// The matrix index of the element or -1 if the element is not inside the matrix.
    /// </returns>
    private int GetElementIndex(CircuitElement element)
    {
        if (ElementIndices.TryGetValue(element, out int idx))
        {
            return idx;
        }

        return -1;
    }

    public void Solve(Junction[] junctions, Component[] components)
    {
        AssignElementIndices(junctions, components);
        BuildMatrices();

        ResolvedVoltages.Clear();
        ResolvedCurrents.Clear();

        // Ground node is treated as 0 volts
        ResolvedVoltages[Ground] = 0;

        // Extract calculated voltages from x matrix
        foreach (var junction in Nodes)
        {
            double voltage = x[GetElementIndex(junction)];
            ResolvedVoltages[junction] = voltage;
        }

        // Extract calculated currents from x matrix
        foreach (var voltageSource in VoltageSources)
        {
            double current = x[GetElementIndex(voltageSource)];
            ResolvedCurrents[voltageSource] = Math.Abs(current);
        }
    }

    public void AssignElementIndices(Junction[] junctions, Component[] components)
    {
        Nodes.Clear();
        VoltageSources.Clear();
        Resistors.Clear();
        ElementIndices.Clear();

        Ground = junctions[0];

        foreach (var junction in junctions.Skip(1))
        {
            Nodes.Add(junction);
            ElementIndices[junction] = ElementIndices.Count;
        }

        foreach (var component in components)
        {
            if (component.Resistance == 0)
            {
                VoltageSources.Add(component);
                ElementIndices[component] = ElementIndices.Count;
            }
            else
            {
                Resistors.Add(component);
            }
        }
    }

    /// <summary>
    /// Builds the A and z matrices.
    /// </summary>
    public void BuildMatrices()
    {
        A = Matrix<double>.Build.Dense(MatrixSize, MatrixSize);
        z = Vector<double>.Build.Dense(MatrixSize);

        foreach (var cell in VoltageSources)
        {
            StampVoltageSource(cell);
        }

        foreach (var resistor in Resistors)
        {
            StampResistor(resistor);
        }
    }

    private void SetSymmetrical(int a, int b, double value)
    {
        A[a, b] = value;
        A[b, a] = value;
    }

    private void StampResistor(Component resistor)
    {
        double conductance = 1 / resistor.Resistance;

        // Matrix row/column index associated with each terminal node
        // Will be -1 if the node is ground (so not in the matrix)
        int positiveIdx = GetElementIndex(resistor.PositiveTerminal);
        int negativeIdx = GetElementIndex(resistor.NegativeTerminal);

        // Add this resistor's conductance to the sum for each terminal node
        // Skip the ground node since ground is not included in the matrix
        if (positiveIdx != -1) A[positiveIdx, positiveIdx] += conductance;
        if (negativeIdx != -1) A[negativeIdx, negativeIdx] += conductance;
       
        // Checks that the resistor isn't connected to ground in either direction
        // This means it is between two nodes in the matrix
        if (positiveIdx != -1 && negativeIdx != -1)
        {
            SetSymmetrical(positiveIdx, negativeIdx, -conductance);
        }
    }

    private void StampVoltageSource(Component voltageSource)
    {
        // Matrix row/column index associated with each terminal node
        // Will be -1 if the node is ground (so not in the matrix)
        int positiveIdx = GetElementIndex(voltageSource.PositiveTerminal);
        int negativeIdx = GetElementIndex(voltageSource.NegativeTerminal);

        // Matrix row/column index associated with the voltage source
        int sourceIdx = GetElementIndex(voltageSource);

        // Skip the ground node since ground is not included in the matrix
        if (positiveIdx != -1) SetSymmetrical(sourceIdx, positiveIdx, 1);
        if (negativeIdx != -1) SetSymmetrical(sourceIdx, negativeIdx, -1);

        // Add the voltage supplied by the cell to the z matrix
        if (voltageSource is Cell cell)
        {
            z[sourceIdx] = cell.EMF;
        }
    }
}