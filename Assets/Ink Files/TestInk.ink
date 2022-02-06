VAR level = 1

VAR interrogation_1_checkpoint = -> interrogation_1

-> idle

=== idle ===
{ level:
- 1: -> idle_1
- else: -> idle_1
}

=== idle_1 ===
+ [Interrogation] -> interrogation_1_checkpoint

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
- PROTEUS: There used to be input but Z0rb14n removed it :(
-> end_interrogation

=== save_and_exit(-> address) ===
{ level:
- 1: 
    ~ interrogation_1_checkpoint = address
} -> end_interrogation

=== end_interrogation ===
#end
-> idle