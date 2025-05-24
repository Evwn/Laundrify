# Laundrify - Laundry Management System

A web-based laundry management system that allows clients to place orders and administrators to manage them.

1. Install Entity Framework Core tools globally (if not already installed):
   ```bash
   dotnet tool install --global dotnet-ef
   ```

2. Run the database migrations:
   ```bash
   dotnet ef database update
   ```

3. Run the application:
   ```bash
   dotnet run
   ```

### Test Credentials

#### Admin Account
- Email: admin@laundrify.com
- Password: Admin123!

#### Client Account
- Email: client1@laundrify.com
- Password: Client123!

## Features

### Admin Features
- View and manage all orders
- Update order status (Pending, Ready, Completed, Postponed, Canceled)
- View client information
- Manage services
- Dashboard with business statistics

### Client Features
- Place new orders
- View order history
- Track order status
- Manage profile

## Order Status Flow
1. Pending - Initial state when order is placed
2. Ready - Order is ready for pickup
3. Completed - Order has been delivered
4. Postponed - Order is delayed
5. Canceled - Order has been canceled
