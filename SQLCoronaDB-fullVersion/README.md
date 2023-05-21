# Covid 19 Management System


The Corona Management System is a web-based application for managing member details and vaccination records in a large health fund during the corona epidemic. The system is built using ASP.NET Web API and MSSQL database, and allows users to add, view, and search for member details, as well as record and view vaccination dates and manufacturers.
# Getting Started
## Prerequisites
- MSSQL Server Management Studio.
- Visual Studio 2019 or later.
- The packages:
    Microsoft.AspNetCore.Mvc.NewtonsoftJson
    Swashbuckle.AspNetCore 
    System.Data.SqlClient
- MSSQL Server Management Studio.
- postman

## Installation
1. Clone the repository to your local machine.
2. Open the project in Visual Studio.
3. Run the Update-Database command in Package Manager Console to create the database and tables.
4. Build the project and run the application.
5. open postman and send API requests


## Usage

To use this API, you can make HTTP requests to the API endpoints using any programming language or tool that supports HTTP requests. Here's how you can interact with the API:

###Endpoints

Get all members

    GET /members
This endpoint retrieves all members from the database.

Get member by ID

    GET /members/{id_card}
This endpoint retrieves a specific member by their ID.

Create a new member

    POST /members
This endpoint allows you to create a new member. You must provide a JSON object in the request body with the following fields:

- **first name** (string, required): the first name of the member
- **last name** (string, required): the last name of the member
- **id card** (string, required): the id of the member
- **city** (string, required): the city of the member
- **street** (string, required): the street of the member
- **number** (integer, required): the home number of the member
- **date of birth** (dateTime, required): the date of birth of the member
- **phone** (string, required): the phone of the member
- **mobile phone** (string, required): the mobile phone of the member

Get all vaccinations

    GET /vaccinationsGetting
This endpoint retrieves all vaccines from the database.

Get vaccinations by ID

    GET /vaccinationsGetting/{id_card}
This endpoint retrieves the vaccines for a specific members by their ID.

Create a new vaccination row

    POST /vaccinationGetting
This endpoint allows you to create a new vaccination row. You must provide a JSON object in the request body with the following fields:

- **member id** (string, required): the id from the member table of the member that got the vaccine
- **vaccin date** (dateTime, required): the date of the vaccine getting
- **manafucture code** (string, required): the id from the manufactures table of this manufacture

Create a new vaccine for a member

    POST /vaccinationGetting/getbyid
This endpoint allows you to create a new vaccination row. You must provide following items:

- **member id_card** (string, required): the id card number of the member 
- **vaccin date** (string, required): the date of the vaccine getting
- **manafucture name** (string, required): the name of the manufacture

Get all diseases

    GET /diseasePeriod
This endpoint retrieves all diseases from the database.
Get diseases by id

    GET /diseasePeriod/{id_card}
This endpoint retrieves the disease for a specific members by their ID.

Create a new disease row

    POST /diseasePeriod
This endpoint allows you to create a new disease row. You must provide a JSON object in the request body with the following fields:

- **member id** (string, required): the id from the member table of the member that got the vaccine
- **detected date** (dateTime, required): the beggining date 
- **recovery date** (dateTime, required): the last  date 
 
Create a new disease by member id_card

    POST /diseasePeriod/getbyid
This endpoint allows you to create a new disease row. You must provide the following fields:

- **member id_card** (string, required): the id_card of the member 
- **detected date** (dateTime, required): the beggining date 
- **recovery date** (dateTime, required): the last  date 

Get all manafuctures

    GET /manafuctures
This endpoint retrieves all manafuctures from the database.
Create a new manafucture row

    POST /manafuctures
This endpoint allows you to create a new manafucture row. You must provide a JSON object in the request body with the following fields:

- **manafucture name** (string, required): the name of the manafuctures



##Examples
Here's an examples of using the API 
![Alt text](take1.jpg) 
![Alt text](take2.jpg )
![Alt text](take3.jpg )
![Alt text](take4.jpg )
![Alt text](take5.jpg )



