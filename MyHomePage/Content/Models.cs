namespace MyHomePage.Content;

// Shapes of the JSON files stored under wwwroot/content.

public sealed record SiteData
{
    public required string Name { get; init; }
    public required string GivenName { get; init; }
    public required string FamilyName { get; init; }
    public required string Initials { get; init; }
    public required string BaseUrl { get; init; }
    public required string Email { get; init; }
    public string? Phone { get; init; }
    public required string LinkedIn { get; init; }
    public required string SourceCode { get; init; }
    public required string CvPdf { get; init; }
    public required string TwitterHandle { get; init; }
    public required Place Location { get; init; }
    public IReadOnlyList<NavItem> Nav { get; init => field = value ?? []; } = [];
    public IReadOnlyList<SocialLink> Socials { get; init => field = value ?? []; } = [];

    /// <summary>Canonical URLs of the public profiles, for search engines (schema.org "sameAs").</summary>
    public IReadOnlyList<string> Profiles { get; init => field = value ?? []; } = [];
}

public sealed record Place(LocalizedText Region, string Country);

public sealed record Organization(string Name, string? Url = null);

public sealed record NavItem(string Path, LocalizedText Label);

public sealed record SocialLink(string Name, string Url);

public sealed record LinkItem(LocalizedText Label, string Url);

public sealed record PageMeta
{
    public required LocalizedText Title { get; init; }
    public required LocalizedText Description { get; init; }
    public LocalizedText? Eyebrow { get; init; }
    public LocalizedText? Heading { get; init; }
    public LocalizedText? Lead { get; init; }
}

public sealed record Profile
{
    public required DateOnly BirthDate { get; init; }
    public required DateOnly CareerStart { get; init; }
    /// <summary>Since when I have been leading development teams.</summary>
    public required DateOnly ManagementStart { get; init; }

    public required LocalizedText Role { get; init; }
    public required LocalizedText Eyebrow { get; init; }
    public required LocalizedText Headline { get; init; }
    public required LocalizedText Intro { get; init; }
    public required ImageRef Portrait { get; init; }
    public IReadOnlyList<Stat> Stats { get; init => field = value ?? []; } = [];
}

public sealed record ImageRef(string Url, LocalizedText Alt, LocalizedText? Caption = null);

public sealed record Stat(LocalizedText Value, LocalizedText Label);

public sealed record Experience
{
    public required string Id { get; init; }
    public required string Company { get; init; }
    public string? Url { get; init; }
    public required LocalizedText Role { get; init; }
    public required LocalizedText Location { get; init; }
    public required DateOnly Start { get; init; }

    /// <summary>Last month of the position (inclusive). Null for the current position.</summary>
    public DateOnly? End { get; init; }

    public required LocalizedText Summary { get; init; }
    public IReadOnlyList<LocalizedText> Details { get; init => field = value ?? []; } = [];
    public IReadOnlyList<LocalizedText> Tags { get; init => field = value ?? []; } = [];

    public bool IsCurrent => End is null;
}

public sealed record ResumeData
{
    public IReadOnlyList<ResumeFact> Facts { get; init => field = value ?? []; } = [];
    public IReadOnlyList<SkillGroup> Skills { get; init => field = value ?? []; } = [];
    public IReadOnlyList<Training> Trainings { get; init => field = value ?? []; } = [];
    public LocalizedText? TrainingsNote { get; init; }
    public IReadOnlyList<Education> Education { get; init => field = value ?? []; } = [];
    public required OtherExperiences Others { get; init; }
    public required LocalizedText Interests { get; init; }
}

public sealed record ResumeFact(LocalizedText Label, LocalizedText Value, bool Highlight = false);

public sealed record SkillGroup(LocalizedText Label, IReadOnlyList<LocalizedText> Items);

public sealed record Training(int Year, LocalizedText Label);

public sealed record Education(LocalizedText Title, int Year, LocalizedText Text, Organization? School = null);

public sealed record OtherExperiences(LocalizedText Period, LocalizedText Intro, IReadOnlyList<OtherExperience> Items);

public sealed record OtherExperience(string Company, string? Url, LocalizedText Location, LocalizedText Role);

public sealed record PersoData
{
    public required ImageRef Photo { get; init; }
    public IReadOnlyList<FamilySite> Family { get; init => field = value ?? []; } = [];
    public IReadOnlyList<FeaturedSite> Featured { get; init => field = value ?? []; } = [];
}

public sealed record FamilySite(string Name, string Url);

public sealed record FeaturedSite(LocalizedText Kind, string Title, LocalizedText Summary, string? Url, ImageRef? Image = null);

public sealed record ProjectsData
{
    public IReadOnlyList<AppItem> Apps { get; init => field = value ?? []; } = [];
    public IReadOnlyList<CardItem> Cards { get; init => field = value ?? []; } = [];
    public required PrivacySummary Privacy { get; init; }
}

public sealed record AppItem
{
    public required string Name { get; init; }
    public LocalizedText? Status { get; init; }
    public IReadOnlyList<LocalizedText> Description { get; init => field = value ?? []; } = [];
    public IReadOnlyList<string> Tags { get; init => field = value ?? []; } = [];
    public string? StoreUrl { get; init; }
}

public sealed record CardItem(LocalizedText Title, LocalizedText Text, LinkItem Link);

public sealed record PrivacySummary(DateOnly Updated, IReadOnlyList<LocalizedText> Points);

public sealed record AboutData
{
    public IReadOnlyList<AboutSection> Sections { get; init => field = value ?? []; } = [];
}

public sealed record AboutSection
{
    public required LocalizedText Title { get; init; }
    public LocalizedText? Text { get; init; }
    public IReadOnlyList<AboutItem> Items { get; init => field = value ?? []; } = [];
    public LinkItem? Link { get; init; }
}

public sealed record AboutItem(LocalizedText Text, LocalizedText? Label = null, string? Url = null);
