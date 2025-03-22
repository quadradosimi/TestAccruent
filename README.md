# React + .NET 8 Employee Crud

## TEST Mout Ti

To run the application is necessary download the project at GitHub.

	https://github.com/quadradosim/Mouts.git

Run Back-end
	
	Open the .Net 8 project on TestMoutTi.sln file. Change the ConnectionStrings with your database settings, at file appsettings.json.
	Run the migration to build database structure with code below 

		add-migration [name]
		
	and after run

		update-database
		
	Choice https to start the software and run. After builded, use swagger to get the token from API. Use the loginJWT endpoint and
	set the parameter username to admin and Password to password and Execute. The response have the token. (this token will be use in front-end)
		
Run Front-end

	The front-end application is inside folder Web/TestMountTi. Change SERVER_URL at the Web/TestMountTi/src/config.json () with the right API url. 
	And change de TOKEN with the value in (this token will be use in front-end). At the prompt get in Web/TestMountTi folder and run code below
	
		npm run dev
		
	The locally server will run and show the url to set in browser. In the login page use admin@admin to the user and 1234 to the password.
	With this user is possible add the initial user and make the crud.
