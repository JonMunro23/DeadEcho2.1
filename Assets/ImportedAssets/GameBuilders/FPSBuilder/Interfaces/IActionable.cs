//=========== Copyright (c) GameBuilders, All rights reserved. ================//

namespace GameBuilders.FPSBuilder.Interfaces
{
    public interface IActionable
    {
        bool RequiresAnimation
        {
            get;
        }

        int Cost
        { 
            get; 
        }

        bool IsOpen 
        { 
            get; 
        }

        void Interact();

        string Message();
    }
}
