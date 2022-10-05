namespace Imobiliaria.Model;

public class Moradores
{
    public int id { get; set; }
    public int id_condominio { get; set; }
    public string? nome { get; set; }
    public string? email { get; set; }
    public string? cep { get; set; }
    
    /*
     *  EXEMPLO DE JSON
     *
        {
            "id_condominio": "",
            "nome": "",
            "email": "",
            "cep": "",
        }
     * 
     */
}