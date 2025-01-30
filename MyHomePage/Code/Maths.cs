namespace MyHomePage.Code;
public static class Maths
{
    public static int GetAge(DateTime refDate)
    {
        DateTime now = DateTime.Today;
        int age = now.Year - refDate.Year;

        if (now < refDate.AddYears(age))
            age--;

        return age;
    }
}
