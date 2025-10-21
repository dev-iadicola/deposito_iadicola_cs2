using System;
using System.Collections.Generic;
namespace DependecyInjection.Test;


// 1️⃣ Interfaccia comune per ogni test
public interface ITest
{
    string Name { get; }
    void Run();
}


