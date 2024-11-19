#Room Management Microservice
#Description
This microservice is part of a hotel reservation management system. It is specifically designed for managing rooms, allowing the creation, updating, and retrieval of available room information.

#Current Features:
Add a new room.
Update existing room details.
Retrieve information about one or more rooms.
Note: User access control is not yet implemented, as the Tenant microservice for access management has not been developed.

#Technologies Used
C# / .NET Core: Backend development for the microservice.
MongoDB: Event store Database.
Docker: Containerization of the microservice.
Kafka : Event streaming platform for asynchronis Event Transfert between Command and Queries instances
Entity Framework Core: Data access management.
ASP.NET Core web API : web server and middleware pipline.
SQLServer : DataBase to store Room details
Installation and Execution
Prerequisites:
Docker: To run MongoDB, SQLServer, Kafka and zookeper and the microservice in a container.
.NET SDK 8.0 or later: To run the project locally.