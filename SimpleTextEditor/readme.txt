==================================
Krystof's Tantalising Text Editor!
=-_****************************_-=

Info
--------------------
Name: Krystof Pavlis
ID: 14184004
Erin's 3pm Monday Tut

Logins
--------------------
cal | 123 (Edit)
user1 | password123 (View)

Code Extensions
--------------------
As you can see from the files, I used a modified RichTextBox to achieve font style toggling in a 
satisfactory way. I have spent hours researching and testing various methods, and this seems to be
the only one that doesn't compromise anything. The other two I considered:

- Not having the functionality work as I want it: I found a way to apply each style to the selected text,
but it would overwrite any font differences. This wasn't good enough
- Going character by character, which I saw some other students used, induced heavy flickering and in some 
cases took a while to execute. Again, didn't feel like a proper solution and I really wanted something that 
worked properly.

The creator of the original code is credited and linked correctly. Using others' code is, I'd say, a pretty
fundamental part of programming (what is a library but someone else's code?). This extension is also something
that I believe I could do myself, given a day or two at most. Lastly, I actually modified the original code:
Initially it had separate functions for toggling the styles on or off, but I combined them into one, with a 
boolean flag. I feel all that together justifies my use of it :)