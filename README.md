# Habit Tracker

## Overview

This project is a small console application that helps in managing habits and habit occurrences. Was made as part of *The C# Academy*.

## How it works?

Internally upon opening the app a small SQlite database is created with two tables
- `habits` which holds basic information about habits (id, name, description)
- `habitsOccurrences` which holds information about when an habit was achieved (id, habitId, occurredAt, notes)

Then, a menu is displayed giving the user 3 options  

  <img width="195" height="71" alt="image" src="https://github.com/user-attachments/assets/39ccb623-7740-4987-871b-003c93f7f565" />  


1. Create a habit
   - This will guide the user in the steps to create a habit
     <img width="618" height="99" alt="image" src="https://github.com/user-attachments/assets/98084202-f395-4278-9884-0cfa4b231d07" />  
2. View and edit habits
    - Shows a list of existing habits at gives the user actions to perform for any of the displayed habits  
    <img width="497" height="114" alt="image" src="https://github.com/user-attachments/assets/07691071-2cd5-4419-b5a1-ad2d061f1f17" />
    - Edit
      - Allows the user to edit a habit
     - 
    - Remove
      - Allows the user to remove a habit 
4. Manage habit occurrence
