# DigitalEnvision Assesment Test
This application using entity framework (code first)

1. First, please setting your application :
 - Create database for hangfire 
 - Create database for application
 - Setting your hookbin url and starting send
 - Try to run application
 - Url for swagger : host/swagger
 - Url for hangfire : host/hangfire

2. After all the above has done. You can do everything ( manipulate data).
3. Step for send birthday message / alert :
 - First, you "MUST" create the user data.
 - Run Hangfire, and recurring alertqueue
 - After recurring alertqueue job. make sure the data has been created in the alertLog table
 - After that, you can recurring sendAlert job to send alert / message.

Make sure your connection is good. !!!

Enjoyed
