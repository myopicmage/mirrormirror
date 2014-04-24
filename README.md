MirrorMirror
============

Generics are cool!

Provides two helper... things...

* Dto abstract class
* JoinToDto extension method

Dto
---

Inherit from Dto to gain access to init and initMult. Give it a class, and it will match the properties of the Dto you've created with the given class, and fill the Dto appropriately. InitMult will do this for two classes. Since you can't make a generic params method, a new initMult would need to be created if you want 3+ classes, but that's not exactly difficult.

JoinToDto
---------

An extension method on List<T>, sort of a weird Aggregate + Zip + Join. Give it a property name to join on, and pass it a List of your Dto class as an out parameter, and it will left join the two lists together and fill the Dto list.
