# _Recipe Book_

#### By _Joey Palchak_

#### _A C# / ASP.NET Core MVC application using Entity Framework Core, MySQL, and Authentication & Role-Based Authorization with Identity._

## Technologies Used

* C#
* .NET 6
* ASP.NET Core MVC
* Entity Framework Core
* Entity Framework Core CLI Tools
* Entity Framework Core Identity
* MySQL
* MySQL Workbench

## Description

An MVC web application allowing users to search for, read, edit, delete, and submit recipes to the application's database. If the user is signed in to their own account, they can only read and search through the sites recipes. However, if a user registers an account, they'll be authorized to author their own recipes, as well as edit or delete previous submissions.

The web application incorporates role-based authorization, with Administrative functionality allowing an Admin user to manage user accounts and user roles throughout the site. If a user creates an Admin account, or is assigned an Admin role, they'll have access to not just the Admin controller, but to edit and delete actions for any user submitted recipe.

## Setup/Installation Requirements

### Install Tools
This project assumes you have MySQL Server and MySQL Workbench installed on your system. _If you do not have these tools installed_, follow along with the installation steps for the the necessary tools introduced in the series of lessons found here on [LearnHowToProgram](https://full-time.learnhowtoprogram.com/c-and-net/getting-started-with-c/installing-and-configuring-mysql).

For Entity Framework Core, we'll use a tool called `dotnet-ef` to reference the project's migrations and update our database accordingly. To install this tool globally, run the following command in your terminal:

```
$ dotnet tool install --global dotnet-ef --version 6.0.0
```

Optionally, you can run the following command to verify that EF Core CLI tools are correctly installed:

```
$ dotnet ef
```

### Install and Run the Project

Assuming you've completed the required steps above:

1. Copy the **[URL](https://github.com/jfpalchak/RecipeBook.Solution.git)** provided for this repository.
2. Open Terminal.
3. Change your working directory to where you want the cloned directory.
4. In your terminal, type `git clone` and use the copied URL from Step 1. Or, copy the following git command:
```bash
$ git clone https://github.com/jfpalchak/RecipeBook.Solution.git
```
5. Open your terminal and navigate to this project's production directory called `Factory`.
6. Within the production directory of the project, create a file called `appsettings.json` and add the following code to it:
   ```json
    {
      "ConnectionStrings": {
          "DefaultConnection": "Server=localhost;Port=3306;database=[DATABASE];uid=[USERNAME];pwd=[PASSWORD];"
      }
    }
   ```
7. Next, make sure to update the connection string with your own choice of naming for the `[DATABASE]`, as well as your own system's values for `[USERNAME]` and `[PASSWORD]` when logging in to MySQL. Don't forget to replace the brackets `[]` as well!
8. With `appsettings.json` properly configured, in the command line, run the following command to reference the project's migrations and re-create the application's database:

   ```
   $ dotnet ef database update
   ```
9.  Then, in the command line, run the following command to compile and run the web application in development mode with a watcher:
   
```bash
$ dotnet watch run
```
> Optionally, you can run `dotnet build` to compile this web app without running it.

10. Open your browser to https://localhost:5001 to navigate and use the web application. 
> If you cannot access localhost:5001 it is likely because you have not configured a .NET developer security certificate for HTTPS.

## Known Bugs

* If any bugs are discovered, please contact the author.

## License

MIT License

Copyright (c) _10/19/2023_ _Joey Palchak_

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.