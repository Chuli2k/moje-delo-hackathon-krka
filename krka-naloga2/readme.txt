Zagon aplikacije:
- Aplikacija je grajena na .net core. 
- Pred zagonom je potrebno generirat lokalno SQL bazo. Zaženemo "update-database" v Package Manager Console.
-- S tem se generirajo Podjetja, Skladišèa in toèke skladišè
- Za tem se lahko aplikacija zažene.

Ob zagonu se kreira par testnih uporabnikov, ki so definirani v "Startup.SeedIdentity(...)". Tam so tudi role definirane.
Vsi generirani uporabniki imajo geslo "Password123.".
- admin uporabnik:
-- admin@krka.si
- uporabniki:
-- user@bayer.com
-- user@lek.si
- skladišènik:
-- sklad@krka.si

Registrirajo se lahko novi uporabniki, katerim se avtomatsko doloèi vloga "Uporabnik". Èe želimo kateremu spremenit vlogo, je potrebno v bazi popravit.
Po registraciji se pojavi stran za potrditev email-a. Tam je potrebno kliknit na link "Click here to confirm your account".
Dejansko pošiljanje potrditvenih mailov nisem vklopil, zaradi lažjega testiranja (sicer je že vkljuèeno v asp.core Identity).