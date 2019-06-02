using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ConsultaGit
{
    [DataContract]
    public class CommitUsuario
    {

        [DataMember(Name = "items")]
        public List<Item> Items { get; set; }
    }

    [DataContract]
    public class Item
    {
        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "sha")]
        public string Sha { get; set; }
    }
}
