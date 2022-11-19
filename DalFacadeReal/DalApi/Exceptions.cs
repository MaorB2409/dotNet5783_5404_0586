using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi;

public class IdNotExistException : Exception
{
    public IdNotExistException(string message) : base(message) { }
}
public class IdExistException : Exception
{
    public IdExistException(string message) : base(message) { }
}