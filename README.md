# ServicePro – Home Services Platform

## 📌 Overview

ServicePro is a full-stack web-based marketplace platform that connects customers with verified professional service providers such as plumbers, electricians, carpenters, and cleaners.

The platform acts as a secure digital intermediary between customers and providers through:

* Service Requests
* Competitive Offers
* Provider Verification
* Wallet-Based Transactions
* Ratings & Reviews
* Notifications
* Complaints Management

The system simulates real-world service marketplace platforms like Upwork and TaskRabbit with complete business workflow management and financial control.

---

# 🚀 Core Features

## 👤 Authentication & Authorization

* Secure Registration & Login
* ASP.NET Core Identity Integration
* Role-Based Authorization
* Password Hashing & Secure Authentication
* Three User Roles:

  * Admin
  * Customer
  * Service Provider

---

# 🛠 Customer Features

* Create service requests
* Browse provider offers
* Accept/reject offers
* Manage profile and address
* Wallet payment support
* Rate and review providers
* Send complaints
* Receive notifications

---

# 🔧 Provider Features

* Create professional profile
* Upload profile image
* Add biography ("About Me")
* Select multiple specialties
* Submit offers on requests
* Complete service requests
* Receive wallet payments
* Access customer contact information after request acceptance

---

# 🛡 Admin Features

* Verify provider accounts
* Monitor platform revenue
* Handle complaints
* Manage categories
* Manage users
* Monitor transactions

---

# 💰 Financial System

The platform includes a secure digital wallet system.

## Financial Workflow

1. Customer accepts provider offer
2. System validates customer wallet balance
3. Service gets completed
4. Automatic payment distribution starts

## 90/10 Revenue Rule

* **90%** → Provider Wallet
* **10%** → Platform Admin

### Important Update

Provider accounts no longer receive default starting balance to ensure realistic financial tracking.

---

# 🔄 Full System Workflow

```text id="0rlrt7"
Registration
     ↓
Role Selection
     ↓
Provider Verification
     ↓
Customer Creates Request
     ↓
Providers Submit Offers
     ↓
Customer Accepts Offer
     ↓
Provider Completes Service
     ↓
Automatic Payment Settlement
     ↓
Customer Rating & Feedback
```

---

# ⭐ Advanced Rating System

* Star-based rating system
* Dynamic average rating calculation
* Customer reviews and feedback
* Reputation-building mechanism for providers

Ratings are automatically displayed across provider profiles to help customers make informed decisions.

---

# 🔔 Smart Notifications System

The system includes real-time internal notifications for:

* New offers
* Offer acceptance
* Service completion
* Verification approval
* Complaint updates

### Logic Enhancement

Notification badges now appear only when unread notifications exist.

---

# 🎨 UI/UX Enhancements

## Modern Interface Improvements

* Redesigned navigation bar
* Customized dashboards for each user role
* Responsive layouts using Bootstrap 5
* Improved user experience and navigation flow

## Personalized Profiles

* Profile picture uploads
* Editable contact information
* Editable addresses
* About Me section for providers

---

# 🧠 Business Logic Rules

* Only verified providers can submit offers
* Customer can accept only one offer per request
* Ratings allowed only after service completion
* Wallet balance validation required before acceptance
* Providers gain access to customer contact details only after request acceptance

---

# 🗄 Database Design

Main Entities:

* Users
* Wallets
* Categories
* Service Requests
* Offers
* Transactions
* Ratings
* Complaints
* Notifications

The database is fully relational and normalized using SQL Server and Entity Framework Core.

---

# 🏗 Tech Stack

## Backend

* ASP.NET Core MVC
* C#
* Entity Framework Core
* ASP.NET Core Identity

## Database

* SQL Server

## Frontend

* HTML5
* CSS3
* Bootstrap 5
* JavaScript
* FontAwesome
* Google Fonts

---

# 🔐 Security Features

* Secure authentication system
* Password hashing
* Role-based authorization
* Protected routes and dashboards
* Verified provider system
* Secure financial transaction handling

---

# 📁 Project Structure

```text id="a8k7cz"
ServicePro/
│
├── Controllers/
├── Models/
├── Views/
├── Data/
├── ViewModels/
├── Migrations/
├── wwwroot/
├── Areas/
└── Program.cs
```

---

# 📊 Latest Major Updates

## 🌟 New Features & Improvements

### ✅ Advanced Rating System

* Star-based reviews
* Automatic average calculation
* Dynamic feedback display

### ✅ Provider Profile Enhancements

* About Me section
* Multiple specialties support
* Professional profile customization

### ✅ Cross-Profile Accessibility

* Customers can view provider profiles
* Providers can access customer contact details after acceptance

### ✅ Identity & Personalization

* Profile image upload support
* Editable customer dashboard
* Personalized account management

### ✅ UI/UX Overhaul

* Distinct dashboards for Customers & Providers
* Redesigned Navbar
* Responsive design improvements
* Added Carpentry service category

### ✅ Logic & Stability Improvements

* Smart notification logic fixes
* Improved financial accuracy
* Complete End-to-End testing
* Platform stability improvements

---

# 🧪 Testing

The platform has undergone:

* End-to-End (E2E) Testing
* Workflow Validation
* Financial Logic Testing
* Authentication & Authorization Testing

System Status:
✅ Fully Functional
✅ Stable
✅ Tested Successfully

---

# 🚀 Future Enhancements

* Mobile Application
* Real-Time Chat
* AI Recommendations
* GPS Tracking
* Online Payment Gateway
* Analytics Dashboard

---

# ⚙️ Installation & Setup

## Requirements

* Visual Studio 2022
* SQL Server
* .NET 6 / .NET 8 SDK

---

## Setup Steps

### 1. Clone Repository

```bash id="mx9bkt"
git clone <repository-link>
```

### 2. Open Project

Open the solution using Visual Studio.

### 3. Configure Database

Update the connection string inside:

```text id="bjlwmk"
appsettings.json
```

### 4. Apply Migrations

```bash id="7i9ew8"
Update-Database
```

### 5. Run the Project

```bash id="jlwm7f"
Ctrl + F5
```

# 👨‍💻 Team Members

Shahd Ashraf , Alshaimaa Ashraf, Dina Aboueloyoun,
Rawan Araby, Amany Mahmoud.
---

# 📄 License

This project was developed for educational purposes as a university final project.

---

# 🙌 Acknowledgment

Special thanks to all team members, instructors, and reviewers who contributed to the development and testing of this project.
