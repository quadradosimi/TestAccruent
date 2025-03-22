# React + .NET 8

## TEST Accruent

To run the application is necessary download the project at GitHub.

	https://github.com/quadradosimi/TestAccruent.git

Run Back-end
	
	Open the .Net 8 project on TestMoutTi.sln file. Change the ConnectionStrings with your database settings, at file appsettings.json.
	Run the migration to build database structure with code below 

		add-migration [name]
		
	and after run

		update-database
		
	Run the TestAccruent.sln file to open the project inside the Visual Studio. Choice https in Visual Studio to start the software and run. The swagger will appear.
		
Run Front-end

	The front-end application is inside folder Web/TestAccruent. Change SERVER_URL at the Web/TestAccruent/config.json () with the right API url. 
	At the prompt get in Web/TestAccruent folder and run code below
	
		npm i
		
		npm run dev
		
	The locally server will run and show the url to set in browser. 
