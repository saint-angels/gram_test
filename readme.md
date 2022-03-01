We will be expecting to receive in a zip file:
• Source code
• Documentation (a small documentation about stuff we should be aware of – Unity version,
target platform etc. and your notes about the project, 3rd party frameworks you used).
- apk

### Documentation:
While I hope this test project shows my ability to develop test projects,
it doesn't exactly have the architecture or consistent style I would expect in production.

For example, some values(like, how often a new hero is unlocked) are hardcoded.
In production values like these would be defined in config. Here, I hope, this is expected.

Real production and real teams require more complex structure, which evolves based on unique goals and constraints,
as well as needs of the team.
This is a simple project, so I've tried to keep the code simple.
Trying to future-proof the test project by introducing complexity, so that an imaginary "real" project can benefit from it, would take a lot of time, without any visible benefit to this test, imo.
I'd happy to chat about how more "real" version of this project might look like.

Also, in professional developer community, there is a popular idea,
that by separating big methods into smaller ones, you are always reducing complexity, even if it doesn't reduce lines of code, or obscures linear order of instructions.
I don't agree with this idea, so here my methods are relatively long and aren't broken into parts without need.
As this could be polarizing, I would be happy to discuss the reasons for this, if needed.

Project was developed with Unity 2020.3.17f1
Target platform - Android.
Used 3rd party frameworks:
- Newtonsoft Json for serialization
- DOTween
