using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presistence.Contracts
{
    public record CreateUser(string Name, string Email, string Password);
}
