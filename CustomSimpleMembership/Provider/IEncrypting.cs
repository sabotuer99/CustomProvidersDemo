using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomSimpleMembership.Provider
{
    public interface IEncrypting
    {
        string Encode(string password); 
    }
}
