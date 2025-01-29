using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Icons.Regular;

namespace MyHomePage.Code;

public static class References
{

    // See: https://www.fluentui-blazor.net/Icon#explorer
    public static class SmallIcons
    {
        public static Icon Lien => new Size16.Link();
    }

    public static class Icons
    {
        public static Icon Info => new Size24.Info();
        public static Icon Close => new Size24.ShareCloseTray();
        public static Icon Back => new Size24.ArrowCircleLeft();
        public static Icon Home => new Size24.Home();
        public static Icon CV => new Size24.Document();
        public static Icon Lien => new Size24.Link();
        public static Icon Famille => new Size24.PeopleCommunity();
        public static Icon Menu => new Size24.Navigation();
        public static Icon Theme => new Size24.DarkTheme();
        public static Icon Personnel => new Size24.InprivateAccount();
    }
}
