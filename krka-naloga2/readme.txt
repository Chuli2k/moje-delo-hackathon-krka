Zagon aplikacije:
- Aplikacija je grajena na .net core. 
- Pred zagonom je potrebno generirat lokalno SQL bazo. Za�enemo "update-database" v Package Manager Console.
-- S tem se generirajo Podjetja, Skladi��a in to�ke skladi��
- Za tem se lahko aplikacija za�ene.

Ob zagonu se kreira par testnih uporabnikov, ki so definirani v "Startup.SeedIdentity(...)". Tam so tudi role definirane.
Vsi generirani uporabniki imajo geslo "Password123.".
- admin uporabnik:
-- admin@krka.si
- uporabniki:
-- user@bayer.com
-- user@lek.si
- skladi��nik:
-- sklad@krka.si

Registrirajo se lahko novi uporabniki, katerim se avtomatsko dolo�i vloga "Uporabnik". �e �elimo kateremu spremenit vlogo, je potrebno v bazi popravit.
Po registraciji se pojavi stran za potrditev email-a. Tam je potrebno kliknit na link "Click here to confirm your account".
Dejansko po�iljanje potrditvenih mailov nisem vklopil, zaradi la�jega testiranja (sicer je �e vklju�eno v asp.core Identity).