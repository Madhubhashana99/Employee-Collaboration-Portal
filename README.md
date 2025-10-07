# Employee-Collaboration-Portal
This solution implements core features like User, Post, and Comment Management, along with Interactions (Likes/Dislikes), Filtering, and Sorting. This project implements a simplified Employee Collaboration Portal using the **.NET Core** framework for the backend API, **Angular** for the frontend, and **MSSQL** for the database.

## 🚀 Project Structure

The repository is structured as follows:

```
/Employee-Portal-Submission
├── /Backend           # .NET Core API project
├── /frontend          # Angular application
├── /Database-Scripts  # MSSQL creation script
└── README.md          # This file
```

-----

## 🛠️ Prerequisites

Before running the application, ensure you have the following installed:

1.  **.NET 8.0 SDK** 
2.  **Node.js** (LTS version recommended)
3.  **Angular CLI** (`npm install -g @angular/cli`)
4.  **SQL Server** (or SQL Server Express/Developer Edition) and **SQL Server Management Studio (SSMS)** or Azure Data Studio for database setup.

-----

## 1\. Database Setup (MSSQL)

The application requires an MSSQL database named `EmployeeDB`.

### A. Run Database Script

1.  Open **SQL Server Management Studio (SSMS)**.
2.  Connect to your local SQL Server instance.
3.  Open the file located at: `Database-Scripts/Create_EmployeeDB.sql`.
4.  Execute the script. This will perform the following actions:
      * Create the `EmployeeDB` database.
      * Create the necessary tables (`Users`, `Posts`, `Comments`, `PostInteractions`).
      * Insert initial data for testing (e.g., an **Admin** user and a default **Employee** user).

### B. Update Connection String

1.  Navigate to the backend project folder: `/Backend/`.

2.  Open the `appsettings.json` file.

3.  Update the `DefaultConnection` string to match your local SQL Server configuration:

    ```json
    {
      "ConnectionStrings": {
        // IMPORTANT: Replace YOUR_SERVER_NAME with your actual SQL Server instance name
        "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=EmployeeDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True" 
      },
      // ... (rest of the file)
    }
    ```

-----

## 2\. Backend Setup (.NET Core API)

The API runs on `https://localhost:7001` by default.

### A. Restore Dependencies

1.  Open your terminal or command prompt.

2.  Navigate to the backend project folder:

    ```bash
    cd Backend
    ```

3.  Restore NuGet packages:

    ```bash
    dotnet restore
    ```

### B. Run the Backend API

1.  Run the application using the following command:

    ```bash
    dotnet run
    ```

2.  The API should start running, typically available at **`https://localhost:7001`**. Keep this terminal window open.

-----

## 3\. Frontend Setup (Angular)

The Angular application runs on `http://localhost:4200` and communicates with the backend on port 7001.

### A. Install Dependencies

1.  Open a **new terminal window**.

2.  Navigate to the frontend project folder:

    ```bash
    cd frontend
    ```

3.  Install NPM packages:

    ```bash
    npm install
    ```

### B. Configure API URL

The API URL is configured in the environment file:

1.  Open `frontend/src/environments/environment.ts`.

2.  Ensure `apiUrl` matches the backend's secure URL:

    ```typescript
    export const environment = {
      production: false,
      apiUrl: 'https://localhost:7001/api'
    };
    ```

### C. Run the Frontend

1.  Run the Angular development server:

    ```bash
    ng serve -o
    ```

2.  The application will automatically open in your browser at **`http://localhost:4200`** and redirect you to the Login page.

-----

