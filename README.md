# dvt-jibble-frontend
## This project require .net 6 to run.
## Project setup
### Step 1: install postgresql. The default config is host: localhost, port: 5432, user: postgres, pass: postgres.
You can run it in docker by the command below.
```
docker run -d --name postgres -e POSTGRES_PASSWORD=postgres -p 5432:5432 postgres
```
### Step 2: Run Web API. Open folder dvt-jibble-backend and run the command below.
```
dotnet run
```
You can change the connection string in the appsettings.json file.

Web API URL after starting is: https://localhost:7078 and http://localhost:5078
### Step 3: Run Unit testing project. Open folder dvt-jibble-backend-tests and run the command below.
```
dotnet test
```
Note: unit tests still need postgresql to run. You need start postgresql before run unit test.

Unit tests can take 2 mins to run because it runs sequential and includes importing 1M rows test case also.

Unit tests will auto-create the database when it runs and auto-delete it when it finishes.

You can change the connection string of the testing database in the DummyData.cs file.
