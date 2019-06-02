using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultaGit
{
    public class Program
    {
        private static List<String> usuarios = new List<String>()
        {
                //"bpasero",
                //"joaomoreno",
                //"jrieken",
                //"isidorn",
                //"alexandrudima",
                //"mjbvz",
                //"sandy081",
                //"aeschli",
                //"Tyriar",
                "roblourens",
                "weinand",
                "chrmarti",
                "rebornix",
                "ramya-rao-a",
                "dbaeumer",
        };

        public static void Main(string[] args)
        {
            try
            {
                foreach (var usuario in usuarios)
                {
                    //Buscar repositórios que o programador contribuiu
                    var lRepositorios = BuscarRepositoriosProgramador(usuario).ConfigureAwait(false).GetAwaiter().GetResult();


                    //Busca a quantidade de estrelas e forks do repositório original
                    foreach (var item in lRepositorios.Where(o=>o.Fork == true))
                    {
                        var detalhe = BuscarDetalhesRepositorioOriginal(item.NomeCompleto).ConfigureAwait(false).GetAwaiter().GetResult();

                        if (detalhe.Source != null)
                        {
                            item.QtdEstrelas = detalhe.Source.QtdEstrelas;
                            item.QtdForks = detalhe.Source.QtdForks;
                            item.NomeCompleto = detalhe.Source.NomeCompleto;
                        }
                    }

                    PreencherCsvModeloRepositorio(lRepositorios, usuario);

                    foreach (var item in lRepositorios)
                    {
                        var lresultado = BuscarCommitsRepositorioProgramador(item.NomeCompleto, usuario).ConfigureAwait(false).GetAwaiter().GetResult();

                        PreencherCsvModeloCommit(lresultado, usuario);
                    }
                }

                //GerarRelatorio();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();

            }
        }

        #region 

        /*private static async void GerarRelatorio()
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
                    {
                        break;
                    }
                }

                PreencherCsv(lista, usuario);
            }

            Console.WriteLine("Concluido.");
        }*/
        #endregion

        private static Resultado ObterResultado(CommitDetalhes detalhes, string usuario, string nomeRepositorio, int pagina)
        {
            var resultado = new Resultado();

            Console.WriteLine(detalhes.Identificador);

            resultado.Identificador = detalhes.Identificador;

            resultado.DataCommit = detalhes.Commit.Commiter.Data;

            resultado.Usuario = usuario;

            resultado.Pagina = pagina;

            resultado.NomeRepositorio = nomeRepositorio;

            var arquivos = detalhes.Files.Where(o => !(o.ArquivoModificado.StartsWith("extensions/") || o.ArquivoModificado.EndsWith(".md"))).ToList();

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


        private static void PreencherCsvModeloCommit(List<Resultado> resultados, string usuario)
        {
            if (resultados.Count > 0)
            {
                List<string> linhas = new List<string>(); // { "NomeRepositorio, Usuario,Identificador,DataCommit,QtdAdicoesCod,QtdExclusoesCod,QtdMudancasCod,QtdAdicoesTeste,QtdExclusoesTeste,QtdMudancasTeste,Pag" };
                foreach (var resultado in resultados)
                {
                    linhas.Add(ObterLinhas(resultado));
                }
               // var name = string.Format("Relatorio_{0}", usuario);
                var name = "RelatorioComCommit";
                System.IO.File.AppendAllLines($"{name}.csv", linhas.ToArray(), Encoding.UTF8);
                Console.WriteLine("Relatorio com os commits atualizado!");
            }
        }

        private static void PreencherCsvModeloRepositorio(List<RepositorioDetalhe> repositorios, string usuario)
        {
            if (repositorios.Count > 0)
            {
                List<string> linhas = new List<string>(); // { "RepositorioCompleto, Repositorio, Fork, QtdForks,QtdEstrelas, usuario" };
                foreach (var resultado in repositorios)
                {
                    var linha = new List<string>();
                    linha.Add(resultado.NomeCompleto);
                    linha.Add(resultado.Nome);
                    linha.Add(resultado.Fork.ToString());
                    linha.Add(resultado.QtdForks.ToString());
                    linha.Add(resultado.QtdEstrelas.ToString());
                    linha.Add(usuario);
                    linhas.Add(string.Join(",", linha));
                }

                var name = "Relatorio_ModeloRepo";
                System.IO.File.AppendAllLines($"{name}.csv", linhas.ToArray(), Encoding.UTF8);
                Console.WriteLine("Relatorio com repositorios atualizado!");
            }
        }

        private static string ObterLinhas(Resultado resultado)
        {
            List<string> listaResultados = new List<string>();
            listaResultados.Add(resultado.NomeRepositorio);
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

        private static async Task<List<RepositorioDetalhe>> BuscarRepositoriosProgramador(string usuarioProgramador)
        {
            var lrespose = new List<RepositorioDetalhe>();

            for (int i = 1; i <= 2; i++)
            {
                lrespose.AddRange(await API<List<RepositorioDetalhe>>.Consulta("users/" + usuarioProgramador + "/repos?per_page=100&page=" + i));
            }

            return lrespose;
        }


        private static async Task<RepositorioForkDetalhe> BuscarDetalhesRepositorioOriginal(string repositorio)
        {
            return await API<RepositorioForkDetalhe>.Consulta("repos/" + repositorio);
        }

        private static async Task<List<Resultado>> BuscarCommitsRepositorioProgramador(string repositorio, string usuarioProgramador)
        {
            var lista = new List<Resultado>();

            for (int i = 1; i < 35; i++)
            {
                var urls = await API<CommitUsuario>.Consulta("search/commits?q=repo:" + repositorio + "+author:" + usuarioProgramador + "&page=" + i);

                if (urls.Items != null && urls.Items.Count > 0)
                {
                    foreach (var item in urls.Items)
                    {
                        var requestUri = "" + item.Url.Replace("https://api.github.com/", "");
                        var detalhes = await API<CommitDetalhes>.Consulta(requestUri);

                        if (detalhes.Commit != null)
                        {
                            lista.Add(ObterResultado(detalhes, usuarioProgramador, repositorio, i));
                        }
                    }
                }
                else
                {
                    break;
                }
            }

            return lista;
        }
    }
}
