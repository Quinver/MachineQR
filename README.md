# MachineQR

A web application that provides an overview of all machines at school, with QR codes linking to individual machine pages. Built with **ASP.NET** and **PostgreSQL**.

## Features
- Generate and scan QR codes for machine details
- add and manage machines
- User-friendly UI for quick machine lookup
- Secure database integration using PostgreSQL

## Tech Stack
- **Backend:** ASP.NET
- **Database:** PostgreSQL
- **Frontend:** HTML, CSS, JavaScript (adjust as needed)
- **Hosting:** Azure

## Setup
(Might make a docker container in the future)
(Have not tested what versions work, you'll need to debug it yourself)
1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/machineqr.git
   cd machineqr
   ```
2. Configure the database connection in `appsettings.json`. (Use postgresql)
3. Run the project:
   ```bash
   dotnet run
   ```

## Contributors
- Quin
- Hayley
- Liam
