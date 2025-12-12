# Role-Based Workflow and Task Management System

## Project Overview
This is a web-based application built using **Angular** for the frontend, designed to manage users, roles, workflows, and tasks in an organizational setup. The system allows secure authentication, role-based access control, and monitoring of workflows and associated tasks.

---

## Features

- **Secure Login**: Email and password authentication with OTP verification for two-factor security.
- **Signup**: New users can register with details such as name, email, role, department, and designation.
- **Navigation Dashboard**: Responsive top and side navigation bars for easy access to different modules.
- **User Management**: Add, update, delete, and view users in a tabular format.
- **Role Management**: Define and manage roles with CRUD operations.
- **Workflow Management**: Create, update, and delete workflows.
- **Task Monitoring**: Assign, update, and track tasks under each workflow.
- **Dynamic Routing**: Angular routing used to navigate between pages efficiently.
- **Real-Time Monitoring**: Task and workflow statuses are displayed clearly for management oversight.

---

## Technologies Used

- **Frontend**: Angular, HTML, CSS
- **Backend**: Node.js / .NET (for API integration)
- **Database**: MySQL / MongoDB
- **Authentication**: OTP-based login system
- **UI Framework**: Bootstrap / Tailwind CSS

---

src/

├── app/

│ ├── components/ # Angular components

│ ├── services/ # HTTP services for API calls

│ ├── pages/ # All pages like login, signup, dashboard

│ ├── interceptors/ # HTTP interceptors for authentication

│ ├── app-routing.module.ts

│ └── app.module.ts

---



## Project Structure

