# Data Manager Of a Power Plant
## Project Overview   
  The project consist of simulating the communication between a power plant and a service that processes the received data a client-service application.
  The project uses real-world power plant data form [OpenEI](https://data.openei.org/submissions/6179)'s Florida power plant dataset.
## System Architecture
  + **Power Plant Client** - simulates the collection of data on the side of the client filters it and sends the valid data to the service.
  + **Data processing service** - filters the received data and raises alarms according to the measured values.
## Technologies
+ C#/.NET
+ WCF (Windows Communication Foundation) for client-service communication
+ OpenEI power plant dataset
+ Client side data filtering
+ Service side data validation
+ Threshold based alarm detection
+ Exception and connection error handling
+ *IDispose* for resource cleanup 
