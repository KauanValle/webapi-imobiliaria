using Imobiliaria.Model;
using Imobiliaria.Responses;
using Imobiliaria.Validator;

namespace Imobiliaria.Validator;

public class CondominioValidator 
{
    public static string validator(Condominio data)
    {
        if (Validator.isEmpty(data.nome_condominio))
        {
            return CondominioResponse.CondominioNomeCondominioVazio;
        }

        if (Validator.isEmpty(data.cidade_condominio))
        {
            return CondominioResponse.CondominioNomeCondominioVazio;
        }

        return CondominioResponse.CondominioSalvoSucesso;
    }
}