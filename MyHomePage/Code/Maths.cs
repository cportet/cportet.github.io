namespace MyHomePage.Code;
public static class Maths
{
    public static int GetAge(DateTime birthDate)
    {
        DateTime now = DateTime.Today;
        int age = now.Year - birthDate.Year;
        if (now < birthDate.AddYears(age)) age--;
        return age;
    }
}
