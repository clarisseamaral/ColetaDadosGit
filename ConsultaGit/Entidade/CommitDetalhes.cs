using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ConsultaGit
{
    [DataContract]
    public class CommitDetalhes
    {
        [DataMember(Name = "sha")]
        public String Identificador { get; set; }

        [DataMember(Name = "commit")]
        public Commit Commit { get; set; }

        [DataMember(Name = "files")]
        public List<File> Files { get; set; }
    }

    [DataContract]
    public class Committer
    {
        [DataMember(Name = "date")]
        public DateTime Data { get; set; }
    }

    [DataContract]
    public class Commit
    {
        [DataMember(Name = "committer")]
        public Committer Commiter { get; set; }
    }

    [DataContract]
    public class File
    {
        [DataMember(Name = "filename")]
        public string ArquivoModificado { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "additions")]
        public int QtdAdicoes { get; set; }

        [DataMember(Name = "deletions")]
        public int QtdExclusoes { get; set; }

        [DataMember(Name = "changes")]
        public int QtdMudancas { get; set; }

    }
}
