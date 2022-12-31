# Icebear Eureka

## What is this '-'
This is a plugin that exposes the Server ID (It's actually the Route ID,
Eureka instances are build differently...) of the instances, with this ID
its possible to identify instances without have to rely only on player list.


##
There is some issues that may make things a bit confusing:

#### Server IDs are reused

From what i've seen, there is a range of IDs that eureka instances use,
this range is from 48 to 51, so what happens is, when an instance lock,
the ID if this instance will appear in an future new instance.

Example: Pagos 48 locks, new Pagos has ID 50, the next new instance
can have the ID 48.

#### New instance can have the same ID as Old (rare)

On rare occasions i got a new instance with the same ID as the old one,
i dont know why this happens, but it does, my wild guess has to do with
soft and hard locks.

What i think it happens is, when an instance locks, it can soft lock, which
i think its just the instance being flagged as locked, or hard lock, when
an hard lock happens the instance actually loses his ID, and since IDs
are reused, it can happen that this same ID got used in the new instance.

All of this is speculative tho, so take it with a ocean of salt '-'
