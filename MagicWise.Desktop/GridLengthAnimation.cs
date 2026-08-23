using System.Windows;
using System.Windows.Media.Animation;

namespace MagicWise.Desktop;

/// <summary>
/// Animates a GridLength value (e.g. a ColumnDefinition Width).
/// </summary>
public class GridLengthAnimation : AnimationTimeline
{
    public GridLength From
    {
        get { return (GridLength)GetValue(FromProperty); }
        set { SetValue(FromProperty, value); }
    }

    public static readonly DependencyProperty FromProperty =
        DependencyProperty.Register(nameof(From), typeof(GridLength), typeof(GridLengthAnimation));

    public GridLength To
    {
        get { return (GridLength)GetValue(ToProperty); }
        set { SetValue(ToProperty, value); }
    }

    public static readonly DependencyProperty ToProperty =
        DependencyProperty.Register(nameof(To), typeof(GridLength), typeof(GridLengthAnimation));

    public IEasingFunction? EasingFunction
    {
        get { return (IEasingFunction?)GetValue(EasingFunctionProperty); }
        set { SetValue(EasingFunctionProperty, value); }
    }

    public static readonly DependencyProperty EasingFunctionProperty =
        DependencyProperty.Register(nameof(EasingFunction), typeof(IEasingFunction), typeof(GridLengthAnimation));

    public override Type TargetPropertyType { get { return typeof(GridLength); } }

    public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
    {
        double from = From.Value;
        double to = To.Value;
        double progress = animationClock.CurrentProgress ?? 0;

        if (EasingFunction != null)
        {
            progress = EasingFunction.Ease(progress);
        }

        return new GridLength(from + (to - from) * progress);
    }

    protected override Freezable CreateInstanceCore()
    {
        return new GridLengthAnimation();
    }
}
