Feature: Querying a member's total penalties

Scenario: A member with two late returns has a summed penalty
    Given a member with a return 5 days late
    And the same member with another return 10 days late
    When the total penalties for the member are requested
    Then the total penalty amount is 3.00

Scenario: A member with no late returns has no penalty
    Given a standard member with no loans
    When the total penalties for the member are requested
    Then the total penalty amount is 0.00
