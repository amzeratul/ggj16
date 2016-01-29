using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class DanceStepPair {
    public DanceStep p0;
    public DanceStep p1;

    public DanceStepPair Flip() {
        return new DanceStepPair {
            p0 = this.p1,
            p1 = this.p0
        };
    }
}
