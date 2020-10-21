using System;
using System.Collections.Generic;
using System.Text;

namespace ServerApplication
{
    public enum GameState
    {
        CantStart,
        Waiting,
        ChooseCells,
        Playing,
        Ended
    }
}
