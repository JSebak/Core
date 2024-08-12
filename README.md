# In order to run the app

## Setting up the DB

1. install [SQLite](https://www.sqlite.org/download.html)
2. run the migrations in the solution, select infrastructure as the default project in the Packet Management Console. And use the following command:

```cmd
Update-Database
```

## Run the app
Run the app normally most of the endpoints are protected by Authentication and Authorization, the users are in DataSeeder in the seed method in the Infrastructure project if you don't want to register a user.

## Tests
You can use the test viewer that come with Visual studio in order to explore and run the tests in a more friendly way or you can use this .NET cli command in the test project directory
```cmd
dotnet test
```
