# Budget CLI
## Current Behavior

## In Progress
- Create Data Module
    - [ ] Models - Create AppInfoModel
    - [ ] Migration Manager - Create database on first open, including AppInfo table
    - [ ] Data Manager - Create class which can read/write from AppInfo table 
    - [ ] Models - Create OneTimeBill Model
    - [ ] Migration Manager - Create OneTimeBills table
    - [ ] Data Manager - Create class which can read/write from DataManager table
## Upcoming
### Minimal Functionality
- Add One-Time Bill
- Delete One-Time Bill by Name
- Pull List of One-Time Bills
### Basic Functionality
- Add Recurring Bill (and corresponding one-time instances)
- Delete Recurring Bill (and all one-time instances)
- Edit Recurring Bill (and all one-time instances)
- Pull List of Recurring Bills ("Budget Summary")
### Goal Functionality
- Add Projection functionality
- Add Projection Summary functionality
- Add "Startup Wizard."  On startup, program will...
    - Show bills which have come due since last open and allow user to mark paid or keep unpaid
    - Show a "Dashboard" featuring upcoming Projection (next week, for example) and Projection Summary

### Future 