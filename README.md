# UserPermissionsAdmin

The main project of this solution is UserPermissionsAdmin which contains all the logic. To be able to run it, we need to run the next commands: 
```
docker-compose build
docker-compose up
```
These are the container that will be created:
1. userpermissionadmin (web api)
2. elasticsearch
3. kibana
4. sqlserver

## Database initialization

For creating the database as well as the tables and initial data we need to run the UserPermissionDb.publish file which belongs to the UserPermissionDb project. In the following image we can see the form to be able to Publish the database changes:

![image](https://github.com/user-attachments/assets/892e7a92-b22b-4c01-aeee-ec4384f0b694)

This process will run some scripts to create tables and insert init data.

## Unit and integration tests

From the UserPermissionAdminTest project we can find a PermissionControllerIntegrationTest for the integration testing and the PermissionServiceTest for the unit ones.

## Considerations

### Elasticserch issues

I was not able to get work ElasticSearch from the app but I could do it from Kibana. It is because I couldn't set the correct configuration for authenticating the app in SSL mode by using the credentials and/or the fingerprint. However, all the logic and configuration was implemented.

### Information logs handle

I decided to implement a middleware to handle the logs for each request and response. Please, check the class RequestLoggingMiddleware out in the Middleware folder from the UserPermissionsAdmin project.
