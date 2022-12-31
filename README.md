<h1 align="center">Icebear Eureka</h1>

<h2 align="center"> What is this '-'</h2>

 <p align="center">This is a plugin that exposes the Server ID (It's actually the Route ID,
 Eureka instances are build differently...) of the instances, with this ID
 its possible to identify instances without having to rely only on player list.</p>

#

<p align="center">There are some issues that may make things a bit confusing: </p>

### Server IDs are reused

From what I've seen, there is a range of IDs that eureka instances use
this range is from 48 to 51. When an instance lock,
the ID if this instance will appear in a future new instance.

> Example: Pagos 48 locks, new Pagos has ID 50, the next new instance can have the ID 48.

### New instance can have the same ID as Old (rare)

On rare occasions I got a new instance with the same ID as the old one,
I don't know why this happens, but it does. My wild guess is it has to do with
soft and hard locks.

What I think it happens is, when an instance locks, it can soft lock, which
I think it's just the instance being flagged as locked or hard lock. When
a hard lock happens the instance actually loses it's ID and since IDs
are reused, it can happen that this same ID got used in the new instance.

All of this is speculative tho, so take it with an ocean of salt '-'
