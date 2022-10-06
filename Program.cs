//criar projeto:
//	dotnet new webabi -minimal -o NomeDoProjeto
//entrar na pasta:
//	cd NomeDoProjeto
//adicionar entity framework no console:
//	dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 6.0
//	dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 6.0
//	dotnet add package Microsoft.EntityFrameworkCore.Design --version 6.0
//incluir namespace do entity framework:
//	using Microsoft.EntityFrameworkCore;
//antes de rodar o dotnet run pela primeira vez, rodar os seguintes comandos para iniciar a base de dados:
//	dotnet ef migrations add InitialCreate
//	dotnet ef database update

using Imobiliaria.Model;
using Imobiliaria.Model.DB;
using Imobiliaria.Responses;
using Imobiliaria.Validator;

namespace Imobiliaria
{
    class Program
    {
        public static HttpContext query;
        static void Main(string[] args)
        {
            var app = loadDataBase(args);
            
            moradoresRoutes(app);
            condominioRoutes(app);
            cobrancaRoutes(app);

            app.Run();
        }

        static WebApplication loadDataBase(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString =
                builder.Configuration.GetConnectionString("Imobiliaria") ?? "Data Source=Imobiliaria.db";
            builder.Services.AddSqlite<DbImobiliaria>(connectionString);

            return builder.Build();
        }

        static void moradoresRoutes(WebApplication app)
        {
            var prefix = "/moradores";
            
            // Pegar todos os moradores /moradores/getAll
            app.MapGet(prefix + "/getAll",
                (DbImobiliaria baseDadosMoradores) =>
                {
                    return baseDadosMoradores.Moradores.ToList();
                });

            app.MapGet(prefix + "/get/{id}",
                (DbImobiliaria baseDadosMoradores, int id) =>
                {
                    return baseDadosMoradores.Moradores.Find(id);
                });

            app.MapPost(prefix + "/cadastrar", (DbImobiliaria baseDadosMoradores, Moradores morador) =>
            {
                var dadosValidados = MoradorValidator.validator(morador);
                var getCondominioId = baseDadosMoradores.Condominios.Find(morador.id_condominio);
                var moradorExistente = verifyId(baseDadosMoradores, morador.id);

                if (moradorExistente != "")
                {
                    return moradorExistente;
                }

                if (getCondominioId == null)
                {
                    return "Condominio do morador não existe!";
                }

                if (dadosValidados == MoradorResponse.MoradorSalvoSucesso)
                {
                    baseDadosMoradores.Moradores.Add(morador);
                    baseDadosMoradores.SaveChanges();
                }
                
                return dadosValidados;
            });

            app.MapPut(prefix + "/atualizar/{id}",
                (DbImobiliaria baseDadosMoradores, Moradores moradorAtualizado, int id) =>
                {
                    var getCondominioId = baseDadosMoradores.Condominios.Find(moradorAtualizado.id_condominio);
                    var moradorAntigo = baseDadosMoradores.Moradores.Find(id);

                    if (moradorAntigo == null)
                    {
                        return "Morador não existe!";
                    }

                    if (moradorAtualizado.cep != null || moradorAtualizado.cep != "")
                    {
                        moradorAntigo.cep = moradorAtualizado.cep;
                    }
                    
                    if (moradorAtualizado.email != null || moradorAtualizado.email != "")
                    {
                        moradorAntigo.email = moradorAtualizado.email;
                    }
                    
                    if (moradorAtualizado.nome != null || moradorAtualizado.nome != "")
                    {
                        moradorAntigo.nome = moradorAtualizado.nome;
                    }

                    if (getCondominioId == null)
                    {
                        return "Condominio do morador não existe!";
                    }
                    
                    moradorAntigo.id_condominio = moradorAtualizado.id_condominio;
                    baseDadosMoradores.SaveChanges();
                    return "Morador Atualizado";
                });

            app.MapDelete(prefix + "/deletar/{id}", (DbImobiliaria baseDadosMoradores, int id) =>
            {
                var morador = baseDadosMoradores.Moradores.Find(id);
                if (morador == null)
                {
                    return "Morador não existe!";
                }
                baseDadosMoradores.Remove(morador);
                baseDadosMoradores.SaveChanges();
                return "Morador Deletado";
            });
        }

        static void condominioRoutes(WebApplication app)
        {
            var prefix = "/condominio";
            
            app.MapGet(prefix + "/getAll", (DbImobiliaria baseDadosCondominio) =>
            {
                return baseDadosCondominio.Condominios.ToList();
            });

            app.MapGet(prefix + "/get/{id}", (DbImobiliaria baseDadosCondominio, int id) =>
            {
                return baseDadosCondominio.Condominios.Find(id);
            });

            app.MapPost(prefix + "/cadastrar", (DbImobiliaria baseDadosCondominio, Condominio condominio) =>
            {
                var dadosValidados = CondominioValidator.validator(condominio);
                var condominioExist = verifyId(baseDadosCondominio, condominio.id);

                if (condominioExist != "")
                {
                    return condominioExist;
                }
                
                if (dadosValidados == CondominioResponse.CondominioSalvoSucesso)
                {
                    baseDadosCondominio.Condominios.Add(condominio);
                    baseDadosCondominio.SaveChanges();
                }
                return dadosValidados;
            });

            app.MapPut(prefix + "/atualizar/{id}", (DbImobiliaria baseDadosCondominio, int id, Condominio condominioAtualizado) =>
            {
                var condominioAntigo = baseDadosCondominio.Condominios.Find(id);
                if (condominioAntigo == null)
                {
                    return "Condominio não existe!";
                }
                
                if (condominioAtualizado.cidade_condominio != null || condominioAtualizado.cidade_condominio != "")
                {
                    condominioAntigo.cidade_condominio = condominioAtualizado.cidade_condominio;
                }
                
                if (condominioAtualizado.nome_condominio != null || condominioAtualizado.nome_condominio != "")
                {
                    condominioAntigo.nome_condominio = condominioAtualizado.nome_condominio;
                }

                baseDadosCondominio.SaveChanges();
                return "Condominio atualizado!";
            });

            app.MapDelete(prefix + "/deletar/{id}", (DbImobiliaria baseDadosCondominio, int id) =>
            {
                var condominio = baseDadosCondominio.Condominios.Find(id);
                if (condominio == null)
                {
                    return "Condominio não existe!";
                }

                baseDadosCondominio.Remove(condominio);
                baseDadosCondominio.SaveChanges();
                return "Morador Deletado";
            });
        }

        static void cobrancaRoutes(WebApplication app)
        {

            var prefix = "/cobranca";

            app.MapPost(prefix + "/adicionar", (DbImobiliaria baseDadosCobranca, Cobranca cobranca) =>
            {
                var cobrancaExist = verifyId(baseDadosCobranca, cobranca.id);

                if (cobrancaExist != "")
                {
                    return cobrancaExist;
                }
                
                if (baseDadosCobranca.Moradores.Find(cobranca.id_morador) == null)
                {
                    return "Morador não existe para cadastrar cobrança!";
                }

                if (baseDadosCobranca.Condominios.Find(cobranca.id_condominio) == null)
                {
                    return "Condominio não existe para cadastrar cobrança!";
                }

                Random random = new Random();
                cobranca.valor_pagamento = random.NextInt64(0, 1500);
                cobranca.cobranca_paga = false;
                baseDadosCobranca.Cobranca.Add(cobranca);
                baseDadosCobranca.SaveChanges();

                return "Cobrança salva com sucesso!";
            });

            app.MapGet(prefix + "/getAll",
                (DbImobiliaria baseDadosCobranca) => { return baseDadosCobranca.Cobranca.ToList(); });

            app.MapGet(prefix + "/get/{id}", (DbImobiliaria baseDadosCobranca, int id) =>
            {
                var cobranca = baseDadosCobranca.Cobranca.Find(id);

                return cobranca;
            });

            app.MapGet(prefix + "/gerar-cobranca/{id}", (DbImobiliaria baseDadosCobranca, int id) =>
            {
                var cobranca = baseDadosCobranca.Cobranca.Find(id);
                if (cobranca == null)
                {
                    return "Cobranca não existe!";
                }

                return "https://actana.com.br/img/editor/gerar-boleto-com-codigo-de-barras.png";
            });

            app.MapGet(prefix + "/pagar-cobranca/{idCobranca}", (DbImobiliaria baseDadosCobranca, int idCobranca) =>
            {
                var cobranca = baseDadosCobranca.Cobranca.Find(idCobranca);
                if (cobranca == null)
                {
                    return "Cobrança não existe!";
                }

                cobranca.cobranca_paga = true;
                baseDadosCobranca.SaveChanges();
                return "Cobraça paga!";
            });

            app.MapGet(prefix + "/get/params", (DbImobiliaria baseDadosCobranca, string column, string value) =>
            {
                var colunas = column.Split(",");
                var valores = value.Split(",");
                List<Cobranca> listaCobrancaRetorno = new List<Cobranca>();
                if (colunas.Length > 1)
                {
                    try
                    {
                        if (colunas[0] == "id_morador")
                        {
                            listaCobrancaRetorno = findMoradorCobranca(baseDadosCobranca, valores, true);
                        }
                        else
                        {
                            listaCobrancaRetorno = findCondominioCobranca(baseDadosCobranca, valores, true);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Informe 2 valores!");
                    }
                }
                else
                {
                    if (colunas[0] == "id_morador")
                    {
                        listaCobrancaRetorno = findMoradorCobranca(baseDadosCobranca, valores, false);
                    }
                    else
                    {
                        listaCobrancaRetorno = findCondominioCobranca(baseDadosCobranca, valores, false);
                    }
                }
                
                return listaCobrancaRetorno;
            });
        }

        static List<Cobranca> findMoradorCobranca(DbImobiliaria baseDados, string[] valores, bool isMultipleColumn)
        {
            List<Cobranca> listaCobrancaRetorno = new List<Cobranca>();
            var listaCobranca = baseDados.Cobranca.Where(cobranca => cobranca.id_morador == Convert.ToInt32(valores[0])).ToList();
            if (isMultipleColumn)
            {
                foreach (var cob in listaCobranca)
                {
                    if (cob.id_condominio == Convert.ToInt32(valores[1]))
                    {
                        listaCobrancaRetorno.Add(cob);
                    }
                }
            }
            else
            {
                listaCobrancaRetorno = listaCobranca;
            }
            return listaCobrancaRetorno;
        }

        static List<Cobranca> findCondominioCobranca(DbImobiliaria baseDados, string[] valores, bool isMultipleColumn)
        {
            List<Cobranca> listaCobrancaRetorno = new List<Cobranca>();
            
            var listaCobranca = baseDados.Cobranca.Where(cobranca => cobranca.id_condominio == Convert.ToInt32(valores[0])).ToList();
            if (isMultipleColumn)
            {
                foreach (var cob in listaCobranca)
                {
                    if (cob.id_morador == Convert.ToInt32(valores[1]))
                    {
                        listaCobrancaRetorno.Add(cob);
                    }
                }
            }
            else
            {
                listaCobrancaRetorno = listaCobranca;
            }

            return listaCobrancaRetorno;

        }

        static string verifyId(DbImobiliaria baseDadosMoradores, int id)
        {
            if (id != null)
            {
                if (baseDadosMoradores.Moradores.Find(id) != null)
                {
                    return "Morador com ID informado já existe!";
                }
            }

            return "";
        }
    }
}