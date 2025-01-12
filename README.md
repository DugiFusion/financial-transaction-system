# Banking Application

A cloud-based banking application designed to manage transactions efficiently. Built with microservice architecture using .NET Core and Angular, this project focuses on seamless backend functionality and a responsive frontend interface.

**NOTE**: This is just example project which emphasizes structure of the project and not the logic behind it. 
---

## **Features**
- **Transaction Management**: Create, view, and manage transactions.
- **Report Management**: Create transaction reports and download them.
- **REST API**: A fully documented RESTful API for all operations.
- **Cloud Deployment**: Hosted on Microsoft Azure for scalability.
- **Frontend**: Built with Angular for an intuitive user interface.
- **Message Queue**: Implemented queueing with RabbitMQ
---

## **Technologies Used**
- **Backend**: .NET Core 9
- **Frontend**: Angular 19
- **Database**: SQL Server
- **Cloud Platform**: Microsoft Azure
- **ORM**: Entity Framework Core
- **API Documentation**: Swagger
---

## Running the Application Locally and Cloud Setup

### Prerequisites
Ensure the following tools are installed on your system:
- **Angular CLI** (v19 or later)
- **.NET Core SDK** (v9 or later)
- A relational database (e.g., SQL Server, PostgreSQL)

### Steps to Run Locally

1. **Frontend (Angular)**  
   - Navigate to the Angular project directory.  
   - Serve the application using the Angular CLI:  
     ```bash
     ng serve
     ```
   - The application will be available at `http://localhost:4200`.

2. **Backend (.NET Microservices)**  
   - Install dependencies for each microservice:  
     Navigate to each microservice folder and run:  
     ```bash
     dotnet restore
     ```
   - Configure the following ports for the microservices:
     - **Gateway**: `5000`
     - **Transaction Service**: `5001`
     - **Reporting Service**: `5002`
   - Install RabbitMQ locally
   - Run the microservices and Gateway in parallel:
     ```bash
     dotnet run --project [MicroserviceName]
     ```

3. **Database Setup**  
   - Create two databases:
     - `TransactionDB` (Table: `Transactions`)
     - `ReportingDB` (Tables: `ReportFiles`, `Reports`)
   - Add connection strings to the `appsettings.Development.json` file for each microservice. See an example configuration below.

### Example `appsettings.json` Configuration

```json
{
  "ConnectionStrings": {
    "TransactionDB": "Server=localhost;Database=TransactionDB;User Id=your_username;Password=your_password;",
    "ReportingDB": "Server=localhost;Database=ReportingDB;User Id=your_username;Password=your_password;"
  },
  "AllowedHosts": "*"
}
```

### Deploying to the Cloud

1. **Containerize the Application**  
   - Create `Dockerfile` for each microservice and the Angular frontend.
   - Build Docker images for all components:  
     ```bash
     docker build -t [image_name]:[tag] .
     ```

2. **Set Up Kubernetes**  
   - Create Kubernetes manifests (`.yaml` files) for deployments and services for each component, or use my files.
      - Expose URLs for Gateway and Client
      - Microservices communicate locally in the cluster

3. **Configure Cloud Environment**  
   - Use a cloud provider which supports K8S like Azure Kubernetes Service (AKS), AWS EKS.
   - Push your Docker images to a container registry (e.g., Azure Container Registry, Docker Hub):
     ```bash
     docker push [registry_url]/[image_name]:[tag]
     ```

5. **Verify Deployment**  
   - Use `kubectl get services` to ensure all services are running.
   - Access the application via the configured domain or IP.


