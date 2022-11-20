# Third Party Services
- IEX API for stock quotes
- Alpha Advantage API for stock quotes
- Azure Service bus for instant messenger

# Third Party Libraries
- Moq
- XUnit

# Objective
Develop a console application that is extensible and provides quick access to desired services. I was experiencing a bit of analysis paralysis thinking about what to build that would also demonstrate different skills. The result I believe is a framework that could be easily extended to support a wide array of features. 

# Requirements
### These requirements were created to get the ball rolling towards creating something. Each requirement was a phase and pull request of the project.
## Phase 1
1. Must support the following keywords
  - help
    - This will provide a list of available commands
  - exit
    - This will exit the console application
    
### Design Decisions:
At this phase I still didn't know exactly what I was going to build as far more advanced modules go. For this reason I knew a command design pattern made sense. I also knew I wanted each command to be tied to the actual method they were supposed to execute, so I added a delegate to each command. This reduced the amount of decision logic needed in the modules. 

## Phase 2
2. Retrieve stock quotes
    - User will enter the command "quote {ticker name}" 
    - The desktop companion will return the stock price
    - Support the ability to swap out stock quote providers
    - Implement two stock quote providers. 
    
### Design Decisions:
At this phase I wanted to demonstrate inversion of control, a bit of domain driven design (DDD), and perform an integration with a third party. So, I found two free APIs to use to get stock quotes. By creating a provider for each API and having each one implement an interface, I was able to have the Financial Module loosely coupled to the providers. Also by passing the providers into the constructor, I was able to have two financial modules up and running each using a different financial service. Finally by using DTOs to communicate with the financial parties, and internal POCOs for internal business logic it demonstrates a bit of DDD. 

## Phase 3
3. Instant Messaging
    - Support the following commands
      - msg send username 
        - Message will be sent to only 1 user
      - msg send all message contents
        - Message will be sent to all users who have logged in so far
      - read 
        - Retrieve all messages from your inbox        
      - read username
        - Retrieve messages from a certain user
    - Users will be able to message individuals
    - Users will be able to message all created users
    - Users will be able to read messages sent to them
      - Directly
      - Sent to all
    - Users will not receive messages from themselves when sent to  "all"
    
### Design Decisions:
At this phase I wanted to demonstrate a cloud integration and the observer design pattern. So, I created a Service bus that supports topics and queues to support direct messaging and messaging all users. I created one provider to interact directly with the service bus, and another provider which acts as a façade between the service bus management of the first provider and the message sending and receiving of the instant messaging module. Finally I added a delegate on the MessageInbox object that is called when a new message is added by the async service bus subscription. It will post to the screen there is a new message. 

