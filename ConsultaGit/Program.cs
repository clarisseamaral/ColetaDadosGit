using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsultaGit
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                GerarRelatorio();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static async void GerarRelatorio()
        {
            List<String> usuarios = new List<String>()
            {
                "bpasero",
                "joaomoreno",
                "jrieken",
                "isidorn",
                "alexandrudima",
                "mjbvz",
                "sandy081",
                "aeschli",
                "Tyriar",
                "roblourens",
                "weinand",
                "chrmarti",
                "rebornix",
                "ramya-rao-a",
                "dbaeumer",
            };

            foreach (var usuario in usuarios)
            {
                var lista = new List<Resultado>();

                for (int i = 1; i < 35; i++)
                {
                    var urls = await API<CommitUsuario>.Consulta("search/commits?q=repo:Microsoft/vscode+author:" + usuario + "&page=" + i);

                    Console.WriteLine("\t" + i + " " + usuario);

                    if (urls.Items != null && urls.Items.Count > 0)
                    {
                        foreach (var item in urls.Items)
                        {
                            //int aguardar = 0;
                            //do
                            //{
                                var requestUri = "" + item.Url.Replace("https://api.github.com/", "");
                                var detalhes = await API<CommitDetalhes>.Consulta(requestUri);

                                if (detalhes.Commit != null)
                                {
                                    lista.Add(ObterResultado(detalhes, usuario, i));
                                    //aguardar = 0;
                                }
                                //else
                                //{
                                //    Console.Write("Falha request\t");
                                //
                                //    if (aguardar == 0)
                                //        PreencherCsv(lista, usuario);
                                //
                                //    await Task.Delay(240000);
                                //    Console.WriteLine("\tTask finalizada");
                                //    aguardar = 1;
                                //}

                            //} while (aguardar == 1);

                        }
                    }
                    else
                        break;

                }

                PreencherCsv(lista, usuario);
            }

            Console.WriteLine("Concluido.");
        }

        private static Resultado ObterResultado(CommitDetalhes detalhes, string usuario, int pagina)
        {
            var resultado = new Resultado();

            Console.WriteLine(detalhes.Identificador);

            resultado.Identificador = detalhes.Identificador;

            resultado.DataCommit = detalhes.Commit.Commiter.Data;

            resultado.Usuario = usuario;

            resultado.Pagina = pagina;

            var arquivos = detalhes.Files.Where(o => !(o.ArquivoModificado.StartsWith("extensions/")|| o.ArquivoModificado.EndsWith(".md"))).ToList();

            foreach (var arquivo in arquivos)
            {

                if (arquivo.ArquivoModificado.Contains("test/"))
                {
                    resultado.QtdAdicoesTeste += arquivo.QtdAdicoes;
                    resultado.QtdExclusoesTeste += arquivo.QtdExclusoes;
                    resultado.QtdMudancasTeste += arquivo.QtdMudancas;
                }
                else
                {
                    resultado.QtdAdicoesCod += arquivo.QtdAdicoes;
                    resultado.QtdExclusoesCod += arquivo.QtdExclusoes;
                    resultado.QtdMudancasCod += arquivo.QtdMudancas;
                }
            }

            return resultado;
        }


        private static void PreencherCsv(List<Resultado> resultados, string usuario)
        {
            if (resultados.Count > 0)
            {
                List<string> linhas = new List<string>() { "Usuario,Identificador,DataCommit,QtdAdicoesCod,QtdExclusoesCod,QtdMudancasCod,QtdAdicoesTeste,QtdExclusoesTeste,QtdMudancasTeste,Pag" };
                foreach (var resultado in resultados)
                {
                    linhas.Add(ObterLinhas(resultado));
                }
                var name = string.Format("Relatorio_{0}", usuario);
                System.IO.File.WriteAllLines($"{name}.csv", linhas.ToArray(), Encoding.UTF8);
                Console.WriteLine("Relatorio gerado!");
            }
        }


        private static string ObterLinhas(Resultado resultado)
        {
            List<string> listaResultados = new List<string>();
            listaResultados.Add(resultado.Usuario);
            listaResultados.Add(resultado.Identificador);
            listaResultados.Add(resultado.DataCommit.ToString("dd/MM/yyyy"));
            listaResultados.Add(resultado.QtdAdicoesCod.ToString());
            listaResultados.Add(resultado.QtdExclusoesCod.ToString());
            listaResultados.Add(resultado.QtdMudancasCod.ToString());
            listaResultados.Add(resultado.QtdAdicoesTeste.ToString());
            listaResultados.Add(resultado.QtdExclusoesTeste.ToString());
            listaResultados.Add(resultado.QtdMudancasTeste.ToString());
            listaResultados.Add(resultado.Pagina.ToString());
            return string.Join(",", listaResultados);
        }
    }
}
