## Habit Tracking ASP .NET Core MVC App
This is a simple app, where you keep track of your behaviour. The Back-end is the focus of the app, so do not judge my Front-end, you don't have the moral right to!

## How to Run
You need MS SQL Server (unless you want to change). All the configuration is in the corresponding JSONs. I didn't use User Secrets (and above security-wise), because there was no need to.
If you want to deploy it, take the measures to hide everything sensitive. The cloud provides you with the ability to set ENV variables, so that's the most obvious option. In any case, it's simple, because ASP .NET Core IConfiguration looks everywhere anyway, with the appropriate 'source; precendence. 

## Project Architecure
The project follows somewhat of a "Clean Architecure" style architecture. It's simply divided intro three parts: Web, Infrastructure, Core.
1. The Web is basically the MVC part. It's job is simply to serve pages. It uses the Services from the Core to provide the necessary output from their operation.
2. The Infrastructure is basically the "back-end" of the back-end. It implements the IRepositories from the Core, which are resposible for providing the necessary data. This project uses EF Core, but the Repositories try to abstract it, ALTHOUGH the Unit of Work pattern is.. preserved.
3. The Core is basically the standalone app. The IRepositories are the input, and the IServices provide the output. Everything in between is internal to the Core.
## Interesting Details
1. There is a simple Observer Pattern implementation. It's not rigid and it's barely Observable, because the loose-coupling "feel" is not quite there, but it has potential. I chose to do it this way, because it made sense at the time; if I had more time, I would rewrite everything else (literally) to enable it fully, because I see potential (makes sense for me now.. again).
2. The Repositories are probably badly designed, EF Core is never going to leave the app, so abstracting far away from it's intricacies was a bad choice, in hindsight. I hit their maximum potential by failing to implement some 'reactive' behaviours, happening in the communication between the services.
3. There is a lot of repetition with Validation Logic. If I had more time, I would separate it in a different Core layer. I KNOW that the Web can do the Validation on its own AND it does it here, too, but I wanted The Core to be fully capable of keeping its state in a Valid state.
4. Although I thoroughly fought against the temptations of SDD (Spaghetti Driven Development), there may be areas in my code where messiness was inevitable due to lack of a good design or time. 
