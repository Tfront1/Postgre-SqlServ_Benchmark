Units of measurement - milliseconds.
M(median) - arithmetic mean.
## POSTGRE

- ### Step by step Insert (100.000 rows)
| Operation number<br><br>ORM | 1 | 2 | 3 | 4 | 5 | M |
| ---- | ---- | :--: | ---- | ---- | ---- | ---- |
| Dapper | 48719 | 50506 | 48580 | 44921 | 47673 | 48080 |
| EF (NpgSql) | VERY long | ... | ... | ... | ... | ... |

- ### Bulk Insert (100.000 rows)
| Operation number<br><br>ORM | 1 | 2 | 3 | 4 | 5 | M |
| ---- | ---- | :--: | ---- | ---- | ---- | ---- |
| Dapper | 43387 | 48598 | 42129 | 42533 | 41093 | 43548 |
| EF (NpgSql) | 10013 | 8970 | 6623 | 8710 | 8874 | 8638 |

- ### Bulk Insert in transaction (100.000 rows)
| Operation number<br><br>ORM | 1 | 2 | 3 | 4 | 5 | M |
| ---- | ---- | :--: | ---- | ---- | ---- | ---- |
| Dapper | 21213 | 21513 | 21642 | 21009 | 21434<br> | 21362 |
| EF (NpgSql) | 8514 | 8984 | 8780 | 8635 | 8708 | 8724 |

- ### Simple select (100.000 rows)
| Operation number<br><br>ORM | 1 | 2 | 3 | 4 | 5 | M |
| ---- | ---- | :--: | ---- | ---- | ---- | ---- |
| Dapper | 311 | 151 | 144 | 131 | 134 | 174 |
| EF (NpgSql) | 1298 | 661 | 776 | 550 | 556 | 768 |

---------------------------------

## SqlServer

- ### Step by step Insert (100.000 rows)
| Operation number<br><br>ORM | 1 | 2 | 3 | 4 | 5 | M |
| ---- | ---- | :--: | ---- | ---- | ---- | ---- |
| Dapper | 50661 | 55045 | 55069 | 53303 | 49902 | 52796 |
| EF | VERY long | ... | ... | ... | ... | ... |

- ### Bulk Insert (100.000 rows)
| Operation number<br><br>ORM | 1 | 2 | 3 | 4 | 5 | M |
| ---- | ---- | :--: | ---- | ---- | ---- | ---- |
| Dapper | 50740 | 51407 | 50461 | 49279 | 49940 | 50365 |
| EF | 12593 | 11203 | 11269 | 11371 | 10984 | 11484 |

- ### Bulk Insert in transaction (100.000 rows)
| Operation number<br><br>ORM | 1 | 2 | 3 | 4 | 5 | M |
| ---- | ---- | :--: | ---- | ---- | ---- | ---- |
| Dapper | 20061 | 18663 | 17421 | 19786 | 17487 | 18683 |
| EF | 12989 | 11970 | 11317 | 10987 | 12272 | 11907 |

- ### Simple select (100.000 rows)
| Operation number<br><br>ORM | 1 | 2 | 3 | 4 | 5 | M |
| ---- | ---- | :--: | ---- | ---- | ---- | ---- |
| Dapper | 268 | 231 | 186 | 111 | 85 | 176 |
| EF | 1156 | 655 | 586 | 565 | 539 | 700 |

---------------------------------------------------------
## Results

- ### Median EF Results
| Operation type<br><br>DB | Bulk Insert | Bulk Insert in transaction | Simple select |
| ---- | ---- | ---- | ---- |
| Postgre | 8638 | 8724 | 768 |
| Sql server | 11484 | 11907 | 700 |
| Percentages | 33% | 36.5% | 9.7% |
