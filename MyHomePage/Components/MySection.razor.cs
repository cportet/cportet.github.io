﻿using Microsoft.AspNetCore.Components;

namespace MyHomePage.Components;

public partial class MySection
{
    [Parameter]
    public string? Id { get; set; }

    [Parameter]
    public string? Title { get; set; }

    [Parameter]
    public string? SubTitle { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public SectionKind Type { get; set; } = SectionKind.Primary;
}

public enum SectionKind
{
    Primary,
    Secondary,
}
