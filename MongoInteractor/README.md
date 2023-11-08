# MongoInteractions

The easiest way to work with a MongoDB cluster.

## Table of Contents

<!--TOC-->
  - [Package Support](#package-support)
  - [Installation](#installation)
    - [Package Manager Console (Visual Studio)](#package-manager-console-visual-studio)
    - [.NET CLI](#.net-cli)
  - [Usage](#usage)
    - [Note](#note)
<!--/TOC-->

## Package Support

| Framework | Is Supported? | Extra Info |
| --------- | ------------- | ---------- |
| **.NET 6**    | :ballot_box_with_check: |
| **.NET 7**    | :ballot_box_with_check: |
| **.NET Framework** | :x: | May never be supported due to storage limitations on this laptop. |
| **.NET Core** | :x: | May never be supported due to storage limitations on this laptop. |
| **Mono**      | :x: | Not supported yet, but **Mono** support is planned. |
| **Unity**     | :x: | Will **NEVER** be supported, along with other C# game engines. |

## Installation

### Package Manager Console (Visual Studio)

```powershell
Install-Package MongoInteractions
```

### .NET CLI

```bash
dotnet add package MongoInteractions
```

## Usage

**This example uses .NET 7, but this package supports .NET 6 as well.**

### Note

This example uses a `.env` file which is recommended if your MongoDB project has the `0.0.0.0/0` IP set in the **Network Access** page.

```csharp
using DotNetEnv;
using MongoDB.Driver;
using MongoInteractions;
using TestProject.Net7.Models;

namespace TestProject.Net7
{
    public class Program
    {
        // Asynchronous usage
        public static void Main() => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            Env.Load(Path.Join(GetProjectDirectory(), ".env"));
            var interactor = new MongoInteractor(Environment.GetEnvironmentVariable("CONNECTION_STRING")! /* This should 100% be defined, so it's not null. */);

            // Make sure to start the client first before doing anything with the interactor!
            await interactor.StartAsync(); // Use `interactor.Start();" if running synchronously.

            // You can drop any useless databases from your cluster.
            // Use "interaction.DropDatabase();" if running synchronously
            await interactor.DropDatabaseAsync("irrelevant"); // Replace "irrelevant" with your actual database name

            // Let's get our awesome database!
            var database = interactor.GetDatabase("test"); // Make sure that "test" is replaced with your actual database name (not the cluster name)!

            // Similarly to dropping databases, we can drop a collection.
            // Make sure to use "database.DropCollection()" if running synchronously
            await database.DropCollectionAsync("irrelevant");

            // We can also create collections!
            // Again, make sure to use "database.CreateCollection()" if running synchronously
            await database.CreateCollectionAsync("helpful-data");

            // Let's get a collection, make sure to view "Models/User.cs" to see the schema.
            var collection = database.GetCollection<User>("users"); // Replace the User generic type with your actual document type and "users" with your collection name

            // All right, let's create a piece of data.
            var document = new User()
            {
                Id = 1,
                Name = "John Doe",
                Email = "johndoe@example.com",
                Password = "password"
            };

            // Use "collection.InsertOne()" if running synchronously.
            await collection.InsertOneAsync(document);

            // Let's save a filter since we're gonna use it a lot here.
            var filter = new FilterDefinitionBuilder<User>().Eq((user) => user.Id, document.Id);

            // User updated their password, let's change it!
            var password = "br4nd$%&N3w$#*P4SsWoRD!!!"; // This is just an example.

            // Make sure to use "collection.FindOneAndUpdate()" if running synchronously.
            await collection.FindOneAndUpdateAsync(filter, new UpdateDefinitionBuilder<User>().Set((user) => user.Password, password));
        }

        // Useful for Visual Studio
        private static string GetProjectDirectory()
        {
            var cwd = Directory.GetCurrentDirectory();
            string projectDirectory;

            if (cwd.Contains("bin")) // Project is running from the "bin/{Configuration}/{Framework}/{ProjectName}.exe" file
            {
                projectDirectory = Directory.GetParent(cwd)!.Parent!.Parent!.FullName;
            }
            else
            {
                projectDirectory = cwd;
            }

            return projectDirectory;
        }
    }
}
```

---

**Ready to use this package? I'm sure you'll love it!**
