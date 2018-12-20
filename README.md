# SMB Demo .NET Core

This repository is a trivial implementation of a .NET Core web application utilized to show features related to connecting a cloud foundry application to a remote SMB Share.  This application will allow you to perform simple CRUD operations on a the SMB share.  The credentials to connect to the share are exposed by the [SMB Volume Service for PCF Tile](https://network.pivotal.io/products/smb-volume-service/).  Configuration details for using Volume Services on Cloud Foundry can be found [here](https://docs.cloudfoundry.org/devguide/services/using-vol-services.html)

1. Clone the application to your local working environment with the following command: `git clone https://github.com/tezizzm/SmbDemoCore`

2. From the location you cloned the git repository, navigate to the project directory using the following command: `cd .\src\LighthouseWebCore\`

3. If you have not already previously created an SMB Share see the following links which give instructions for creating an SMB Share in [Powershell](https://docs.microsoft.com/en-us/powershell/module/smbshare/new-smbshare?view=win10-ps) or using [Windows Explorer or Computer Management](https://docs.microsoft.com/en-us/windows-hardware/drivers/debugger/file-share--smb--symbol-server)

4. We will now create a user provided service which our application will use to communicate with SMB share.

5. Open the `service-config.json` file and note it's contents.  Once again in the placeholders, you will need to enter the credentials of your SMB Share.

    *Note: there are no user credentials entered at this step  of the service creation.*

    ```json
    {
        "share":"//<ip address>/share name"
    }
    ```

    ```json
    {
        "share":"//123.456.789.012/share"
    }
    ```
6. Once done entering your SMB share information save and close the `service-config.json` file

7. Now that the configuration is set, we will create this service in cloud foundry with the following command: `cf create-service smbvolume Existing winfs-share -c .\service-config.json`

8. Publish the application locally to a publish folder at the project root directory with the following command `dotnet publish -o .\publish`

9. Take note of the `manifest.yml` file.  Note that we are **NOT** binding to the service we just created.

    ```yml
    ---
    applications:
    - name: lighthousewebuicore
    buildpack: dotnet_core_buildpack
    path: .\publish
    health-check-type: http
    ```
10. We will now deploy our application to Cloud Foundry by running the following command `cf push` **`--no-start`**. The --no-start flag tells Cloud Foundry not to stage or deploy our application at this time.

11. Now that the application is deployed we will not bind our service to our application.  It is on this bind step where we set the credentials for connecting to the share.

12. Open the `binding-config.json` file and substitute your share credentials in the given placeholders

    ```json
    {
        "username":"<username>",
        "password":"<password>"
    }
    ```

    ```json
    {
        "username":"username123",
        "password":"P@ssw0rd#%#@"
    }
    ```

13. Once done entering your SMB share credentials, save and close the `binding-config.json` file.

14. We will bind our service to our application and inject credentials for the broker to use to connect to our SMB share by using the following command: `cf bind-service lighthousewebuicore winfs-share -c .\binding-config.json`

15. Now that our service is bound with the SMB share credentials, we will now start the application using the following command: `cf start lighthousewebuicore`.

16. Click around the app.  If you have files in the directory they will be read from the SMB share and listed on the initial (Index) page.  You also have the opportunity to create a file on the SMB share by uploading a file from your local system.  Once created the file should show up in the index.  You can also read a file from the share and see processing of that files contents (approximate line numbers, word counts and content).

    <!-- 17. In the permissions tab you will see the user currently used to access the share as well as the users and that are assigned permissions on the share.  Note you will see several common Windows account (NT AUTHORITY\SYSTEM, BUILTIN\Administrators) as well as some accounts listed with their SID (S-1-5-21-24743843688-3923987316-3868828779-1000).   -->

17. Play around with the permissions on the shared folder and/or create multiple shares with different permissions.  

18. Use the command `cf update-service winfs-framework -p .\service-config.json` to update your service.  Note you will have to make edits to the `service-config.json` that correspond to your SMB share changes.

19. After the service has been update if you have made credential changes it will also be necessary to unbind and the rebind the service with the update credentials.  Update the `binding-config.json` accordingly.  Once complete run the following commands to first unbind `cf unbind-serivce lighthousewebuicore winfs-share` and once complete then rebind your service with the updated credentials `cf bind-service lighthousewebuicore winfs-share -c .\binding-config.json`.

20. As you update the user provided service to toggle the share you're connected to or the user you're connecting as, note how in some cases you will not be able to read from the directory or you will not be able to upload a file to the share.