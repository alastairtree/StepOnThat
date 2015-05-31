# StepOnThat

## What is it?

StepOnThat is a tool to allow you to describe a list of actions in a JSON notation and execute them.

StepOnThat is like a simple scripting language writen in json.

StepOnThat is a bit like Ant/NAnt or Rake or Grunt/Gulp but with a focus on tasks like browser automation or making http calls. 

StepOnThat tries to be easy and forgiving so you can GetThingsDone. It's goal is to make you more productive by automating boring jobs.

## Who is it for?

It is probably a tool aimed at web developers and testers. If you can write JSON and need to do basic browser automation or testing then StepOnThat.exe might work for you.

## Example

    StepOnThat.exe --file instructions.json

Where instructions.json looks like this:

    [
        {type:'http', url:'http://api.example.com/entity'}
        {type:'http', url:'http://api.example.com/other', method:'post'}
    ]

or in a more complicated example:

    {
        'steps': [
            {
                type: 'Browser',
                url: 'http://www.google.com',
                steps: [
                    { action: 'set', target: 'input[title=Search]', value: 'hello world' },
                    { action: 'submit' },
                    { action: 'title', match: 'hello world*' },
                    { action: 'click', target: 'div[role=main] a:link' },
                    { action: 'waitfor', target: '.thumb img' },
                    { action: 'address', match: '*.wikipedia.*' },
                ]
        
            },
            {
                type: 'Http', 
                url: 'http://requestb.in/vzw90wvz', 
                method: 'post', 
                data : '{message:\"hello APIs\"}'
            }
        ]
    }

And we also support properties from the json or on the command line

    StepOnThat.exe --file instructions.json --properties search-engine=http://www.google.com

with file

    {
        'properties':[{key:'search-term', value:'hello world'}],
        'steps': [
            {
                type: 'Browser',
                url: '${search-engine}',
                steps: [
                    { action: 'set', target: 'input[title=Search]', value: '${search-term}' },
                    { action: 'submit' },
                    { action: 'title', match: '${search-term}*' },
                    { action: 'click', target: 'div[role=main] a:link' },
                    { action: 'waitfor', target: '.thumb img' },
                    { action: 'address', match: '*.wikipedia.*' },
                ]
            }
        ]
    }


## Tools and libs used to StepOnThat

Thank you to these OSS projects who made this possible.

- Newstonsoft.JSON to read and write JSON
- Selenium WebDriver to do Browser automation
- Command Line Parser to read your inputs
- NUnit and Moq to test it all works
- HttpClient to make API calls
