using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ConsultaGit
{
    [DataContract]

    public class RepositorioDetalhe
    {
        [DataMember(Name = "forks_count")]
        public int QtdForks { get; set; }

        [DataMember(Name = "stargazers_count")]
        public int QtdEstrelas { get; set; }

        [DataMember(Name = "name")]
        public string Nome { get; set; }

        [DataMember(Name = "full_name")]
        public string NomeCompleto { get; set; }

        [DataMember(Name = "fork")]
        public bool Fork { get; set; }

    }
}
