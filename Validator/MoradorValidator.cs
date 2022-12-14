using Imobiliaria.Model;
using Imobiliaria.Responses;

namespace Imobiliaria.Validator;

public class MoradorValidator
{
    public static string validator(Moradores data)
    {
        if (Validator.isEmpty(data.cep))
        {
            return MoradorResponse.MoradorCepObrigat√≥rio;
        }

        if (Validator.isEmpty(data.email))
        {
            return MoradorResponse.MoradorEmailObrigat√≥rio;
        }

        if (Validator.isEmpty(data.nome))
        {
            return MoradorResponse.MoradorNomeObrigat√≥rio;
        }

        return MoradorResponse.MoradorSalvoSucesso;
    }
}