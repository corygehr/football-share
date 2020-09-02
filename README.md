# FootballShare

Web Application allowing multiple users to (casually) place wagers on NFL games using the spread. Created for use with friends for the 2019-2020 season.

Features:
* Ability to create multiple pools of users.
* Google or Microsoft Account authentication.
* Regular updates of schedules and scores.
* Automatic adjustment to player cash balance based on each week of results.

## Namespace summaries

### Automation

Azure Functions for updating schedules and scores on a recurring basis.

### DAL

**D**ata **A**ccess **L**ayer for easier database communication.

### Entities

Base entity classes shared by all projects in the solution.

### Sql

SQL scripts for database deployment.

### Web

Web Application project for the actual site.
