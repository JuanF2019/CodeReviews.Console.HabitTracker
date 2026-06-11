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
   - <img width="618" height="99" alt="image" src="https://github.com/user-attachments/assets/98084202-f395-4278-9884-0cfa4b231d07" />  
2. View and edit habits
    - Shows a list of existing habits at gives the user actions to perform for any of the displayed habits 
    - <img width="632" height="104" alt="image" src="https://github.com/user-attachments/assets/94e3c0c4-2e01-40f2-b48d-587ce3fa9f40" />  
 
      - Edit
        - Allows the user to edit a habit  
        1. <img width="634" height="147" alt="image" src="https://github.com/user-attachments/assets/56ed3c71-a801-46f1-863c-bdaf1657a1b1" />
        2. <img width="635" height="103" alt="image" src="https://github.com/user-attachments/assets/18bd3e49-b9a1-43e3-b3e2-0272113d8263" />  
        3. <img width="633" height="113" alt="image" src="https://github.com/user-attachments/assets/4c461e13-f046-49ad-9dfd-9076c4ec7360" />  

      - Remove
        - Allows the user to remove a habit
        - <img width="632" height="185" alt="image" src="https://github.com/user-attachments/assets/da93d83c-f2ca-49be-9d3f-0f9ab64f47d0" />

3. Manage habit occurrences
    - Displays the list of habit occurrences and gives the user actions to manage them.
    - <img width="735" height="100" alt="image" src="https://github.com/user-attachments/assets/85d3ac84-b16c-4cdb-9710-cab9057e429d" />
    
      - Create
        - <img width="734" height="166" alt="image" src="https://github.com/user-attachments/assets/295122ec-3cbf-4e2e-b3f9-5a7df4666f46" />
 
      - Edit
        1. <img width="734" height="167" alt="image" src="https://github.com/user-attachments/assets/9705f86f-9f32-4a48-9f5a-df961abed087" />
        2. <img width="734" height="110" alt="image" src="https://github.com/user-attachments/assets/d879cc35-14b0-4d7f-8514-0e8b39da36f8" />
        3. <img width="732" height="116" alt="image" src="https://github.com/user-attachments/assets/86bdd2dd-b56a-4ac9-9964-a6432388b11f" />

      - Remove
        - <img width="734" height="185" alt="image" src="https://github.com/user-attachments/assets/5cca8000-c471-4768-a433-3f652150e383" />


