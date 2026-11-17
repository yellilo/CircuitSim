namespace CircuitSim;

/// <summary>
/// Numeric values represent character code points.
/// </summary>
public enum CircuitSymbolType
{
             Ammeter = 0xE000,
                Cell = 0xE001,
       FixedResistor = 0xE002,
                Fuse = 0xE003,
         FuseTripped = 0xE004,
                Lamp = 0xE005,
            Ohmmeter = 0xE006,
        SwitchClosed = 0xE007,
          SwitchOpen = 0xE008,
    VariableResistor = 0xE009,
       Potentiometer = 0xE00A,
           Voltmeter = 0xE00B
}
