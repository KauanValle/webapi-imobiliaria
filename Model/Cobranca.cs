namespace Imobiliaria.Model;

public class Cobranca
{
    public int id { get; set; }
    public int id_morador { get; set; }
    public int id_condominio { get; set; }
    public string? tipo_pagamento { get; set; }
    public double valor_pagamento { get; set; }
    public string? vencimento { get; set; }
    public bool cobranca_paga { get; set; }

    /*
    {
        "id_morador": "",
        "id_condominio": "",
        "tipo_pagamento": "" {pix, boleto, credito, debito},
        "valor_pagamento": "",
        "vencimento": ""
    }
    */
}