namespace Imobiliaria.Model;

public class Condominio
{
    public int id { get; set; }
    public string? nome_condominio { get; set; }
    public string? cidade_condominio { get; set; }
    
    /*
     *
     *  EXEMPLO DE JSON
        {
            "nome_condominio": "",
            "cidade_condominio": ""
        }
     */
}