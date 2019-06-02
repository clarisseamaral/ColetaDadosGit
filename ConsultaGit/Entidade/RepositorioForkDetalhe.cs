using System.Runtime.Serialization;

namespace ConsultaGit
{
    [DataContract]

    public class RepositorioForkDetalhe
    {
        [DataMember(Name = "source")]
        public Source Source { get; set; }
    }

    [DataContract]
    public class Source
    {
        [DataMember(Name = "full_name")]
        public string NomeCompleto { get; set; }

        [DataMember(Name = "forks_count")]
        public int QtdForks { get; set; }

        [DataMember(Name = "stargazers_count")]
        public int QtdEstrelas { get; set; }
    }
}
