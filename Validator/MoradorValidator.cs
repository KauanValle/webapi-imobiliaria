using Imobiliaria.Model;
using Imobiliaria.Responses;

namespace Imobiliaria.Validator;

public class MoradorValidator
{
    public static string validator(Moradores data)
    {
        if (Validator.isEmpty(data.cep))
        {
            return MoradorResponse.MoradorCepObrigatório;
        }

        if (Validator.isEmpty(data.email))
        {
            return MoradorResponse.MoradorEmailObrigatório;
        }

        if (Validator.isEmpty(data.nome))
        {
            return MoradorResponse.MoradorNomeObrigatório;
        }

        return MoradorResponse.MoradorSalvoSucesso;
    }
}