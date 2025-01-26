using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Regular;

namespace MyHomePage.Layout;

public partial class NavMenu
{
    private Icon HomeIcon => new Size24.Home();
    private Icon CounterIcon => new Size24.ArrowCounterclockwise();
    private Icon WeatherIcon => new Size24.WeatherCloudy();
}