#Objective
Develop a console application that is extensible and provides quick access to desired services

##Requirements
1. Must support the following keywords
  - help
    - This will provide a list of available commands
  - exit
    - This will exist the console application

2. Retrieve stock quotes
    - User will enter the command "quote {ticker name}" 
    - The desktop companion  will return the stock price
    - Implement support the ability to swap out stock quote providers
    - Implement two stock quote providers. 

3. Instant Messaging
    - Support the following commands
      - msg send username 
        - Message will be sent to only 1 user
      - msg send all message contents
        - Message will be sent to all users who have logged in so far
      - read 
        - Retrieve all messages from your inbox
    - Users will be able to message individuals
    - Users will be able to message all created users
    - Users will be able to read messages sent to them
      - Directly
      - Sent to all
    - Users will not receive messages from them selves when sent to  "all"
