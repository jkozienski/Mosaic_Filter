Mozaikowanie obrazu


1.	Wymagane elementy aplikacji
•	Graficzny interfejs użytkownika
•	Działająca aplikacja x64
•	Dowolna technologia GUI
•	2 tożsame biblioteki
  o	Język wysokiego poziomu (C++/C#)
  o	ASM x64
•	Wykorzystanie wielowątkowości w zakresie 1-64 wątki
•	Wykrycie i domyślne ustawienie liczby wątków odpowiadającej liczbie procesorów logicznych.
•	Pomiar czasu (w dobrych jednostkach)
  o	3 zestawy danych
  o	2 biblioteki
  o	Wątki 1, 2, 4, 8, 16, 32 i 64
  o	Uśredniony wyniki z 5 wywołań
•	Wykorzystanie instrukcji wektorowych (sensowne!)
•	Całość projektu realizowana w systemie kontroli wersji (preferowany github) (Opcjonalnie)

2.
Opis
Projekt polega na stworzeniu programu, który przekształca podany obraz w mozaikę.
Proces mozaikowania oznacza podzielenie obrazu na małe sekcje (kafelki), z których
każda zostaje zastąpiona średnim kolorem tej sekcji. Celem jest utrzymanie wyglądu obrazu
podobnego do oryginału, ale z charakterystycznym, mozaikowym efektem.


3.
Wykorzystanie wielowątkowości oraz instrukcji wektorowych
Wielowątkowość będzie wykorzystana w następujący sposób, obraz zostanie podzielony
na prostokątne bloki, które byłyby przetwarzane przez różne wątki.
Natomiast jeśli chodzi o instrukcje wektorowe, mogę ich użyć do jednoczesnego ładowania kilku kolorów pikseli,
 aby następnie obliczyć średnią wartość koloru danej sekcji.
5.
Parametryzacja
Użytkownik może zmienić rozmiar sekcji, która miałaby być w tym samym kolorze.
