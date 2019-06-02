using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultaGit
{
    public class Resultado
    {
        public string Usuario { get; set; }

        public string Identificador { get; set; }

        public DateTime DataCommit { get; set; }

        public int QtdAdicoesCod { get; set; }

        public int QtdExclusoesCod { get; set; }

        public int QtdMudancasCod { get; set; }

        public int QtdAdicoesTeste { get; set; }

        public int QtdExclusoesTeste { get; set; }

        public int QtdMudancasTeste { get; set; }

        public int Pagina { get; set; }

        public string NomeRepositorio { get; set; }

    }
}
