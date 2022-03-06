VAR interrogation_1_checkpoint = -> interrogation_1

-> idle

=== idle ===
+ [Interrogation] -> interrogation_1_checkpoint
+ [OnArrive] -> event_on_arrive


=== event_on_arrive ===
EVA: skill issue ratio l dance dogwater seethe get rekt noobs
-> end_interrogation


=== interrogation_1 ===
EVA: skill issue ratio l dance dogwater seethe get rekt noobs 
-> end_interrogation

//  skill issue ratio l dance dogwater seethe get rekt noobs


=== save_and_exit(-> address) ===
~ interrogation_1_checkpoint = address
-> end_interrogation

=== end_interrogation ===
#end
-> idle