# The Othello game
Gra Othello znana jest również pod nazwą Reversi. Rozgrywana jest na planszy o wymiarach 8x8, na której w chwili rozpoczęcia gry zajęte są cztery środkowe pola, tworzące małą szachownicę.

## Zasady gry 
- Gracze zajmują na przemian pola planszy, przejmując przy tym wszystkie pola przeciwnika znajdujące się między nowo zajętym polem a innymi polami gracza wykonującego ruch. Między nimi nie może być pustych pól. 
- Celem gry jest zdobycie większej liczby pól niż przeciwnik. 
- Gracz może zająć jedynie takie pole, które pozwoli mu przejąć przynajmniej jedno pole przeciwnika. Jeżeli takiego pola nie ma, musi oddać ruch. 
Aby w pełni zrozumieć zasady, gdy należy poprostu zagrać :)

## Koniec gry 
Gra kończy się, gdy wszystkie pola są zajęte lub gdy żaden z graczy nie może wykonać ruchu. O zwycięstwie decyduje liczba zajętych pól.

## Architektura kodu 
W grze zostaje oddzielony model (zasady gry) od widoku (klasy okna).


 Check
