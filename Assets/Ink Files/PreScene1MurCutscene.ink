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
DETECTIVE 1: Aaah, Eva! It is quite my surprise that I have caught you on my way out of the office. How are you?
EVA: I'm fine, Robert.
DETECTIVE 1: You're brash as always, dearest Eva. You know, I've just thought of the most perfect metaphor to describe you!
EVA: It's practically your thousanth metaphor at this point, but go ahead.
DETECTIVE 1: "She's like the sandpaper in a construction zone; working hard but coarse through and through." I think it's much more accurate than my last, don't you think?
EVA: That is a simile.
DETECTIVE 1: No, it's a metaphor. A highly educated person such as myself would know that.
EVA: ...
DETECTIVE 1: But! I have not just come here for fun and games, dearest Eva. I have a key piece of intel I am eager to share with you!
DETECTIVE 1: Last night we received a call on the phone from a chef at a local bar. You know, one of those run down, dingy places here? That practically smells of rotten oil and grease?
EVA: What? Go on...
DETECTIVE 1: Well, to repeat what he said, *ahem* "Ayeee know sumthun abut the one yer tryin' tooo catch, but I can't say nuthin' here. It's real important!"
EVA: I'm sure he didn't sound like that, but go on.
DETECTIVE 1: Well, you know how those... ugh... people speak. If I did not know any better, I would assume they were drunk every time they spoke!
DETECTIVE 1: Regardless, they supposedly have key information about those murders we have been trying to solve. Hey, if they are good, maybe they will take your spot here in the force!
EVA: ...
DETECTIVE 1: We are going to go tomorrow afternoon at thirteen hundred hours. This investigation is your specialty, after all, so it is important that you be there.
EVA: I'll be there, Robert. And where exactly is 'there'?
DETECTIVE 1: Uncle John's Bar. I do have to leave, now. I have an appointment with a stunning young lady very soon. As the kids say nowadays, do not 'snap your cap' because I knew about this before you. I know you love your case but you just have to accept that you won't always live up to your father's reputation.
EVA: Hey, I can and am living up to his reputation! Ugh. See you tomorrow. Oh, and you said the man we are interviewing is a chef... correct?
DETECTIVE: You do crack me up! That is correct. Goodbye now. *leaves*
EVA, TO HERSELF: How on earth would a chef know about... wait... damn it.
EVA, GETTING UP TO LEAVE: He knew what would happen if he told anyone. I guess I'd better do something about this. 
-> end_interrogation

=== save_and_exit(-> address) ===
{ level:
- 1: 
    ~ interrogation_1_checkpoint = address
} -> end_interrogation

=== end_interrogation ===
#end
-> idle