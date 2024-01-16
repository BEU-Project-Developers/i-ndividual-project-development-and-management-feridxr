# WGU Scheduler

This project is a mockup of a desktop business scheduling application. This project is written in C#, targeting WPF, and using the MVVM pattern. This was course work for my degree program at Western Governors University (WGU). 

To run and compile this, you will need Visual Studio. I have tested against Visual Studio 2019 and Visual Studio 2022, I do not know if this will compile against older versions.

This was originally designed to be run against an online MySQL database. As the database was taken offline by WGU after I completed this course, I have modified it to use a local Sqlite DB. The database schema was part of the rubric, I did not choose it (there are some questionable choices for the data structure).

## Functionality

- Automatically determine user's region/language, and output error messages in that language.
- Add, update, and delete customer records in a database.
- Add, update, and delete appointments in a database (connected to the customer).
- Toggle appointment calendar between a monthly and a weekly view.
- Automatically adjust appointment times based on user's time zone and DST.
- Exception controls to prevent:
    - scheduling an appointment outside business hours
    - scheduling overlapping appointments
    - entering nonexistent or invalid customer data
    - entering an incorrect username and password
- Trigger alerts before appointments.
- Generate reports:
    - number of appointment types by month
    - the schedule for each  consultant
    - a potential fraud report
- Log user activity