# Car Management API - Clean Architecture

A modern .NET 9 Web API for managing cars and their owners, built with Entity Framework Core, PostgreSQL, and AWS integration.

## 🏗️ Architecture Overview

This project follows clean architecture principles with clear separation of concerns:

```
TodoWebApi/
├── Configuration/          # Configuration models and options
├── Controllers/           # API controllers
├── Extensions/            # Service collection extensions
├── Migrations/           # EF Core migrations
├── Services/             # Business services and implementations
├── AppDbContext.cs       # Entity Framework DbContext
├── AppDbContextFactory.cs # Design-time DbContext factory
└── Program.cs            # Application entry point
```

## 🔧 Configuration Strategy

### Development Environment

- **Database**: Local PostgreSQL via Docker
- **Configuration**: `appsettings.Development.json`
- **Connection String**: Direct connection to local database
- **Seeding**: Automatic demo data seeding
- **Logging**: Enhanced with sensitive data logging

### Production Environment

- **Database**: AWS RDS PostgreSQL
- **Configuration**: Environment variables + `appsettings.Production.json`
- **Connection String**: Retrieved from AWS Secrets Manager
- **Security**: AWS IAM roles or environment variables for credentials

## 🚀 Getting Started

### Prerequisites

- .NET 9 SDK
- Docker Desktop
- AWS CLI (for production deployment)

### Local Development Setup

1. **Start Local Database**

   ```bash
   docker-compose up -d
   ```

2. **Apply Migrations**

   ```bash
   dotnet ef database update --project TodoWebApi/TodoWebApi.csproj
   ```

3. **Run the Application**

   ```bash
   cd TodoWebApi
   dotnet run
   ```

4. **Access Swagger UI**
   - Navigate to: `https://localhost:5022/swagger`

### Configuration Files

#### appsettings.Development.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=car-management;Username=postgres;Password=localpassword;Include Error Detail=true"
  },
  "DatabaseOptions": {
    "Provider": "PostgreSQL",
    "EnableSensitiveDataLogging": true,
    "EnableDetailedErrors": true
  }
}
```

#### appsettings.Production.json

```json
{
  "AWS": {
    "Region": "us-east-1"
  },
  "DatabaseOptions": {
    "Provider": "PostgreSQL",
    "SecretName": "rds!db-4763ee1e-667f-435e-aa96-3b77955ec956",
    "DatabaseName": "car-management",
    "HostName": "demo-database.ca1uii040tyz.us-east-1.rds.amazonaws.com"
  }
}
```

## 🗄️ Database Schema

### Cars Table

- `Id` (Primary Key)
- `Manufacturer`
- `Model`
- `Year`
- `Price`

### CarOwners Table

- `Id` (Primary Key)
- `Name`
- `Email`
- `PhoneNumber`
- `CarId` (Foreign Key to Cars)

## 📚 API Endpoints

### Cars Controller

- `GET /Cars` - Get all cars
- `GET /Cars/{id}` - Get car by ID
- `POST /Cars` - Create new car
- `PUT /Cars/{id}` - Update car
- `DELETE /Cars/{id}` - Delete car

### CarOwners Controller

- `GET /CarOwners` - Get all car owners
- `GET /CarOwners/{id}` - Get car owner by ID
- `GET /CarOwners/by-car/{carId}` - Get owner by car ID
- `POST /CarOwners` - Create new car owner
- `PUT /CarOwners/{id}` - Update car owner
- `DELETE /CarOwners/{id}` - Delete car owner
- `GET /CarOwners/cars-with-owners` - Get cars with their owners

## 🔐 Security & Best Practices

### Development

- ✅ No sensitive data in source control
- ✅ Local Docker environment for consistency
- ✅ Enhanced logging for debugging

### Production

- ✅ AWS Secrets Manager for database credentials
- ✅ Environment variables for AWS configuration
- ✅ IAM roles for secure access
- ✅ Minimal logging for performance

## 🚀 Deployment

### GitHub Actions Workflow

The deployment process uses GitHub Actions with the following steps:

1. **Build & Test**: Restore dependencies and build the project
2. **Create Migration Bundle**: Generate EF Core migration bundle
3. **Apply Migrations**: Run migrations against production database
4. **Deploy**: Deploy to Azure App Service

### Required GitHub Secrets

- `AWS_RDS_HOST_CONNECTIONSTRING`: Full production database connection string
- `AWS_ACCESS_KEY_ID`: AWS access key for Secrets Manager
- `AWS_SECRET_ACCESS_KEY`: AWS secret key for Secrets Manager
- `AZUREAPPSERVICE_PUBLISHPROFILE_*`: Azure App Service publish profile

### Environment Variables (Production)

```bash
ASPNETCORE_ENVIRONMENT=Production
AWS__AccessKeyId=<your-aws-access-key>
AWS__SecretAccessKey=<your-aws-secret-key>
AWS__Region=us-east-1
```

## 🔧 Entity Framework Core

### Design-Time Operations

The `AppDbContextFactory` enables EF Core tooling to work seamlessly:

```bash
# Add migration
dotnet ef migrations add NewMigration --project TodoWebApi/TodoWebApi.csproj

# Update database
dotnet ef database update --project TodoWebApi/TodoWebApi.csproj

# Create migration bundle
dotnet ef migrations bundle --project TodoWebApi/TodoWebApi.csproj --output efbundle
```

### Migration Strategy

- **Development**: Direct database updates via EF Core CLI
- **Production**: Migration bundles executed during deployment

## 🧪 Testing

### Manual Testing

Use the provided HTTP files for manual API testing:

- `CarOwners.http` - Car owners API tests
- `TodoWebApi.http` - General API tests

### Example Commands

```bash
# Create a new car
curl -X POST "https://localhost:5022/Cars" \
  -H "Content-Type: application/json" \
  -d '{"manufacturer":"Tesla","model":"Model 3","year":2023,"price":45000}'

# Create a car owner
curl -X POST "https://localhost:5022/CarOwners" \
  -H "Content-Type: application/json" \
  -d '{"name":"John Doe","email":"john@example.com","phoneNumber":"+1-555-0123","carId":1}'
```

## 🎯 Key Features

- ✅ **Clean Architecture**: Well-organized, maintainable codebase
- ✅ **Environment-Specific Configuration**: Proper separation of dev/prod settings
- ✅ **AWS Integration**: Secure credential management with Secrets Manager
- ✅ **EF Core Migrations**: Automated database schema management
- ✅ **OpenAPI/Swagger**: Complete API documentation
- ✅ **Docker Support**: Consistent local development environment
- ✅ **CI/CD Pipeline**: Automated deployments with GitHub Actions
- ✅ **Error Handling**: Comprehensive validation and error responses
- ✅ **Business Logic**: Proper relationship management and constraints

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.
