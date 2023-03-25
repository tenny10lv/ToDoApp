# **Hello**
### I am Tenny, fullstack developer

> Welcome to my repository.

## **How to run this project****How to run this project**
##### * Pre-requirement

- Download latest SQL Server from : https://www.microsoft.com/en-us/sql-server/sql-server-downloads
- Install SQL server following this tutorial: https://learn.microsoft.com/en-us/sql/database-engine/install-windows/install-sql-server

> *OR*

- Try using docker to setup SQL server
	 - setup SQL server by docker
	 ```docker compose up -d``` => this command will create SQL server container

####### Run migration
- Create database:
	- Using [SSMS](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16 "SSMS") to connect with database then create `ToDoApp` database schema

- Create database table and constraints
	 - Run this command on project folder
	 ```cd src && dotnet ef database update``` => this command will create SQL server container


##### 1. Run manually

- Clone this project
**- Finish the `Pre-requirement`**
- Make sure the connection string in database is correct in file `appsettings.Development.json` (dev env) and  `appsettings.json` (prod env)
- Run the command ``` dotnet run ``` in `src` folder or using ` Visual studio` IDE to run the project

##### 2. Run by Docker
- Clone this project
**- Finish the `Pre-requirement`**
- Then run:
	 ```docker compose -f docker-compose-app.yml up -d``` => this command will create application container

#### Enjoy
Go to: http://localhost:5000/swagger/index.html
