namespace MyHomePage.Code;

public sealed record CultureItem(string Code, string Name) : ISelectItem
{
    public string SelectKey => Code;
    public string SelectText => Name;
};
