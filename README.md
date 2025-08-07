# Emplo zadania rekrutacyjne

## Zadanie 1

Zaimplementowałem **precomputed hierarchy pattern** z wykorzystaniem algorytmu **tree traversal** dla każdego węzła. Struktura danych oparta na **Dictionary lookup (O(1))** zapewnia szybkie zapytania przy jednorazowej inicjalizacji **O(N×H)**.

**Kluczowe optymalizacje:**
- **Memoization** relacji hierarchicznych
- **Circular reference detection** z HashSet
- **Fail-fast validation** z comprehensive error handling

**Złożoność:** Init O(N×H), Query O(1), Memory O(R) gdzie N=pracownicy, H=głębokość, R=relacje.

**Trade-off:** Wyższa pamięć za maksymalną wydajność zapytań + data integrity.

## Zadania 2-5
Zdecydowałem się odwzorować dane w C# i zmapować do bazy za pomocą EF, ponieważ zadania korzystają z tego samego schematu, a dzięki temu mogę łatwo przetestować napisane metody.

Jeśli będzie potrzeba przetestowania aplikacji z innymi danymi wystarczy zmienić dane w `DataSeeder` następnie usunąć z `/bin` plik `vacation.db` po czym **uruchomić na nowo aplikację**

## Zadanie 6

Z takich głównych sposobów optymalizacji liczby zapytań SQL w przypadkach, gdzie parametry metod pobieramy z bazy danych myślę że można wymienić

**Eager Loading**

Polega na pobraniu wszystkich powiązanych danych w jednym zapytaniu. W Entity Framework używamy do tego metody `.Include()`. Dzięki temu zamiast wykonywać osobne zapytania dla każdej relacji (np. najpierw pracownicy, potem ich zespoły), pobieramy wszystkie potrzebne dane za jednym razem.

**Batch Loading**

Zamiast wykonywać wiele podobnych zapytań pojedynczo (problem N+1), grupujemy je w jedno większe zapytanie. Na przykład, zamiast N zapytań dla N pracowników, wykonujemy jedno zapytanie z klauzulą `IN` zawierającą wszystkie potrzebne identyfikatory.

**Caching**

Przechowujemy wyniki zapytań w pamięci podręcznej np. Redis, dzięki czemu nie musimy za każdym razem odpytywać bazy danych. Szczególnie przydatne dla danych, które rzadko się zmieniają, jak słowniki czy konfiguracje.
