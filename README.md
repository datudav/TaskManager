# TaskManager
This is the Law Advisor project for the engineering test.
The TaskManager is a RESTful API that can be used to manage tasks and group them to lists.

# Stack
The TaskManager was built using these Microsoft technologies:
- C# and .NET 5
- ASP .NET 
- SQL Server

# Starting the TaskManager
These are the steps to start the TaskManager API:
1. Restore the SQL server database in a SQL Server instance. The backup can be found in /SQL Server/Database Files/TaskManagerOLTP.bak. The scripts to create the users, table, and stored procedures can be found in /SQL Server/Scripts.
2. Open appsettings.json and update the connection string. The format must be "data Source={Your SQL Server Instance};User Id=Development;Password=Devel0pment123!;Connect Timeout=60;Initial Catalog={The name of the restored OLTP database};TrustServerCertificate=True;";
3. Run the windows batch file StartTaskManager.cmd. It will start hosting the service. The address by default is http://localhost/5000.

# Using TaskManager
These are the steps to use the TaskMananger API:
1. Open postman or any app that can communicate to APIs. 
2. Create user. You must create a user first to gain access by accessing POST http://localhost/5000/users. It will ask for a username and password. The reuest must be submitted from the body.
- Example: {
    "username": "string",
    "password": "string"
  }
3. After a succesful registration, get an authentication token by accessing POST http://localhost/5000/users/authenticationToken. The request is the same as the registration endpoint. If the provided credentials are valid, it will return a token that will be usaed to access the other endpoints. The TaskManager is using bearer atuthentication, therefore a "Bearer" prefix must be added in the token when passing the authentication token.
4. You can now access the other endpoints.

# TaskManager Basic Functionalities
These are the basic functionalities offered by the TaskManager API:
1. Creating a list. This list is capable of creating task and ordering them according to "rank". A user can create multiple lists. Each lists will have a List ID.
2. Creating a task. Once a list is created, tasks can be created as part of the lists. A user can not create a task without a list.
3. Arranging the tasks accorsing to rank. Access PATCH /toDoLists/{listId}/tasks/{taskId} to move a task from one place to another. It will return an error if the rank provided exceeeds the number of tasks in the list. The API will take crea of the other tasks affected by the move.
4. Deleting a task from a list. Access DELETE /toDoLists/{listId}/tasks/{taskId} and input the listId and taskId to delete a task from a list.

