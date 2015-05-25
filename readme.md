# StepOnThat

## What is it?
StepOnThat is a tool to allow you to describe a list of actions in a JSON notation and execute them.

StepOnThat is a bit like Ant/NAnt or Rake or Grunt/Gulp but with a focus on tasks like browser automation.

## Who is it for?
It is probably a tool aimed at web developers and testers. If you can write JSON and need to do basic browser automation or testing then StepOnThat.exe might work for you.

## Example

    StepOnThat.exe --file instructions.json

Where instructions.json looks like this:

    {
        "steps": [
            {
                "type": "BrowserStep",
                "url": "http://www.google.com",
                "steps": [
                    { "action": "set", "target": "input[title=Search]", "value": "hello world" },
                    { "action": "submit" },
                    { "action": "title", "match": "hello world*" },
                    { "action": "click", "target": "div[role=main] a:link" },
                    { "action": "waitfor", "target": ".thumb img" }
                ]
            }
        ]
    }