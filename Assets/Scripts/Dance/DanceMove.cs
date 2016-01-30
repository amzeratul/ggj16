using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class DanceMove {
    public DanceStepPair[] Steps;
    public string Description;
    public EffectType Effect;
    public IsAvailableType IsAvailable;

    public delegate void EffectType(World world);
    public delegate bool IsAvailableType(World world);
}
