﻿namespace OleksiiStepanov.Game
{
    #region AppLoader

    public enum LoadingStep
    {
        None,
        AppInit,
        UIInit,
        Complete
    }

    #endregion
    
    #region AppLoader
    
    public enum RoadType
    {
        Left,
        Right,
        CrossX,
        CrossTTop,
        CrossTRight,
        CrossTBottom,
        CrossTLeft,
        TurnTop,
        TurnBottom,
        TurnLeft,
        TurnRight,
    }
    
    #endregion
    
    #region AppLoader
    
    public enum CreationMode
    {
        None,
        Road,
        Building2x2,
        Building2x3,
    }
    
    #endregion

}