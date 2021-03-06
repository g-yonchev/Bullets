# Bullets 

### TexasHoldemGameAI

The AI logic is divided in two parts. One for the preflop part of the game and another one - Monte Carlo method for the rest of the game.

#### Preflop

We evaluate our cards from 0 to 2 according to this table:

| AA | AKs | AQs | AJs | ATs | A9s | A8s | A7s | A6s | A5s | A4s | A3s | A2s |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| 2 | 2 | 2 | 2 | 2 | 1 | 1 | 1 | 1 | 0 | 0 | 0 | 0 |

| AKo | KK | KQs | KJs | KTs | K9s | K8s | K7s | K6s | K5s | K4s | K3s | K2s |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| 1 | 2 | 2 | 2 | 2 | 1 | 1 | 1 | 0 | 0 | 0 | 0 | 0 |

| AQo | KQo | QQ | QJs | QTs | Q9s | Q8s | Q7s | Q6s | Q5s | Q4s | Q3s | Q2s |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| 1 | 2 | 2 | 2 | 1 | 1 | 1 | 1 | 1 | 0 | 0 | 0 | 0 |

| AJo | KJo | QJo | JJ | JTs | J9s | J8s | J7s | J6s | J5s | J4s | J3s | J2s |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| 1 | 1 | 1 | 2 | 1 | 1 | 1 | 1 | 1 | 0 | 0 | 0 | 0 |

| ATo | KTo | QTo | JTo | TT | T9s | T8s | T7s | T6s | T5s | T4s | T3s | T2s |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| 1 | 1 | 1 | 1 | 2 | 2 | 1 | 1 | 0 | 0 | 0 | 0 | 0 |

| A9o | K9o | Q9o | J9o | T9o | 99 | 98s | 97s | 96s | 95s | 94s | 93s | 92s |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| 1 | 0 | 0 | 1 | 1 | 2 | 1 | 1 | 0 | 0 | 0 | 0 | 0 |

| A8o | K8o | Q8o | J8o | T8o | 98o | 88 | 87s | 86s | 85s | 84s | 83s | 82s |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| 0 | 0 | 0 | 0 | 1 | 1 | 1 | 1 | 0 | 0 | 0 | 0 | 0 |

| A7o | K7o | Q7o | J7o | T7o | 97o | 87o | 77 | 76s | 75s | 74s | 73s | 72s |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| 0 | 0 | 0 | 0 | 0 | 1 | 0 | 1 | 0 | 0 | 0 | 0 | 0 |

| A6o | K6o | Q6o | J6o | T6o | 96o | 86o | 76o | 66 | 65s | 64s | 63s | 62s |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 0 | 0 |

| A5o | K5o | Q5o | J5o | T5o | 95o | 85o | 75o | 65o | 55 | 54s | 53s | 52s |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 0 |

| A4o | K4o | Q4o | J4o | T4o | 94o | 84o | 74o | 64o | 54o | 44 | 43s | 42s |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |

| A3o | K3o | Q3o | J3o | T3o | 93o | 83o | 73o | 63o | 53o | 43o | 33 | 32s |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |

| A2o | K2o | Q2o | J2o | T2o | 92o | 82o | 72o | 62o | 52o | 42o | 32o | 22 |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |
| 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |

Then we check how much money we have. If our money are less than 500 or more than 1500 if our cards are evaluated as 2 we are playing all in. Otherwise (our money are between 500 and 1500) we raise the amount of the pot times 4.

If our cards are evaluated as 0 we check or call if possible. Otherwise we fall.

If our cards are evaluated as 1 we ckeck or call.

#### Flop, Turn, River (Monte Carlo algorithm)

We create a deck of cards containing all cards without ours and the community cards. Then we use this deck to make 30 (or more) simulations for each turn of the player. If it wins in less than 33 percent of the simulations, the player checks or folds. If it wins between 33 and 66 percent it checks or calls, if it wins more it raises the current pot once and if it wins in more than 90 percent it raises with all the money.