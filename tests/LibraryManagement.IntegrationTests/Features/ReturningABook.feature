Feature: Returning a book

Scenario: Returning a book on time
    Given a borrowed book due today
    When the member returns the book
    Then the return is accepted with no penalty

Scenario: Returning a book five days late
    Given a borrowed book due 5 days ago
    When the member returns the book
    Then the return is accepted with a penalty of 1.00

Scenario: Returning a book already returned
    Given a borrowed book due today
    And the book has already been returned
    When the member returns the book
    Then the return is rejected with status 409
