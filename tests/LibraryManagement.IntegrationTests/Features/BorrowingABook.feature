Feature: Borrowing a book

Scenario: Borrowing an available book
    Given a book titled "Clean Code" by "Robert C. Martin" with 2 available copies
    And a standard member named "Alice"
    When the member borrows the book
    Then the loan is accepted
    And the book has 1 available copy

Scenario: Borrowing a book with no available copy
    Given a book titled "Clean Code" by "Robert C. Martin" with 0 available copies
    And a standard member named "Alice"
    When the member borrows the book
    Then the loan is rejected with status 409

Scenario: Borrowing a book beyond the member's quota
    Given a standard member named "Alice" with 3 active loans
    And a book titled "Clean Code" by "Robert C. Martin" with 2 available copies
    When the member borrows the book
    Then the loan is rejected with status 409
