VAR interrogation_1_checkpoint = -> interrogation_1
VAR interrogation_failed = false
VAR interrogation_done = false

-> idle

=== idle ===
+ [Interrogation] -> interrogation_1_checkpoint
+ [OnArrive] -> event_on_arrive
+ [OnHoboDiscover] -> event_on_hobo_discover
+ [OnLockedDoorInteract] -> event_on_locked_door_interact
+ [OnLockedDoorInteractMany] -> event_on_locked_door_interact_many
+ [OnSeeCat] -> event_on_see_cat
+ [OnCatRun] -> event_on_cat_run
+ [OnLureCat] -> event_on_lure_cat
+ [OnKeyGet] -> event_on_key_get
+ [OnEnterBackroom] -> event_on_enter_backroom
+ [OnPlateDrop] -> event_on_plate_drop


=== event_on_arrive ===
EVA: I can't believe Mario is planning to rat me out. I guess I have to have a "private session" with him before he does anything stupid.
EVA: There's an alley that leads to the back of the bar. I must find a way to get inside.
Use A/D to move; Left click to interact with items; Press E to pick up items
-> end_interrogation

=== event_on_hobo_discover ===
EVA: A sleeping hobo, fitting for such a dingy place
-> end_interrogation

=== event_on_locked_door_interact ===
EVA: {~Tchh... the door is locked|I need a key to open it|It won't budge}
-> end_interrogation

=== event_on_locked_door_interact_many ===
EVA: This is getting old.
-> end_interrogation

=== event_on_see_cat ===
EVA: I heard its a bad omen to see a black cat.
-> end_interrogation

=== event_on_cat_run ===
EVA: Wait, was that... something shiny on its neck?
->end_interrogation

=== event_on_lure_cat ===
EVA: Come here kitty, don't be shy.
->end_interrogation

=== event_on_key_get ===
EVA: Ohh, you are my lucky star.
-> end_interrogation

=== event_on_enter_backroom ===
EVA: Urgh, when was the last time they cleaned this room?
-> end_interrogation

=== event_on_plate_drop ===
EVA: Here goes nothing!
-> end_interrogation

=== interrogation_1 ===
{ interrogation_failed:
        -> end_interrogation
}
{ interrogation_done:
        -> end_interrogation
}
It's a homeless man, who has clearly been weathered by the years. Speak with him?
// because of course i can't f---ing figure out how to do nested choices without text i have to make another thing
+   [Yes] -> _interrogation_start
+   [No] -> end_interrogation

//  skill issue ratio l dance dogwater seethe get rekt noobs

=== _interrogation_start ===
+   [Hello.]
    -- EVA: Hello.
+   [Hey, you there?]
    -- EVA: Hey, you there?
- HOBO: zzz... zzz...
+   [*Gently tap shoulder.*]
    -- EVA: *gently taps his shoulder*
    -- HOBO: *rolls over*
    -- EVA: *tap tap tap*
+   [Can you hear me? C'mon, wake up.]
    -- EVA: Can you hear me? C'mon, wake up.
- HOBO: Alriiiight, alriiiight, I'm awaaaaake. Whaddoya wan? #name: HoboWakeUp
+   [Hey, are you busy? I want to get into this bar.]
    -- EVA: Hey, are you busy? I want to get into this bar.
    -- HOBO: Yeah, I'm busy. Away you go.
++      [Wait! We can talk this out.]
        --- EVA: Wait! We can talk this out.
        --- HOBO: The moon is out, missy, it's time for bed.
+++         [No, there is so much left for us to do!]
            ---- EVA: No, there is so much left for us to do!
            ---- HOBO: For example?
            ---- EVA: For example, party! I am sure a fine minister like you will enjoy some hot food and cool drinks.
            ---- HOBO: Party?
            ---- EVA: Yeah, if we can get inside the bar.
            -> _interrogation_hobo_is_sus
+++         [If you could get me inside, I would get you some booze.]
            ---- EVA: If you could get me inside, I would get you some booze.
            -> _interrogation_pre_alrighty_missy_section
++      [What's more important than getting me inside?]
        --- EVA: What's more important than getting me inside?
        -> _interrogation_why_get_in_there
+   [I need you to help me get into this building.]
    -- HOBO: Why? Whazz in it for me?
++      [I could get you some food from inside if you'd like.]
        --- EVA: I could get you some food from inside if you'd like.
        --- HOBO: FOOD? Missy, food ain't gonna cure the hole in my heart that burns for my wife. *cough* Can't believe she left me for that pimp of a man.
+++         [What happened with your wife?] -> _interrogation_what_happened_with_wife
+++         [As much as I'm sorry about whatever happened to you, I'm sure we can work something out.]
            ---- EVA: As much as I'm sorry about whatever happened to you, I'm sure we can work something out.
            ---- HOBO: If things worked out, I won't be here. Get lost, missy, I don't wanna talk no more.
            -> _interrogation_fail_state
++      [To be honest with you, nothing.]
        --- EVA: To be honest with you, nothing.
        --- HOBO: HAH! Looong shot then, missy. I ain't helping you.
+++         [Please? I really need to get in here.]
            ---- EVA: Please? I really need to get in here.
            -> _interrogation_why_get_in_there
+++         [Look. If you get me in, you can have anything you want out of the back store room after I leave.]
            ---- EVA: Look. If you get me in, you can have anything you want out of the back store room after I leave.
            ---- HOBO: *cough* I dunno if there's anythin' I even want back there!
++++            [Right.]
                ----- EVA: Right.
                ----- HOBO: *shrugs* If that's all you got to say, I'm headin' back to bed.
                ----- EVA: Do you like the way you live right now?
                ----- HOBO: Missy, you would give a damn if there is enough alcohol? Would you?
+++++               [There is a whole world out there, do you really want to live like this for the rest of your life?]
                    ------ EVA: There is a whole world out there, do you really want to live like this for the rest of your life?
                    ------ HOBO: WHY SHOULD I CARE? I've got nothing! NOTHING!
                    -> _interrogation_hate_remembering
+++++               [You are right. Speaking of alcohol, I could gets you some if you can help me inside.]
                    ------ EVA: You are right. Speaking of alcohol, I could get you some if you can help me inside.
                    ------ HOBO: Now we are talkin! Missy, you kept blabbing about life and that kind of nonsense, who even cares if you got BOOZE! There, there, you are thinking too much. Here, have a sip. *offers alcohol*
                    ------ \*there's white stuff floating in the brown liquid. Take a sip?
++++++                  [*No.]
                        ------- HOBO: You are no fun. Reminds me of that friend I had. Hmmm... what happened to him? Oh, he got murdered, by a fair lady that looks just like you...
                        ------- HOBO: Or am I mistaken? Whatever, I am heading back to sleep.
                        -> _interrogation_fail_state
++++++                  [*Yes.]
                        ------- \*you take a small sip. It burns.*
                        ------- HOBO: A sip of booze clears all your whooze!
                        -> _interrogation_alright_missy_section
++++            [It's a bar. There likely is alcohol.]
                ----- EVA: It's a bar. There likely is alcohol.
                -> _interrogation_pre_alrighty_missy_section

=== _interrogation_why_get_in_there ===
HOBO: Why do you wanna get in there?
+   [I have a personal matter to deal with. I advise you not be involved.]
    -- EVA: I have a personal matter to deal with. I advise you not be involved.
    -- HOBO: Say, I had a nosy friend who went around butting in all kinds of business. Last time I checked, he is murdered...by a fair lady that looks like you.
++      [What is your point? Are you going to do it or not?]
        --- HOBO: Woah! Getting aggressive! Do you want to fight? My punches hurts more than anything! *STUMBLES*
+++         [Sorry, that's not what I meant. I am simply startled because you mentioned a murder.]
            ---- EVA: Sorry, that's not what I meant. I am simply startled because you mentioned a murder.
            ---- HOBO: Why would ya be startled by that missy? Got something to hide?
++++            [OF COURSE NOT!]
                ----- EVA: OF COURSE NOT!
                -> _interrogation_hobo_is_sus
++++            [My father murdered...]
                ----- EVA: My father murdered...
                ----- HOBO: Poor missy, you can't possibly have it worst than me. My wife ditched me for another man! That son of a bitch has nothing worthy, don't even know what she was thinking.
+++++               [You are right, your wife is dumb for leaving you.]
                    ------ EVA: You are right, your wife is dumb for leaving you.
                    ------ HOBO: DON'T TALK ABOUT MY WIFE LIKE that
                    ------ HOBO: I love her... my kids... if only... *DRINKS ALCOHOL*
                    -> _interrogation_fail_state
+++++               [My father was a true hero in my life.]
                    ------ EVA: My father was a true hero in my life. He loved my mother and I with everything he had. If only I can go back in time, I would have done anything to save him.
                    ------ HOBO: Hell, aren't you something.
                    -> _interrogation_alright_missy_section
+++         [I AM NOT AFRAID OF YOU!]
            ---- EVA: I AM NOT AFRAID OF YOU!
            -> _interrogation_hobo_is_sus
++      [I am sorry to hear about your friend.]
        --- EVA: ... I am sorry to hear about your friend.
        --- HOBO: Ya. Me too.
+++         [So... could you open the door for me?]
            ---- EVA: So... could you open the door for me?
            -> _interrogation_hobo_is_sus
+++         [You sound like you're from the south. How did you get here?] -> _interrogation_from_south
+   [I have my reasons, don't ask.]
    -- EVA: I have my reasons, don't ask.
    -- HOBO: HA! You remind me of my wife...before she left with that stupid man.
    -> _interrogation_what_happened_with_wife


=== _interrogation_what_happened_with_wife ===
EVA: What happened with your wife?
HOBO: Arghh... somethin' about me not lovin' her enough. She.... she didn't understand that I had to put in a lotta work just to keep the farm alive. I woulda loved her more if I could, but our kids were dyin faster than the crops. Found a new man and off she went.
+   [What happened to the farm?]
    -- HOBO: After she was gone, I had no more hope. Sold the farm for twelve cents.
++      [You sound like you're from the south. How did you get here?] -> _interrogation_from_south
++      [Who bought it?]
        --- EVA: Twelve cents? Who bought it?
        --- HOBO: I dunno. Some company. *cough*
        --- EVA: I have connections. I can try to help you get back your farm if you help me get into this building.
        --- HOBO: Someone like YOU has CONNECTIONS? What are ya, some sorta detective?
+++         [Sort of...]
            ---- EVA: Sort of...
            ---- HOBO: Sorta? Shouldn't you have other detectives with ya? Why can't ya get in if you're investigatin'?
            ---- EVA: So I-
            ---- HOBO: Are ya part of those guys who swindled me outta my farm? Argh, what does anyting even matter! I need another bottle. But...
            -> _interrogation_hobo_is_sus
+++         [Well, not really...]
            ---- EVA: Well, not really...
            ---- HOBO: Strange. You kinda remind me of an old detective I read about in the paper years ago! What was his name...? Heard he killed himself, though.
++++            [That sounds so sad. I... definitely don't know who that is.]
                ----- EVA: That sounds so sad. I... definitely don't know who that is.
                ----- HOBO: ...missy, if I wasn't drunk outta my mind I'da said you paused a lil too long there. Are you... hidin' sumthing?
+++++               [No, I'm not hiding anything.]
                    ------ EVA: No, I'm not hiding anything.
                    -> _interrogation_hobo_is_sus
+++++               [Yes. I have things I am not willing to share, and so do you.]
                    ------ EVA: Yes. I have things I am not willing to share, and so do you.
                    ------ HOBO: ...
                    ------ EVA: I don't think we should further this conversation. Look, if you get me into that bar, I'll try to get you some alcohol.
                    -> _interrogation_pre_alrighty_missy_section
++++            [Riiiight... well, back to getting into this bar...]
                ----- EVA: Riiiight... well, back to getting into this bar...
                ----- HOBO: I like you. Yer not a pryer, not one of those cockeyed sucklers that stole my farm.
                -> _interrogation_alright_missy_section
+   [What happened to your kids?]
    -- EVA: What happened to your kids?
    -- HOBO: My- my wife... sold our kids. For two dollars. I dunno where they are...
    -> _interrogation_hate_remembering


=== _interrogation_from_south ===
EVA: You sound like you're from the south. How did you get here?
HOBO: I walked. Trust me little missy, alcohol lets you do anythin'!
HOBO: Speaking of, I've sobered up wayyy too much fer my tastes. I hate rememberin'. Good luck with yer thing ya were doin'. *DRINKS ALCOHOL*
-> _interrogation_fail_state


=== _interrogation_pre_alrighty_missy_section ===
HOBO: You sure are one direct little lady. I like that about'cha. Too many fake people in this world, all tryna sugar coat how absolute horse manure this world is. And ALCOHOL! That's my favourite word.
-> _interrogation_alright_missy_section
=== _interrogation_alright_missy_section ===
HOBO: Alrighty, missy. I'll help ya out. Let me just... urgh... #name: BreakDownDoor
~ interrogation_done = true
-> end_interrogation
=== _interrogation_hobo_is_sus ===
HOBO: Missy, you are hella fishy. I ain't helping you.
-> _interrogation_fail_state


=== _interrogation_hate_remembering ===
HOBO: Damn it! I hate rememberin'! Fuck you, fuck you, fuck you! *DRINKS ALCOHOL*
-> _interrogation_fail_state



=== _interrogation_fail_state ===
HOBO: *LAYS DOWN AND FALLS ASLEEP*
~ interrogation_failed = true
#name: HoboSleep
-> end_interrogation


=== save_and_exit(-> address) ===
~ interrogation_1_checkpoint = address
-> end_interrogation

=== end_interrogation ===
#end
-> idle