using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Regular;

namespace MyHomePage.Pages;

[Route("/")]
public partial class Home
{
    private string PersonImage => 
        new Size48.Person().ToDataUri(size: "25px", color: "white");
}