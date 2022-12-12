//datime
using System;
//using models
using ApiControleDespesas.Models;

namespace ApiControleDespesas.Models
    
{
    public class UsuarioToken
    {
        public bool Authenticated { get; set; }
        public DateTime Expiration { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }

    }
}
