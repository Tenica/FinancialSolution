using FinancialSolution.Infrastructure.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialSolution.Infrastructure.Settings
{
    public class DojahSettings
    {
        public string AppId { get; set; } = default!;

        public string SecretKey { get; set; } = default!;

        public string PublicKey { get; set; } = default!;

        public string BaseUrl { get; set; } = default!;
    }
}


//The POCO: This class is a "Plain Old CLR Object." Its only job is to provide a strictly typed,
//exact mirror of the JSON block.
//The = default!; tells the compiler to stop worrying about nulls,
//as we guarantee the framework will fill these properties when the app starts.

//The Binding Matrix: This line tells the .NET Dependency Injection container to act as a bridge. It says: "Go find the "DojahSettings" block in the JSON file. Read those strings, and automatically map them into the DojahSettings C# class."

//The Singleton: Once it does this mapping at startup, it saves that filled C# object in the server's memory for the entire lifespan of the application.
