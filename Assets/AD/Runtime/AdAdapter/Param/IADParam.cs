using System;
using AD.Decorator;
using AD.Tools;
public interface IADParam : IADInit
{
    int GameOrientation { get; set; }
    ADParamDecoratorManager Decorator { get; set; }
}

public abstract class BaseADParam : IADParam
{
    public abstract int GameOrientation { get; set; }
    
    public virtual ADParamDecoratorManager Decorator { get; set; }

    public virtual void Init()
    {
        if (Decorator != null) Decorator.Init(this);
    }
}