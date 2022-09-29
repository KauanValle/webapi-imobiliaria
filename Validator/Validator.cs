namespace Imobiliaria.Validator;

public class Validator
{
    public static bool isEmpty(string data)
    {
        if (data == null || data == "")
        {
            return true;
        }

        return false;
    }
}