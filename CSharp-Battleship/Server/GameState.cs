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
        Player1Turn,
        Player2Turn,
        Ended
    }
}
