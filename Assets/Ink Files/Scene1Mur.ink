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


=== on_arrive ===
EVA: I can't believe Mario is planning to rat me out. I guess I have to have a "private session" with him before he does anything stupid.
EVA: There's an alley that leads to the back of the bar. I must find a way to get inside.
Use A/D to move; Left click to interact with items
-> end_interrogation


=== interrogation_1 ===
It's a homeless man, who has clearly been weathered by the years. Speak with him?
+   [Yes]
+       [Hello.]
        -- EVA: Hello.
+       [Hey, you there?]
        -- EVA: Hey, you there?
    HOBO: zzz... zzz...
    -> end_interrogation
+   [No] -> end_interrogation

=== save_and_exit(-> address) ===
{ level:
- 1: 
    ~ interrogation_1_checkpoint = address
} -> end_interrogation

=== end_interrogation ===
#end
-> idle