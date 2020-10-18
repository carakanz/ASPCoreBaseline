using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class AuthorizationOptions
    {
        public bool ValidateIssuer { get; set; }
        public string ValidIssuer { get; set; }
        public bool ValidateAudience { get; set; }
        public string ValidAudience { get; set; }
        public string Key { get; set; }
        public bool ValidateLifetime { get; set; }
        public TimeSpan LifeTime { get; set; }
        public bool ValidateIssuerSigningKey { get; set; }
        public IDictionary<string, OAuchOptions> OAuch { get; set; }
    }
}
