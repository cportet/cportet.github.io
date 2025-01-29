namespace MyHomePage.Code;

public static class DisplayHelper
{
    public static int AgeCyril => Maths.GetAge(new DateTime(1979, 11, 07));

    public static int AnciennetePro => Maths.GetAge(new DateTime(1998, 09, 01));

    public static int AncienneteDerniereExp => Maths.GetAge(new DateTime(2021, 05, 01));
}
