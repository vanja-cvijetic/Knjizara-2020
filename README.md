WCF servis i klijentske aplikacije za servis knjižare. 

Servis obezbeđuje  rad sa podacima: 

• Korisnik (id_korisnika, korisnicko_ime, lozinka), 

• Knjiga (id_knjige, naziv, autor, cena, popust,kolicina), 

• Porudzbine(id_porudzbine, id_knjige, id_korisnika, kolicina) 

Za potrebe projekta servis se hostuje u konzolnoj aplikaciji. 

U okviru sistema postoje sledeće klijentske aplikacije sa navedenim mogućnostima: 

1. Operater (.NET klijentska aplikacija u okviru lokalne računarske mreže) koji omogućava: 

➢ Prijavu korisnika 

➢ Unos i izmenu podataka o knjigama i korisnicima. 

➢ Pregled porudžbina. 
 
 
2. Klijent (samostalna aplikacija) koja omogućava: 

➢ Prijavu korisnika 

➢ Prikaz podataka o knjigama  

➢ Poručivanja knjiga  
 
 
3. Web Klijent (web aplikacija) koja pristupa preko RESTful servisa i ima iste funkcije kao i aplikacija za Klijenta. 


4. Administrator (.NET aplikacija na istom računaru na kojem je i WCF servis) koja omućava: 

➢ Brisanje podataka  

➢ Sve funkcionalnosti koje imaju ostali klijenti 


Klijentske aplikacije su konfigurisane za rad sa delovima servisa koji su im namenjeni. 


Autor: Vanja Cvijetić
