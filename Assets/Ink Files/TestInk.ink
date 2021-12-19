VAR level = 1

VAR interrogation_1_checkpoint = -> interrogation_1
VAR interrogation_2_checkpoint = -> interrogation_2
VAR interrogation_3_checkpoint = -> interrogation_3
VAR interrogation_4_checkpoint = -> interrogation_4
VAR interrogation_5_checkpoint = -> interrogation_5

-> idle

=== idle ===
{ level:
- 1: -> idle_1
- 2: -> idle_2
- 3: -> idle_3
- 4: -> idle_4
- 5: -> idle_5
- else: -> idle_1
}

=== idle_1 ===
+ [Phone] -> contacts_1
+ [Interrogation] -> interrogation_1_checkpoint

=== idle_2 ===
+ [Phone] -> contacts_2
+ [Interrogation] -> interrogation_2_checkpoint

=== idle_3 ===
+ [Phone] -> contacts_3
+ [Interrogation] -> interrogation_3_checkpoint

=== idle_4 ===
+ [Phone] -> contacts_4
+ [Interrogation] -> interrogation_4_checkpoint

=== idle_5 ===
+ [Phone] -> contacts_5
+ [Interrogation] -> interrogation_5_checkpoint

=== contacts_1 ===
+ [CLOSE] -> idle
+ [Agent 1,111-111-1111,Icon-Agent489,true] -> agent_1
+ [Agent 2,222-222-2222,Icon-Unknown,true] -> agent_2
+ [UNKNOWN,589-337-1872,Icon-Unknown,{unknown_joke: true | false}] -> unknown_joke

===  contacts_2 ===
+ [CLOSE] -> idle

===  contacts_3 ===
+ [CLOSE] -> idle

===  contacts_4 ===
+ [CLOSE] -> idle

===  contacts_5 ===
+ [CLOSE] -> idle

=== incoming_random_person ===
UNKNOWN: I'm a random person
UNKNOWN: Hello
+ You suck lol
UNKNOWN: Ok bye now
-> hang_up

=== agent_1 ===
{
- agent_1 == 1: -> first_call
- agent_1 > 1: -> spam_call
}

= first_call
#version: 1
+ Hi
+ Hello
- AGENT_1: Hi, I'm Agent 1
AGENT_1: First time calling huh
AGENT_1: Blah blah blah
AGENT_1: Blah blah
AGENT_1: Blah blah blah blah
AGENT_1: Ok bye now
-> hang_up

= spam_call
#version: 2
AGENT_1: Don't call me again lol
-> hang_up

=== agent_2 ===
{
- agent_2 <= 2: -> first_call
- agent_2 > 1: -> spam_call
}

= first_call
# version: 1
AGENT_2: Hi I'm Agent 2
AGENT_2: {agent_2 == 1: First | Second} time calling huh
+   Choice 1
+   Choice 2
- AGENT_Y: Blah blah blah...
- AGENT_2: Blah blah blah blah blah...
+   Choice 1
    --  AGENT_2: Entered choice 1
+   Choice 2
    --  AGENT_2: Entered choice 2
- AGENT_2: Ok bye now
-> hang_up

= spam_call
#version: 2
AGENT_2: Don't call me again lol
-> hang_up

=== unknown_joke ===
{
- unknown_joke == 1: -> first_call
- else: -> afterward
}

= first_call
#version: 1
UNKNOWN: Hello? Who is this?
AGENT_Y: Oh, my friend gave me this number. They said that I should call you to test out my phone, since I just got it and I'm trying to learn how to use it.
UNKNOWN: Well, you've done it! You're a calling expert. Do you want to hear a joke?
+ Sure.
-- UNKNOWN: What do you call an undercover tarantula?
-- AGENT_Y: ...
-- UNKNOWN: A SPY-der!! Bye now! 
    -> hang_up
+ I don't have time for jokes.
-- UNKNOWN: Okay then, have a great day.
    -> hang_up

= afterward
-> unavailable("589-337-1872")

=== unavailable(text) ===
:<i>{text} is currently unavailable.</i>
-> hang_up

=== hang_up ===
#end
{ level:
- 1: -> contacts_1
- 2: -> contacts_2
- 3: -> contacts_3
- 4: -> contacts_4
- 5: -> contacts_5
}

=== interrogation_1 ===
PROTEUS: Testing the interrogation
+   [Choice 1]
+   [Choice 2]
- AGENT_Y: Blah blah blah...
- PROTEUS: Blah blah blah blah blah...
+   [Choice 1]
    --  PROTEUS: Entered choice 1
+   [Choice 2]
    --  PROTEUS: Entered choice 2
- (checkpoint1) PROTEUS: Checkpoint 1
+   [Leave] -> save_and_exit(-> checkpoint1)
+   [Don't leave]
- PROTEUS: You stayed
- PROTEUS: Now enter some input
    -> input ->
~ interrogation_1_checkpoint = -> after_guess
- (after_guess) - PROTEUS: You guessed the right word!
- AGENT_Y: Back to the main story
-> end_interrogation

= input
- TYPE IN A WORD: #input
+   [back] -> save_and_exit(-> input)
+   [else]
    --  PROTEUS: You didn't guess any of the correct choices.
+   [shoe]
    --  PROTEUS: You guessed the word "shoe".
+   [duck]
    --  PROTEUS: You guessed the word "duck".
+   [corn]
    --  PROTEUS: You guessed the word "corn".
+   [boat]
    --  PROTEUS: You guessed the word "boat". Success!
    --  ->->
- -> input

=== interrogation_2 ===
NOVA: Hello this is interrogation level 2
-> end_interrogation

=== interrogation_3 ===
-> end_interrogation

=== interrogation_4 ===
-> end_interrogation

=== interrogation_5 ===
-> end_interrogation

=== save_and_exit(-> address) ===
{ level:
- 1: 
    ~ interrogation_1_checkpoint = address
- 2:
    ~ interrogation_2_checkpoint = address
- 3:
    ~ interrogation_3_checkpoint = address
- 4:
    ~ interrogation_4_checkpoint = address
- 5:
    ~ interrogation_5_checkpoint = address
} -> end_interrogation

=== end_interrogation ===
#end
-> idle