using Microsoft.AspNetCore.Components;

namespace MyHomePage.Services;

public class HistoryService(NavigationManager navigationManager)
{
    private readonly List<string> _history = [];

    public void Add(string url)
    {
        if (_history.Count == 0 || _history[^1] != url)
        {
            _history.Add(url);
        }
    }

    public bool CanGoBack()
    {
        return _history.Count > 1;
    }

    public void GoBack()
    {
        if (CanGoBack())
        {
            var url = _history[^2];
            _history.RemoveAt(_history.Count - 1);
            _history.RemoveAt(_history.Count - 1);

            navigationManager.NavigateTo(url);
        }
    }
}
